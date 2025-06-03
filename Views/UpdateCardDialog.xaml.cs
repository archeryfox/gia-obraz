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
    /// Логика взаимодействия для UpdateCardDialog.xaml
    /// </summary>
    public partial class UpdateCardDialog : Window
    {
        DBEntits db = new DBEntits();
        private Card _currentCard;
        public Card UpdatedCard { get; private set; }

        public UpdateCardDialog(Card card)
        {
            InitializeComponent();
            _currentCard = card;
            LoadUsers();
            FillCardData();
        }

        private void LoadUsers()
        {
            // Загружаем список пользователей
            UsersListBox.ItemsSource = db.Users.ToList();
            UsersListBox.DisplayMemberPath = "name";
        }

        private void FillCardData()
        {
            // Заполняем форму данными текущей карточки
            NameForm.Text = _currentCard.name;
            ImagePathForm.Text = _currentCard.img;
            
            // Выбираем связанных пользователей в ListBox
            foreach (User user in _currentCard.Users1)
            {
                foreach (User listUser in UsersListBox.Items)
                {
                    if (user.id == listUser.id)
                    {
                        UsersListBox.SelectedItems.Add(listUser);
                    }
                }
            }
        }

        private void UpdateCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameForm.Text))
            {
                MessageBox.Show("Введите название карточки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Обновляем данные карточки
                _currentCard.name = NameForm.Text;
                _currentCard.img = ImagePathForm.Text;

                // Обновляем связи с пользователями
                _currentCard.Users1.Clear();
                foreach (User user in UsersListBox.SelectedItems)
                {
                    _currentCard.Users1.Add(user);
                }

                db.SaveChanges();

                UpdatedCard = _currentCard;
                MessageBox.Show($"Карточка {_currentCard.name} успешно обновлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении карточки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
