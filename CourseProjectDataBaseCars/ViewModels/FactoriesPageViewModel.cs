using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectDataBaseCars
{
    public class FactoriesPageViewModel : BaseViewModel
    {
        public FactoriesPageViewModel()
        {
            using var context = new CarDealerContext();
            context.Cars.Load();
            FactoryItems = context.Factories.Include(f => f.CarsFactories).ToList();


            CreateFactoryCommand = new RelayCommand(CreateFactory);
            DeleteFactoryCommand = new RelayCommand(DeleteFactory);
        }

        public List<Factory> FactoryItems { get; set; }

        private void CreateFactory(object param)
        {
            var window = new AddFactoryWindow();
            if ((bool)window.ShowDialog())
                MessageBox.Show("Завод успешно создан.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            FactoryItems = new CarDealerContext().Factories.ToList();
        }
        private void DeleteFactory(object id)
        {
            try
            {
                using var context = new CarDealerContext();

                context.Factories.Remove(context.Factories.Find((int)id));
                context.SaveChanges();
                FactoryItems = context.Factories.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public RelayCommand CreateFactoryCommand { get; private set; }
        public RelayCommand DeleteFactoryCommand { get; private set; }
    }
}
