using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using API.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace API.BD
{
    public class MensajesCollection
    {
        internal MongoDB repositorio = new MongoDB();
        private IMongoCollection<Mensajes> Collection;
        public MensajesCollection()
        {
            Collection = repositorio.db.GetCollection<Mensajes>("Mensajes");
        }
        public void AgregarMen(Mensajes valor)
        {
            Collection.InsertOneAsync(valor);
        }
        public List<Mensajes> BuscarMensajesGrupo(ObjectId id)
        {
            var filter = Builders<Mensajes>.Filter.Eq(s => s.Id, id);
            var results = Collection.Find(filter).ToList();
            return results;
        }
        public List<Mensajes> BuscarMensajesE(string Nombre)
        {
            var filter = Builders<Mensajes>.Filter.Eq(s => s.name, Nombre);
            var results = Collection.Find(filter).ToList();
            return results;
        }
       
    }
}
