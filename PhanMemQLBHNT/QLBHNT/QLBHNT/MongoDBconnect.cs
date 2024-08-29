using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace QLBHNT
{
    class MongoDBconnect
    {
        private IMongoCollection<BsonDocument> collection;
        private MongoClient client;
        private IMongoDatabase database;
        public MongoDBconnect()
        {
            string connectionString = "mongodb://localhost:27017"; // Thay đổi tên địa chỉ và cổng MongoDB tại đây
            client = new MongoClient(connectionString);
            database = client.GetDatabase("QLBH");
            collection = database.GetCollection<BsonDocument>("HopDong"); // Thay tên collection mong muốn
        }
    }
}
