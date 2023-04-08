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
using TaskNest.Services;
using TaskNest.ViewModels;

namespace TaskNest.Views
{
    /// <summary>
    /// Interaction logic for ListChangePathView.xaml
    /// </summary>
    public partial class ListChangePathView : Window
    {
        private ListChangePathVM Lcpvm { get; set; }

        public ListChangePathView(ToDoDatabase db, ToDoList listToMove)
        {
            InitializeComponent();

            Lcpvm = new ListChangePathVM(db, listToMove);
            DataContext = Lcpvm;
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            ToDoListService.ChangePath(Lcpvm.ListToMove, Lcpvm.CurrentSelected);

            DialogResult = true;
            Close();
            MessageBox.Show("List moved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
