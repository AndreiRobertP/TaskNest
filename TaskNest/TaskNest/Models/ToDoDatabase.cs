using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace TaskNest.Models
{
    [Serializable]
    public class ToDoDatabase : IToDoListNode, INotifyPropertyChanged
    {
        private BindingList<ToDoList> _rootLists;
        [XmlArray]
        public BindingList<ToDoList> RootLists
        {
            get => _rootLists;
            set
            {
                _rootLists = value;
                NotifyPropertyChanged();
            }
        }

        private Categories _categories;
        [XmlElement]
        public Categories Categories { get => _categories;
            set
            {
                _categories = value;
                NotifyPropertyChanged();
            }
        }

        public ToDoDatabase()
        {
            RootLists = new BindingList<ToDoList>();
            Categories = new Categories();
            Categories.LoadCats(new List<string>(){"None"});
        }

        public ObservableCollection<ToDoList> GetToDoListsSubtree()
        {
            ObservableCollection<ToDoList> result = new ObservableCollection<ToDoList>();

            foreach (var sublist in RootLists)
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

            foreach (var sublist in RootLists)
            {
                foreach (var subsubtask in sublist.GetToDoTasksSubtree())
                {
                    result.Add(subsubtask);
                }
            }

            return result;
        }

        public int GetNumSubTasksByPredicate(Func<ToDoTask, bool> predicate)
        {
            var allTasks = GetToDoTasksSubtree();

            int count = 0;
            foreach (var tsk in allTasks)
            {
                if (predicate(tsk))
                    count++;
            }

            return count;
        }

        public ObservableCollection<TDTSearchResult> FindTask(Func<ToDoTask, bool> predWhatFind)
        {
            ObservableCollection<TDTSearchResult> result = new ObservableCollection<TDTSearchResult>();

            foreach (var lst in RootLists)
            {
                var partialResultsSublist = lst.FindTask(predWhatFind);
                foreach (var pr in partialResultsSublist)
                {
                    result.Add(pr);
                }
            }

            return result;
        }

        public BindingList<ToDoList> GetDirectDescendentsSublists()
        {
            return RootLists;
        }

        public void AddSublist(ToDoList list)
        {
            RootLists.Add(list);
        }

        public void RemoveSublist(ToDoList list)
        {
            RootLists.Remove(list);
        }

        public void SortTasksBy(Func<ToDoTask, ToDoTask, bool> predSortByCrit)
        {
            foreach (var sublist in RootLists)
            {
                sublist.SortTasksBy(predSortByCrit);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public override string ToString()
        {
            return "_Root";
        }
    }
}
