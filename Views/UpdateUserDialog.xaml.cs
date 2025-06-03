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
    /// Логика взаимодействия для UpdateUserDialog.xaml
    /// </summary>
    public partial class UpdateUserDialog : Window
    {
        DBEntits db = new DBEntits();
        private User _currentUser;
        public User UpdatedUser { get; private set; }

        public UpdateUserDialog(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadRoles();
            FillUserData();
        }

        private void LoadRoles()
        {
            // Загружаем список ролей в ComboBox
            RoleComboBox.ItemsSource = db.Roles.ToList();
        }

        private void FillUserData()
        {
            // Заполняем форму данными текущего пользователя
            NameForm.Text = _currentUser.name;
            
            // Выбираем текущую роль пользователя в ComboBox
            if (_currentUser.roleId.HasValue)
            {
                foreach (Role role in RoleComboBox.Items)
                {
                    if (role.id == _currentUser.roleId.Value)
                    {
                        RoleComboBox.SelectedItem = role;
                        break;
                    }
                }
            }
        }

        private void UpdateUserFormButton_Click(object sender, RoutedEventArgs e)
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
                
                // Получаем свежую копию пользователя из базы данных
                User userToUpdate = db.Users.Find(_currentUser.id);
                if (userToUpdate != null)
                {
                    // Обновляем данные пользователя
                    userToUpdate.name = NameForm.Text;
                    userToUpdate.roleId = selectedRole.id;
                    
                    // Сохраняем изменения
                    db.SaveChanges();
                    
                    UpdatedUser = userToUpdate;
                    MessageBox.Show($"Пользователь {userToUpdate.name} успешно обновлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Пользователь не найден в базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
