// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.IO;
using System.Windows;
using CableSector.ViewModels;

namespace CableSector.Views
{
    /// <summary>
    /// Страница истории версий.
    /// </summary>
    public partial class PageChangeLog
    {
        #region Private fields and properties

        /// <summary>
        /// Программные настройки.
        /// </summary>
        private readonly ProgramSettings _settings;

        #endregion

        #region Private methods - Page

        public PageChangeLog()
        {
            InitializeComponent();

            _settings = null;
            var context = FindResource("ViewModelProgramSettings");
            if (context is ProgramSettings settings)
            {
                _settings = settings;
            }

            LoadChangeLog();
        }

        /// <summary>
        /// Загрузить файл истории.
        /// </summary>
        private void LoadChangeLog()
        {
            var sr = new StreamReader("CHANGELOG.md");
            TextBlockMain.Text = sr.ReadToEnd();
            sr.Dispose();
        }

        private void PageChangeLog_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Загрузка страницы предварительного просмотра завершена.
            _settings.PageChangeLogIsLoaded = true;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Обновить страницу.
        /// </summary>
        /// <returns></returns>
        public void Refresh()
        {
            if (_settings != null)
            {
                
            }
        }

        #endregion
    }
}
