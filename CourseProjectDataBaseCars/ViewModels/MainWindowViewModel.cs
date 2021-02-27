using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;

namespace CourseProjectDataBaseCars
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            DevPageCommand = new RelayCommand(param => ApplicationViewModel.Instance.CurrentPage = PageTypes.Dev);
            CatalogPageCommand = new RelayCommand(param => ApplicationViewModel.Instance.CurrentPage = PageTypes.Catalog);
            BanksPageCommand = new RelayCommand(param => ApplicationViewModel.Instance.CurrentPage = PageTypes.Banks);
            FactoriesPageCommand = new RelayCommand(param => ApplicationViewModel.Instance.CurrentPage = PageTypes.Factories);
        }
        public RelayCommand DevPageCommand { get; private set; }
        public RelayCommand CatalogPageCommand { get; private set; }
        public RelayCommand BanksPageCommand { get; private set; }
        public RelayCommand FactoriesPageCommand { get; private set; }
    }
}
