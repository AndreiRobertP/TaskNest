using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TaskNest.Models
{
    public class Categories
    {
        public static ObservableCollection<string> Cats { get; set; } = new ObservableCollection<string>();

        public static int GetCategoryIndex(string cat)
        {
            for (int i = 0; i < Cats.Count; i++)
            {
                if (Cats[i].Equals(cat))
                    return i;
            }

            return 0;
        }

        public static void LoadCats(List<string> categories)
        {
            Cats.Clear();
            foreach (var cat in categories)
            {
                Cats.Add(cat);
            }
        }

        public static bool CheckCategoryExists(string cat)
        {
            foreach (var c in Cats)
            {
                if (c.Equals(cat))
                    return true;
            }

            return false;
        }

        public static void AddNewCat(string cat)
        {
            Cats.Add(cat);
        }

        public static void DeleteCat(string cat)
        {
            var foundAt = GetCategoryIndex(cat);
            if(foundAt == -1)
                return;
            Cats.RemoveAt(foundAt);
        }

        public static bool IsJustDefaultCategory()
        {
            return Cats.Count == 1;
        }

        public static void UpdateCategoryName(string oldName, string newName)
        {
            var index = GetCategoryIndex(oldName);
            Cats[index] = String.Copy(newName);
        }
    }
}