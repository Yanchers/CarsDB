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
            mCarId = (int)ApplicationViewModel.Instance.PageParam;

            UpdateData();

            PreviousPageCommand = new RelayCommand(param => ApplicationViewModel.Instance.CurrentPage = PageTypes.Catalog);
            AddRefCommand = new RelayCommand(AddRef);
            DeleteCreditRefCommand = new RelayCommand(DeleteCreditRef);
            DeleteFactoryRefCommand = new RelayCommand(DeleteFactoryRef);
        }

        #region Private Properties

        private int mCarId;
        private int mSelectedFactory;
        private int mSelectedCredit;

        #endregion

        #region Public Properties

        public Car CurrentCar { get; set; }
        public List<CarSummaryInfo> CarInfoCollection { get; set; } = new List<CarSummaryInfo>();
        public ObservableCollection<Credit> CreditItems { get; set; } = new ObservableCollection<Credit>();
        public ObservableCollection<Factory> FactoryItems { get; set; } = new ObservableCollection<Factory>();

        public float TotalCost { get; set; }
        public float MonthlyPay { get; set; }

        public int SelectedFactory
        {
            get => mSelectedFactory;
            set
            {
                if (value == -1) return;

                mSelectedFactory = value;
                SetInfo();
            }
        }
        public int SelectedCredit
        {
            get => mSelectedCredit;
            set
            {
                if (value == -1) return;

                mSelectedCredit = value;
                SetInfo();
            }
        }

        #endregion

        #region Private Methods

        private void SetInfo()
        {
            if (CarInfoCollection.Count < 0) return;

            var info = CarInfoCollection.Find(c => c.FactoryId == FactoryItems[SelectedFactory].Id && c.CreditId == CreditItems[SelectedCredit].Id);
            TotalCost = (float)info.TotalCost;
            MonthlyPay = (float)info.MonthlyPay;
        }
        private void AddRef(object param)
        {
            var carRef = new AddCarRefWindow(CurrentCar.Id);

            if ((bool)carRef.ShowDialog())
                MessageBox.Show("Модель связана.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            UpdateData();
        }
        private void DeleteCreditRef(object param)
        {
            if (SelectedCredit == -1) return;

            using var context = new CarDealerContext();

            context.CarsCredits.Remove(context.CarsCredits.Find(CurrentCar.Id, CreditItems[SelectedCredit].Id));
            context.SaveChanges();

            UpdateData();
        }
        private void DeleteFactoryRef(object param)
        {
            if (SelectedFactory == -1) return;

            using var context = new CarDealerContext();

            context.CarsFactories.Remove(context.CarsFactories.Find(CurrentCar.Id, FactoryItems[SelectedFactory].Id));
            context.SaveChanges();

            UpdateData();
        }
        private void UpdateData()
        {
            using (var context = new CarDealerContext())
            {
                CurrentCar = context.Cars.Where(c => c.Id == mCarId)
                    .Include(c => c.CarsFactories).ThenInclude(cf => cf.Factory)
                    .Include(c => c.CarsCredits).ThenInclude(cc => cc.Credit).ThenInclude(c => c.Bank)
                    .FirstOrDefault();

                CreditItems.Clear();
                foreach (var cc in CurrentCar.CarsCredits)
                    CreditItems.Add(cc.Credit);

                FactoryItems.Clear();
                foreach (var cf in CurrentCar.CarsFactories)
                    FactoryItems.Add(cf.Factory);
            }

            var builder = new ConfigurationBuilder();
            builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            using (var connection = new SqlConnection(builder.Build().GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var command = new SqlCommand($"select * from Prog.GetCarInfo('{CurrentCar.Name}')", connection);
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
                    CarInfoCollection.Add(info);
                }
            }

            if (CarInfoCollection.Count > 0)
            {
                mSelectedCredit = 0;
                mSelectedFactory = 0;

                SetInfo();
            }
            else
            {
                TotalCost = (int)CurrentCar.Cost;
                MonthlyPay = 0;
            }
        }

        #endregion

        #region Commands

        public RelayCommand PreviousPageCommand { get; private set; }
        public RelayCommand AddRefCommand { get; private set; }
        public RelayCommand DeleteCreditRefCommand { get; private set; }
        public RelayCommand DeleteFactoryRefCommand { get; private set; }

        #endregion
    }
}
