using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskNest.Models;

namespace TaskNest.ViewModels
{
    public class FilterVM
    {
        public Categories Categories { get; set; }
        public ObservableCollection<string> Cats => Categories.Cats;
        public string CurrentCat { get; set; }

        public FilterVM(Categories cats)
        {
            Categories = cats;
        }
    }
}
