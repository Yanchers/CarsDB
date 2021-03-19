namespace CourseProjectDataBaseCars
{
    public class ApplicationViewModel : BaseViewModel
    {
        public bool HasMainWindow { get; set; } = false;
        public PageTypes CurrentPage { get; set; } = PageTypes.Catalog;
        public object PageParam { get; set; }

        public static ApplicationViewModel Instance { get; set; } = new ApplicationViewModel();
    }
}
