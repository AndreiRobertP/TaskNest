using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TaskNest.Models;
using TaskNest.Services;

namespace TaskNest.ViewModels
{
    public class MainWindowVm : INotifyPropertyChanged
    {
        public ToDoDatabase Db { get; set; }

        // Filtering criterium
        private Func<ToDoTask, bool> _taskFilteringCriterium = task => true;
        public Func<ToDoTask, bool> TaskFilteringCriterium
        {
            get => _taskFilteringCriterium;
            set
            {
                _taskFilteringCriterium = value;
                NotifyPropertyChangedTasks();
            }
        }

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
                BindingList<ToDoTask> response = new BindingList<ToDoTask>();

                if (CurrentToDoList == null)
                    return response;

                foreach (var task in CurrentToDoList.Tasks)
                {
                    if(TaskFilteringCriterium(task))
                        response.Add(task);
                }

                return response;
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

        public void NotifyPropertyChangedStatistics()
        {
            StatsVM.NotifyPropertyChangedStatistics();
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

        private RelayCommand _cmdOpenDatabase;

        public RelayCommand CmdOpenDatabase
        {
            get
            {
                if (_cmdOpenDatabase != null)
                    return _cmdOpenDatabase;

                _cmdOpenDatabase = new RelayCommand(
                    () => { }
                );
            }
        } 
    }
}
