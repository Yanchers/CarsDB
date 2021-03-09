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
            Car = id == 0 ? new Car() { Name = "" } : new CarDealerContext().Cars.Find(id);

            CreateCarCommand = new RelayCommand(CreateCar);
        }


        public Car Car { get; set; }


        private void CreateCar(object param)
        {
            if (Car.Name.Trim() == "" || Car.Cost <= 0)
            {
                MessageBox.Show("Заполните все поля!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            try
            {
                using var context = new CarDealerContext();

                if (Car.Id == 0)
                    context.Database.ExecuteSqlInterpolated($"Prog.AddCar {Car.Name}, {Convert.ToInt32(Car.Cost)}");
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
