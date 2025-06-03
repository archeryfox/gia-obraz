using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WSDF.Views;

namespace WSDF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Role model = new Role();
        User usermodel = new User();
        DBEntits db = new DBEntits();
        public MainWindow()
        {
            InitializeComponent();
            DataB.ItemsSource = db.Users.ToList();
        }

        private void addUser_Click(object sender, RoutedEventArgs e)
        {
            AddUserDialog dlg = new AddUserDialog();
            if (dlg.ShowDialog() == true)
            {
                Clear(); // Обновляем список пользователей
            }
        }

        private void DataB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем, что выбран элемент
            if (DataB.SelectedItem != null)
            {
                User user = (User)DataB.SelectedItem;
                // Здесь можно добавить логику для отображения деталей пользователя, если нужно
            }
        }

        private void Clear()
        {
            DataB.ItemsSource = db.Users.ToList();
            DataB.Columns[2].Visibility = Visibility.Collapsed;
        }

        private void DataB_Loaded(object sender, RoutedEventArgs e)
        {
            //DataB.Columns[2].Visibility = Visibility.Collapsed;
        }

        private void updUser_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = DataB.SelectedItem as User;
            if (selectedUser == null)
            {
                MessageBox.Show("Выберите пользователя для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            UpdateUserDialog dlg = new UpdateUserDialog(selectedUser);
            if (dlg.ShowDialog() == true)
            {
                Clear(); // Обновляем список пользователей
            }
        }

        private void delUser_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = DataB.SelectedItem as User;
            if (selectedUser == null)
            {
                MessageBox.Show("Выберите пользователя для удаления", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить пользователя {selectedUser.name}?", 
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Удаляем связи с карточками
                    selectedUser.Cards1.Clear();
                    
                    // Удаляем пользователя
                    db.Users.Remove(selectedUser);
                    db.SaveChanges();
                    
                    MessageBox.Show("Пользователь успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    Clear(); // Обновляем список пользователей
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void cardsWind_Click(object sender, RoutedEventArgs e)
        {
           CardsView cardsView = new CardsView();
            cardsView.Show();
        }
    }

}