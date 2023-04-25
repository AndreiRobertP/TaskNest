using System;
using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using TaskNest.Models;

namespace TaskNest.ViewModels
{
    public class CategoriesManageVM
    {
        public Categories Categories => Db.Categories;
        public ObservableCollection<string> Cats => Categories.Cats;
        public string CurrentCat { get; set; }
        public string NewCatName { get; set; }
        public ToDoDatabase Db { get; set; }
        public CategoriesManageVM(ToDoDatabase db)
        {
            Db = db;
        }


        // ===========
        // Commands
        // ===========

        private RelayCommand _cmdNew;
        public RelayCommand CmdNew
        {
            get
            {
                return _cmdNew ?? (_cmdNew = new RelayCommand(
                    () =>
                    {
                        // Check if category name is empty
                        if (NewCatName == null)
                            return;

                        // Check if category name already exists
                        if (Categories.CheckCategoryExists(NewCatName))
                        {
                            MessageBox.Show($"The category {NewCatName} already exists", "Category can't be added again",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        //Add new category
                        Categories.AddNewCat(NewCatName);

                        MessageBox.Show($"The category {NewCatName} has been added", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    },
                    () => true
                ));
            }
        }


        private RelayCommand _cmdDelete;
        public RelayCommand CmdDelete
        {
            get
            {
                return _cmdDelete ?? (_cmdDelete = new RelayCommand(
                    () =>
                    {
                        //If nothing is selected
                        if (CurrentCat == null) return;

                        // Check if there are tasks having this category
                        if (Db.FindTask((tsk) => tsk.Category.Equals(CurrentCat)).Count > 0)
                        {
                            MessageBox.Show($"There are tasks having the selected category", "Category in use",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        // If it is the empty category
                        if (Categories.IsJustDefaultCategory())
                        {
                            MessageBox.Show($"The default category can't be removed", "There should be at least one category",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        Categories.DeleteCat(CurrentCat);
                    },
                    () => true
                ));
            }
        }


        private RelayCommand _cmdRename;
        public RelayCommand CmdRename
        {
            get
            {
                return _cmdRename ?? (_cmdRename = new RelayCommand(
                    () =>
                    {
                        //If nothing is selected
                        if (CurrentCat == null) return;

                        // Check if category name is empty
                        if (NewCatName == null) return;

                        // Check if category name already exists
                        if (Categories.CheckCategoryExists(NewCatName))
                        {
                            MessageBox.Show($"The category {NewCatName} already exists", "Category can't be added again",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        //Find tasks having the old category name and update
                        var tasks = Db.FindTask((tsk) => tsk.Category.Equals(CurrentCat));

                        // Update category name
                        Categories.UpdateCategoryName(CurrentCat, NewCatName);

                        foreach (var tsk in tasks)
                        {
                            tsk.Task.Category = String.Copy(NewCatName);
                        }
                    },
                    () => true
                ));
            }
        }
    }
}
