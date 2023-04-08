using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskNest.Models;

namespace TaskNest.ViewModels
{
    public class StatisticsVM: INotifyPropertyChanged
    {
        public ToDoDatabase Db { get; set; }

        // Stats
        public string StDueToday => $"Tasks due today: {Db.GetNumSubTasksByPredicate((tsk) => !tsk.IsDone && tsk.DueDateTime.Date == DateTime.Today.Date)}";
        public string StDueTomorrow => $"Tasks due tomorrow: {Db.GetNumSubTasksByPredicate((tsk) => !tsk.IsDone && tsk.DueDateTime.Date == DateTime.Today.Date + TimeSpan.FromDays(1))}";
        public string StDueOverdue => $"Tasks overdue: {Db.GetNumSubTasksByPredicate((tsk) => !tsk.IsDone && tsk.DueDateTime.Date < DateTime.Today.Date)}";
        public string StDone => $"Tasks done: {Db.GetNumSubTasksByPredicate((tsk) => tsk.IsDone)}";
        public string StNotDone => $"Tasks to be done: {Db.GetNumSubTasksByPredicate((tsk) => !tsk.IsDone)}";

        public StatisticsVM(ToDoDatabase db)
        {
            Db = db;
        }

        public void NotifyPropertyChangedStatistics()
        {
            NotifyPropertyChanged("StDueToday");
            NotifyPropertyChanged("StDueTomorrow");
            NotifyPropertyChanged("StDueOverdue");
            NotifyPropertyChanged("StDone");
            NotifyPropertyChanged("StNotDone");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
