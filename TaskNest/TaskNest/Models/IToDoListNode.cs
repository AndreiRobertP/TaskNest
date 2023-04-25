using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TaskNest.Models
{
    public interface IToDoListNode
    {
        ObservableCollection<ToDoList> GetToDoListsSubtree();
        ObservableCollection<ToDoTask> GetToDoTasksSubtree();
        ObservableCollection<TDTSearchResult> FindTask(Func<ToDoTask, bool> predWhatFind);
        BindingList<ToDoList> GetDirectDescendentsSublists();

        void AddSublist(ToDoList list);
        void RemoveSublist(ToDoList list);
        void SortTasksBy(Func<ToDoTask, ToDoTask, bool> predSortByCrit);


    }
}
