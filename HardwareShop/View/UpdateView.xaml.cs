using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HardwareShop.View
{
    /// <summary>
    /// Логика взаимодействия для UpdateView.xaml
    /// </summary>
    public partial class UpdateView : Window
    {
        public UpdateView()
        {
            InitializeComponent();
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);                // Закрытие программы
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            // Свёртывание окна

            string path = @"/Resources/";

            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                path += "Maximize_Off_Black.png";
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                path += "Maximize_On_Black.png";
            }

            //BitmapImage bitmap = new BitmapImage();
            //bitmap.BeginInit();
            //bitmap.UriSource = new Uri(path);
            //bitmap.EndInit();

            //imageMaximize.Source = bitmap;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)                 // Кнопка "Назад"
        {
            this.Close();
        }
    }
}
