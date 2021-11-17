using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace API.BD
{
    public class MongoDB
    {
        public MongoClient client;
        public IMongoDatabase db;
        public MongoDB()
        {
            client = new MongoClient("mongodb://localhost:27017");
            db = client.GetDatabase("MensajesRelevantes");
        }
    }
}
