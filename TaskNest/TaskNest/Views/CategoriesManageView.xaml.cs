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

        private void BtnCatNew_OnClick(object sender, RoutedEventArgs e)
        {
            // Check if category name is empty
            if(Cmvm.NewCatName == null)
                return;

            // Check if category name already exists
            if (Cmvm.Categories.CheckCategoryExists(Cmvm.NewCatName))
            {
                MessageBox.Show($"The category {Cmvm.NewCatName} already exists", "Category can't be added again",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Add new category
            Cmvm.Categories.AddNewCat(Cmvm.NewCatName);

            MessageBox.Show($"The category {Cmvm.NewCatName} has been added", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnCatDel_OnClick(object sender, RoutedEventArgs e)
        {
            //If nothing is selected
            if(Cmvm.CurrentCat == null) return;

            // Check if there are tasks having this category
            if (Cmvm.Db.FindTask((tsk) => tsk.Category.Equals(Cmvm.CurrentCat)).Count > 0)
            {
                MessageBox.Show($"There are tasks having the selected category", "Category in use",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // If it is the empty category
            if (Cmvm.Categories.IsJustDefaultCategory())
            {
                MessageBox.Show($"The default category can't be removed", "There should be at least one category",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Cmvm.Categories.DeleteCat(Cmvm.CurrentCat);
        }

        private void BtnCatRen_OnClick(object sender, RoutedEventArgs e)
        {
            //If nothing is selected
            if (Cmvm.CurrentCat == null) return;

            // Check if category name is empty
            if (Cmvm.NewCatName == null) return;

            // Check if category name already exists
            if (Cmvm.Categories.CheckCategoryExists(Cmvm.NewCatName))
            {
                MessageBox.Show($"The category {Cmvm.NewCatName} already exists", "Category can't be added again",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Find tasks having the old category name and update
            var tasks = Cmvm.Db.FindTask((tsk) => tsk.Category.Equals(Cmvm.CurrentCat));

            // Update category name
            Cmvm.Categories.UpdateCategoryName(Cmvm.CurrentCat, Cmvm.NewCatName);

            foreach (var tsk in tasks)
            {
                tsk.Task.Category = String.Copy(Cmvm.NewCatName);
            }
        }
    }
}
