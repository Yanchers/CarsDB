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
            DbLoginPageCommand = new RelayCommand(DBLoginPage);
            HelpCommand = new RelayCommand(param => MessageBox.Show("База Данных Автодилер.\nПомощь:\n1. Для изменения существующих объектов БД, щелкните по объекту ЛКМ два раза." +
                "\n2. Все вопросы по телефону 8(981)8066616.\nКурсовой проект сделал Орешко Ян Александрович гр.32928/4 | 2021г.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information));
        }


        private void DBLoginPage(object param)
        {
            var window = new DBLoginWindow();
            window.ShowDialog();
        }


        public RelayCommand DevPageCommand { get; private set; }
        public RelayCommand CatalogPageCommand { get; private set; }
        public RelayCommand BanksPageCommand { get; private set; }
        public RelayCommand FactoriesPageCommand { get; private set; }
        public RelayCommand DbLoginPageCommand { get; private set; }
        public RelayCommand HelpCommand { get; private set; }
    }
}
