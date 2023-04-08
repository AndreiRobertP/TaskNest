using System.ComponentModel;
using System.IO;
using System.Windows.Documents;
using System.Xml.Serialization;
using TaskNest.Models;

namespace TaskNest.Services
{
    public class ToDoDatabaseService
    {
        public static void SerializeDatabase(ToDoDatabase db)
        {
            XmlSerializer xmlser = new XmlSerializer(typeof(ToDoDatabase));
            FileStream fileStr = new FileStream("db_test.xml", FileMode.Create);
            xmlser.Serialize(fileStr, db);
            fileStr.Dispose();
        }

        public static bool DeserializeObject(ToDoDatabase db)
        {
            XmlSerializer xmlser = new XmlSerializer(typeof(ToDoDatabase));

            ToDoDatabase tmp;
            try
            {
                FileStream file = new FileStream($"db_test.xml", FileMode.Open);
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
    }
}
