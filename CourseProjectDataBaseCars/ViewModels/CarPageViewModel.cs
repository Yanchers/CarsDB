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
            var infoCollection = new ObservableCollection<CarSummaryInfo>();

            CurrentCar = ApplicationViewModel.Instance.PageParam as Car;

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

            //CreditItems = (from credit in context.Credits
            //                join cc in context.CarsCredits on credit.Id equals cc.CreditId
            //                where cc.CarId == CurrentCar.Id
            //                select new Credit(credit)).ToList();
            //SelectedCredit = CreditItems[0].Id.ToString();

            //FactoryItems = (from f in context.Factories
            //                join cf in context.CarsFactories on f.Id equals cf.FactoryId
            //                join c in context.Cars on cf.CarId equals c.Id
            //                where c.Id == CurrentCar.Id
            //                select new Factory(f)).ToList();
            //SelectedFactory = FactoryItems[0].Country + " " + FactoryItems[0].City;

            //BankItems = (from c in CreditItems
            //             join b in context.Banks on c.BankId equals b.Id
            //             select new Bank(b)).ToList();
            //SelectedBank = BankItems[0].Name;

            CarInfoCollection = CollectionViewSource.GetDefaultView(infoCollection);
            TotalCost = infoCollection[0].TotalCost;
            MonthlyPay = infoCollection[0].MonthlyPay;

            PreviousPageCommand = new RelayCommand(param => ApplicationViewModel.Instance.CurrentPage = PageTypes.Catalog);
        }

        private int mSelectedSummary { get; set; }

        public decimal TotalCost { get; set; }
        public decimal MonthlyPay { get; set; }
        public int SelectedSummary
        {
            get => mSelectedSummary;
            set
            {
                var temp = CarInfoCollection.Cast<CarSummaryInfo>().ElementAt(value);
                TotalCost = temp.TotalCost;
                MonthlyPay = temp.MonthlyPay;

                mSelectedSummary = value;
            }
        }
        public ICollectionView CarInfoCollection { get; set; }
        public Car CurrentCar { get; set; }

        public RelayCommand PreviousPageCommand { get; private set; }
    }
}
