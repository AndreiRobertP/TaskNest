using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace TaskNest.Models
{
    [Serializable]
    public class ToDoList : IToDoListNode, INotifyPropertyChanged
    {
        private string _name;
        [XmlAttribute]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        private int _iconId;
        [XmlAttribute]
        public int IconId
        {
            get => _iconId;
            set
            {
                _iconId = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(IconUriStr));
            }
        }
        public string IconUriStr => $"/Icons/{IconId}.png";


        private BindingList<ToDoTask> _tasks;
        [XmlArray]
        public BindingList<ToDoTask> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                NotifyPropertyChanged();
            }
        }


        private BindingList<ToDoList> _subTasks;
        [XmlArray]
        public BindingList<ToDoList> SubLists
        {
            get => _subTasks;
            set
            {
                _subTasks = value;
                NotifyPropertyChanged();
            }
        }

        private IToDoListNode _parent;
        [XmlIgnore]
        public IToDoListNode Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                NotifyPropertyChanged();
            }
        }

        public ToDoList(string name, int iconId, IToDoListNode parent)
        {
            Name = name;
            IconId = iconId;
            Tasks = new BindingList<ToDoTask>();
            SubLists = new BindingList<ToDoList>();
            Parent = parent;
        }

        public ToDoList()
        {

        }

        public ObservableCollection<ToDoList> GetToDoListsSubtree()
        {
            ObservableCollection<ToDoList> result = new ObservableCollection<ToDoList>();

            foreach (var sublist in SubLists)
            {
                result.Add(sublist);
                foreach (var subsublist in sublist.GetToDoListsSubtree())
                {
                    result.Add(subsublist);
                }
            }

            return result;
        }

        public ObservableCollection<ToDoTask> GetToDoTasksSubtree()
        {
            ObservableCollection<ToDoTask> result = new ObservableCollection<ToDoTask>();

            foreach (var task in Tasks)
            {
                result.Add(task);
            }

            foreach (var sublist in SubLists)
            {
                foreach (var subsubtask in sublist.GetToDoTasksSubtree())
                {
                    result.Add(subsubtask);
                }
            }

            return result;
        }

        public ObservableCollection<TDTSearchResult> FindTask(Func<ToDoTask, bool> predWhatFind)
        {
            ObservableCollection<TDTSearchResult> result = new ObservableCollection<TDTSearchResult>();
            foreach (var tsk in Tasks)
            {
                if (predWhatFind(tsk))
                    result.Add(new TDTSearchResult(tsk, this.Name));
            }

            foreach (var lst in SubLists)
            {
                var partialResultsSublist = lst.FindTask(predWhatFind);
                foreach (var pr in partialResultsSublist)
                {
                    result.Add(new TDTSearchResult(pr.Task, this.Name + " > " + pr.Path));
                }
            }

            return result;
        }

        public BindingList<ToDoList> GetDirectDescendentsSublists()
        {
            return SubLists;
        }


        public void AddSublist(ToDoList list)
        {
            SubLists.Add(list);
        }

        public void RemoveSublist(ToDoList list)
        {
            SubLists.Remove(list);
        }

        public void SortTasksBy(Func<ToDoTask, ToDoTask, bool> predSortByCrit)
        {
            foreach (var sublist in SubLists)
            {
                sublist.SortTasksBy(predSortByCrit);
            }

            for (int i = 0; i < Tasks.Count; i++)
            {
                for (int j = i + 1; j < Tasks.Count; j++)
                {
                    if (!predSortByCrit(Tasks[i], Tasks[j]))
                    {
                        (Tasks[i], Tasks[j]) = (Tasks[j], Tasks[i]);
                    }
                }
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class TDTSearchResult
    {
        public ToDoTask Task { get; set; }
        public string Path { get; set; }

        public TDTSearchResult(ToDoTask task, string path)
        {
            Task = task;
            Path = path;
        }
    }
}
