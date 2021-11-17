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
        public void ActualizarCont(Contactos value)
        {
            Collection.ReplaceOne(s => (s.Usuario == value.Usuario && s.Usuario2 == value.Usuario2) || (s.Usuario == value.Usuario2 && s.Usuario2 == value.Usuario), value);
        }
        public Contactos Buscar2(Contactos valor)
        {
            var results2 = Collection.Find(s => (s.Usuario == valor.Usuario && s.Usuario2 == valor.Usuario2) || (s.Usuario == valor.Usuario2 && s.Usuario2 == valor.Usuario)).ToList();
            Contactos retornar = new Contactos();
            if (results2.Count == 0)
            {
                return default;
            }
            retornar = results2[0];
            return retornar;
        }
        public List<Contactos> Buscar(UsuariosModel valor)
        {
            var results = Collection.Find(s => s.Usuario == valor.Usuario || s.Usuario2 == valor.Usuario || s.Usuarios.Contains(valor.Usuario)).ToList();
            return results;
        }
        public List<Contactos> BuscarGrupo(Contactos Valor)
        {
            var results = Collection.Find(s => s.nombre == Valor.nombre).ToList();
            return results;
        }
        public Contactos BuscarGrupoEsp(Contactos Valor)
        {
            var results = Collection.Find(s => s.nombre == Valor.nombre && s.num == Valor.num).First();
            return results;
        }
        public List<Contactos> BuscarGruposUsuario(string valor)
        {
            var x = Collection.Find(s => s.Usuarios.Contains(valor)).ToList();
            return x;
        }
        public void ActualizarContactoGrupo(Contactos valor)
        {
            Collection.ReplaceOne(s => s.nombre == valor.nombre && s.num == valor.num, valor);
        }
    }
}
