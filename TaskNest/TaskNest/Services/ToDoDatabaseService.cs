using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TaskNest.Models;

namespace TaskNest.Services
{
    public class ToDoDatabaseService
    {
        public static void SerializeDatabase(ToDoDatabase db, string filename)
        {
            XmlSerializer xmlser = new XmlSerializer(typeof(ToDoDatabase));
            FileStream fileStr = new FileStream(filename, FileMode.Create);
            xmlser.Serialize(fileStr, db);
            fileStr.Dispose();

            SetLastDatabaseFilepath(filename);
        }

        public static bool DeserializeObject(ToDoDatabase db, string filename)
        {
            XmlSerializer xmlser = new XmlSerializer(typeof(ToDoDatabase));

            ToDoDatabase tmp;
            try
            {
                FileStream file = new FileStream(filename, FileMode.Open);
                tmp = xmlser.Deserialize(file) as ToDoDatabase;
                file.Close();
            }
            catch
            {
                return false;
            }

            if (tmp == null)
                return false;

            db.Categories = tmp.Categories;
            db.RootLists = tmp.RootLists;
            TreeParentRestructureList(db);

            SetLastDatabaseFilepath(filename);

            return true;
        }

        public static void TreeParentRestructureList(IToDoListNode parentNode)
        {
            foreach (var subList in parentNode.GetDirectDescendentsSublists())
            {
                subList.Parent = parentNode;
                TreeParentRestructureList(subList);
            }
        }

        public static ToDoDatabase GenerateNewDatabase()
        {
            var categories = new Categories();
            categories.LoadCats(new List<string>() { "None" });

            var newDb = new ToDoDatabase();
            newDb.Categories = categories;

            return newDb;
        }

        public static string GetLastDatabaseFilepath()
        {
            string filename = null;

            try
            {
                using (StreamReader reader = File.OpenText("config.txt"))
                {
                    filename = reader.ReadLine();
                }
            }
            catch (Exception e)
            {
                return null;
            }

            if(File.Exists(filename))
                return filename;
            else
                return null;
        }

        public static void SetLastDatabaseFilepath(string filename)
        {
            try
            {
                using (StreamWriter reader = File.CreateText("config.txt"))
                {
                    reader.Write(filename);
                }
            }
            catch (Exception e)
            {
                //Nothing
            }
        }
    }
}
