using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CourseProjectDataBaseCars
{
    class AddCarRefWindowViewModel : BaseViewModel
    {
        public AddCarRefWindowViewModel() { }
        public AddCarRefWindowViewModel(int id)
        {
            using var context = new CarDealerContext();
            FactoryItems = context.Factories.OrderBy(f => f.Country).ToList();
            CreditItems = context.Credits.Include(c => c.Bank).OrderBy(c => c.Bank.Name).ToList();

            CarId = id;

            AddRefCommand = new RelayCommand(AddRef);
        }
        #region Private Properties

        private int CarId;

        #endregion

        #region Public Properties

        public List<Factory> FactoryItems { get; set; }
        public List<Credit> CreditItems { get; set; }
        public int SelectedFactory { get; set; } = 0;
        public int SelectedCredit { get; set; } = 0;

        #endregion

        private void AddRef(object param)
        {
            using var context = new CarDealerContext();

            if (context.Database.ExecuteSqlInterpolated($"Prog.AddCarCreditFactoryRef {CarId}, {CreditItems[SelectedCredit].Id}, {FactoryItems[SelectedFactory].Id}") > 0)
                ((Window)param).DialogResult = true;
            else
                MessageBox.Show("Модель уже связана.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        public RelayCommand AddRefCommand { get; private set; }

    }
}
