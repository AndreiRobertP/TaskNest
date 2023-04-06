using System;
using System.Collections.ObjectModel;
using TaskNest.Models;

namespace TaskNest.ViewModels
{
    public class TaskSearchVM
    {
        public ObservableCollection<TDTSearchResult> SearchResults { get; set; }
        public string TextEntered { get; set; }
        public DateTime DateEntered { get; set; }
        public bool IsSearchingByName { get; set; }

        public ToDoDatabase Database { get; set; }

        public TaskSearchVM()
        {
            SearchResults = new ObservableCollection<TDTSearchResult>();
            IsSearchingByName = true;
            DateEntered = DateTime.Now.Date;
            TextEntered = "Clean";
        }

        public void SetDatabase(ToDoDatabase db)
        {
            Database = db;
        }

        public void RunSearch()
        {
            string criteriumName = TextEntered;
            Func<ToDoTask, bool> predFindWhat;

            if (IsSearchingByName)
            {
                string processedStr = TextEntered.ToLower();
                predFindWhat = (x) => x.Name.ToLower().Contains(processedStr);
            }
            else
            {
                DateTime processedDate = DateEntered.Date;
                predFindWhat = (x) => x.DueDateTime.Date.Equals(processedDate);
            }

            SearchResults.Clear();
            foreach (var result in Database.FindTask(predFindWhat))
            {
                SearchResults.Add(result);
            }
        }

    }
}
