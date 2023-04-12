using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskNest.Models;
using TaskNest.Services;

namespace TaskNest.ViewModels
{
    public class MainWindowVm : INotifyPropertyChanged
    {
        public ToDoDatabase Db { get; set; }

        // Current List
        private ToDoList _currentToDoList;
        public ToDoList CurrentToDoList
        {
            get => _currentToDoList;
            set
            {
                _currentToDoList = value;
                NotifyPropertyChanged("Tasks");
            }
        }
        public BindingList<ToDoTask> Tasks
        {
            get
            {
                if (CurrentToDoList == null)
                    return new BindingList<ToDoTask>();

                return CurrentToDoList.Tasks;
            }
        }
        public IToDoListNode CurrentNodeParent => CurrentToDoList.Parent;


        // Current Task
        private ToDoTask _currentToDoTask;
        public ToDoTask CurrentToDoTask
        {
            get => _currentToDoTask;
            set
            {
                _currentToDoTask = value;
                NotifyPropertyChanged("TaskContent");
            }
        }
        public string TaskContent
        {
            get
            {
                if (CurrentToDoTask == null)
                    return "";
                return CurrentToDoTask.Description;
            }
        }


        // TreeView Navigation Content
        public BindingList<ToDoList> ToDoLists => Db.RootLists;

        //Extra
        public ObservableCollection<ToDoTask> AllTasks => Db.GetToDoTasksSubtree();

        //Statistics
        public StatisticsVM StatsVM { get; set; }

        public MainWindowVm()
        {
            Db = new ToDoDatabase();
            StatsVM = new StatisticsVM(Db);

            ToDoDatabaseService.DeserializeObject(Db, ToDoDatabaseService.GetLastDatabaseFilepath());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyPropertyChangedTasks()
        {
            NotifyPropertyChanged("Tasks");
            StatsVM.NotifyPropertyChangedStatistics();
        }

        public void NotifyPropertyChangedLists()
        {
            NotifyPropertyChanged("ToDoLists");
            NotifyPropertyChangedTasks();
        }

        public void NotifyPropertyChangedStatistics()
        {
            StatsVM.NotifyPropertyChangedStatistics();
        }
    }
}
