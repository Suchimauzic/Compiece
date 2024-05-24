using HardwareShop.Classes;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HardwareShop.View
{
    /// <summary>
    /// Логика взаимодействия для Authentication.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        private int tryCount = 0;
        private bool isWaiting = false;

        private string login, pass;

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

            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)                 // Кнопка "Назад"
        {
            this.Close();
        }

        private async void btnAurotiz_Click(object sender, RoutedEventArgs e)
        {
            login = tbLogin.Text;

            if (passCheck.IsChecked == true)
                pass = tbPassShow.Text;
            else
                pass = tbPass.Password;


            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(pass))
            {
                lockUnlockInterface();                 
                Thread threadAuthor = new Thread(authorizThread);       // Подключение к базе данных и получение аккаунта
                threadAuthor.Start();

                // Текст ожидания
                while (isWaiting)
                {
                    changeTextWaiting();
                    await Task.Delay(1000);
                }

                lockUnlockInterface();

                if (!string.IsNullOrEmpty(UserHelper.login) && !string.IsNullOrEmpty(UserHelper.password))
                {
                    CatalogView catalog = new CatalogView();
                    this.Hide();
                    catalog.ShowDialog();
                    UserHelper.login = UserHelper.password = string.Empty;
                    this.Close();
                }
            }
            else
            {
                login = pass = string.Empty;
                MessageBox.Show("Не все поля были введены коррекитно");
            }
        }

        private void changeTextWaiting()
        {
            if (textWaiting.Text.Contains("Ожидайте..."))
            {
                textWaiting.Text = "Идёт процесс подключения к базе. Ожидайте.";
            }
            else
            {
                textWaiting.Text += '.';
            }
        }

        private void authorizThread()
        {
            
            Entities.Authoriz userSearch = null;

            try                 // Попытка подключения к базе и получения логина и пароля
            {
                userSearch = App.DB.Authoriz.Where(u => u.UserLogin == login && u.UserPassword == pass).ToList().FirstOrDefault();
            }
            catch
            {
                isWaiting = false;
                MessageBox.Show("Не удалось подключится к базе!", "Ошибка подключения!");
                return;
            }

            if (userSearch != null)             // Есть ли такой пользователь
            {
                isWaiting = false;
                UserHelper.login = userSearch.UserLogin;
                UserHelper.password = userSearch.UserPassword;
                MessageBox.Show("Вы успешно зашли!");
            }
            else
            {
                isWaiting = false;
                if (++tryCount >= 3)
                {
                    tryCount = 0;
                    MessageBox.Show("Вы 3 раза неверно вошли, поэтому система заблокирована на 10 сек", "Ошибка входа");
                    Thread.Sleep(10000);
                }
                else
                {
                    MessageBox.Show("Неверно были введены логин или/и пароль. Попробуйте снова", "Ошибка входа");
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

        private void lockUnlockInterface()
        {
            if (btnBack.IsEnabled)
            {
                btnSignIn.Visibility = Visibility.Hidden;
                textWaiting.Visibility = Visibility.Visible;
                btnBack.IsEnabled = false;
                isWaiting = true;
            }
            else
            {
                btnSignIn.Visibility = Visibility.Visible;
                textWaiting.Visibility = Visibility.Hidden;
                btnBack.IsEnabled = true;
                isWaiting = false;
            }
        }

        private void passCheck_Checked(object sender, RoutedEventArgs e)
        {
            tbPassShow.Text = tbPass.Password;
            tbPass.Visibility = Visibility.Hidden;
            passCheck.Content = "Скрыть пароль";
        }

        private void passCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            tbPass.Password = tbPassShow.Text;
            tbPass.Visibility = Visibility.Visible;
            passCheck.Content = "Показать пароль";
        }
    }
}
