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

            BankItems = context.Banks.Include(b => b.Credits).OrderBy(b => b.Name).ToList();

            AddCreditCommand = new RelayCommand(AddCredit);
            CreateBankCommand = new RelayCommand(CreateBank);
            DeleteBankCommand = new RelayCommand(DeleteBank);
            DeleteCreditCommand = new RelayCommand(DeleteCredit);
        }

        #region Public Properties

        public List<Bank> BankItems { get; set; } = new List<Bank>();
        public int SelectedBankIndex { get; set; }

        #endregion

        #region Private Methods

        private void UpdateItems()
        {
            using var context = new CarDealerContext();
            BankItems = context.Banks.Include(b => b.Credits).OrderBy(b => b.Name).ToList();
        }
        private void CreateBank(object param)
        {
            var window = new AddBankWindow();
            if ((bool)window.ShowDialog())
                MessageBox.Show("Банк успешно создан.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            
            UpdateItems();
        }
        private void DeleteBank(object param)
        {
            if (SelectedBankIndex == -1) return;
            try
            {
                using var context = new CarDealerContext();
                context.Banks.Remove(context.Banks.Find(BankItems[SelectedBankIndex].Id));
                context.SaveChanges();

                UpdateItems();
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
                MessageBox.Show($"Кредит успешно добавлен в банк {BankItems.Find(b => b.Id == ((AddCreditWindowViewModel)window.DataContext).Credit.BankId).Name}.",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            UpdateItems();
        }
        private void DeleteCredit(object creditId)
        {
            if (creditId == null) return;
            try
            {
                using var context = new CarDealerContext();
                context.Credits.Remove(context.Credits.Find((int)creditId));
                context.SaveChanges();

                UpdateItems();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion

        #region Commands

        public RelayCommand CreateBankCommand { get; set; }
        public RelayCommand DeleteBankCommand { get; set; }
        public RelayCommand AddCreditCommand { get; set; }
        public RelayCommand DeleteCreditCommand { get; set; }

        #endregion

    }
}
