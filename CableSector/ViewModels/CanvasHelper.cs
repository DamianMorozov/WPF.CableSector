// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CableSector.ViewModels
{
    /// <summary>
    /// Помощник холста.
    /// </summary>
    public class CanvasHelper
    {
        #region Constructor

        /// <summary>
        /// Помощник холста.
        /// </summary>
        /// <param name="cable"></param>
        /// <param name="page"></param>
        public CanvasHelper(TableSettings cable, PageSettings page)
        {
            // Таблица кабельных жил.
            _cable = cable;
            // Таблица барабанов.
            //_drum = drum;
            // Настройки окна.
            _page = page;
            CanvasPrint = null;
            _gridMain = null;
            _gridCanvas = null;
            FontSize = 12;
        }

        #endregion

        #region Private fields & properties

        public Canvas CanvasPrint { get; private set; }
        public double FontSize { get; set; }
        private Grid _gridMain;
        private Grid _gridCanvas;
        private readonly TableSettings _cable;
        private readonly PageSettings _page;
        private int _calcMaxLength;
        private readonly Brush _brushCanvasPrint = Brushes.Transparent;
        private readonly int _labelItemMinHeight = 40;
        private readonly int _rectItemMinHeight = 11;
        private readonly int _rectItemMaxHeight = 18;
        private readonly Color _colorFillRectCable = Colors.Transparent;
        private readonly Color _colorStrokeRectCable = Colors.Transparent;
        private readonly Color _colorFillRectItem = Colors.LightGray;
        private readonly Color _colorFillRectOut = Colors.Gray;
        private readonly Color _colorStrokeRectItem = Colors.Black;
        private readonly Color _colorFillRectReminder = Colors.Transparent;
        private readonly Color _colorStrokeRectReminder = Colors.Transparent;
        private readonly Color _colorFillEllipse = Colors.AliceBlue;
        private readonly Color _colorStrokeEllipse = Colors.Black;
        private const int _priorityRectangle = 1;
        private const int _priorityLine = 2;
        private const int _priorityEllipse = 3;
        private const int _priorityLabel = 5;
        private const double _shiftLeft = 130;
        private double _shiftTop;
        private const int _shiftTopMin = 30;
        private const double _canvasBorderWidth = 35;
        private const double _windowBorderWidth = 160;
        private const double _windowBorderHeight = 175;
        private double _heightCable;
        private double _heightItem;

        #endregion

        #region Public methods

        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="gridMain"></param>
        /// <param name="gridCanvas"></param>
        /// <returns></returns>
        public void Init(Grid gridMain, Grid gridCanvas)
        {
            _gridMain = gridMain;
            _gridCanvas = gridCanvas;
        }

        /// <summary>
        /// Обновить вычисляемые свойства.
        /// </summary>
        public Grid Default()
        {
            // Максимальная длина.
            _calcMaxLength = 0;
            if (_gridCanvas != null)
            {
                _gridCanvas.Children.Clear();
                CanvasPrint = null;
                // Холст и содержимое фона.
                GetCanvas(_gridMain, _gridCanvas);
            }
            return _gridCanvas;
        }

        /// <summary>
        /// Обновить.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="calcMaxLength"></param>
        public void Update(string header, int calcMaxLength)
        {
            // Максимальная длина.
            _calcMaxLength = calcMaxLength;
            if (_gridCanvas != null)
            {
                _gridCanvas.Children.Clear();
                // Холст и содержимое фона.
                CanvasPrint = GetCanvas(_gridMain, _gridCanvas);
                double count = _cable.RowsCount + 1.2;
                _shiftTop = (CanvasPrint.Height - 10) / (count * 1.85);
                if (_shiftTop < _shiftTopMin)
                    _shiftTop = _shiftTopMin;
                // Строки.
                SetRows(CanvasPrint, count);
                // Метка заголовка.
                SetLabelHeader(CanvasPrint, header);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Холст фона.
        /// </summary>
        /// <param name="gridMain"></param>
        /// <param name="gridCanvas"></param>
        /// <returns></returns>
        private Canvas GetCanvas(Grid gridMain, Panel gridCanvas)
        {
            var width = default(double);
            var height = default(double);
            if (gridMain != null)
            {
                var listRows = new List<GridLength>();
                var heightAll = default(double);
                foreach (var row in gridMain.RowDefinitions)
                {
                    listRows.Add(row.Height);
                    heightAll += row.Height.Value;
                }
                double.TryParse(gridMain.ActualWidth.ToString(CultureInfo.CurrentCulture), out width);
                double.TryParse(gridMain.ActualHeight.ToString(CultureInfo.CurrentCulture), out var outHeight);
                height = outHeight * listRows[listRows.Count - 1].Value / heightAll;
            }
            var canvas = new Canvas
            {
                Width = width - _canvasBorderWidth,
                Height = height - _windowBorderHeight - 10,
                Background = _brushCanvasPrint,
            };
            gridCanvas.Children.Add(canvas);
            //Canvas.SetLeft(canvas, 0);
            return canvas;
        }

        /// <summary>
        /// Строки.
        /// </summary>
        /// <param name="canvasPrint"></param>
        /// <param name="count"></param>
        private void SetRows(Panel canvasPrint, double count)
        {
            var firstLine = true;
            double width = _page.Width - _shiftLeft - _windowBorderWidth;
            _heightCable = (canvasPrint.Height - _shiftTop) / count;
            double top = _shiftTop;
            for (var row = 0; row < _cable.RowsCount + 1; row++)
            {
                double left = _shiftLeft;
                double allWidth = 0;
                // Кабельная жила.
                var rectCable = GetRectCable(canvasPrint, width, _heightCable, top);
                // Колонки с жилами.
                if (_cable.RecordsCurrent != null)
                {
                    // Кабельные жилы.
                    if (row < _cable.RecordsCurrent.Count)
                    {
                        var i = 0;
                        foreach (var item in _cable.RecordsCurrent[row].Items)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                int.TryParse(item, out int itemWidth);
                                allWidth += itemWidth;
                                // Отрезок кабельной жилы.
                                var rectItem = GetRectItem(canvasPrint, rectCable, left, itemWidth, top, false);
                                left += rectItem.Width;
                                // Начало и конец кабельной жилы.
                                SetEllipses(canvasPrint, rectCable, rectItem, left, i, top);
                                // Линии вниз.
                                SetLines(canvasPrint, rectCable, rectItem, ref firstLine, row, left, top);
                                // Метки длин отрезков.
                                SetLabelLength(canvasPrint, rectItem, itemWidth, left, top);
                            }
                            i++;
                        }
                    }
                    // Готовый кабель.
                    else if (row == _cable.RecordsCurrent.Count)
                    {
                        var i = 0;
                        foreach (var item in _cable.RecordCableOut.Items)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                int.TryParse(item, out int itemWidth);
                                allWidth += itemWidth;
                                // Отрезок кабельной жилы.
                                var rectItem = GetRectItem(canvasPrint, rectCable, left, itemWidth, top, true);
                                left += rectItem.Width;
                                // Начало и конец кабельной жилы.
                                SetEllipses(canvasPrint, rectCable, rectItem, left, i, top);
                                // Метки длин отрезков.
                                SetLabelLength(canvasPrint, rectItem, itemWidth, left, top);
                            }
                            i++;
                        }
                    }
                    // Барабаны.
                    else if (row == _cable.RecordsCurrent.Count + 1)
                    {
                        //var canvasRow = new Canvas
                        //{
                        //    Width = _window.Width - _shiftLabel,
                        //    Height = (canvasPrint.Height - 10) / count - 0,
                        //};
                        //Canvas.SetLeft(canvasRow, 5);
                        //Canvas.SetTop(canvasRow, row * ((canvasPrint.Height - 10) / count) + _shiftTop);
                        //// Прямоугольник жилы.
                        //var rectRow = GetRectCable(canvasRow);
                        //// Вложенные жилы.
                        //SetRectanglesRow(canvasPrint, canvasRow, row, count);
                        //// Результат.
                        //canvasPrint.Children.Add(rectRow);
                    }
                    // Метки названий жил.
                    SetLabelItem(canvasPrint, row, top);
                }

                // Колонка без жил.
                if (_cable.CalcMaxSum % allWidth > 0 || allWidth < _cable.CalcMaxSum || (int)allWidth == 0)
                {
                    // Прямоугольник остатка жилы.
                    SetRectReminder(canvasPrint, rectCable, left, _cable.CalcMaxSum - allWidth, top);
                }

                // Смещение вниз.
                top = _shiftTop + _heightCable * (row + 1);
            }
        }

        #endregion

        #region Private methods - Rectangles

        /// <summary>
        /// Кабельная жила.
        /// </summary>
        /// <param name="canvasPrint"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        private Rectangle GetRectCable(Panel canvasPrint, double width, double height, double top)
        {
            var rectCable = new Rectangle()
            {
                Fill = new SolidColorBrush(_colorFillRectCable),
                Stroke = new SolidColorBrush(_colorStrokeRectCable),
                StrokeThickness = 1,
                Width = width,
                Height = height,
            };
            Canvas.SetLeft(rectCable, _shiftLeft);
            Canvas.SetTop(rectCable, top);
            Panel.SetZIndex(rectCable, _priorityRectangle);
            canvasPrint.Children.Add(rectCable);
            return rectCable;
        }

        /// <summary>
        /// Отрезок кабельной жилы.
        /// </summary>
        /// <param name="canvasPrint"></param>
        /// <param name="rectCable"></param>
        /// <param name="left"></param>
        /// <param name="itemWidth"></param>
        /// <param name="top"></param>
        /// <param name="isOut"></param>
        /// <returns></returns>
        private Rectangle GetRectItem(Panel canvasPrint, FrameworkElement rectCable, double left, double itemWidth, double top, bool isOut)
        {
            var width = itemWidth * rectCable.Width / _calcMaxLength;
            if (width > 0)
            {
                _heightItem = rectCable.Height / (_cable.RowsCount - 0.5);
                if (_heightItem < _rectItemMinHeight) _heightItem = _rectItemMinHeight;
                if (_heightItem > _rectItemMaxHeight) _heightItem = _rectItemMaxHeight;
                {
                    var rectItem = new Rectangle()
                    {
                        Fill = new SolidColorBrush(isOut ? _colorFillRectOut : _colorFillRectItem),
                        Stroke = new SolidColorBrush(_colorStrokeRectItem),
                        StrokeThickness = 1,
                        Width = width,
                        Height = _heightItem,
                    };
                    Canvas.SetLeft(rectItem, left);
                    //Canvas.SetTop(rectItem, top + rectCable.Height - rectItem.Height * 1.75);
                    Canvas.SetTop(rectItem, top + _heightCable - _heightItem * 1.75);
                    Panel.SetZIndex(rectItem, _priorityRectangle);
                    canvasPrint.Children.Add(rectItem);
                    return rectItem;
                }
            }
            return new Rectangle();
        }

        /// <summary>
        /// Прямоугольник остатка жилы.
        /// </summary>
        /// <param name="canvasPrint"></param>
        /// <param name="rectCable"></param>
        /// <param name="left"></param>
        /// <param name="itemWidth"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        private void SetRectReminder(Panel canvasPrint, FrameworkElement rectCable, double left, double itemWidth, double top)
        {
            // itemWidth * (canvasPrint.Width - _shiftLeft) / _calcMaxLength
            var width = itemWidth * rectCable.Width / _calcMaxLength;
            if (width > 0)
            {
                var height = rectCable.Height;
                var rectRowReminder = new Rectangle()
                {
                    Fill = new SolidColorBrush(_colorFillRectReminder),
                    Stroke = new SolidColorBrush(_colorStrokeRectReminder),
                    StrokeThickness = 1,
                    Width = width,
                    Height = height,
                };
                Canvas.SetLeft(rectRowReminder, left);
                Canvas.SetTop(rectRowReminder, top);
                Panel.SetZIndex(rectRowReminder, _priorityRectangle);
                canvasPrint.Children.Add(rectRowReminder);
            }
        }

        #endregion

        #region Private methods - Lines

        /// <summary>
        /// Линия вниз. Начало жилы.
        /// </summary>
        /// <param name="canvasPrint"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        private static void SetLineDownStart(Panel canvasPrint, double y1, double y2)
        {
            var x = _shiftLeft;
            var lineStart = new Line()
            {
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 1,
                X1 = x,
                Y1 = y1,
                X2 = x,
                Y2 = y2,
            };
            Panel.SetZIndex(lineStart, _priorityLine);
            canvasPrint.Children.Add(lineStart);
        }

        /// <summary>
        /// Линия вниз. Конец жилы.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        /// <param name="canvasPrint"></param>
        private static void SetLineDownEnd(Panel canvasPrint, double x, double y1, double y2)
        {
            var lineEnd = new Line()
            {
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 1,
                X1 = x,
                Y1 = y1,
                X2 = x,
                Y2 = y2,
            };
            Panel.SetZIndex(lineEnd, _priorityLine);
            canvasPrint.Children.Add(lineEnd);
        }

        /// <summary>
        /// Линии вниз.
        /// </summary>
        /// <param name="canvasPrint"></param>
        /// <param name="rectCable"></param>
        /// <param name="rectItem"></param>
        /// <param name="firstLine"></param>
        /// <param name="row"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        private static void SetLines(Panel canvasPrint, FrameworkElement rectCable, FrameworkElement rectItem, 
            ref bool firstLine, int row, double left, double top)
        {
            //double y1 = height - (height / (_cable.RowsCount + 0.5)) * 1.5;
            double y1 = top + rectCable.Height - rectItem.Height * 1.75 - 2;
            //double y2 = canvasPrint.Height + _shiftTop * 1.00 - (canvasPrint.Height - _shiftTop) / count;
            double y2 = canvasPrint.Height - 10;
            // Первая жила.
            if (firstLine)
            {
                firstLine = false;
                // Первая строка.
                if (row == 0)
                {
                    // Начало жилы.
                    SetLineDownStart(canvasPrint, y1, y2);
                }
                // Конец жилы.
                SetLineDownEnd(canvasPrint, (int)(left - 2), y1, y2);
            }
            // Последующие жилы.
            else
            {
                // Конец жилы.
                SetLineDownEnd(canvasPrint, (int)(left - 2), y1, y2);
            }
        }

        #endregion

        #region Private methods - Ellipses

        /// <summary>
        /// Кабельная жила. Начало жилы.
        /// </summary>
        /// <param name="canvasPrint"></param>
        /// <param name="len"></param>
        /// <param name="top"></param>
        private void SetEllipseRowStart(Panel canvasPrint, double len, double top)
        {
            var ellipseStart = new Ellipse()
            {
                Fill = new SolidColorBrush(_colorFillEllipse),
                Stroke = new SolidColorBrush(_colorStrokeEllipse),
                StrokeThickness = 1,
                Width = len,
                Height = len,
            };
            Canvas.SetLeft(ellipseStart, _shiftLeft - ellipseStart.Width / 2);
            Canvas.SetTop(ellipseStart, top);
            Panel.SetZIndex(ellipseStart, _priorityEllipse);
            canvasPrint.Children.Add(ellipseStart);
        }

        /// <summary>
        /// Кабельная жила. Конец жилы.
        /// </summary>
        /// <param name="canvasPrint"></param>
        /// <param name="len"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        private void SetEllipseRowEnd(Panel canvasPrint, double len, double left, double top)
        {
            var ellipseEnd = new Ellipse()
            {
                Fill = new SolidColorBrush(_colorFillEllipse),
                Stroke = new SolidColorBrush(_colorStrokeEllipse),
                StrokeThickness = 1,
                Width = len,
                Height = len,
            };
            Canvas.SetLeft(ellipseEnd, left - ellipseEnd.Width / 2);
            Canvas.SetTop(ellipseEnd, top);
            Panel.SetZIndex(ellipseEnd, _priorityEllipse);
            canvasPrint.Children.Add(ellipseEnd);
        }

        /// <summary>
        /// Кабельная жила.
        /// </summary>
        /// <param name="canvasPrint"></param>
        /// <param name="rectCable"></param>
        /// <param name="rectItem"></param>
        /// <param name="left"></param>
        /// <param name="i"></param>
        /// <param name="top"></param>
        private void SetEllipses(Panel canvasPrint, FrameworkElement rectCable, FrameworkElement rectItem, 
            double left, int i, double top)
        {
            var len = rectItem.Height + 4;
            top = top + rectCable.Height - rectItem.Height * 1.75 - 2;
            left -= 2;
            // Первая жила.
            if (i == 0)
            {
                // Начало жилы.
                SetEllipseRowStart(canvasPrint, len, top);
                // Конец жилы.
                SetEllipseRowEnd(canvasPrint, len, left, top);
            }
            // Последующие жилы.
            else
            {
                // Конец жилы.
                SetEllipseRowEnd(canvasPrint, len, left, top);
            }
        }

        #endregion

        #region Private methods - Labels

        /// <summary>
        /// Метка заголовка.
        /// </summary>
        /// <param name="canvasPrint"></param>
        /// <param name="header"></param>
        private void SetLabelHeader(Panel canvasPrint, string header)
        {
            var labelHeader = new Label()
            {
                FontSize = FontSize + 2,
                FontWeight = FontWeights.Bold,
                Content = header,
            };
            Canvas.SetLeft(labelHeader, 0);
            Canvas.SetTop(labelHeader, -10);
            labelHeader.Width = canvasPrint.Width;
            labelHeader.Height = _shiftTop;
            labelHeader.HorizontalContentAlignment = HorizontalAlignment.Center;
            labelHeader.VerticalContentAlignment = VerticalAlignment.Center;
            Panel.SetZIndex(labelHeader, _priorityLabel);
            canvasPrint.Children.Add(labelHeader);
            // Дата.
            var labelHeaderDt = new Label()
            {
                FontSize = FontSize + 2,
                FontWeight = FontWeights.Bold,
                Content = $"Дата: {DateTime.Today.Day:00}-{DateTime.Today.Month:00}-{DateTime.Today.Year}.  ",
            };
            Canvas.SetLeft(labelHeaderDt, 0);
            Canvas.SetTop(labelHeaderDt, -10);
            labelHeaderDt.Width = canvasPrint.Width;
            labelHeaderDt.Height = _shiftTop;
            labelHeaderDt.HorizontalContentAlignment = HorizontalAlignment.Right;
            labelHeaderDt.VerticalContentAlignment = VerticalAlignment.Center;
            Panel.SetZIndex(labelHeaderDt, _priorityLabel);
            canvasPrint.Children.Add(labelHeaderDt);
        }

        /// <summary>
        /// Метки названий жил.
        /// </summary>
        /// <param name="canvasPrint"></param>
        /// <param name="row"></param>
        /// <param name="top"></param>
        private void SetLabelItem(Panel canvasPrint, int row, double top)
        {
            var text = string.Empty;
            if (row < _cable.RowsCount)
                text = $"{row + 1} жила";
            else if (row == _cable.RowsCount)
                text = "Кабель";
            else if (row == _cable.RowsCount + 1)
                text = "Барабаны";

            var labelRow = new Label()
            {
                FontSize = FontSize,
                Width = _shiftLeft - 18,
                //Height = _heightCable,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                //VerticalContentAlignment = VerticalAlignment.Center,
            };
            if (row < _cable.RowsCount)
                labelRow.Content = text + $" ({_cable.GetWidthAll(row)})";
            else if (row == _cable.RowsCount)
                labelRow.Content = text + $" ({_cable.CalcMinSum})";
            else if (row == _cable.RowsCount + 1)
                labelRow.Content = text + $" ({_cable.CalcMaxSum})";

            Canvas.SetLeft(labelRow, _canvasBorderWidth * 0.5);
            Canvas.SetTop(labelRow, GetLabelTop(top));
            Panel.SetZIndex(labelRow, _priorityLabel);
            canvasPrint.Children.Add(labelRow);
        }

        /// <summary>
        /// Метки длин отрезков.
        /// </summary>
        /// <param name="canvasPrint"></param>
        /// <param name="rectItem"></param>
        /// <param name="itemWidth"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        private void SetLabelLength(Panel canvasPrint, FrameworkElement rectItem, int itemWidth, double left, double top)
        {
            var labelRow = new Label()
            {
                FontSize = FontSize,
                Width = rectItem.Width < _labelItemMinHeight ? _labelItemMinHeight : rectItem.Width,
                //Height = _heightCable,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                //VerticalContentAlignment = VerticalAlignment.Center,
                Content = itemWidth.ToString(),
            };
            Canvas.SetLeft(labelRow, left - rectItem.Width - rectItem.Height * 0.35);
            Canvas.SetTop(labelRow, GetLabelTop(top));
            Panel.SetZIndex(labelRow, _priorityLabel);
            canvasPrint.Children.Add(labelRow);
        }

        /// <summary>
        /// Высота текста.
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        private double GetLabelTop(double top)
        {
            return top + _heightCable - _heightItem * 3.55; // 1.75
            //return _shiftTop + row * ((canvasPrint.Height - _shiftTop) / count);
        }

        #endregion
    }
}
