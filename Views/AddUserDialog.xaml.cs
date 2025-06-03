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
    /// Логика взаимодействия для AddUserDialog.xaml
    /// </summary>
    public partial class AddUserDialog : Window
    {
        DBEntits db = new DBEntits();
        public User NewUser { get; private set; }

        public AddUserDialog()
        {
            InitializeComponent();
            LoadRoles();
        }

        private void LoadRoles()
        {
            // Загружаем список ролей в ComboBox
            RoleComboBox.ItemsSource = db.Roles.ToList();
            if (RoleComboBox.Items.Count > 0)
            {
                RoleComboBox.SelectedIndex = 0;
            }
        }

        private void AddUserFormButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameForm.Text))
            {
                MessageBox.Show("Введите имя пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите роль для пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Role selectedRole = RoleComboBox.SelectedItem as Role;
                
                User newUser = new User()
                {
                    name = NameForm.Text,
                    roleId = selectedRole.id,
                    Role = selectedRole
                };

                db.Users.Add(newUser);
                db.SaveChanges();

                NewUser = newUser;
                MessageBox.Show($"Пользователь {newUser.name} успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
