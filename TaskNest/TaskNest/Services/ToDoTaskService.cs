﻿using System.Collections.ObjectModel;
using TaskNest.Models;

namespace TaskNest.Services
{
    public class ToDoTaskService
    {
        public static bool ValidateTask(ToDoTask taskToValidate, ObservableCollection<ToDoTask> allTasks,
            ToDoTask originalTask = null)
        {
            //Validate name
            foreach (var tsk in allTasks)
            {
                if (tsk.Name == taskToValidate.Name)
                {
                    if (originalTask != null && originalTask.Name == taskToValidate.Name)
                        break;
                    return false;
                }
            }

            return true;
        }

        public static void AddTaskToList(ToDoTask taskToAdd, ToDoList listParent, ToDoTask originalTask = null)
        {
            while (originalTask != null)
            {
                var originalElementPosition = listParent.Tasks.IndexOf(originalTask);
                if (originalElementPosition == -1)
                    break;

                listParent.Tasks[originalElementPosition] = taskToAdd;

                return;
            }

            listParent.Tasks.Add(taskToAdd);
        }

        public static void ToggleTaskDone(ToDoTask taskToToggle)
        {
            taskToToggle.IsDone = !taskToToggle.IsDone;
        }

        public static void MoveTask(ToDoTask taskToMove, ToDoList list, bool moveUp)
        {
            var originalElementPosition = list.Tasks.IndexOf(taskToMove);
            
            if (originalElementPosition == -1)
                return;

            if (moveUp && originalElementPosition == 0)
                return;

            if (!moveUp && originalElementPosition == list.Tasks.Count - 1)
                return;

            if (moveUp)
            {
                var tmp = list.Tasks[originalElementPosition - 1];
                list.Tasks[originalElementPosition - 1] = taskToMove;
                list.Tasks[originalElementPosition] = tmp;
            }
            else
            {
                var tmp = list.Tasks[originalElementPosition + 1];
                list.Tasks[originalElementPosition + 1] = taskToMove;
                list.Tasks[originalElementPosition] = tmp;
            }
        }

        public static void DeleteTask(ToDoTask taskToDelete, ToDoList list)
        {
            list.Tasks.Remove(taskToDelete);
        }
    }
}