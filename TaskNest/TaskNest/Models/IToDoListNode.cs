using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskNest.Models
{
    public interface IToDoListNode
    {
        ObservableCollection<ToDoList> GetToDoListsSubtree();
        ObservableCollection<ToDoTask> GetToDoTasksSubtree();
        ObservableCollection<TDTSearchResult> FindTask(Func<ToDoTask, bool> predWhatFind);

        void AddSublist(ToDoList list);
        void RemoveSublist(ToDoList list);
    }
}
