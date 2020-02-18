// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Windows;
using System.Windows.Controls;
using CableSector.ViewModels;

namespace CableSector.Views
{
    /// <summary>
    /// Страница редактора.
    /// </summary>
    public partial class PageEditor
    {
        #region Private fields and properties

        /// <summary>
        /// Программные настройки.
        /// </summary>
        private readonly ProgramSettings _settings;

        #endregion

        #region Private methods - Page

        public PageEditor()
        {
            InitializeComponent();

            _settings = null;
            var context = FindResource("ViewModelProgramSettings");
            if (context is ProgramSettings settings)
            {
                _settings = settings;
                _settings.Canvas.Init(GridMain, GridCanvas);
            }
        }

        private void PageEditor_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataGridCable != null)
            {
                DataGridCable.Columns[0].IsReadOnly = true;
                DataGridCable.Columns[DataGridCable.Columns.Count - 1].IsReadOnly = true;
            }
            if (DataGridDrum != null)
            {
                DataGridDrum.Columns[0].IsReadOnly = true;
                //dataGridDrum.Columns[dataGridCable.Columns.Count - 1].IsReadOnly = true;
            }

            // Загрузка страницы редактора завершена.
            _settings.PageEditorIsLoaded = true;
            CanvasMain.Children.Clear();
        }

        private void PageEditor_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //
        }

        #endregion
        
        #region Private methods

        /// <summary>
        /// Обновить страницу.
        /// </summary>
        /// <returns></returns>
        public void Refresh()
        {
            ExpanderCable.IsExpanded = true;
            //ExpanderDrum.IsExpanded = true;
            
            if (_settings != null)
            {
                DataGridCable.DataContext = _settings.Cable.RecordsCurrent;
                DataGridDrum.DataContext = _settings.Drum.RecordsCurrent;
            }
        }

        /// <summary>
        /// Поле "Количество жил".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fieldRowsCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_settings != null)
            {
                DataGridCable.DataContext = _settings.Cable?.RecordsCurrent;
                DataGridDrum.DataContext = _settings.Drum?.RecordsCurrent;
                dataGridCable_CellEditEnding(sender, null);
            }
        }

        /// <summary>
        /// Задать фокус таблицы.
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="table"></param>
        private void SetFocusCell(DataGrid dg, int row, int col, TableSettings table)
        {
            table.LockerDataGrid = true;
            DataGridCellInfo cell;
            // Не последняя строка.
            if (row < dg.Items.Count - 1)
            {
                // Последняя колонка.
                cell = col >= 10
                    ? new DataGridCellInfo(dg.Items[row + 1], dg.Columns[1])
                    : new DataGridCellInfo(dg.Items[row], dg.Columns[col + 1]);
            }
            // Последняя строка.
            else
            {
                // Последняя колонка.
                cell = col >= 10
                    ? new DataGridCellInfo(dg.Items[row], dg.Columns[1])
                    : new DataGridCellInfo(dg.Items[row], dg.Columns[col + 1]);
            }
            if (cell != default)
            {
                //dg.SelectedCells.Clear();
                //dg.SelectedCells.Add(cell);
                //dg.ScrollIntoView(cell);
                //dg.CurrentCell = cell;
                //dg.BeginEdit();
            }

            table.LockerDataGrid = false;
        }

        /// <summary>
        /// Кабельные жилы. Редактирование ячейки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridCable_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Блокировщик таблиц.
            if (_settings.Cable.LockerDataGrid || _settings.Drum.LockerDataGrid)
                return;
            var row = e?.Row.GetIndex() ?? 0;
            var col = e?.Column.DisplayIndex ?? 0;
            // По-умолчанию.
            if (_settings.PageEditorIsLoaded)
                _settings.Canvas.Default();
            // Задать фокус таблицы.
            SetFocusCell(DataGridCable, row, col, _settings.Cable);
        }

        /// <summary>
        /// Барабаны. Редактирование ячейки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridDrum_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Блокировщик таблиц.
            if (_settings.Cable != null && _settings.Drum != null && (_settings.Cable.LockerDataGrid || _settings.Drum.LockerDataGrid))
                return;
            var row = e?.Row.GetIndex() ?? 0;
            var col = e?.Column.DisplayIndex ?? 0;
            // Табличные настройки барабанов. Обновить.
            _settings.Drum?.Update();
            // По-умолчанию.
            if (_settings.PageEditorIsLoaded)
                _settings.Canvas.Default();
            // Задать фокус таблицы.
            SetFocusCell(DataGridDrum, row, col, _settings.Drum);
        }

        #endregion
    }
}
