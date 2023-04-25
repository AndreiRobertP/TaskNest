﻿using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TaskNest.Models;
using TaskNest.Services;
using TaskNest.Views;

namespace TaskNest.ViewModels
{
    public class MainWindowVm : INotifyPropertyChanged
    {
        public ToDoDatabase Db { get; set; }

        // Filtering criterium
        private Func<ToDoTask, bool> _taskFilteringCriterium = task => true;

        public Func<ToDoTask, bool> TaskFilteringCriterium
        {
            get => _taskFilteringCriterium;
            set
            {
                _taskFilteringCriterium = value;
                NotifyPropertyChangedTasks();
            }
        }

        // Current List
        private ToDoList _currentToDoList;

        public ToDoList CurrentToDoList
        {
            get => _currentToDoList;
            set
            {
                _currentToDoList = value;
                NotifyPropertyChanged("Tasks");
            }
        }

        public BindingList<ToDoTask> Tasks
        {
            get
            {
                BindingList<ToDoTask> response = new BindingList<ToDoTask>();

                if (CurrentToDoList == null)
                    return response;

                foreach (var task in CurrentToDoList.Tasks)
                {
                    if (TaskFilteringCriterium(task))
                        response.Add(task);
                }

                return response;
            }
        }

        public IToDoListNode CurrentNodeParent => CurrentToDoList.Parent;


        // Current Task
        private ToDoTask _currentToDoTask;

        public ToDoTask CurrentToDoTask
        {
            get => _currentToDoTask;
            set
            {
                _currentToDoTask = value;
                NotifyPropertyChanged("TaskContent");
            }
        }

        public string TaskContent
        {
            get
            {
                if (CurrentToDoTask == null)
                    return "";
                return CurrentToDoTask.Description;
            }
        }


        // TreeView Navigation Content
        public BindingList<ToDoList> ToDoLists => Db.RootLists;

        //Extra
        public ObservableCollection<ToDoTask> AllTasks => Db.GetToDoTasksSubtree();

        //Statistics
        public StatisticsVM StatsVM { get; set; }

        public MainWindowVm()
        {
            Db = new ToDoDatabase();
            StatsVM = new StatisticsVM(Db);

            ToDoDatabaseService.DeserializeObject(Db, ToDoDatabaseService.GetLastDatabaseFilepath());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyPropertyChangedStatistics()
        {
            StatsVM.NotifyPropertyChangedStatistics();
        }

        public void NotifyPropertyChangedTasks()
        {
            NotifyPropertyChanged("Tasks");
            StatsVM.NotifyPropertyChangedStatistics();
        }

        public void NotifyPropertyChangedLists()
        {
            NotifyPropertyChanged("ToDoLists");
            NotifyPropertyChangedTasks();
        }




        //================================
        // COMMANDS FOR FILE MANIPULATION
        //================================

        private RelayCommand _cmdOpenDatabase;
        public RelayCommand CmdOpenDatabase
        {
            get
            {
                return _cmdOpenDatabase ?? (_cmdOpenDatabase = new RelayCommand(
                    () =>
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "TaskNest Database Files | *.xml";
                        openFileDialog.Title = "Choose ToDoDatabase";
                        openFileDialog.Multiselect = false;

                        var resp = openFileDialog.ShowDialog();
                        if (!resp.HasValue || resp.Value == false)
                            return;

                        ToDoDatabaseService.DeserializeObject(Db, openFileDialog.FileName);
                        NotifyPropertyChangedLists();
                    },
                    () => true
                ));
            }
        }


        private RelayCommand _cmdNewDatabase;
        public RelayCommand CmdNewDatabase
        {
            get
            {
                return _cmdNewDatabase ?? (_cmdNewDatabase = new RelayCommand(
                    () =>
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "TaskNest Database Files | *.xml";
                        saveFileDialog.Title = "Choose ToDODatabase";

                        var resp = saveFileDialog.ShowDialog();
                        if (!resp.HasValue || resp.Value == false)
                            return;

                        ToDoDatabaseService.SerializeDatabase(ToDoDatabaseService.GenerateNewDatabase(),
                            saveFileDialog.FileName);
                        ToDoDatabaseService.DeserializeObject(Db, saveFileDialog.FileName);
                        NotifyPropertyChangedLists();
                    },
                    () => true
                ));
            }
        }


