using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using TaskNest.Models;
using TaskNest.Services;
using TaskNest.ViewModels;
using TaskNest.Views;

namespace TaskNest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowVm Mvvm { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Mvvm = DataContext as MainWindowVm;
        }

        private void TvwMenu_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Mvvm.CurrentToDoList = e.NewValue as ToDoList;

        }

        private void DtgTasks_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Mvvm.CurrentToDoTask = DtgTasks.SelectedItem as ToDoTask;
        }

        private void MniStatsToggle_OnClick(object sender, RoutedEventArgs e)
        {
            StpStats.Visibility = StpStats.IsVisible ? Visibility.Collapsed : Visibility.Visible;
            Mvvm.NotifyPropertyChangedTasks();
        }

        private void MniTaskAdd_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoList == null)
                return;

            TaskEditView editWindow = new TaskEditView(Mvvm.AllTasks, Mvvm.Db.Categories, Mvvm.CurrentToDoList);
            editWindow.ShowDialog();
            Mvvm.NotifyPropertyChangedTasks();
        }

        private void MniTaskEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoList == null)
                return;

            if (Mvvm.CurrentToDoTask == null)
                return;

            TaskEditView editWindow = new TaskEditView(Mvvm.AllTasks, Mvvm.Db.Categories, Mvvm.CurrentToDoList, Mvvm.CurrentToDoTask);
            editWindow.ShowDialog();
            Mvvm.NotifyPropertyChangedTasks();
        }

        private void MniTaskToggle_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoTask == null)
                return;

            ToDoTaskService.ToggleTaskDone(Mvvm.CurrentToDoTask);
            Mvvm.NotifyPropertyChangedTasks();
        }

        private void MniTaskMoveUp_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoTask == null || Mvvm.CurrentToDoList == null)
                return;

            ToDoTaskService.MoveTask(Mvvm.CurrentToDoTask, Mvvm.CurrentToDoList, true);
            Mvvm.NotifyPropertyChangedTasks();
        }

        private void MniTaskMoveDown_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoTask == null || Mvvm.CurrentToDoList == null)
                return;

            ToDoTaskService.MoveTask(Mvvm.CurrentToDoTask, Mvvm.CurrentToDoList, false);
            Mvvm.NotifyPropertyChangedTasks();
        }

        private void MniTaskDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoTask == null || Mvvm.CurrentToDoList == null)
                return;

            ToDoTaskService.DeleteTask(Mvvm.CurrentToDoTask, Mvvm.CurrentToDoList);
            Mvvm.NotifyPropertyChangedTasks();
        }

        private void MniTaskFind_OnClick(object sender, RoutedEventArgs e)
        {
            TaskSearchView taskSearchView = new TaskSearchView();
            taskSearchView.SetDatabase(Mvvm.Db);
            taskSearchView.Show();
        }

        private void MniTaskManage_OnClick(object sender, RoutedEventArgs e)
        {
            CategoriesManageView categoriesManageView = new CategoriesManageView(Mvvm.Db);
            categoriesManageView.Show();

            Mvvm.NotifyPropertyChangedTasks();
        }

        private void MniTdlRootAdd_OnClick(object sender, RoutedEventArgs e)
        {
            ListEditView listEditView = new ListEditView(Mvvm.Db, Mvvm.Db.GetToDoListsSubtree(), null);
            listEditView.ShowDialog();

            Mvvm.NotifyPropertyChangedLists();
        }

        private void MniTdlSubAdd_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoList == null)
                return;

            ListEditView listEditView = new ListEditView(Mvvm.CurrentToDoList, Mvvm.Db.GetToDoListsSubtree(), null);
            listEditView.ShowDialog();

            Mvvm.NotifyPropertyChangedLists();
        }

        private void MniTdlEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoList == null)
                return;

            ListEditView listEditView = new ListEditView(Mvvm.CurrentToDoList, Mvvm.Db.GetToDoListsSubtree(), Mvvm.CurrentToDoList);
            listEditView.ShowDialog();
        }

        private void MniTdlDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoList == null)
                return;

            var responseSure = MessageBox.Show(
                "Are you sure you want to delete this list? This will delete all the nested lists inside with their respective tasks. This action can't be undone.",
                "Are you sure?", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (responseSure == MessageBoxResult.Cancel)
                return;

            ToDoListService.RemoveList(Mvvm.CurrentToDoList);
            Mvvm.NotifyPropertyChangedStatistics();
        }

        private void MniTdlChangePath_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoList == null)
                return;

            ListChangePathView listChangePathView = new ListChangePathView(Mvvm.Db, Mvvm.CurrentToDoList);
            listChangePathView.ShowDialog();
        }


        private void MniFileNew_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "TaskNest Database Files | *.xml";
            saveFileDialog.Title = "Choose ToDODatabase";

            var resp = saveFileDialog.ShowDialog();
            if (!resp.HasValue || resp.Value == false)
                return;

            ToDoDatabaseService.SerializeDatabase(ToDoDatabaseService.GenerateNewDatabase(), saveFileDialog.FileName);
            ToDoDatabaseService.DeserializeObject(Mvvm.Db, saveFileDialog.FileName);
            Mvvm.NotifyPropertyChangedLists();
        }

        protected override void OnClosed(EventArgs e)
        {
            ToDoDatabaseService.SaveChangesToCurrentDatabase(Mvvm.Db);
            base.OnClosed(e);
        }

        private void FileOpen_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TaskNest Database Files | *.xml";
            openFileDialog.Title = "Choose ToDoDatabase";
            openFileDialog.Multiselect = false;

            var resp = openFileDialog.ShowDialog();
            if (!resp.HasValue || resp.Value == false)
                return;

            ToDoDatabaseService.DeserializeObject(Mvvm.Db, openFileDialog.FileName);
            Mvvm.NotifyPropertyChangedLists();
        }

        private void Command_CanExecuteAlways(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void FileArchive_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "TaskNest Database Files | *.xml";
            saveFileDialog.Title = "Choose ToDODatabase";

            var resp = saveFileDialog.ShowDialog();
            if (!resp.HasValue || resp.Value == false)
                return;

            ToDoDatabaseService.SerializeDatabase(Mvvm.Db, saveFileDialog.FileName);
        }

        private void FileSave_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ToDoDatabaseService.SaveChangesToCurrentDatabase(Mvvm.Db);
        }

        private void MniFileExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MniSortDeadline_OnClick(object sender, RoutedEventArgs e)
        {
            Mvvm.Db.SortTasksBy((ToDoTask a, ToDoTask b) => a.DueDateTime < b.DueDateTime);
            Mvvm.NotifyPropertyChangedTasks();
        }

        private void MniSortPriority_OnClick(object sender, RoutedEventArgs e)
        {
            Mvvm.Db.SortTasksBy((ToDoTask a, ToDoTask b) => a.Priority < b.Priority);
            Mvvm.NotifyPropertyChangedTasks();
        }

        private void MniFilterClear_OnClick(object sender, RoutedEventArgs e)
        {
            Mvvm.TaskFilteringCriterium = task => true;
        }

        private void MniFilterCategory_OnClick(object sender, RoutedEventArgs e)
        {
            FilterView filterView = new FilterView(Mvvm.Db.Categories);
            var response = filterView.ShowDialog();

            if (!(response.HasValue && response == true))
                return;

            Mvvm.TaskFilteringCriterium = task => task.Category.Equals(filterView.SelectedCategory);
        }


        private void MniFilterDone_OnClick(object sender, RoutedEventArgs e)
        {
            Mvvm.TaskFilteringCriterium = task => task.IsDone;
        }

        private void MniFilterOverdue_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MniFilterExcDeadline_OnClick(object sender, RoutedEventArgs e)
        {
            Mvvm.TaskFilteringCriterium = task => !task.IsDone && task.DueDateTime < DateTime.Now;
        }

        private void MniFilterToBeDone_OnClick(object sender, RoutedEventArgs e)
        {
            Mvvm.TaskFilteringCriterium = task => !task.IsDone && task.DueDateTime >= DateTime.Now;
        }
    }
}
