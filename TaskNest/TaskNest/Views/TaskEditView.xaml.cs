using System;
using System.Collections.ObjectModel;
using System.Windows;
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

        public TaskEditView(ObservableCollection<ToDoTask> listOfTasks, Categories categories, ToDoList parent, ToDoTask originalTask = null)
        {
            InitializeComponent();
            Tevm = new TaskEditVM(listOfTasks, categories, parent, originalTask);
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
            CmbCategory.SelectedIndex = Tevm.Categories.GetCategoryIndex(Tevm.CurrentTask.Category);
        }

        private void CmbPriority_OnLoaded(object sender, RoutedEventArgs e)
        {
            CmbPriority.SelectedIndex = (int)Tevm.CurrentTask.Priority;
        }
    }
}
