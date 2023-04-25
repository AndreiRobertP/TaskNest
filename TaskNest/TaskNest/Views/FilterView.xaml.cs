using System;
using System.Windows;
using TaskNest.Models;
using TaskNest.ViewModels;

namespace TaskNest.Views
{
    /// <summary>
    /// Interaction logic for FilterView.xaml
    /// </summary>
    public partial class FilterView : Window
    {
        public FilterVM Fvm { get; set; }
        public String SelectedCategory => Fvm.CurrentCat;

        public FilterView(Categories cats)
        {
            InitializeComponent();

            Fvm = new FilterVM(cats);
            DataContext = Fvm;
        }


        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
