using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            UpdateItems();

            CreateFactoryCommand = new RelayCommand(CreateFactory);
            UpdateFactoryCommand = new RelayCommand(UpdateFactory);
            DeleteFactoryCommand = new RelayCommand(DeleteFactory);
        }
        #region Public Properties

        public ObservableCollection<Factory> FactoryItems { get; set; }

        #endregion

        #region Private Methods

        private void CreateFactory(object param)
        {
            var window = new AddFactoryWindow();
            if ((bool)window.ShowDialog())
                MessageBox.Show("Завод успешно создан.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            UpdateItems();
        }
        private void UpdateFactory(object id)
        {
            var window = new AddFactoryWindow((int)id);
            if((bool)window.ShowDialog())
                MessageBox.Show("Завод успешно создан.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            UpdateItems();
        }
        private void DeleteFactory(object id)
        {
            try
            {
                using var context = new CarDealerContext();
                
                context.Factories.Remove(context.Factories.Find((int)id));
                context.SaveChanges();

                UpdateItems();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void UpdateItems()
        {
            using var context = new CarDealerContext();

            FactoryItems = new ObservableCollection<Factory>(context.Factories.Include(f => f.CarsFactories).ThenInclude(cf => cf.Car));
        }

        #endregion

        #region Commands

        public RelayCommand CreateFactoryCommand { get; private set; }
        public RelayCommand UpdateFactoryCommand { get; private set; }
        public RelayCommand DeleteFactoryCommand { get; private set; }

        #endregion

    }
}
