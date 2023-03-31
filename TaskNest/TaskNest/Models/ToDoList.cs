using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TaskNest.Models
{
    public class ToDoList
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
    }
}
