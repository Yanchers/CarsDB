using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CourseProjectDataBaseCars
{
    class AddCreditWindowViewModel : BaseViewModel
    {
        public AddCreditWindowViewModel(int bankId)
        {
            Credit = new Credit() { BankId = bankId };

            CreateCreditCommand = new RelayCommand(CreateCredit);
        }

        public Credit Credit { get; set; }

        private void CreateCredit(object param) // TODO: Добавить проверку на дубликат !!!
        {
            try
            {
                using var context = new CarDealerContext();
                context.Database.ExecuteSqlInterpolated($"Dealer.AddCredit {Credit.BankId}, {Credit.Rate}, {Credit.Expiration}");
                context.Credits.Include(c => c.Bank);
                ((Window)param).DialogResult = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public RelayCommand CreateCreditCommand { get; private set; }
    }
}
