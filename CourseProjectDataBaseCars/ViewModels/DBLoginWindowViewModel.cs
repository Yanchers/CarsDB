using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CourseProjectDataBaseCars
{
    class DBLoginWindowViewModel : BaseViewModel
    {
        public DBLoginWindowViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        public string UserName { get; set; }
        public string Password { get; set; }

        private void Login(object param)
        {
            var jsonStr = File.ReadAllText("appsettings.json");
            var obj = JsonConvert.DeserializeObject(jsonStr) as JObject;
            var token = obj.SelectToken("UserConnection");

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(token.ToString());

            builder.UserID = UserName;
            builder.Password = Password;

            token.Replace(builder.ConnectionString);

            File.WriteAllText("appsettings.json", JsonConvert.SerializeObject(obj.ToString()));
        }

        public RelayCommand LoginCommand { get; private set; }
    }
}
