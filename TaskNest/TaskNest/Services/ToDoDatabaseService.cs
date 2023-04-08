using System.IO;
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
    }
}
