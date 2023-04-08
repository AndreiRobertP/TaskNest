using System.Collections.ObjectModel;
using TaskNest.Models;
using TaskNest.Services;

namespace TaskNest.ViewModels
{
    public class ListChangePathVM
    {
        public ObservableCollection<IToDoListNode> AllNodes { get; set; }
        public ToDoList ListToMove { get; set; }
        public IToDoListNode CurrentSelected { get; set; }

        public ListChangePathVM(ToDoDatabase db, ToDoList listToMove)
        {
            ListToMove = listToMove;
            CurrentSelected = ListToMove.Parent;

            InitAllNodes(db);
        }

        void InitAllNodes(ToDoDatabase db)
        {
            var allLists = db.GetToDoListsSubtree();
            AllNodes = new ObservableCollection<IToDoListNode>() { db };
            foreach (var list in allLists)
            {
                if (!list.Equals(ListToMove) && !ToDoListService.IsDescendentOf(list, ListToMove))
                    AllNodes.Add(list);
            }
        }
    }
}
