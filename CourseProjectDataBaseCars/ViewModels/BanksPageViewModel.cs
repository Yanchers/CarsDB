using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            CreditItems = new List<CreditNode>();
            using var context = new CarDealerContext();

            var credits = context.Credits.Include(c => c.Bank).ToList();
            foreach (var credit in credits)
            {
                if (CreditItems.Contains(new CreditNode() { BankId = credit.BankId }, new CreditNodeComparer()))
                    CreditItems[CreditItems.FindIndex(c => c.BankId == credit.BankId)].Children.Add(credit);
                else
                    CreditItems.Add(new CreditNode()
                    {
                        Bank = credit.Bank,
                        BankId = credit.BankId,
                        Expiration = credit.Expiration,
                        Rate = (float)credit.Rate
                    });
            }
        }

        #region Public Properties

        public List<CreditNode> CreditItems { get; set; }

        #endregion

    }
}
