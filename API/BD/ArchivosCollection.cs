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
    public class ArchivosCollection
    {
        internal MongoDB repositorio = new MongoDB();
        private IMongoCollection<ArchivosValores> Collection;
        public ArchivosCollection()
        {
            Collection = repositorio.db.GetCollection<ArchivosValores>("Archivos");
        }
        public void AgregarArchivo(ArchivosValores valor)
        {
            Collection.InsertOneAsync(valor);
        }
        public ArchivosValores BuscarArchivo(ArchivosValores valor)
        {
            ArchivosValores respuesta = Collection.Find(s => s.Nombre == valor.Nombre).First();
            return respuesta;
        }
        public List<ArchivosValores> TotalArchivos()
        {
            return Collection.Find(s => true).ToList();
        }
    }
}
