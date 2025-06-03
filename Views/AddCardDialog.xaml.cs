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
    /// Логика взаимодействия для AddCardDialog.xaml
    /// </summary>
    public partial class AddCardDialog : Window
    {
        DBEntits db = new DBEntits();
        public Card NewCard { get; private set; }

        public AddCardDialog()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            // Загружаем список пользователей
            UsersListBox.ItemsSource = db.Users.ToList();
            UsersListBox.DisplayMemberPath = "name";
        }

        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameForm.Text))
            {
                MessageBox.Show("Введите название карточки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Создаем новую карточку
                Card newCard = new Card()
                {
                    name = NameForm.Text,
                    img = ImagePathForm.Text
                };

                // Добавляем выбранных пользователей
                foreach (User user in UsersListBox.SelectedItems)
                {
                    newCard.Users1.Add(user);
                }

                db.Cards.Add(newCard);
                db.SaveChanges();

                NewCard = newCard;
                MessageBox.Show($"Карточка {newCard.name} успешно добавлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении карточки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
