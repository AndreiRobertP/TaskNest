using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for TaskEditView.xaml
    /// </summary>
    public partial class TaskEditView : Window
    {
        public TaskEditVM Tevm { get; set; }

        public TaskEditView(ObservableCollection<ToDoTask> listOfTasks, ToDoList parent, ToDoTask originalTask = null)
        {
            InitializeComponent();
            Tevm = new TaskEditVM(listOfTasks, parent, originalTask);
            DataContext = Tevm;

            CmbCategory.ItemsSource = Tevm.AvaliableCategories;
            CmbPriority.ItemsSource = Enum.GetNames(typeof(EPriority));
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (!ToDoTaskService.ValidateTask(Tevm.CurrentTask, Tevm.ListOfTasks, Tevm.OriginalTask))
            {
                MessageBox.Show("Please enter a unique name for the task", "Name already existing", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            ToDoTaskService.AddTaskToList(Tevm.CurrentTask, Tevm.ParentList, Tevm.OriginalTask);

            DialogResult = true;
            Close();
            MessageBox.Show("Task saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CmbCategory_OnLoaded(object sender, RoutedEventArgs e)
        {
            CmbCategory.SelectedIndex = Categories.GetCategoryIndex(Tevm.CurrentTask.Category);
        }

        private void CmbPriority_OnLoaded(object sender, RoutedEventArgs e)
        {
            CmbPriority.SelectedIndex = (int)Tevm.CurrentTask.Priority;
        }
    }
}
