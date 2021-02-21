using System.Windows.Controls;

namespace CourseProjectDataBaseCars
{
    public class BasePage<VM> : Page
        where VM : BaseViewModel, new()
    {
        private VM mViewModel;

        public BasePage()
        {
            ViewModel = new VM();
        }

        public VM ViewModel
        {
            get => mViewModel;
            set 
            { 
                mViewModel = value;
                DataContext = mViewModel;
            }
        }
    }
}
