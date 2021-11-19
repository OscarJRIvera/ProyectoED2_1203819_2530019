using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using API.Models;

namespace API.BD
{
    public class ContactosCollection
    {
        internal MongoDB repositorio = new MongoDB();
        private IMongoCollection<Contactos> Collection;
        public ContactosCollection()
        {
            Collection = repositorio.db.GetCollection<Contactos>("Contactos");
        }
        public void AgregarCont(Contactos valor)
        {
            Collection.InsertOneAsync(valor);
        }
        public List<Contactos> Buscar(Contactos valor)
        {
            var filter = Builders<Contactos>.Filter.Eq(s => s.Usuario, valor.Usuario);
            var results = Collection.Find(filter).ToList();
            return results;
        }
    }
}
