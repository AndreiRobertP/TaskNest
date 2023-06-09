﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using TaskNest.Models;

namespace TaskNest.Services
{
    public class ToDoListService
    {
        public static bool ValidateToDoList(ObservableCollection<ToDoList> allLists, string name, ToDoList oldList)
        {
            //Validate name
            if (string.IsNullOrEmpty(name))
                return false;

            foreach (var list in allLists)
            {
                if (list.Name.Equals(name))
                {
                    if (oldList == null)
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
            if (listToMove.Parent == newParent)
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

        public static void MoveList(ToDoList listToMove, BindingList<ToDoList> container, bool moveUp)
        {
            var originalElementPosition = container.IndexOf(listToMove);

            if(!CanMoveList(listToMove, container, moveUp))
                return;

            if (moveUp)
            {
                var tmp = container[originalElementPosition - 1];
                container[originalElementPosition - 1] = listToMove;
                container[originalElementPosition] = tmp;
            }
            else
            {
                var tmp = container[originalElementPosition + 1];
                container[originalElementPosition + 1] = listToMove;
                container[originalElementPosition] = tmp;
            }
        }

        public static bool CanMoveList(ToDoList listToMove, BindingList<ToDoList> container, bool moveUp)
        {
            var originalElementPosition = container.IndexOf(listToMove);

            if (originalElementPosition == -1)
                return false;

            if (moveUp && originalElementPosition == 0)
                return false;

            if (!moveUp && originalElementPosition == container.Count - 1)
                return false;

            return true;
        }
    }
}
