using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CourseProjectDataBaseCars
{
    class AddCreditWindowViewModel : BaseViewModel
    {
        public AddCreditWindowViewModel(int bankId, int creditId)
        {
            Credit = creditId == 0 ? new Credit() { BankId = bankId } : new CarDealerContext().Credits.Find(creditId);

            CreateCreditCommand = new RelayCommand(CreateCredit);
        }

        public Credit Credit { get; set; }

        private void CreateCredit(object param) 
        {
            if (Credit.Rate < 0.1f || Credit.Expiration <= 0)
            {
                MessageBox.Show("Введите корректные данные!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            try
            {
                using var context = new CarDealerContext();

                if (Credit.Id == 0)
                    context.Database.ExecuteSqlInterpolated($"Prog.AddCredit {Credit.BankId}, {Credit.Rate}, {Credit.Expiration}");
                else
                {
                    context.Credits.Update(Credit);
                    context.SaveChanges();
                }

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
