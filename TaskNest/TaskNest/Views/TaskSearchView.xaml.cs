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
    /// Interaction logic for TaskSearchView.xaml
    /// </summary>
    public partial class TaskSearchView : Window
    {
        public TaskSearchVM Tsvm { get; set; }

        public TaskSearchView()
        {
            InitializeComponent();

            Tsvm = DataContext as TaskSearchVM;
        }

        public void SetDatabase(ToDoDatabase toDoDatabase)
        {
            Tsvm.SetDatabase(toDoDatabase);
        }

        private void BtnSearch_OnClick(object sender, RoutedEventArgs e)
        {
            Tsvm.RunSearch();

            MessageBox.Show($"The search resulted in {Tsvm.SearchResults.Count} results");
        }
    }
}
