﻿using System;
using System.Windows;
using System.Windows.Input;

namespace HardwareShop.View
{
    /// <summary>
    /// Логика взаимодействия для CartOfOrders.xaml
    /// </summary>
    public partial class CartOfOrders : Window
    {
        public CartOfOrders()
        {
            InitializeComponent();
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

        private void btnBack_Click(object sender, RoutedEventArgs e)                            // Кнопка "Назад"
        {
            this.Close();
        }

        private void btnCatalog_Click(object sender, RoutedEventArgs e)                            // Кнопка "Назад"
        {
            CatalogView catalog = new CatalogView();
            this.Hide();
            catalog.ShowDialog();
            this.ShowDialog();
        }
    }
}
