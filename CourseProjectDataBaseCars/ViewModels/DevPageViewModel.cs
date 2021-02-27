using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace CourseProjectDataBaseCars
{
    public class DevPageViewModel : BaseViewModel
    {
        public DevPageViewModel()
        {
            using var context = new CarDealerContext();
            CarItems = new ObservableCollection<Car>(context.Cars.ToList());
            FactoryItems = new ObservableCollection<Factory>(context.Factories.ToList());
            CreditItems = new ObservableCollection<Credit>(context.Credits.ToList());
            CarsFactoriesItems = new ObservableCollection<CarsFactory>(context.CarsFactories.ToList());
            CarsCreditsItems = new ObservableCollection<CarsCredit>(context.CarsCredits.ToList());
            BankItems = new ObservableCollection<Bank>(context.Banks.ToList());

            AddCarCommand = new RelayCommand(AddCar);
            AddFactoryCommand = new RelayCommand(AddFactory);

            SaveItemsCommand = new RelayCommand(SaveItems);
        }

        #region Public Properties

        public ObservableCollection<Car> CarItems { get; set; }
        public ObservableCollection<Factory> FactoryItems { get; set; }
        public ObservableCollection<Credit> CreditItems { get; set; }
        public ObservableCollection<CarsFactory> CarsFactoriesItems { get; set; }
        public ObservableCollection<CarsCredit> CarsCreditsItems { get; set; }
        public ObservableCollection<Bank> BankItems { get; set; }

        #endregion


        #region Private Methods

        private async void SaveItems(object param)
        {
            using var context = new CarDealerContext();
            //var cars = context.Cars.ToList();
            context.Cars.UpdateRange(CarItems);
            context.Factories.UpdateRange(FactoryItems);
            await context.SaveChangesAsync();
        }
        private void AddCar(object param)
        {
            using var context = new CarDealerContext();
            CarItems.Add(context.Cars.Add(new Car() { Name = "Новая машина", Cost = 100000 }).Entity);
            context.SaveChanges();
        }
        private void AddFactory(object param)
        {
            using var context = new CarDealerContext();
            FactoryItems.Add(context.Factories.Add(new Factory() { Country="Страна", City="Город", DeliveryTime=1, TranspCost=1m, Type=TransportationTypes.None}).Entity);
            context.SaveChanges();
        }

        #endregion
        public RelayCommand AddCarCommand { get; private set; }
        public RelayCommand SaveItemsCommand { get; private set; }
        public RelayCommand AddFactoryCommand { get; private set; }

    }
}
