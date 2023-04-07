using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TaskNest.Models
{
    public class ToDoList : IToDoListNode
    {
        public string Name { get; set; }
        public int IconId { get; set; }
        public string IconUriStr => $"/Icons/{IconId}.png" ;
        public ObservableCollection<ToDoTask> Tasks { get; set; }
        public ObservableCollection<ToDoList> SubLists { get; set; }

        public ToDoList(string name, int iconId)
        {
            Name = name;
            IconId = iconId;
            Tasks = new ObservableCollection<ToDoTask>();
            SubLists = new ObservableCollection<ToDoList>();
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
                    result.Add(new TDTSearchResult(pr.Task,  this.Name + " > " + pr.Path));
                }
            }

            return result;
        }


        public void AddSublist(ToDoList list)
        {
            SubLists.Add(list);
        }

        public void RemoveSublist(ToDoList list)
        {
            SubLists.Remove(list);
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
