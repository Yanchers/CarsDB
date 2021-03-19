using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Windows;

namespace CourseProjectDataBaseCars
{
    public class DBLoginWindowViewModel : BaseViewModel
    {
        public DBLoginWindowViewModel()
        {
            AuthHelper.SetDefaultConnectionString();

            LoginCommand = new RelayCommand(Authorize);
            CancelCommand = new RelayCommand(Cancel);
        }

        public User User { get; set; } = new User();

        private void Authorize(object param)
        {
            var builder = new SqlConnectionStringBuilder(AuthHelper.ConnectionString)
            {
                UserID = User.UserId,
                Password = User.Password
            };

            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                try
                {
                    connection.Open();
                    connection.Close();
                }
                catch (Exception e)
                {
                    connection.Close();
                    MessageBox.Show(e.Message, "Ошибка подключения");
                    return;
                }
            }

            AuthHelper.ChangeConnectionString(builder.ConnectionString);

            ApplicationViewModel.Instance.CurrentPage = PageTypes.Catalog;
            if (!ApplicationViewModel.Instance.HasMainWindow)
            {
                var window = new MainWindow();
                window.Show();
                ApplicationViewModel.Instance.HasMainWindow = true;
            }

            (param as Window).Close();
        }
        private void Cancel(object param)
        {
            (param as Window).Close();
        }

        public RelayCommand LoginCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
    }
}
