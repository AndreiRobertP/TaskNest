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
                var newList = new ToDoList(name, iconId);
                parent.AddSublist(newList);
            }
        }

        public static void RemoveList(IToDoListNode parent, ToDoList oldList)
        {
            
        }
    }
}
