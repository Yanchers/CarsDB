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
        }

        public User User { get; set; } = new User();

        private void Authorize(object param)
        {
            ChangeConnectionString();
        }

        public RelayCommand LoginCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        private void ChangeConnectionString()
        {
            var builder = new SqlConnectionStringBuilder(AuthHelper.ConnectionString);
            builder.UserID = User.UserId;
            builder.Password = User.Password;

            AuthHelper.ConnectionString = builder.ConnectionString;

            using (var connection = new SqlConnection(AuthHelper.ConnectionString))
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
            string jsonStr = File.ReadAllText("appsettings.json");
            var obj = JsonConvert.DeserializeObject(jsonStr) as JObject;
            var token = obj.SelectToken("ConnectionStrings").SelectToken("UserConnection");
            token.Replace(AuthHelper.ConnectionString);
            File.WriteAllText("appsettings.json", obj.ToString());
        }
    }
}
