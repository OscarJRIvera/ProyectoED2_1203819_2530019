using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using API.Models;


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
    }
}
