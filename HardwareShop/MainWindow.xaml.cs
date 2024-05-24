using HardwareShop.View;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HardwareShop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.DB = new Entities.CompieceEntities();           // Подключение к базе данных
            Application.Current.MainWindow.MaxHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            Application.Current.MainWindow.MaxWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
        }

        /*========================================
                    Работа ToolBar
        ========================================*/

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
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                path += "Maximize_Off_Black.png";
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
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
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        /*========================================
              Кнопки переходов между окнами
        ========================================*/

        private void btnWindows_Click(object sender, RoutedEventArgs e)
        {
            Window window = null;
            string windowName = (string)(sender as Button).Name;        // Получение названия кнопки
            switch (windowName)                                         // Поиск кнопки и создание окна
            {
                case "btnCatalog": window = new CatalogView(); break;
                case "btnOrder": window = new CreateOrder(); break;
                case "btnUpdate": window = new Authorization(); break;
            }
            this.Hide();
            window.ShowDialog();
            this.ShowDialog();
        }

        private void getLocateWindow()
        {
            //WindowHelper.points = this.PointFromScreen(;
        }
    }
}
