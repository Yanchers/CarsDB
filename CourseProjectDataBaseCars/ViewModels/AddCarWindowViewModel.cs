using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CourseProjectDataBaseCars
{
    class AddCarWindowViewModel : BaseViewModel
    {
        public AddCarWindowViewModel(int id)
        {
            Car = id == 0 ? new Car() : new CarDealerContext().Cars.Find(id);

            CreateCarCommand = new RelayCommand(CreateCar);
        }


        public Car Car { get; set; }


        private void CreateCar(object param)
        {
            try
            {
                using var context = new CarDealerContext();

                if (Car.Id == 0)
                    context.Database.ExecuteSqlInterpolated($"Dealer.AddCar {Car.Name}, {Convert.ToInt32(Car.Cost)}");
                else
                {
                    context.Cars.Update(Car);
                    context.SaveChanges();
                }

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
