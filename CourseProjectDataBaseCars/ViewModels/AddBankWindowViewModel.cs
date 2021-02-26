using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CourseProjectDataBaseCars
{
    class AddBankWindowViewModel : BaseViewModel
    {
        public AddBankWindowViewModel()
        {
            CreateBankCommand = new RelayCommand(CreateBank);
        }

        public Bank Bank { get; set; } = new Bank();

        private void CreateBank(object param)
        {
            try
            {
                using var context = new CarDealerContext();
                context.Database.ExecuteSqlInterpolated($"Dealer.AddBank {Bank.Name}");
                ((Window)param).DialogResult = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public RelayCommand CreateBankCommand { get; private set; }
    }
}
