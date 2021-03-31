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

            Style style = new Style(typeof(Button));
            MultiDataTrigger multi = new MultiDataTrigger();
            multi.Conditions.Add(new Condition(new Binding("IsMouseOver") { ElementName = "textBox" }, true));
            multi.Conditions.Add(new Condition(new Binding("IsFocused") { ElementName = "textBox" }, true));
            DataTrigger triggerMouseOver = new DataTrigger();
            DataTrigger triggerIsFocused = new DataTrigger();
            Storyboard sbEnter = new Storyboard();
            Storyboard sbExit = new Storyboard();
            ThicknessAnimation animEnter = new ThicknessAnimation(new Thickness(0, 0, 20, 0), new Duration(new TimeSpan(0, 0, 0, 0, 200)));
            ThicknessAnimation animExit = 
                new ThicknessAnimation(new Thickness(0, 0, -searchBtn.Width, 0), new Duration(new TimeSpan(0, 0, 0, 0, 200)));

            triggerMouseOver.Binding = new Binding("IsMouseOver") { ElementName = "textBox" };
            triggerMouseOver.Value = true;

            animEnter.EasingFunction = new CubicEase();
            sbEnter.Children.Add(animEnter);
            Storyboard.SetTargetProperty(animEnter, new PropertyPath("Margin"));

            var beginSbEnter = new BeginStoryboard();
            beginSbEnter.Storyboard = sbEnter;

            animExit.EasingFunction = new CubicEase();
            sbExit.Children.Add(animExit);
            Storyboard.SetTargetProperty(animExit, new PropertyPath("Margin"));

            var beginSbExit = new BeginStoryboard();
            beginSbExit.Storyboard = sbExit;

            multi.EnterActions.Add(beginSbEnter);
            multi.ExitActions.Add(beginSbExit);
            //triggerMouseOver.EnterActions.Add(beginSbEnter);
            //triggerMouseOver.ExitActions.Add(beginSbExit);
            //triggerIsFocused.Binding = new Binding("IsFocused") { ElementName = "textBox" };
            //triggerIsFocused.EnterActions.Add(beginSbEnter);
            //triggerIsFocused.ExitActions.Add(beginSbExit);

            style.Triggers.Add(multi);
            style.Setters.Add(new Setter(MarginProperty, new Thickness(0, 0, -searchBtn.Width, 0)));
            style.Setters.Add(new Setter(BackgroundProperty, Brushes.Transparent));
            style.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(0)));

            searchBtn.Style = style;
        }
    }
}
