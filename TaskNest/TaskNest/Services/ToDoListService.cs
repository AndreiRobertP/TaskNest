using System.Collections.ObjectModel;
using TaskNest.Models;

namespace TaskNest.Services
{
    public class ToDoListService
    {
        public static bool ValidateToDoList(ObservableCollection<ToDoList> allLists, string name, ToDoList oldList)
        {
            foreach (var list in allLists)
            {
                if (list.Name.Equals(name))
                {
                    if(oldList == null)
                        return false;
                    if (!oldList.Name.Equals(name))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static void AddListToList(IToDoListNode parent, string name, int iconId, ToDoList oldList)
        {
            if (oldList != null)
            {
                oldList.Name = name;
                oldList.IconId = iconId;
            }
            else
            {
                var newList = new ToDoList(name, iconId, parent);
                parent.AddSublist(newList);
            }
        }

        public static void RemoveList(ToDoList listToRemove)
        {
            IToDoListNode parent = listToRemove.Parent;
            parent.RemoveSublist(listToRemove);
            listToRemove.Parent = null;
        }

        public static void ChangePath(ToDoList listToMove, IToDoListNode newParent)
        {
            if(listToMove.Parent == newParent)
                return;

            listToMove.Parent.RemoveSublist(listToMove);
            newParent.AddSublist(listToMove);
            listToMove.Parent = newParent;
        }

        public static bool IsDescendentOf(ToDoList listToCheck, IToDoListNode expectedParent)
        {
            var parent = listToCheck.Parent as ToDoList;
            while (parent != null)
            {
                if (parent == expectedParent)
                    return true;
                parent = parent.Parent as ToDoList;
            }

            return false;
        }
    }
}
