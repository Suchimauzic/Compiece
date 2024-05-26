using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HardwareShop.View
{
    /// <summary>
    /// Логика взаимодействия для CartOfOrders.xaml
    /// </summary>
    public partial class CartOfOrders : Window
    {
        CreateOrder createOrder;
        List<Classes.ComponentInOrder> listComponentsInOrder;
        double orderCost;

        public CartOfOrders()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            createOrder = this.Owner as CreateOrder;
            listComponentsInOrder = createOrder.listComponentsInOrder;
            ShowOrder();
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
               Работа окна корзины заказов
        ========================================*/

        private void btnBack_Click(object sender, RoutedEventArgs e)                            // Кнопка "Назад"
        {
            this.Close();
        }

        private void ShowOrder()
        {
            dgOrder.ItemsSource = null;
            dgOrder.ItemsSource = listComponentsInOrder;
            orderCost = 0;
            foreach (Classes.ComponentInOrder comp in listComponentsInOrder)
            {
                orderCost += comp.ComponentTotalCost;
            }

            textOrder.Text = "Сумма заказа: " + Math.Round(orderCost, 2);
        }

        private void btnAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Classes.ComponentInOrder selectComponentInOrder = btn.DataContext as Classes.ComponentInOrder;

            switch (btn.Content)
            {
                case "+":
                    selectComponentInOrder.ComponentCount++;
                    break;
                case "-":
                    selectComponentInOrder.ComponentCount--;
                    break;
                case "x":
                    listComponentsInOrder.Remove(selectComponentInOrder);
                    break;
            }

            if (selectComponentInOrder.ComponentCount <= 0)
            {
                listComponentsInOrder.Remove(selectComponentInOrder);
            }
            else
            {
                selectComponentInOrder.ComponentTotalCost = selectComponentInOrder.ComponentCount * selectComponentInOrder.ComponentCost;
            }

            ShowOrder();
        }
    }
}
