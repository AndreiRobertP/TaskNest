using System.Windows;
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
