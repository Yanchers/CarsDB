using Microsoft.EntityFrameworkCore;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace CourseProjectDataBaseCars
{
    [AddINotifyPropertyChangedInterface]
    public class FactoryBoolPair : INotifyPropertyChanged
    {
        public FactoryBoolPair(Factory factory, bool value = true)
        {
            Key = factory;
            Value = value;
        }

        public Factory Key { get; set; }
        public bool Value { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

    public class CatalogViewModel : BaseViewModel
    {
        public CatalogViewModel()
        {
            using var context = new CarDealerContext();
            CarItems = context.Cars.ToList().OrderBy(c => c.Name).ToList();
            DownBorder = Math.Round(context.Cars.Min((c) => c.Cost));
            UpperBorder = Math.Round(context.Cars.Max((c) => c.Cost));

            FactoryItems = new ObservableCollection<FactoryBoolPair>(context.Factories.Select((f) => new FactoryBoolPair(f, true)));

            ApplyFilterCommand = new RelayCommand(name => Filter((string)name));
            SelectCarCommand = new RelayCommand(param => SelectCar((int)param));
            FindCarCommand = new RelayCommand(name => Filter((string)name));
            SelectAllFactoriesCommand = new RelayCommand(param => SelectAllFactories());
            AddCarCommand = new RelayCommand(param => AddCar());
        }

        #region Private Properties

        private List<Car> mCarItems = new List<Car>();
        private int mSelectedSorting;
        private int mSelectedGrouping;

        #endregion

        #region Public Properties

        public List<Car> CarItems
        {
            get => mCarItems;
            set
            {
                mCarItems = new List<Car>(value.Select((c) => { c.Cost = Math.Round(c.Cost); return c; }));
            }
        }
        public ObservableCollection<FactoryBoolPair> FactoryItems { get; set; }
        public decimal DownBorder { get; set; }
        public decimal UpperBorder { get; set; }
        public int SelectedSorting
        {
            get { return mSelectedSorting; }
            set
            {
                SortBy((SortingTypes)value);
                mSelectedSorting = value;
            }
        }
        public bool HasFactory { get; set; }
        //public int SelectedGrouping
        //{
        //    get { return mSelectedGrouping; }
        //    set 
        //    { 

        //        mSelectedGrouping = value;
        //    }
        //}



        #endregion

        #region Private Methods

        private void SelectCar(int id)
        {
            ApplicationViewModel.Instance.PageParam = CarItems.Find(c=>c.Id == id);
            ApplicationViewModel.Instance.CurrentPage = PageTypes.Car;
        }
        private void SelectAllFactories()
        {
            foreach (var pair in FactoryItems)
                pair.Value = true;
        }
        private void Filter(string name)
        {
            using var context = new CarDealerContext();
            var factories = FactoryItems.Where(f => f.Value).Select(f => new Factory(f.Key));
            var factoryCars = (from f in factories
                            join cf in context.CarsFactories on f.Id equals cf.FactoryId
                            join c in context.Cars on cf.CarId equals c.Id
                            select new Car(c)).Distinct(new CarComparer());
            var factoryCarsAll = (from f in context.Factories.ToList()
                                  join cf in context.CarsFactories on f.Id equals cf.FactoryId
                                  join c in context.Cars on cf.CarId equals c.Id
                                  select new Car(c)).Distinct(new CarComparer());
            var nameCars = context.GetCarsByName(name);
            var priceCars = context.GetCarsByPrice((float)UpperBorder, (float)DownBorder);

            CarItems = factoryCars.Intersect(priceCars, new CarComparer()).Intersect(nameCars, new CarComparer()).ToList();
            if (HasFactory) CarItems.AddRange(context.Cars.ToList().Except(factoryCarsAll, new CarComparer()).Intersect(nameCars, new CarComparer()));

            SortBy((SortingTypes)SelectedSorting);
        }
        private void SortBy(SortingTypes type)
        {
            switch (type)
            {
                case SortingTypes.Alphabet:
                    CarItems = mCarItems.OrderBy(c => c.Name).ToList();
                    break;
                case SortingTypes.AscendingPrice:
                    CarItems = mCarItems.OrderBy(c => c.Cost).ToList();
                    break;
                case SortingTypes.DescendingPrice:
                    CarItems = mCarItems.OrderByDescending(c => c.Cost).ToList();
                    break;
                default:
                    break;
            }
        }
        private void AddCar()
        {
            var window = new AddCarWindow();
            if ((bool)window.ShowDialog())
                MessageBox.Show("Модель успешно создана.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        //private void GroupBy(GroupingTypes type)
        //{
        //    switch (type)
        //    {
        //        case GroupingTypes.None:
        //            SortBy((SortingTypes)SelectedSorting);
        //            break;
        //        case GroupingTypes.Factory:
                    
        //            break;
        //        default:
        //            break;
        //    }
        //}

        #endregion

        #region Commands

        public RelayCommand ApplyFilterCommand { get; private set; }
        public RelayCommand SelectCarCommand { get; private set; }
        public RelayCommand FindCarCommand { get; private set; }
        public RelayCommand SelectAllFactoriesCommand { get; private set; }
        public RelayCommand AddCarCommand { get; private set; }

        #endregion
    }
}
