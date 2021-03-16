using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseProjectDataBaseCars
{
    public static class AuthHelper
    {
        public static string ConnectionString { get; set; }
        public static User CurrentUser { get; set; }

        public static void SetDefaultConnectionString()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");

            ConnectionString = builder.Build().GetConnectionString("UserConnection");
        }

    }
}
