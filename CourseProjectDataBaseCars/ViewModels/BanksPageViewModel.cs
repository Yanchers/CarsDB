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
            UpdateCreditCommand = new RelayCommand(UpdateCredit);
            DeleteCreditCommand = new RelayCommand(DeleteCredit);
            CreateBankCommand = new RelayCommand(CreateBank);
            UpdateBankCommand = new RelayCommand(UpdateBank);
            DeleteBankCommand = new RelayCommand(DeleteBank);
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
        private void UpdateBank(object bankId)
        {
            var window = new AddBankWindow((int)bankId);
            if ((bool)window.ShowDialog())
                MessageBox.Show("Банк успешно изменен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

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
        private void UpdateCredit(object creditId)
        {
            var window = new AddCreditWindow(0, (int)creditId);
            
            if ((bool)window.ShowDialog())
                MessageBox.Show("Кредит успешно изменен.",
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

        public RelayCommand CreateBankCommand { get; private set; }
        public RelayCommand UpdateBankCommand { get; private set; }
        public RelayCommand DeleteBankCommand { get; private set; }
        public RelayCommand AddCreditCommand { get; private set; }
        public RelayCommand UpdateCreditCommand { get; private set; }
        public RelayCommand DeleteCreditCommand { get; private set; }

        #endregion

    }
}
