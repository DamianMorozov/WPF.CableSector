using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace CableSector.Views
{
    /// <summary>
    /// Окно "О программе".
    /// </summary>
    public partial class WindowAbout : Window
    {
        public WindowAbout()
        {
            InitializeComponent();
        }

        private void WindowAbout_OnLoaded(object sender, RoutedEventArgs e)
        {
            LabelVersion.Content = "Версия программы: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void UIElement_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
