using System;
using System.Windows;
using TaskNest.Models;
using TaskNest.ViewModels;

namespace TaskNest.Views
{
    /// <summary>
    /// Interaction logic for CategoriesManageView.xaml
    /// </summary>
    public partial class CategoriesManageView : Window
    {
        public CategoriesManageVM Cmvm { get; set; }

        public CategoriesManageView(ToDoDatabase db)
        {
            InitializeComponent();

            Cmvm = new CategoriesManageVM(db);
            DataContext = Cmvm;
        }
    }
}
