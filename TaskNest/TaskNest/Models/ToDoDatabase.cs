using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace TaskNest.Models
{
    public class ToDoDatabase
    {
        public ObservableCollection<ToDoList> RootLists { get; set; }
        public ToDoDatabase()
        {
            RootLists = new ObservableCollection<ToDoList>();
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
    }
}
