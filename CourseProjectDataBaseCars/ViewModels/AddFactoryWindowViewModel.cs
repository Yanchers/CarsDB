using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CourseProjectDataBaseCars
{
    class AddFactoryWindowViewModel : BaseViewModel
    {
        public AddFactoryWindowViewModel()
        {


            CreateFactoryCommand = new RelayCommand(CreateFactory);
        }


        public Factory Factory { get; set; } = new Factory() { Type=TransportationTypes.None };
        public List<string> TranspTypeItems { get; set; } = new List<string>()
        {
            "Ничего", "Ж/Д", "Машина", "Самолет", "Корабль"
        };

        
        private void CreateFactory(object param)
        {
            try
            {
                using var context = new CarDealerContext();
                context.Database.ExecuteSqlInterpolated($"Dealer.AddFactory {Factory.Country}, {Factory.City}, {Factory.DeliveryTime}, {Factory.Type}, {Factory.TranspCost}");
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
