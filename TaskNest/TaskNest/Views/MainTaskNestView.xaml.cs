using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            Mvvm.NotifyPropertyChangedStatistics();
        }

        private void DtgTasks_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Mvvm.NotifyPropertyChangedStatistics();
        }

        private void MniTaskAdd_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoList == null)
                return;

            TaskEditView editWindow = new TaskEditView(Mvvm.AllTasks, Mvvm.CurrentToDoList);
            var result = editWindow.ShowDialog();
            if (result.HasValue && result.Value)
            {
                Mvvm.NotifyPropertyChangedStatistics();
            }
        }

        private void MniTaskEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoList == null)
                return;

            if (Mvvm.CurrentToDoTask == null)
                return;

            TaskEditView editWindow = new TaskEditView(Mvvm.AllTasks, Mvvm.CurrentToDoList, Mvvm.CurrentToDoTask);
            var result = editWindow.ShowDialog();
            if (result.HasValue && result.Value)
            {
                Mvvm.NotifyPropertyChangedStatistics();
            }
        }

        private void MniTaskToggle_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoTask == null)
                return;

            ToDoTaskService.ToggleTaskDone(Mvvm.CurrentToDoTask);
            Mvvm.NotifyPropertyChangedStatistics();
        }

        private void MniTaskMoveUp_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoTask == null || Mvvm.CurrentToDoList == null)
                return;

            ToDoTaskService.MoveTask(Mvvm.CurrentToDoTask, Mvvm.CurrentToDoList, true);
        }

        private void MniTaskMoveDown_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoTask == null || Mvvm.CurrentToDoList == null)
                return;

            ToDoTaskService.MoveTask(Mvvm.CurrentToDoTask, Mvvm.CurrentToDoList, false);
        }

        private void MniTaskDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mvvm.CurrentToDoTask == null || Mvvm.CurrentToDoList == null)
                return;

            ToDoTaskService.DeleteTask(Mvvm.CurrentToDoTask, Mvvm.CurrentToDoList);
        }
    }
}
