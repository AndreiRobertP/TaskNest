using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskNest.Models;

namespace TaskNest.Services
{
    public class ToDoListService
    {
        public static bool ValidateToDoList(ObservableCollection<ToDoList> allLists, string name, ToDoList oldList)
        {
            foreach (var list in allLists)
            {
                if (list.Name.Equals(name) && !oldList.Name.Equals(name))
                    return false;
            }

            return true;
        }

        public static void AddListToList(IToDoListNode parent, string name, int iconId, ToDoList oldList)
        {
            var newList = new ToDoList(name, iconId);

            if (oldList != null)
            {
                oldList = newList;
            }
            else
            {
                parent.AddSublist(newList);
            }
        }

        public static void RemoveList(IToDoListNode parent, ToDoList oldList)
        {
            
        }
    }
}
