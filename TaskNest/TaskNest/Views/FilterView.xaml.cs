using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
