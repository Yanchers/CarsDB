using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CourseProjectDataBaseCars
{
    class AddFactoryWindowViewModel : BaseViewModel
    {
        public AddFactoryWindowViewModel(int id)
        {
            Factory = id == 0 ? new Factory() { Type = TransportationTypes.None } : new CarDealerContext().Factories.Find(id);

            CreateFactoryCommand = new RelayCommand(CreateFactory);
        }


        public Factory Factory { get; set; }
        public List<string> TranspTypeItems { get; set; } = new List<string>()
        {
            "Ничего", "Ж/Д", "Машина", "Самолет", "Корабль"
        };

        
        private void CreateFactory(object param)
        {
            try
            {
                using var context = new CarDealerContext();

                if (Factory.Id == 0)
                    context.Database.ExecuteSqlInterpolated($"Prog.AddFactory {Factory.Country}, {Factory.City}, {Factory.DeliveryTime}, {Factory.Type}, {Factory.TranspCost}");
                else
                {
                    context.Factories.Update(Factory);
                    context.SaveChanges();
                }

                ((Window)param).DialogResult = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }
        public RelayCommand CreateFactoryCommand { get; private set; }
    }
}
