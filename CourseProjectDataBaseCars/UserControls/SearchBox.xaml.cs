using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseProjectDataBaseCars
{
    /// <summary>
    /// Логика взаимодействия для SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        public SearchBox()
        {
            InitializeComponent();

            DataTrigger trigger = new DataTrigger();
            var style = new Style(typeof(Button));

            trigger.Binding = new Binding("IsMouseOver") { ElementName = "textBox" };
            trigger.Value = true;
            trigger.Setters.Add(new Setter(MarginProperty, new Thickness(0)));
            Storyboard sb = new Storyboard();
            ThicknessAnimation anim = new ThicknessAnimation(new Thickness(0), new Duration(new TimeSpan(100)));
            sb.BeginAnimation(MarginProperty, anim);
            trigger.EnterActions.Add();

            style.Triggers.Add(trigger);
            style.Setters.Add(new Setter(MarginProperty, new Thickness(0, 0, -searchBtn.Width, 0)));
            style.Setters.Add(new Setter(BackgroundProperty, Brushes.Transparent));
            style.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(0)));

            searchBtn.Style = style;
        }
    }
}
