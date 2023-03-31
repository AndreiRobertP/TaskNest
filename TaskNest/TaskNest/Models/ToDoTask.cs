﻿using System;

namespace TaskNest.Models
{
    public class ToDoTask
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int Category { get; set; }
        public DateTime DueDateTime { get; set; }

        public string DueDate => DueDateTime.ToString("d/M/yy");
        public bool IsDone { get; set; }

        public ToDoTask(string name, string description, int priority, int category, DateTime dueDateTime, bool isDone = false)
        {
            Name = name;
            Description = description;
            Priority = priority;
            Category = category;
            DueDateTime = dueDateTime;
            IsDone = isDone;
        }
    }
}