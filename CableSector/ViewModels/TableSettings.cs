// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using CableSector.Models;

namespace CableSector.ViewModels
{
    /// <summary>
    /// Табличные настройки.
    /// </summary>
    public class TableSettings : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Табличные настройки.
        /// </summary>
        public TableSettings()
        {
            Headers = new List<string>();
            RowsCount = 1;
            LockerDataGrid = false;
            // Изменилась коллекция.
            ItemsChanged = false;
        }

        /// <summary>
        /// Табличные настройки.
        /// </summary>
        /// <param name="rowsCount"></param>
        public TableSettings(int rowsCount) : this()
        {
            RowsCount = rowsCount <= 0 ? 1 : rowsCount;
        }

        #endregion

        #region Public fields and properties

        private ObservableCollection<ArrayRecord> _recordsCurrent;
        /// <summary>
        /// Текущие записи.
        /// </summary>
        public ObservableCollection<ArrayRecord> RecordsCurrent
        { 
            get => _recordsCurrent;
            private set 
            {
                _recordsCurrent = value;
                _recordsCurrent.CollectionChanged += _recordsCurrent_CollectionChanged;
                _recordsCurrent_CollectionChanged(_recordsCurrent, null);
                OnPropertyRaised();
            }
        }

        private ArrayRecord _recordCableOut;
        /// <summary>
        /// Готовый кабель.
        /// </summary>
        public ArrayRecord RecordCableOut
        {
            get => _recordCableOut;
            private set
            {
                _recordCableOut = value;
                OnPropertyRaised();
            }
        }

