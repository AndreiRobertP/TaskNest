using System.Collections.ObjectModel;
using TaskNest.Models;

namespace TaskNest.ViewModels
{
    public class CategoriesManageVM
    {
        public ObservableCollection<string> Cats => Categories.Cats;
        public string CurrentCat { get; set; }
        public string NewCatName { get; set; }
        public ToDoDatabase Db { get; set; }
        public CategoriesManageVM(ToDoDatabase db)
        {
            Db = db;
        }
    }
}
