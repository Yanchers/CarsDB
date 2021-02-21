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
        }
        public RelayCommand DevPageCommand { get; private set; }
    }
}
