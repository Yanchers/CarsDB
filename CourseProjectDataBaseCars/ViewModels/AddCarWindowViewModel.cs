using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CourseProjectDataBaseCars
{
    class AddCarWindowViewModel : BaseViewModel
    {
        public AddCarWindowViewModel()
        {
            CreateCarCommand = new RelayCommand(CreateCar);
        }


        public string CarName { get; set; }
        public string CarCost { get; set; }


        private void CreateCar(object param)
        {
            using var context = new CarDealerContext();

            if (context.Database.ExecuteSqlInterpolated($"Dealer.AddCar {CarName}, {int.Parse(CarCost)}") > 0)
                ((Window)param).DialogResult = true;
            else
                MessageBox.Show("Модель уже существует, либо проблема в чем-то другом.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        public RelayCommand CreateCarCommand { get; private set; }
    }
}
