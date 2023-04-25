using System;
using System.Windows;
using System.Windows.Controls;
using TaskNest.Models;
using TaskNest.Services;
using TaskNest.ViewModels;

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

        protected override void OnClosed(EventArgs e)
        {
            ToDoDatabaseService.SaveChangesToCurrentDatabase(Mvvm.Db);
            base.OnClosed(e);
        }

        private void MniFileExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MniAbout_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Popa Andrei Robert" + Environment.NewLine + "10LF313" + Environment.NewLine +
                            "andrei-robert.popa@student.unitbv.ro");
        }
    }
}
