using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WSDF.Views;

namespace WSDF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Свойство для хранения текущего авторизованного пользователя
        public static User CurrentUser { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Показываем окно авторизации
            LoginWindow loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                // Если авторизация успешна, сохраняем пользователя и показываем главное окно
                CurrentUser = loginWindow.AuthenticatedUser;
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                // Если авторизация не удалась, закрываем приложение
                this.Shutdown();
            }
        }
    }
}