        /// <summary>
        /// Изменилась коллекция.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _recordsCurrent_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ItemsChanged = true;
        }

        private List<string> _headers;
        /// <summary>
        /// Список заголовков.
        /// </summary>
        public List<string> Headers
        {
            get => _headers;
            set
            {
                _headers = value;
                OnPropertyRaised();
            }
        }

        private int _rowsCount;
        /// <summary>
        /// Количество строк.
        /// </summary>
        public int RowsCount
        {
            get => _rowsCount;
            set
            {
                if (value < 1)
                {
                    value = 1;
                    MessageBox.Show("Количество строк не может быть меньше 1!");
                }
                if (value > 10)
                {
                    value = 10;
                    MessageBox.Show("Количество строк не может быть больше 10!");
                }
                _rowsCount = value;
                // Новая коллекция записей.
                UpdateRecords();
                // Обновить вычисляемые свойства.
                Update();
                OnPropertyRaised();
            }
        }

        private bool _lockerDataGrid;
        /// <summary>
        /// Блокировщик таблицы.
        /// </summary>
        public bool LockerDataGrid
        {
            get => _lockerDataGrid;
            set
            {
                _lockerDataGrid = value;
                OnPropertyRaised();
            }
        }

        private bool _itemsChanged;
        /// <summary>
        /// Изменилась коллекция.
        /// </summary>
        public bool ItemsChanged
        {
            get => _itemsChanged;
            set
            {
                _itemsChanged = value;
                OnPropertyRaised();
                if (_itemsChanged)
                    // Обновить вычисляемые свойства.
                    Update();
            }
        }

        #endregion

        #region Public fields and properties - Calculated

        private int _calcMaxRow;
        /// <summary>
        /// Максимальная строка.
        /// </summary>
        public int CalcMaxRow
        { 
            get => _calcMaxRow;
            private set 
            {
                _calcMaxRow = value;
                OnPropertyRaised();
            }
        }

        private int _calcMinRow;
        /// <summary>
        /// Минимальная строка.
        /// </summary>
        public int CalcMinRow
        { 
            get => _calcMinRow;
            private set 
            {
                _calcMinRow = value;
                OnPropertyRaised();
            }
        }

        private int _calcMaxSum;
        /// <summary>
        /// Максимальная сумма.
        /// </summary>
        public int CalcMaxSum
        {
            get => _calcMaxSum;
            private set
            {
                _calcMaxSum = value;
                OnPropertyRaised();
            }
        }

        private int _calcMinSum;
        /// <summary>
        /// Минимальная сумма.
        /// </summary>
        public int CalcMinSum
        {
            get => _calcMinSum;
            private set
            {
                _calcMinSum = value;
                OnPropertyRaised();
            }
        }

        private int _calcMaxColCount;
        /// <summary>
        /// Максимальное количество колонок.
        /// </summary>
        public int CalcMaxColCount
        {
            get => _calcMaxColCount;
            private set
            {
                _calcMaxColCount = value;
                OnPropertyRaised();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// По-умолчанию.
        /// </summary>
        public void Default()
        {
            foreach (var rec in RecordsCurrent)
            {
                for (var i = 0; i < rec.Items.Count; i++)
                {
                    rec.Items[i] = default;
                }
            }
        }

        /// <summary>
        /// Обновить.
        /// </summary>
        public void Update()
        {
            var calcMaxSum = 0;
            var calcMaxColCount = 0;
            if (RecordsCurrent == null || RecordsCurrent.Count == 0)
                return;

            int row = default;
            var listMinSum = new List<int>();
            foreach (var itemRec in RecordsCurrent)
            {
                int rowMinSum = default;
                if (itemRec is ArrayRecord rec)
                {
                    var colCount = 0;
                    foreach (var item2 in rec.Items)
                    {
                        if (!string.IsNullOrEmpty(item2))
                        {
                            int.TryParse(item2, out var value);
                            if (value > 0)
                            {
                                colCount++;
                                rowMinSum += value;
                            }
                        }
                    }
                    listMinSum.Add(rowMinSum);
                    if (calcMaxColCount < colCount)
                        calcMaxColCount = colCount;
                    if (calcMaxSum < rec.CalcSum)
                    {
                        CalcMaxRow = row;
                        calcMaxSum = rec.CalcSum;
                    }
                }
                row++;
            }
            // Максимальная сумма.
            CalcMaxSum = calcMaxSum;
            CalcMaxColCount = calcMaxColCount;

            // Минимальная сумма.
            var calcMinSum = 0;
            var calcMinRow = 0;
            row = default;
            foreach (var itemMinSum in listMinSum)
            {
                if (itemMinSum > 0)
                {
                    if (calcMinSum == 0)
                        calcMinSum = itemMinSum;
                    else if (calcMinSum > itemMinSum)
                    {
                        calcMinRow = row;
                        calcMinSum = itemMinSum;
                    }
                }
                row++;
            }
            CalcMinSum = calcMinSum;
            CalcMinRow = calcMinRow;

            // Обновить готовый кабель.
            UpdateRecordsCableOut();
            _itemsChanged = false;
        }

        /// <summary>
        /// Обновить готовый кабель.
        /// </summary>
        public void UpdateRecordsCableOut()
        {
            var recordCableOut = new ArrayRecord("Готовый кабель", 0);
            if (RecordsCurrent == null || RecordsCurrent.Count == 0)
                return;

            var listRecords = new List<List<int?>>();
            foreach (var itemRec in RecordsCurrent)
            {
                if (itemRec is ArrayRecord rec)
                {
                    var listItems = new List<int?>();
                    var shift = 0;
                    foreach (var item in rec.Items)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            int.TryParse(item, out int value);
                            if (value > 0)
                            {
                                shift += value;
                                listItems.Add(shift);
                            }
                        }
                    }
                    if (listItems.Count > 0)
                        listRecords.Add(listItems);
                }
            }

            IEnumerable<int?> result = new List<int?>();
            listRecords.ForEach(i => result = result.Union(i));
            var sort = SortQuickHelper.Instance;
            result = sort.ExecuteRecursiveFast(result.ToArray(), EnumSort.Asc);
            
            var last = 0;
            foreach (var item in result)
            {
                int.TryParse(item.ToString(), out var value);
                if (value > 0)
                {
                    if (last == 0)
                    {
                        recordCableOut.AddItem(item.ToString());
                    }
                    else
                    {
                        if (value <= CalcMinSum)
                            recordCableOut.AddItem((value - last).ToString());
                    }
                    last = value;
                }
            }
            RecordCableOut = recordCableOut;
        }

        /// <summary>
        /// Новая коллекция записей.
        /// </summary>
        /// <returns></returns>
        public void UpdateRecords()
        {
            var RecordsNew = new ObservableCollection<ArrayRecord>();
            for (int row = 0; row < _rowsCount; row++)
            {
                if (row < RecordsCurrent?.Count)
                {
                    if (row < Headers.Count)
                        RecordsNew.Add(new ArrayRecord(Headers[row], RecordsCurrent[row].Items)); 
                    else
                        RecordsNew.Add(RecordsCurrent[row]);
                }
                else
                { 
                    if (row < Headers.Count)
                        RecordsNew.Add(new ArrayRecord(Headers[row]));
                    else
                        RecordsNew.Add(new ArrayRecord("-"));
                }
            }
            RecordsCurrent = RecordsNew;
        }

        /// <summary>
        /// Общая длина отрезков жилы.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public int GetWidthAll(int row)
        {
            var result = default(int);
            // Табличные настройки жил.
            if (RecordsCurrent != null)
            {
                if (row < RecordsCurrent.Count)
                {
                    foreach (var item in RecordsCurrent[row].Items)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            int.TryParse(item, out int value);
                            result += value;
                        }
                    }
                }
            }
            return result;
        }

        #endregion
    }
}
