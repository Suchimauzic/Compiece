using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HardwareShop.View
{
    /// <summary>
    /// Логика взаимодействия для CreateOrder.xaml
    /// </summary>
    public partial class CreateOrder : Window
    {
        // Глобальные переменные
        public double orderCost { get; set; }
        string pathPhoto = Environment.CurrentDirectory + "\\Resources\\Images\\";      // Путь к папке с картинками
        public List<Classes.ComponentInOrder> listComponentsInOrder;             // Список копмлектующих в составе заказа


        public CreateOrder()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Отображение списка категорий

            List<Entities.Category> categories = App.DB.Category.ToList();

            // Создание категории "Все категории"
            Entities.Category category = new Entities.Category();
            category.CategId = 0;
            category.CategName = "Все категории";
            categories.Insert(0, category);

            // Отображение категорий в ListBox
            lbCategory.ItemsSource = categories;
            lbCategory.DisplayMemberPath = "CategName";
            lbCategory.SelectedValuePath = "CategId";
            lbCategory.SelectedIndex = 0;

            // Отображение пустого списка комплектующих
            if (listComponentsInOrder == null)
                listComponentsInOrder = new List<Classes.ComponentInOrder>();
            orderCost = 0;
            textPlaceAnOrder.Text = "Сумма заказа: " + orderCost;
            ShowComponents();
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

            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        /*========================================
               Работа окна оформления заказа
        ========================================*/

        private void btnBack_Click(object sender, RoutedEventArgs e)                            // Кнопка "Назад"
        {
            this.Close();
        }

        private void ShowComponents()
        {
            using (Entities.CompieceEntities DBTemp = new Entities.CompieceEntities())
            {
                List<Entities.Component> components = DBTemp.Component.ToList();

                // Фильтрация по категории
                if (lbCategory.SelectedValue != null && (int)lbCategory.SelectedValue > 0)
                {
                    components = components.Where(c => c.CategId == (int)lbCategory.SelectedValue).ToList();
                }

                string photo = "";
                
                foreach (Entities.Component component in components)
                {
                    //Вставка картинки по условию
                    if (String.IsNullOrEmpty(component.PictPath))
                    {
                        photo = pathPhoto + "Unknow.jpg";
                    }
                    else
                    {
                        photo = pathPhoto + component.PictPath;

                        if (!File.Exists(photo))
                            photo = pathPhoto + "Unknow.jpg";
                    }

                    component.PictPath = photo;
                }

                lbCatalog.ItemsSource = components;     // Вывод Компонентов
            }
        }

        private void btnInOrder_Click(object sender, RoutedEventArgs e)
        {
            Entities.Component componentSelect = (sender as Button).DataContext as Entities.Component;
            Classes.ComponentInOrder componentInOrder;

            if (componentSelect != null)
            {
                int index = listComponentsInOrder.FindIndex(x => x.ComponentName == componentSelect.ComName);

                if (index < 0)
                {
                    componentInOrder = new Classes.ComponentInOrder();
                    componentInOrder.ComponentName = componentSelect.ComName;
                    componentInOrder.ComponentCount = 1;
                    componentInOrder.ComponentCost = (double)componentSelect.Price;
                    listComponentsInOrder.Add(componentInOrder);
                }
                else
                {
                    componentInOrder = listComponentsInOrder[index];
                    componentInOrder.ComponentCost++;
                }

                componentInOrder.ComponentTotalCost = componentInOrder.ComponentCost * componentInOrder.ComponentCount;

                componentInOrder.ComponentPictPath = componentSelect.PictPath;

                orderCost += componentInOrder.ComponentCost;
                textPlaceAnOrder.Text = "Сумма заказа: " + orderCost;
            }
        }

        private void lbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowComponents();
        }

        private void btnPlaceAnOrder_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            CartOfOrders coo = new CartOfOrders();
            coo.Owner = this;
            coo.ShowDialog();
            this.Close();
        }
    }
}
