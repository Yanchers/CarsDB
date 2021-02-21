using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProjectDataBaseCars
{
    public class MenuPageViewModel : BaseViewModel
    {
        public MenuPageViewModel()
        {

            CatalogPageCommand = new RelayCommand(param => ApplicationViewModel.Instance.CurrentPage = PageTypes.Catalog);
        }

        public RelayCommand CatalogPageCommand { get; private set; }
    }
}
