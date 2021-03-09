using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CourseProjectDataBaseCars
{
    class AddBankWindowViewModel : BaseViewModel
    {
        public AddBankWindowViewModel(int bankId)
        {
            Bank = bankId == 0 ? new Bank() : new CarDealerContext().Banks.Find(bankId);

            CreateBankCommand = new RelayCommand(CreateBank);
        }

        public Bank Bank { get; set; }

        private void CreateBank(object param)
        {
            try
            {
                using var context = new CarDealerContext();

                if (Bank.Id == 0)
                    context.Database.ExecuteSqlInterpolated($"Prog.AddBank {Bank.Name}");
                else
                {
                    context.Banks.Update(Bank);
                    context.SaveChanges();
                }

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
