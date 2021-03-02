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

            UpdateData();

            PreviousPageCommand = new RelayCommand(param => ApplicationViewModel.Instance.CurrentPage = PageTypes.Catalog);
            AddRefCommand = new RelayCommand(AddRef);
            DeleteCreditRefCommand = new RelayCommand(DeleteCreditRef);
            DeleteFactoryRefCommand = new RelayCommand(DeleteFactoryRef);
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
        public List<Credit> CreditItems { get; set; }
        public List<Factory> FactoryItems { get; set; }

        #endregion

        private void AddRef(object param)
        {
            var carRef = new AddCarRefWindow(CurrentCar.Id);

            if ((bool)carRef.ShowDialog())
                MessageBox.Show("Модель связана.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            UpdateData();
        }
        private void DeleteCreditRef(object param)
        {
            if (SelectedSummary == -1) return;

            using var context = new CarDealerContext();

            context.CarsCredits.Remove(context.CarsCredits.Find(CurrentCar.Id, CarInfoCollection.Cast<CarSummaryInfo>().ElementAt(SelectedSummary).CreditId));
            context.SaveChanges();

            UpdateData();
        }
        private void DeleteFactoryRef(object param)
        {
            if (SelectedSummary == -1) return;

            using var context = new CarDealerContext();

            context.CarsFactories.Remove(context.CarsFactories.Find(CurrentCar.Id, CarInfoCollection.Cast<CarSummaryInfo>().ElementAt(SelectedSummary).FactoryId));
            context.SaveChanges();

            UpdateData();
        }
        private void UpdateData()
        {
            using (var context = new CarDealerContext())
            {
                CreditItems = context.Cars.Find(CurrentCar.Id).CarsCredits.
            }

            var infoCollection = new ObservableCollection<CarSummaryInfo>();

            var builder = new ConfigurationBuilder();
            builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            using (var connection = new SqlConnection(builder.Build().GetConnectionString("NonaConnection")))
            {
                connection.Open();
                var command = new SqlCommand($"select * from Dealer.GetCarInfo('{CurrentCar.Name}')", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var info = new CarSummaryInfo()
                    {
                        CreditId = (int)reader.GetValue(0),
                        FactoryId = (int)reader.GetValue(1),
                        CarCost = (decimal)reader.GetValue(2),
                        TotalCost = (decimal)reader.GetValue(3),
                        MonthlyPay = (decimal)reader.GetValue(4),
                        Expiration = ((DateTime)reader.GetValue(5)).ToShortDateString(),
                        BankName = reader.GetValue(6).ToString(),
                        Country = reader.GetValue(7).ToString(),
                        City = reader.GetValue(8).ToString(),
                        TranspCost = (decimal)reader.GetValue(9),
                        Arrival = ((DateTime)reader.GetValue(10)).ToShortDateString()
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
            else
            {
                TotalCost = (float)CurrentCar.Cost;
                MonthlyPay = 0;
            }
        }

        public RelayCommand PreviousPageCommand { get; private set; }
        public RelayCommand AddRefCommand { get; private set; }
        public RelayCommand DeleteCreditRefCommand { get; private set; }
        public RelayCommand DeleteFactoryRefCommand { get; private set; }
    }
}
