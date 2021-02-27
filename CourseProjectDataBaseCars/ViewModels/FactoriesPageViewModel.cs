using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProjectDataBaseCars
{
    public class FactoriesPageViewModel : BaseViewModel
    {
        public FactoriesPageViewModel()
        {
            using var context = new CarDealerContext();

            FactoryItems = context.Factories.ToList();


            CreateFactoryCommand = new RelayCommand(CreateFactory);
            DeleteFactoryCommand = new RelayCommand(DeleteFactory);
        }

        public List<Factory> FactoryItems { get; set; }

        private void CreateFactory(object param)
        {
            // TODO: Окно добавления нового завода
        }
        private void DeleteFactory(object id)
        {
            using var context = new CarDealerContext();

            context.Factories.Remove(context.Factories.Find((int)id));
            context.SaveChanges();
        }

        public RelayCommand CreateFactoryCommand { get; private set; }
        public RelayCommand DeleteFactoryCommand { get; private set; }
    }
}
