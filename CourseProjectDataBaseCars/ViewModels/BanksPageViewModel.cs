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
    public class CreditNodeComparer : IEqualityComparer<CreditNode>
    {
        public bool Equals([AllowNull] CreditNode x, [AllowNull] CreditNode y)
        {
            return x.BankId == y.BankId;
        }

        public int GetHashCode([DisallowNull] CreditNode obj)
        {
            return base.GetHashCode();
        }
    }
    public class CreditNode
    {
        public Bank Bank { get; set; }
        public int BankId { get; set; }
        public float Rate { get; set; }
        public int Expiration { get; set; }
        public ObservableCollection<Credit> Children { get; set; } = new ObservableCollection<Credit>();
    }

    public class BanksPageViewModel : BaseViewModel
    {
        public BanksPageViewModel()
        {
            using var context = new CarDealerContext();

            BankItems = context.Banks.Include(b => b.Credits).ToList();

            AddCreditCommand = new RelayCommand(AddCredit);
            CreateBankCommand = new RelayCommand(CreateBank);
        }

        #region Public Properties

        public List<Bank> BankItems { get; set; } = new List<Bank>();
        public string BankName { get; set; }

        #endregion

        #region Private Methods

        private void CreateBank(object param)
        {
            var window = new AddBankWindow();
            if ((bool)window.ShowDialog())
                MessageBox.Show("Банк успешно создан.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void AddCredit(object bankId)
        {

        }

        #endregion

        #region Commands

        public RelayCommand CreateBankCommand { get; set; }
        public RelayCommand AddCreditCommand { get; set; }

        #endregion

    }
}
