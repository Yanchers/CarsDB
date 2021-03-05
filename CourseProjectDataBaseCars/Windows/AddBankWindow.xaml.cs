﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourseProjectDataBaseCars
{
    /// <summary>
    /// Логика взаимодействия для AddCarWindow.xaml
    /// </summary>
    public partial class AddBankWindow : Window
    {
        public AddBankWindow(int bankId = 0)
        {
            InitializeComponent();
            DataContext = new AddBankWindowViewModel(bankId);
        }
    }
}
