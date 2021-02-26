﻿using Microsoft.EntityFrameworkCore;
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
            try
            {
                using var context = new CarDealerContext();
                context.Database.ExecuteSqlInterpolated($"Dealer.AddCar {CarName}, {int.Parse(CarCost)}");
                ((Window)param).DialogResult = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }
        public RelayCommand CreateCarCommand { get; private set; }
    }
}
