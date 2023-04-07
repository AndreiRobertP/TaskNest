using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Remoting.Channels;
using System.Windows.Documents;

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

    }
}