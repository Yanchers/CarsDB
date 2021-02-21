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
            //CarView = CollectionViewSource.GetDefaultView(CarItems);
            //FactoryView = CollectionViewSource.GetDefaultView(FactoryItems);
            //CreditView = CollectionViewSource.GetDefaultView(CreditItems);

            AddCarCommand = new RelayCommand(param => 
            {
                CarItems.Add(new Car());
            });
            SaveItemsCommand = new RelayCommand(param => SaveItems());
        }
        public ObservableCollection<Car> CarItems { get; set; }
        public ObservableCollection<Factory> FactoryItems { get; set; }
        public ObservableCollection<Credit> CreditItems { get; set; }
        //public ICollectionView CarView { get; set; }
        //public ICollectionView FactoryView { get; set; }
        //public ICollectionView CreditView { get; set; }


        private void SaveItems()
        {
            using var context = new CarDealerContext();
            context.UpdateRange(CarItems);
            context.SaveChanges();
        }

        public RelayCommand AddCarCommand { get; private set; }
        public RelayCommand SaveItemsCommand { get; private set; }

    }
}
