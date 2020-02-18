// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// *************************************************************************************************
// Расчёт многожильного кабеля
// *************************************************************************************************

using System.ComponentModel;
using System.Windows;
using CableSector.Models;
using CableSector.ViewModels;

namespace CableSector.Views
{
    /// <summary>
    /// Главное окно.
    /// </summary>
    public partial class WindowMain
    {
        #region Public fields and properties

        /// <summary>
        /// Программные настройки.
        /// </summary>
        private readonly ProgramSettings _settings;
        /// <summary>
        /// Таймер.
        /// </summary>
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(1000) { Enabled = false };
        /// <summary>
        /// Страница редактора.
        /// </summary>
        private PageEditor _pageEditor;
        /// <summary>
        /// Страница истории версий.
        /// </summary>
        private PageChangeLog _pageChangeLog;
        /// <summary>
        /// Помощник XAML.
        /// </summary>
        private readonly XamlHelper _xamlHelper = XamlHelper.Instance;

        #endregion

        #region Private methods - Window

        /// <summary>
        /// Главное окно.
        /// </summary>
        public WindowMain()
        {
            InitializeComponent();

            _xamlHelper.CenterWindowOnScreen(this);
            _timer.Elapsed += ResizingDone;
            
            var context = FindResource("ViewModelProgramSettings");
            if (context is ProgramSettings settings)
            {
                _settings = settings;
            }
        }

        private void ResizingDone(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();
            if (Dispatcher != null && !Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() =>
                {
                    // Обновить.
                    ButtonCanvas_Click(sender, null);
                });
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _settings.Page.Width = _settings.Page.MinWidth = _settings.Page.MaxWidth =
                Width = 1000;
            _settings.Page.Height = _settings.Page.MinHeight = _settings.Page.MaxHeight =
                Height = 800;
            _settings.Page.FontSize = FontSize = 18;
            // Загрузка главного окна завершена.
            _settings.WindowMainIsLoaded = true;
            // По-умолчанию.
            ButtonDefault_Click(sender, e);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize != new Size())
            {
                _timer.Stop();
                _timer.Start();
            }
        }

        private void Window_OnClosing(object sender, CancelEventArgs e)
        {
            _timer.Dispose();
        }
        
        #endregion

        #region Private methods - Buttons

        /// <summary>
        /// Страница редактора.
        /// </summary>
        private void FrameMainSwitchEditor()
        {
            if (_pageEditor == null)
                _pageEditor = new PageEditor();
            if (FrameMain.Content != null)
            {
                if (!(FrameMain.Content is PageEditor))
                    FrameMain.Navigate(_pageEditor);
            }
            else
                FrameMain.Navigate(_pageEditor);
            // Обновить страницу.
            _pageEditor.Refresh();
            //FrameMain.Refresh();
        }

        /// <summary>
        /// Страница истории версий.
        /// </summary>
        private void FrameMainSwitchChangeLog()
        {
            if (_pageChangeLog == null)
                _pageChangeLog = new PageChangeLog();
            if (FrameMain.Content != null)
            {
                if (!(FrameMain.Content is PageChangeLog))
                    FrameMain.Navigate(_pageChangeLog);
            }
            else
                FrameMain.Navigate(_pageChangeLog);
            // Обновить страницу.
            _pageChangeLog.Refresh();
            //FrameMain.Refresh();
        }

        /// <summary>
        /// По-умолчанию.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDefault_Click(object sender, RoutedEventArgs e)
        {
            _settings.Mode = WorkMode.Default;
            FrameMainSwitchEditor();
        }

        /// <summary>
        /// Обновить.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCanvas_Click(object sender, RoutedEventArgs e)
        {
            _settings.Mode = WorkMode.Canvas;
            FrameMainSwitchEditor();
        }

        /// <summary>
        /// Печать.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {
            _settings.Mode = WorkMode.Print;
        }

        /// <summary>
        /// История версий.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonChangeLog_OnClick(object sender, RoutedEventArgs e)
        {
            _settings.Mode = WorkMode.ChangeLog;
            FrameMainSwitchChangeLog();
        }
        
        /// <summary>
        /// О программе.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAbout_OnClick(object sender, RoutedEventArgs e)
        {
            var windowAbout = new WindowAbout();
            _xamlHelper.CenterWindowOnWindow(windowAbout, this);
            windowAbout.ShowDialog();
        }
        
        #endregion

    }
}
