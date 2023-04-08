using System;
using System.Collections.ObjectModel;

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
