﻿using System;
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

        protected override void OnClosed(EventArgs e)
        {
            ToDoDatabaseService.SaveChangesToCurrentDatabase(Mvvm.Db);
            base.OnClosed(e);
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
            Mvvm.TaskFilteringCriterium = task => task.DoneDateTime > task.DueDateTime;
        }

        private void MniFilterExcDeadline_OnClick(object sender, RoutedEventArgs e)
        {
            Mvvm.TaskFilteringCriterium = task => !task.IsDone && task.DueDateTime < DateTime.Now;
        }

        private void MniFilterToBeDone_OnClick(object sender, RoutedEventArgs e)
        {
            Mvvm.TaskFilteringCriterium = task => !task.IsDone && task.DueDateTime >= DateTime.Now;
        }

        private void MniAbout_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Popa Andrei Robert" + Environment.NewLine + "10LF313" + Environment.NewLine +
                            "andrei-robert.popa@student.unitbv.ro");
        }
    }
}
