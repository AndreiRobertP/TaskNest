using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskNest.Models;

namespace TaskNest.ViewModels
{
    public class ListEditVM
    {
        public ToDoList OldToDoList { get; set; }
        public ObservableCollection<ToDoList> AllLists { get; set; }
        public IToDoListNode Parent { get; set; }

        public string CurrentName { get; set; }
        public int CurrentIconId { get; set; }

        public static ObservableCollection<string> Icons { get; set; } = new ObservableCollection<string>() { "Home", "Work", "Learning", "Shopping", "Hobby", "Travel", "Entertainment" };

        public ListEditVM(IToDoListNode parent, ObservableCollection<ToDoList> allLists, ToDoList oldToDoList)
        {
            Parent = parent;
            OldToDoList = oldToDoList;
            AllLists = allLists;

            if (OldToDoList != null)
            {
                CurrentName = oldToDoList.Name;
                CurrentIconId = oldToDoList.IconId;
            }
            else
            {
                CurrentName = "";
                CurrentIconId = 0;
            }
        }
    }
}
