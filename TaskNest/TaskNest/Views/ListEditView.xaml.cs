using System.Collections.ObjectModel;
using System.Windows;
using TaskNest.Models;
using TaskNest.Services;
using TaskNest.ViewModels;

namespace TaskNest.Views
{
    /// <summary>
    /// Interaction logic for ListEditView.xaml
    /// </summary>
    public partial class ListEditView : Window
    {
        private ListEditVM Levm { get; set; }

        public ListEditView(IToDoListNode parent, ObservableCollection<ToDoList> allLists, ToDoList oldToDoList)
        {
            InitializeComponent();

            Levm = new ListEditVM(parent, allLists, oldToDoList);
            DataContext = Levm;
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (!ToDoListService.ValidateToDoList(Levm.AllLists, Levm.CurrentName, Levm.OldToDoList))
            {
                MessageBox.Show("Please enter a unique name for the task", "Name already existing", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            ToDoListService.AddListToList(Levm.Parent, Levm.CurrentName, Levm.CurrentIconId, Levm.OldToDoList);

            DialogResult = true;
            Close();
            MessageBox.Show("List saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
