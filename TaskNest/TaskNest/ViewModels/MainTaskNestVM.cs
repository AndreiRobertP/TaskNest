using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TaskNest.Models;

namespace TaskNest.ViewModels
{
    public class MainWindowVm : INotifyPropertyChanged
    {
        private ToDoDatabase Db { get; set; }

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
        public ObservableCollection<ToDoTask> Tasks
        {
            get
            {
                if (CurrentToDoList == null)
                    return new ObservableCollection<ToDoTask>();

                return CurrentToDoList.Tasks;
            }
        }

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
        public ObservableCollection<ToDoList> ToDoLists => Db.RootLists;


        // Stats
        public string StDueToday {
            get
            {
                return $"Tasks due today: {Db.GetNumSubTasksByPredicate((tsk) => !tsk.IsDone && tsk.DueDateTime.Date == DateTime.Today.Date)}";
            }
        }
        public string StDueTomorrow
        {
            get
            {
                return $"Tasks due tomorrow: {Db.GetNumSubTasksByPredicate((tsk) => !tsk.IsDone && tsk.DueDateTime.Date == DateTime.Today.Date + TimeSpan.FromDays(1))}";
            }
        }
        public string StDueOverdue
        {
            get
            {
                return $"Tasks overdue: {Db.GetNumSubTasksByPredicate((tsk) => !tsk.IsDone && tsk.DueDateTime.Date < DateTime.Today.Date)}";
            }
        }
        public string StDone
        {
            get
            {
                return $"Tasks done: {Db.GetNumSubTasksByPredicate((tsk) => tsk.IsDone)}";
            }
        }
        public string StNotDone
        {
            get
            {
                return $"Tasks to be done: {Db.GetNumSubTasksByPredicate((tsk) => !tsk.IsDone)}";
            }
        }

        //Extra
        public ObservableCollection<ToDoTask> AllTasks => Db.GetToDoTasksSubtree();

        public MainWindowVm()
        {
            Db = new ToDoDatabase();

            var td1 = new ToDoList("RC", 2);
            var td2 = new ToDoList("MVP", 2);
            var td3 = new ToDoList("Home", 1);

            Db.RootLists.Add(td1);
            Db.RootLists.Add(td2);

            var std1 = new ToDoList("Token Ring Homework", 2);
            std1.SubLists.Add(new ToDoList("Research", 2));
            std1.SubLists.Add(new ToDoList("Implement", 2));
            std1.SubLists.Add(new ToDoList("Present", 2));

            td1.SubLists.Add(std1);
            td1.SubLists.Add(new ToDoList("Review IPv4", 2));

            td2.SubLists.Add(new ToDoList("Homework 2", 2));

            td3.SubLists.Add(new ToDoList("Clean", 1));
            td3.Tasks.Add(new ToDoTask("Clean bedroom", ":)", EPriority.Low, ECategory.Work, DateTime.Today));
            td3.Tasks.Add(new ToDoTask("Clean kitchen", "Don't forget about refrigerator", EPriority.High, ECategory.Work, DateTime.Today));
            td3.Tasks.Add(new ToDoTask("Do the dishes", "Check if there is any Fairy left", EPriority.High, ECategory.Work, DateTime.Today));
            Db.RootLists.Add(td3);

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyPropertyChangedStatistics()
        {
            NotifyPropertyChanged("StDueToday");
            NotifyPropertyChanged("StDueTomorrow");
            NotifyPropertyChanged("StDueOverdue");
            NotifyPropertyChanged("StDone");
            NotifyPropertyChanged("StNotDone");
        }
    }
}
