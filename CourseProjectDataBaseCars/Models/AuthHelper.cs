using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CourseProjectDataBaseCars
{
    public static class AuthHelper
    {
        public static User CurrentUser { get; set; }
        public static string ConnectionString { get; private set; }

        public static void SetDefaultConnectionString()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");

            ConnectionString = builder.Build().GetConnectionString("UserConnection");
        }
        public static void ChangeConnectionString(string conStr)
        {
            ConnectionString = conStr;

            string jsonStr = File.ReadAllText("appsettings.json");

            var obj = JsonConvert.DeserializeObject(jsonStr) as JObject;
            var token = obj.SelectToken("ConnectionStrings").SelectToken("UserConnection");

            token.Replace(ConnectionString);

            File.WriteAllText("appsettings.json", obj.ToString());
        }
    }
}
