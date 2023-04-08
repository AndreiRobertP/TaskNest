using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
