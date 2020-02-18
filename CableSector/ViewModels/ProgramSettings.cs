// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CableSector.Models;

namespace CableSector.ViewModels
{
    /// <summary>
    /// Программные настройки.
    /// </summary>
    public class ProgramSettings : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        #endregion

        #region Constructor

        public ProgramSettings()
        {
            // Таблица кабельных жил.
            Cable = new TableSettings(4);
            // Таблица барабанов.
            Drum = new TableSettings(1);
            // Настройки окна.
            Page = new PageSettings();
            // Загрузка не завершена.
            PageEditorIsLoaded = false;
            PageChangeLogIsLoaded = false;
            WindowMainIsLoaded = false;
            // Помощник холста.
            Canvas = new CanvasHelper(Cable, Page);
            // По-умолчанию.
            Default();
            // Режим работы.
            Mode = WorkMode.Default;
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Помощник XAML.
        /// </summary>
        private readonly XamlHelper _xamlHelper = XamlHelper.Instance;

        #endregion

        #region Public properties

        public WorkMode _mode;
        /// <summary>
        /// Режим работы.
        /// </summary>
        public WorkMode Mode
        {
            get => _mode;
            set
            {
                switch (value)
                {
                    case WorkMode.Default:
                        if (PageEditorIsLoaded)
                        {
                            if (MessageBox.Show("Выполнить сброс настроек по-умолчанию?",
                                    Application.Current.MainWindow != null
                                        ? Application.Current.MainWindow.Title : string.Empty,
                                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                RowsCountCable = 4;
                                Default();
                                Canvas.Default();
                            }
                        }
                        else
                        {
                            RowsCountCable = 4;
                            Default();
                            Canvas.Default();
                        }
                        break;
                    case WorkMode.Canvas:
                        if (PageEditorIsLoaded)
                        {
                            Update();
                            Canvas.Update(Header, CalcMaxLength);
                        }
                        break;
                    case WorkMode.ChangeLog:
                        if (PageChangeLogIsLoaded)
                        {
                            //
                        }
                        break;
                    case WorkMode.Print:
                        if (PageEditorIsLoaded)
                        {
                            _xamlHelper.Print(Canvas.CanvasPrint, Header);
                        }
                        break;
                }
                _mode = value;
                OnPropertyRaised();
            }
        }

        private string _header;
        /// <summary>
        /// Заголовок.
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

        private int _rowsCountCable;
        /// <summary>
        /// Количество кабельных жил.
        /// </summary>
        public int RowsCountCable
        {
            get => _rowsCountCable;
            set
            {
                if (Cable != null)
                {
                    Cable.RowsCount = value;
                    _rowsCountCable = Cable.RowsCount;
                    OnPropertyRaised();
                    // Обновить.
                    Update();
                }
            }
        }

        public bool _pageEditorIsLoaded;
        /// <summary>
        /// Загрузка страницы редактора завершена.
        /// </summary>
        public bool PageEditorIsLoaded
        {
            get => _pageEditorIsLoaded;
            set
            {
                _pageEditorIsLoaded = value;
                OnPropertyRaised();
            }
        }

        public bool _pageChangeLogIsLoaded;
        /// <summary>
        /// Загрузка страницы истории версий завершена.
        /// </summary>
        public bool PageChangeLogIsLoaded
        {
            get => _pageChangeLogIsLoaded;
            set
            {
                _pageChangeLogIsLoaded = value;
                OnPropertyRaised();
            }
        }

        public bool _windowMainIsLoaded;
        /// <summary>
        /// Загрузка главного окна завершена.
        /// </summary>
        public bool WindowMainIsLoaded
        {
            get => _windowMainIsLoaded;
            set
            {
                _windowMainIsLoaded = value;
                OnPropertyRaised();
            }
        }

        private CanvasHelper _canvas;
        /// <summary>
        /// Холст.
        /// </summary>
        public CanvasHelper Canvas
        {
            get => _canvas;
            set
            {
                _canvas = value;
                OnPropertyRaised();
            }
        }

        private TableSettings _cable;
        /// <summary>
        /// Таблица кабельных жил.
        /// </summary>
        public TableSettings Cable
        {
            get => _cable;
            set
            {
                _cable = value;
                OnPropertyRaised();
            }
        }

        private TableSettings _drum;
        /// <summary>
        /// Таблица барабанов.
        /// </summary>
        public TableSettings Drum
        {
            get => _drum;
            set
            {
                _drum = value;
                OnPropertyRaised();
            }
        }

        private PageSettings _page;
        /// <summary>
        /// Настройки окна.
        /// </summary>
        public PageSettings Page
        {
            get => _page;
            set
            {
                _page = value;
                OnPropertyRaised();
            }
        }

        private int _calcMaxLength;
        /// <summary>
        /// Максимальная длина.
        /// </summary>
        public int CalcMaxLength
        {
            get => _calcMaxLength;
            set 
            {
                _calcMaxLength = value;
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
            set
            {
                _calcMaxColCount = value;
                OnPropertyRaised();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Привязка данных.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Binding GetBinding(string path)
        {
            return new Binding(path + "Property")
            {
                Source = this,
                Mode = BindingMode.OneWay,
                Path = new PropertyPath(path),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            };
        }

        // Поток ожидания изменения коллекций.
        //private async void WaitItemsChanged()
        //{
        //    bool exit = false;
        //    await Task.Run(() =>
        //    {
        //        while (!exit)
        //        {
        //            exit = true;
        //            // Таблица кабельных жил.
        //            if (Cable.ItemsChanged)
        //            {
        //                exit = false;
        //                Cable.Update();
        //            }
        //            if (Cable.RecordsCurrent != null)
        //            {
        //                foreach (ArrayRecord item in Cable.RecordsCurrent)
        //                {
        //                    item.Update();
        //                }
        //            }
        //            // Таблица барабанов.
        //            if (Drum.ItemsChanged)
        //            {
        //                exit = false;
        //                Drum.Update();
        //            }
        //            if (Drum.RecordsCurrent != null)
        //            {
        //                foreach (var item in Drum.RecordsCurrent)
        //                {
        //                    item.Update();
        //                }
        //            }
        //            // Пауза.
        //            Thread.Sleep(100);
        //        }
        //    });
        //}

        /// <summary>
        /// Обновить.
        /// </summary>
        public void Update()
        {
            CalcMaxLength = 0;
            CalcMaxColCount = 0;
            Cable.Update();
            Drum.Update();
            // Максимальная длина.
            CalcMaxLength = Cable.CalcMaxSum > Drum.CalcMaxSum ? Cable.CalcMaxSum : Drum.CalcMaxSum;
            // Максимальное количество колонок.
            CalcMaxColCount = Cable.CalcMaxColCount > Drum.CalcMaxColCount ? Cable.CalcMaxColCount : Drum.CalcMaxColCount;
        }

        /// <summary>
        /// По-умолчанию.
        /// </summary>
        public void Default()
        {
            Header = "<Марка> <кол-во жил> х <сечение>";
            Cable.Headers = new List<string>() { "1 жила", "2 жила", "3 жила", "4 жила", "5 жила",
                "6 жила", "7 жила", "8 жила", "9 жила", "10 жила", };
            Cable.RowsCount = 4;
            Cable.Default();
            Drum.Headers = new List<string>() { "Длина", "№ тары", "Тип тары" };
            Drum.RowsCount = 1;
            Drum.Default();
            CalcMaxLength = 0;
            CalcMaxColCount = 0;
        }

        /// <summary>
        /// Безопасное обновление DataGrid.
        /// </summary>
        /// <param name="dataGrid"></param>
        public static void RefreshSafely(DataGrid dataGrid)
        {
            ((IEditableCollectionView)dataGrid.Items).CommitEdit();
            dataGrid.Items.Refresh();
        }

        #endregion
    }
}

