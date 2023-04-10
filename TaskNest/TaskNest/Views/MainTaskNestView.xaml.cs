using System.Windows;
using System.Windows.Controls;
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
            var result = editWindow.ShowDialog();
            if (result.HasValue && result.Value)
            {
                Mvvm.NotifyPropertyChangedTasks();
            }
        }

        private void MniTaskEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoList == null)
                return;

            if (Mvvm.CurrentToDoTask == null)
                return;

            TaskEditView editWindow = new TaskEditView(Mvvm.AllTasks, Mvvm.Db.Categories, Mvvm.CurrentToDoList, Mvvm.CurrentToDoTask);
            var result = editWindow.ShowDialog();
            if (result.HasValue && result.Value)
            {
                Mvvm.NotifyPropertyChangedTasks();
            }
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

        private void MniFileArchive_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "TaskNest Database Files | *.xml";
            saveFileDialog.Title = "Choose ToDODatabase";

            var resp = saveFileDialog.ShowDialog();
            if (!resp.HasValue || resp.Value == false)
                return;

            ToDoDatabaseService.SerializeDatabase(Mvvm.Db, saveFileDialog.FileName);
        }

        private void MniFileOpen_OnClick(object sender, RoutedEventArgs e)
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
    }
}
