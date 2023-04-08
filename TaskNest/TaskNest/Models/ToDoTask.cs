using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace TaskNest.Models
{
    [Serializable]
    public class ToDoTask: INotifyPropertyChanged
    {
        private string _name;
        [XmlAttribute]
        public string Name {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        private string _description;
        [XmlAttribute]
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
        [XmlAttribute]
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
        [XmlAttribute]
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
        [XmlAttribute]
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
        [XmlAttribute]
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

        public ToDoTask()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
