using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Documents;
using System.Windows.Shapes;
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
            categories.LoadCats(new List<string>(){"None"});

            var newDb = new ToDoDatabase();
            newDb.Categories = categories;

            return newDb;
        }

        public static string GetLastDatabaseFilepath()
        {
            using (StreamReader reader = File.OpenText("config.txt"))
            {
                return reader.ReadLine();
            }
        }

        public static void SetLastDatabaseFilepath(string filename)
        {
            using (StreamWriter reader = File.CreateText("config.txt"))
            {
                reader.Write(filename);
            }
        }
    }
}
