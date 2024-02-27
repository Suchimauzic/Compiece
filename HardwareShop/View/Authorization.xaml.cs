using HardwareShop.Classes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для Authentication.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        private int tryCount = 0;
        public Authorization()
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

        private void btnAurotiz_Click(object sender, RoutedEventArgs e)
        {
            string login, pass;
            Entities.Authoriz userSearch = null;

            if (!string.IsNullOrEmpty(tbLogin.Text) && !string.IsNullOrEmpty(tbPass.Password))
            {
                login = tbLogin.Text;
                pass = tbPass.Password;

                try                 // Попытка подключения к базе и получения логина и пароля
                {
                    userSearch = App.DB.Authoriz.Where(u => u.UserLogin == login && u.UserPassword == pass).ToList().FirstOrDefault();
                }
                catch
                {
                    MessageBox.Show("Не удалось подключится к базе!", "Ошибка подключения!");
                    return;
                }

                if (userSearch != null)             // Есть ли такой пользователь
                {
                    MessageBox.Show("Вы успешно зашли!");
                    UserHelper.login = login;
                    UserHelper.password = pass;
                    new View.CatalogView().ShowDialog();
                    this.Close();
                }
                else
                {
                    if (++tryCount >= 3)
                    {
                        tryCount = 0;
                        MessageBox.Show("Вы 3 раза неверно вошли, поэтому система заблокирована на 10 сек", "Ошибка входа");
                        Thread.Sleep(10000);
                    }
                    else
                    {
                        MessageBox.Show("Неверно были введены логин или/и пароль", "Ошибка входа");
                    }
                }

                /* =========================== Other methods to find a user   ================================== */

                //string sql = "SELECT * FROM [Authoriz] WHERE [Authoriz].UserLogin = '" + login + "' AND [Authoriz].UserPassword = '" + pass + "'";
                //Entities.Authoriz userSearch = App.DB.Authoriz.SqlQuery(sql).ToList().FirstOrDefault();

                //List<Entities.Authoriz> users = (List<Entities.Authoriz>)from u in App.DB.Authoriz
                //                                                         where u.UserLogin == login && u.UserPassword == pass
                //                                                         select u;
                //Entities.Authoriz user = users.FirstOrDefault();

                /* =============================================================================================== */
            }
            else
            {
                MessageBox.Show("Не все поля были введены коррекитно");
            }
        }
    }
}
