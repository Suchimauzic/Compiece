using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HardwareShop.View
{
    /// <summary>
    /// Логика взаимодействия для CatalogView.xaml
    /// </summary>
    public partial class CatalogView : Window
    {
        private List<Entities.Component> components;
        private int allCount;
        private List<Entities.Category> categories;

        public CatalogView()
        {
            // Загрузка информации из базы
            components = App.DB.Component.ToList();
            allCount = components.Count;

            // Создание списка категорий для фильтрации
            categories = App.DB.Category.ToList();

            Entities.Category category = new Entities.Category();
            category.CategName = "Все категории";
            category.CategId = 0;
            categories.Insert(0, category);

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            dtCatalog.ItemsSource = components;

            // Поле по умолчанию для сортировки по цене
            cbSort.SelectedIndex = 0;

            // Присоединение категорий к фильтрации cbCateg
            cbCateg.ItemsSource = categories;
            cbCateg.DisplayMemberPath = "CategName";
            cbCateg.SelectedValuePath = "CategId";

            // Поле по умолчанию для категорий
            cbCateg.SelectedValue = 0;

            // Количество поиска
            textCountFields.Text = allCount.ToString() + "/" + allCount.ToString();
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
                    Работа каталога
        ========================================*/

        private void btnBack_Click(object sender, RoutedEventArgs e)                            // Кнопка "Назад"
        {
            this.Close();
        }


        /*========================================
                      Фильтрация
        ========================================*/

        // Реализация фильтрации

        private void SortingComponents()
        {
            // Получение всего списка
            components = App.DB.Component.ToList();

            // Для работы сортировщика по количеству (возрастание/убывание)
            if (cbSort.SelectedIndex == 1)
            {
                components = components.OrderBy(c => c.Price).ToList();
            }
            else if (cbSort.SelectedIndex == 2)
            {
                components = components.OrderByDescending(c => c.Price).ToList();
            }
            else if (cbCateg.SelectedValue != null && (int)cbCateg.SelectedValue > 0)
            {
                components = components.Where(compon => compon.CategId == (int)cbCateg.SelectedValue).ToList();
            }

            // Фильтрация по категории
            if (cbCateg.SelectedValue != null && (int)cbCateg.SelectedValue > 0)
            {
                components = components.Where(c => c.CategId == (int)cbCateg.SelectedValue).ToList();
            }

            // Фильтрация по введённому имени
            string search = ThingName.Text.ToLower();
            if (search.Length > 0)
            {
                components = components.Where(c => c.ComName.ToLower().Contains(search)).ToList();
            }

            int countFilter = components.Count;
            textCountFields.Text = countFilter.ToString() + "/" + allCount.ToString();

            dtCatalog.ItemsSource = components;
        }

        private void ThingName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SortingComponents();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortingComponents();
        }

        private void cbCateg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortingComponents();
        }
    }
}
