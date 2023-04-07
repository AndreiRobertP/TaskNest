using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskNest.Models;
using static System.String;

namespace TaskNest.ViewModels
{
    public class TaskEditVM
    {
        public ObservableCollection<ToDoTask> ListOfTasks { get; set; }
        public ToDoTask OriginalTask { get; set; }
        public ToDoTask CurrentTask { get; set; }
        public ToDoList ParentList { get; set; }
        public ObservableCollection<string> AvaliableCategories => Categories.Cats;

        public TaskEditVM(ObservableCollection<ToDoTask> listOfTasks, ToDoList parent, ToDoTask originalTask)
        {
            ListOfTasks = listOfTasks;
            ParentList = parent;
            OriginalTask = originalTask;
            CurrentTask = new ToDoTask("", "", EPriority.Low, "", DateTime.Today);

            if (OriginalTask != null)
            {
                CurrentTask = new ToDoTask(
                    Copy(OriginalTask.Name),
                    Copy(OriginalTask.Description),
                    OriginalTask.Priority,
                    Copy(OriginalTask.Category),
                    OriginalTask.DueDateTime,
                    OriginalTask.IsDone
                );
            }
        }
    }
}
