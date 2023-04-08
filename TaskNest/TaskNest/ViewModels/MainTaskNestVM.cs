using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskNest.Models;

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


        // Stats
        public string StDueToday => $"Tasks due today: {Db.GetNumSubTasksByPredicate((tsk) => !tsk.IsDone && tsk.DueDateTime.Date == DateTime.Today.Date)}";
        public string StDueTomorrow => $"Tasks due tomorrow: {Db.GetNumSubTasksByPredicate((tsk) => !tsk.IsDone && tsk.DueDateTime.Date == DateTime.Today.Date + TimeSpan.FromDays(1))}";
        public string StDueOverdue => $"Tasks overdue: {Db.GetNumSubTasksByPredicate((tsk) => !tsk.IsDone && tsk.DueDateTime.Date < DateTime.Today.Date)}";
        public string StDone => $"Tasks done: {Db.GetNumSubTasksByPredicate((tsk) => tsk.IsDone)}";
        public string StNotDone => $"Tasks to be done: {Db.GetNumSubTasksByPredicate((tsk) => !tsk.IsDone)}";

        //Extra
        public ObservableCollection<ToDoTask> AllTasks => Db.GetToDoTasksSubtree();

        public MainWindowVm()
        {
            Db = new ToDoDatabase();

            var td1 = new ToDoList("RC", 2, Db);
            var td2 = new ToDoList("MVP", 2, Db);
            var td3 = new ToDoList("Home", 1, Db);

            Db.RootLists.Add(td1);
            Db.RootLists.Add(td2);

            var std1 = new ToDoList("Token Ring Homework", 2, td1);
            std1.SubLists.Add(new ToDoList("Research", 2, std1));
            std1.SubLists.Add(new ToDoList("Implement", 2, std1));
            std1.SubLists.Add(new ToDoList("Present", 2, std1));

            td1.SubLists.Add(std1);
            td1.SubLists.Add(new ToDoList("Review IPv4", 2, td1));

            td2.SubLists.Add(new ToDoList("Homework 2", 2, td2));

            td3.SubLists.Add(new ToDoList("Clean", 1, td3));
            td3.Tasks.Add(new ToDoTask("Clean bedroom", ":)", EPriority.Low, "Work", DateTime.Today));
            td3.Tasks.Add(new ToDoTask("Clean kitchen", "Don't forget about refrigerator", EPriority.High, "Work", DateTime.Today));
            td3.Tasks.Add(new ToDoTask("Do the dishes", "Check if there is any Fairy left", EPriority.High, "Work", DateTime.Today));
            Db.RootLists.Add(td3);


            Categories.LoadCats(new List<string>(){"", "Home", "Work", "School"});
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

        public void NotifyPropertyChangedTasks()
        {
            NotifyPropertyChanged("Tasks");
            NotifyPropertyChangedStatistics();
        }

        public void NotifyPropertyChangedLists()
        {
            NotifyPropertyChanged("ToDoLists");
            NotifyPropertyChangedTasks();
        }
    }
}
