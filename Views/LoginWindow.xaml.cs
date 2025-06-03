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

namespace WSDF.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private DBEntits db = new DBEntits();
        
        public User AuthenticatedUser { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Введите имя пользователя и пароль");
                return;
            }

            // Проверяем учетные данные
            User user = db.Users.FirstOrDefault(u => u.name == username);

            if (user == null)
            {
                ShowError("Пользователь не найден");
                return;
            }

            // В реальном приложении здесь должна быть проверка хеша пароля
            // Для простоты предположим, что пароль - это "password"
            if (password != "password")
            {
                ShowError("Неверный пароль");
                return;
            }

            // Авторизация успешна
            AuthenticatedUser = user;
            this.DialogResult = true;
            this.Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Открываем окно регистрации
            RegisterWindow registerWindow = new RegisterWindow();
            
            // Скрываем окно авторизации
            this.Hide();
            
            // Показываем окно регистрации
            bool? result = registerWindow.ShowDialog();
            
            // Если регистрация успешна, используем зарегистрированного пользователя для входа
            if (result == true && registerWindow.RegisteredUser != null)
            {
                AuthenticatedUser = registerWindow.RegisteredUser;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                // Если регистрация отменена, показываем окно авторизации снова
                this.Show();
            }
        }

        private void ShowError(string message)
        {
            ErrorTextBlock.Text = message;
            ErrorTextBlock.Visibility = Visibility.Visible;
        }
    }
}
