namespace CourseProjectDataBaseCars
{
    public class ApplicationViewModel : BaseViewModel
    {
        public PageTypes CurrentPage { get; set; } = PageTypes.Catalog;
        public object PageParam { get; set; }
        public static ApplicationViewModel Instance { get; set; } = new ApplicationViewModel();
    }
}
