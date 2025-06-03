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
    /// Логика взаимодействия для CardsView.xaml
    /// </summary>
    public partial class CardsView : Window
    {
        DBEntits db = new DBEntits();

        public CardsView()
        {
            InitializeComponent();
            LoadCards();
        }

        private void LoadCards()
        {
            // Загружаем карточки с дополнительным полем для количества пользователей
            DataB.ItemsSource = db.Cards.Select(c => new
            {
                c.id,
                c.name,
                c.img,
                UsersCount = c.Users1.Count,
                // Сохраняем оригинальный объект для доступа к нему при выборе
                OriginalCard = c
            }).ToList();
        }

        private void DataB_Loaded(object sender, RoutedEventArgs e)
        {
            // Дополнительная настройка DataGrid при загрузке, если необходимо
        }

        private void DataB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Обработка выбора карточки, если необходимо
        }

        private void usersWind_Click(object sender, RoutedEventArgs e)
        {
            // Переход к окну пользователей
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void addCard_Click(object sender, RoutedEventArgs e)
        {
            AddCardDialog dlg = new AddCardDialog();
            if (dlg.ShowDialog() == true)
            {
                LoadCards(); // Обновляем список карточек
            }
        }

        private void updCard_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную карточку
            if (DataB.SelectedItem == null)
            {
                MessageBox.Show("Выберите карточку для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Получаем оригинальный объект карточки из выбранного элемента
            Card selectedCard = (Card)((dynamic)DataB.SelectedItem).OriginalCard;

            UpdateCardDialog dlg = new UpdateCardDialog(selectedCard);
            if (dlg.ShowDialog() == true)
            {
                LoadCards(); // Обновляем список карточек
            }
        }

        private void delCard_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную карточку
            if (DataB.SelectedItem == null)
            {
                MessageBox.Show("Выберите карточку для удаления", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Получаем оригинальный объект карточки из выбранного элемента
            Card selectedCard = (Card)((dynamic)DataB.SelectedItem).OriginalCard;

            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить карточку {selectedCard.name}?", 
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Удаляем связи с пользователями
                    selectedCard.Users1.Clear();
                    
                    // Удаляем карточку
                    db.Cards.Remove(selectedCard);
                    db.SaveChanges();
                    
                    MessageBox.Show("Карточка успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCards(); // Обновляем список карточек
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении карточки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
