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
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private DBEntits db = new DBEntits();
        
        public User RegisteredUser { get; private set; }

        public RegisterWindow()
        {
            InitializeComponent();
            LoadRoles();
        }

        private void LoadRoles()
        {
            // Загружаем список ролей в ComboBox
            RoleComboBox.ItemsSource = db.Roles.ToList();
            
            // По умолчанию выбираем первую роль, если она есть
            if (RoleComboBox.Items.Count > 0)
            {
                RoleComboBox.SelectedIndex = 0;
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;
            Role selectedRole = RoleComboBox.SelectedItem as Role;

            // Проверка введенных данных
            if (string.IsNullOrEmpty(username))
            {
                ShowError("Введите имя пользователя");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowError("Введите пароль");
                return;
            }

            if (selectedRole == null)
            {
                ShowError("Выберите роль");
                return;
            }

            // Проверяем, существует ли пользователь с таким именем
            if (db.Users.Any(u => u.name == username))
            {
                ShowError("Пользователь с таким именем уже существует");
                return;
            }

            try
            {
                // Создаем нового пользователя
                User newUser = new User
                {
                    name = username,
                    roleId = selectedRole.id,
                    Role = selectedRole
                    // В реальном приложении здесь должно быть хеширование пароля
                    // и сохранение хеша в соответствующее поле
                };

                // Добавляем пользователя в базу данных
                db.Users.Add(newUser);
                db.SaveChanges();

                RegisteredUser = newUser;
                MessageBox.Show($"Пользователь {username} успешно зарегистрирован", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Возвращаем true для указания успешной регистрации
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при регистрации: {ex.Message}");
            }
        }

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем окно регистрации и возвращаемся к окну входа
            this.DialogResult = false;
            this.Close();
        }

        private void ShowError(string message)
        {
            ErrorTextBlock.Text = message;
            ErrorTextBlock.Visibility = Visibility.Visible;
        }
    }
}
