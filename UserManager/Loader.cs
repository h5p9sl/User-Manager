using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace UserManager
{
    public class DBMetadata
    {
        public int databaseVersion;
    }

    public class Database
    {
        public DBMetadata metadata = new DBMetadata();

        public List<UserInformation> users = null;
    }

    /// <summary>
    /// Loads/saves users from/to a database file
    /// </summary>
    class Loader
    {
        public void SaveDatabase(ref UserManager userManager, string filePath)
        {
            using (StreamWriter stream = new StreamWriter(filePath))
            {
                if (userManager != null)
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(stream))
                    {
                        Database database = new Database();
                        database.metadata.databaseVersion = Globals.currentDatabaseVersion;
                        database.users = userManager;

                        XmlSerializer serializer = new XmlSerializer(typeof(Database));
                        serializer.Serialize(xmlWriter, database);
                    }
                }
                stream.Close();
            }
        }

        public void LoadDatabase(ref UserManager userManager, string filePath)
        {
            using (StreamReader stream = new StreamReader(filePath))
            {
                if (userManager != null)
                {
                    Database database = null;

                    using (XmlReader xmlWriter = XmlReader.Create(stream))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Database));
                        database = (Database)serializer.Deserialize(xmlWriter);
                    }

                    if (database.metadata.databaseVersion != Globals.currentDatabaseVersion)
                    {
                        // TODO
                    }

                    // Load all users
                    foreach (var user in database.users)
                    {
                        userManager.Add(user);
                    }
                }
                stream.Close();
            }
        }
    }
}
