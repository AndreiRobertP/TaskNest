using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskNest.Models
{
    public class ToDoTask: INotifyPropertyChanged
    {
        private string _name;
        public string Name {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                NotifyPropertyChanged();
            }
        }

        private EPriority _priority;
        public EPriority Priority
        {
            get => _priority;
            set
            {
                _priority = value;
                NotifyPropertyChanged();
            }
        }

        private string _category;
        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime _dueDateTime;

        public DateTime DueDateTime
        {
            get => _dueDateTime;
            set
            {
                _dueDateTime = value;
                NotifyPropertyChanged();
            }
        }

        public string DueDate => DueDateTime.ToString("d/M/yy");


        private bool _isDone;
        public bool IsDone
        {
            get => _isDone;
            set
            {
                _isDone = value;
                NotifyPropertyChanged();
            }
        }

        public ToDoTask(string name, string description, EPriority priority, string category, DateTime dueDateTime, bool isDone = false)
        {
            Name = name;
            Description = description;
            Priority = priority;
            Category = category;
            DueDateTime = dueDateTime;
            IsDone = isDone;
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
