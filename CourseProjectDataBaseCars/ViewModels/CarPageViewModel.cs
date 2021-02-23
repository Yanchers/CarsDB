using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace CourseProjectDataBaseCars
{
    public class CarPageViewModel : BaseViewModel
    {
        public CarPageViewModel()
        {
            CurrentCar = ApplicationViewModel.Instance.PageParam as Car;

            LoadInfo();

            PreviousPageCommand = new RelayCommand(param => ApplicationViewModel.Instance.CurrentPage = PageTypes.Catalog);
            AddRefCommand = new RelayCommand(AddRef);
        }

        private int mSelectedSummary { get; set; }

        #region Public Properties

        public float TotalCost { get; set; }
        public float MonthlyPay { get; set; }
        public int SelectedSummary
        {
            get => mSelectedSummary;
            set
            {
                if (value < 0) return;
                var temp = CarInfoCollection.Cast<CarSummaryInfo>().ElementAt(value);
                TotalCost = (float)temp.TotalCost;
                MonthlyPay = (float)temp.MonthlyPay;

                mSelectedSummary = value;
            }
        }
        public ICollectionView CarInfoCollection { get; set; }
        public Car CurrentCar { get; set; }

        #endregion

        private void AddRef(object param)
        {
            var carRef = new AddCarRefWindow(CurrentCar.Id);
            carRef.ShowDialog();
            LoadInfo();
        }
        private void LoadInfo()
        {
            var infoCollection = new ObservableCollection<CarSummaryInfo>();

            var builder = new ConfigurationBuilder();
            builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            using (var connection = new SqlConnection(builder.Build().GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var command = new SqlCommand($"select * from Dealer.GetCarInfo('{CurrentCar.Name}')", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var info = new CarSummaryInfo()
                    {
                        CarCost = (decimal)reader.GetValue(0),
                        TotalCost = (decimal)reader.GetValue(1),
                        MonthlyPay = (decimal)reader.GetValue(2),
                        Expiration = ((DateTime)reader.GetValue(3)).ToShortDateString(),
                        BankName = reader.GetValue(4).ToString(),
                        Country = reader.GetValue(5).ToString(),
                        City = reader.GetValue(6).ToString(),
                        TranspCost = (decimal)reader.GetValue(7),
                        Arrival = ((DateTime)reader.GetValue(8)).ToShortDateString()
                    };
                    infoCollection.Add(info);
                }
            }

            CarInfoCollection = CollectionViewSource.GetDefaultView(infoCollection);
            if (infoCollection.Count > 0)
            {
                TotalCost = (float)infoCollection[0].TotalCost;
                MonthlyPay = (float)infoCollection[0].MonthlyPay;
            }
        }

        public RelayCommand PreviousPageCommand { get; private set; }
        public RelayCommand AddRefCommand { get; private set; }
    }
}
