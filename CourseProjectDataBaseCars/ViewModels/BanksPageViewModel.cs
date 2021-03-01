using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProjectDataBaseCars
{
    public class BanksPageViewModel : BaseViewModel
    {
        public BanksPageViewModel()
        {
            using var context = new CarDealerContext();

            BankItems = context.Banks.Include(b => b.Credits).ToList();

            AddCreditCommand = new RelayCommand(AddCredit);
            CreateBankCommand = new RelayCommand(CreateBank);
            DeleteBankCommand = new RelayCommand(DeleteBank);
        }

        #region Public Properties

        public List<Bank> BankItems { get; set; } = new List<Bank>();
        public int SelectedBankIndex { get; set; }

        #endregion

        #region Private Methods

        private void CreateBank(object param)
        {
            var window = new AddBankWindow();
            if ((bool)window.ShowDialog())
                MessageBox.Show("Банк успешно создан.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void DeleteBank(object param)
        {
            if (SelectedBankIndex == -1) return;
            try
            {
                using var context = new CarDealerContext();
                context.Banks.Remove(context.Banks.Find(BankItems[SelectedBankIndex].Id));
                context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void AddCredit(object bankId)
        {
            var window = new AddCreditWindow((int)bankId);
            if ((bool)window.ShowDialog())
                MessageBox.Show($"Кредит успешно добавлен в банк {BankItems.Find(b=>b.Id == ((AddCreditWindowViewModel)window.DataContext).Credit.BankId).Name}.", 
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void Delete

        #endregion

        #region Commands

        public RelayCommand CreateBankCommand { get; set; }
        public RelayCommand DeleteBankCommand { get; set; }
        public RelayCommand AddCreditCommand { get; set; }

        #endregion

    }
}
