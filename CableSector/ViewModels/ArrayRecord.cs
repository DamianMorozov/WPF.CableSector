// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CableSector.ViewModels
{
    /// <summary>
    /// Массив значений длин.
    /// </summary>
    public class ArrayRecord : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        #endregion

        #region Constructor

        public ArrayRecord()
        {
            Header = "";
            Items = new ObservableCollection<string>();
            for (var i = 0; i < 10; i++)
            {
                Items.Add(default);
            }
        }

        public ArrayRecord(string header, int count = 10)
        {
            Header = header;
            Items = new ObservableCollection<string>();
            for (var i = 0; i < count; i++)
            {
                Items.Add(default);
            }
        }

        public ArrayRecord(string header, ObservableCollection<string> items) : this(header, 0)
        {
            Items = items;
        }

        #endregion

        #region Public fields and properties

        private string _header;
        /// <summary>
        /// Заголовки.
        /// </summary>
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyRaised();
            }
        }

        private int _calcSum;
        /// <summary>
        /// Сумма значений массива.
        /// </summary>
        public int CalcSum
        {
            get => _calcSum;
            set
            {
                _calcSum = value;
                OnPropertyRaised();
            }
        }

        private ObservableCollection<string> _items;
        /// <summary>
        /// Элементы.
        /// </summary>
        public ObservableCollection<string> Items
        {
            get => _items;
            set
            {
                _items = value;
                _items.CollectionChanged += items_CollectionChanged;
                items_CollectionChanged(_items, null);
                OnPropertyRaised();
            }
        }

        /// <summary>
        /// Изменилась коллекция.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Обновить вычисляемые свойства.
            Update();
        }

        /// <summary>
        /// Обновить.
        /// </summary>
        public void Update()
        {
            CalcSum = 0;
            foreach (var item in Items)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    int.TryParse(item, out int value);
                    CalcSum += value;
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Добавить элемент.
        /// </summary>
        /// <param name="value"></param>
        public void AddItem(string value)
        {
            Items.Add(value);
        }

        /// <summary>
        /// Обновить элемент.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void UpdateItem(int index, string value)
        {
            if (index < Items.Count)
            {
                Items[index] = value;
            }
        }

        #endregion
    }
}