        private RelayCommand _cmdArchiveDatabase;
        public RelayCommand CmdArchiveDatabase
        {
            get
            {
                return _cmdArchiveDatabase ?? (_cmdArchiveDatabase = new RelayCommand(
                    () =>
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "TaskNest Database Files | *.xml";
                        saveFileDialog.Title = "Choose ToDODatabase";

                        var resp = saveFileDialog.ShowDialog();
                        if (!resp.HasValue || resp.Value == false)
                            return;

                        ToDoDatabaseService.SerializeDatabase(Db, saveFileDialog.FileName);
                    },
                    () => true
                ));
            }
        }


        private RelayCommand _cmdSaveDatabase;
        public RelayCommand CmdSaveDatabase
        {
            get
            {
                return _cmdSaveDatabase ?? (_cmdSaveDatabase = new RelayCommand(
                    () => { ToDoDatabaseService.SaveChangesToCurrentDatabase(Db); },
                    //ToDoDatabaseService.IsSaveDatabasePathValid
                    () => true
                ));
            }
        }



        //================================
        // COMMANDS FOR TO DO LIST MANIPULATION
        //================================

        private RelayCommand _cmdAddRootTdl;
        public RelayCommand CmdAddRootTdl
        {
            get
            {
                return _cmdAddRootTdl ?? (_cmdAddRootTdl = new RelayCommand(
                    () =>
                    {
                        ListEditView listEditView = new ListEditView(Db, Db.GetToDoListsSubtree(), null);
                        listEditView.ShowDialog();

                        NotifyPropertyChangedLists();
                    },
                    () => true
                ));
            }
        }


        private RelayCommand _cmdAddSubTdl;
        public RelayCommand CmdAddSubTdl
        {
            get
            {
                return _cmdAddSubTdl ?? (_cmdAddSubTdl = new RelayCommand(
                    () =>
                    {
                        if (CurrentToDoList == null)
                            return;

                        ListEditView listEditView = new ListEditView(CurrentToDoList, Db.GetToDoListsSubtree(), null);
                        listEditView.ShowDialog();

                        NotifyPropertyChangedLists();
                    },
                    //() => CurrentToDoList != null
                    () => true
                ));
            }
        }


        private RelayCommand _cmdEditTdl;
        public RelayCommand CmdEditTdl
        {
            get
            {
                return _cmdEditTdl ?? (_cmdEditTdl = new RelayCommand(
                    () =>
                    {
                        if (CurrentToDoList == null)
                            return;

                        ListEditView listEditView = new ListEditView(CurrentToDoList, Db.GetToDoListsSubtree(), CurrentToDoList);
                        listEditView.ShowDialog();
                    },
                    //() => CurrentToDoList != null
                    () => true
                ));
            }
        }


        private RelayCommand _cmdDeleteTdl;
        public RelayCommand CmdDeleteTdl
        {
            get
            {
                return _cmdDeleteTdl ?? (_cmdDeleteTdl = new RelayCommand(
                    () =>
                    {
                        if (CurrentToDoList == null)
                            return;

                        var responseSure = MessageBox.Show(
                            "Are you sure you want to delete this list? This will delete all the nested lists inside with their respective tasks. This action can't be undone.",
                            "Are you sure?", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        if (responseSure == MessageBoxResult.Cancel)
                            return;

                        ToDoListService.RemoveList(CurrentToDoList);
                        NotifyPropertyChangedStatistics();
                    },
                    //() => CurrentToDoList != null
                    () => true
                ));
            }
        }


        private RelayCommand _cmdChangePathTdl;
        public RelayCommand CmdChangePathTdl
        {
            get
            {
                return _cmdChangePathTdl ?? (_cmdChangePathTdl = new RelayCommand(
                    () =>
                    {
                        if (CurrentToDoList == null)
                            return;

                        ListChangePathView listChangePathView = new ListChangePathView(Db, CurrentToDoList);
                        listChangePathView.ShowDialog();
                    },
                    //() => CurrentToDoList != null
                    () => true
                ));
            }
        }


        private RelayCommand<bool> _cmdMoveTdl;
        public RelayCommand<bool> CmdMoveTdl
        {
            get
            {
                return _cmdMoveTdl ?? (_cmdMoveTdl = new RelayCommand<bool>(
                    (direction) =>
                    {
                        if (CurrentToDoList == null)
                            return;

                        ToDoListService.MoveList(CurrentToDoList, CurrentToDoList.Parent.GetDirectDescendentsSublists(), direction);
                        NotifyPropertyChangedLists();
                    },
                    //() => CurrentToDoList != null
                    (direction) => true
                ));
            }
        }


        //================================
        // COMMANDS FOR TO DO TASKS MANIPULATION
        //================================

        private RelayCommand _cmdAddTdt;
        public RelayCommand CmdAddTdt
        {
            get
            {
                return _cmdAddTdt ?? (_cmdAddTdt = new RelayCommand(
                    () =>
                    {
                        if (CurrentToDoList == null)
                            return;

                        TaskEditView editWindow = new TaskEditView(AllTasks, Db.Categories, CurrentToDoList);
                        editWindow.ShowDialog();
                        NotifyPropertyChangedTasks();
                    },
                    //() => CurrentToDoList != null
                    () => true
                ));
            }
        }


        private RelayCommand _cmdEditTdt;
        public RelayCommand CmdEditTdt
        {
            get
            {
                return _cmdEditTdt ?? (_cmdEditTdt = new RelayCommand(
                    () =>
                    {
                        if (CurrentToDoList == null)
                            return;

                        if (CurrentToDoTask == null)
                            return;

                        TaskEditView editWindow = new TaskEditView(AllTasks, Db.Categories, CurrentToDoList, CurrentToDoTask);
                        editWindow.ShowDialog();
                        NotifyPropertyChangedTasks();
                    },
                    //() => CurrentToDoList != null && CurrentToDoTask != null
                    () => true
                ));
            }
        }


        private RelayCommand _cmdDeleteTdt;
        public RelayCommand CmdDeleteTdt
        {
            get
            {
                return _cmdDeleteTdt ?? (_cmdDeleteTdt = new RelayCommand(
                    () =>
                    {
                        if (CurrentToDoTask == null || CurrentToDoList == null)
                            return;

                        ToDoTaskService.DeleteTask(CurrentToDoTask, CurrentToDoList);
                        NotifyPropertyChangedTasks();
                    },
                    //() => CurrentToDoList != null && CurrentToDoTask != null
                    () => true
                ));
            }
        }


        private RelayCommand _cmdToggleTdt;
        public RelayCommand CmdToggleTdt
        {
            get
            {
                return _cmdToggleTdt ?? (_cmdToggleTdt = new RelayCommand(
                    () =>
                    {
                        if (CurrentToDoTask == null)
                            return;

                        ToDoTaskService.ToggleTaskDone(CurrentToDoTask);
                        NotifyPropertyChangedTasks();
                    },
                    //() => CurrentToDoTask != null
                    () => true
                ));
            }
        }


        private RelayCommand<bool> _cmdMoveTdt;
        public RelayCommand<bool> CmdMoveTdt
        {
            get
            {
                return _cmdMoveTdt ?? (_cmdMoveTdt = new RelayCommand<bool>(
                    (direction) =>
                    {
                        if (CurrentToDoTask == null || CurrentToDoList == null)
                            return;

                        ToDoTaskService.MoveTask(CurrentToDoTask, CurrentToDoList, direction);
                        NotifyPropertyChangedTasks();
                    },
                    //() => CurrentToDoTask != null && CurrentToDoList != null
                    (direction) => true
                ));
            }
        }


        private RelayCommand _cmdManageCats;
        public RelayCommand CmdManageCats
        {
            get
            {
                return _cmdManageCats ?? (_cmdManageCats = new RelayCommand(
                    () =>
                    {
                        CategoriesManageView categoriesManageView = new CategoriesManageView(Db);
                        categoriesManageView.Show();

                        NotifyPropertyChangedTasks();
                    },
                    () => true
                ));
            }
        }


        private RelayCommand _cmdFindTdt;
        public RelayCommand CmdFindTdt
        {
            get
            {
                return _cmdFindTdt ?? (_cmdFindTdt = new RelayCommand(
                    () =>
                    {
                        TaskSearchView taskSearchView = new TaskSearchView();
                        taskSearchView.SetDatabase(Db);
                        taskSearchView.Show();
                    },
                    () => true
                ));
            }
        }
    }
}
