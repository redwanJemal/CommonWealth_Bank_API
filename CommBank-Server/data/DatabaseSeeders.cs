using CommBank.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommBank_Server.Data
{
    public static class DatabaseSeeders
    {
        public static void Seed(IMongoDatabase database)
        {
            var collections = new string []{ 
                "Accounts",
                "Tags",
                "Goals",
                "Users",
                "Transactions",
            };

            foreach (var table in collections)
            {
                SeedTables(database, table);
            }
            return;
           
        }

        private static void SeedTables(IMongoDatabase database, string tableName)
        {
            var collection = database.GetCollection<BsonDocument>(tableName);

            if (collection.Find(x => true).Any())
            {
                return; // database has already been seeded
            }

            var data = ReadDataFromJsonFile(tableName);

            var bsonData = new List<BsonDocument>();
            foreach (var item in data)
            {
                var document = BsonDocument.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(item));
                bsonData.Add(document);
            }

            collection.InsertMany(bsonData);
        }

        private static List<dynamic> ReadDataFromJsonFile(string filePath)
        {
            var data = new List<dynamic>();

            using (var reader = new StreamReader("Data/" + filePath + ".json"))
            {
                var json = reader.ReadToEnd();
                data = JsonConvert.DeserializeObject<List<dynamic>>(json);
            }

            return data;
        }
    }
}
