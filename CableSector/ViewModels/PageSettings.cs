// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CableSector.ViewModels
{
    /// <summary>
    /// Настройки окна.
    /// </summary>
    public class PageSettings : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        #endregion

        #region Constructor

        public PageSettings()
        {
            // Ширина.
            MaxWidth = 800;
            MinWidth = 800;
            Width = 800;
            // Высота.
            MaxHeight = 600;
            MinHeight = 600;
            Height = 600;
            // Размер шрифта.
            FontSize = 18;
        }

        public PageSettings(double width, double height) : this()
        {
            // Ширина.
            MaxWidth = width;
            MinWidth = width;
            Width = width;
            // Высота.
            MaxHeight = height;
            MinHeight = height;
            Height = height;
            // Размер шрифта.
            FontSize = 14;
        }

        public PageSettings(double width, double height, double fontSize = 20) : this(width, height)
        {
            // Размер шрифта.
            FontSize = fontSize;
        }

        #endregion

        #region Public fields and properties

        private double _width;
        /// <summary>
        /// Ширина окна.
        /// </summary>
        public double Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyRaised();
            }
        }

        private double _minWidth;
        /// <summary>
        /// Минимальная ширина окна.
        /// </summary>
        public double MinWidth
        {
            get => _minWidth;
            set
            {
                _minWidth = value;
                OnPropertyRaised();
            }
        }

        private double _maxWidth;
        /// <summary>
        /// Максимальная ширина окна.
        /// </summary>
        public double MaxWidth
        {
            get => _maxWidth;
            set
            {
                _maxWidth = value;
                OnPropertyRaised();
            }
        }

        private double _height;
        /// <summary>
        /// Высота окна.
        /// </summary>
        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyRaised();
            }
        }

        private double _minHeight;
        /// <summary>
        /// Минимальная высота окна.
        /// </summary>
        public double MinHeight
        {
            get => _minHeight;
            set
            {
                _minHeight = value;
                OnPropertyRaised();
            }
        }

        private double _maxHeight;
        /// <summary>
        /// Максимальная высота окна.
        /// </summary>
        public double MaxHeight
        {
            get => _maxHeight;
            set
            {
                _maxHeight = value;
                OnPropertyRaised();
            }
        }

        private double _fontSize;
        /// <summary>
        /// Размер шрифта.
        /// </summary>
        public double FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                OnPropertyRaised();
            }
        }

        #endregion
    }
}
