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
        public List<Mensajes> BuscarMensajesE(Mensajes Chat)
        {
            var results = Collection.Find(s => (s.UsuarioE == Chat.UsuarioE && s.UsuarioR == Chat.UsuarioR) || (s.UsuarioE == Chat.UsuarioR && s.UsuarioR == Chat.UsuarioE)).ToList();
            return results;
        }
        public List<Mensajes> BuscarMensajesGrupo(Mensajes Chat)
        {
            var results = Collection.Find(s => (s.Usuarios.Contains(Chat.UsuarioE)) && (s.GNombre == Chat.GNombre) && (s.Num == Chat.Num)).ToList();
            return results;
        }
        public void EliminarMensajeSolo(Mensajes Chat, ObjectId id)
        {
            Collection.ReplaceOne(s => s.Id == id, Chat);

        }
        public void EliminarMensajeTodos(List<ObjectId> id)
        {
            foreach (var k in id)
            {
                Collection.DeleteOne(s => s.Id == k);
            }
        }
        public List<Mensajes> BuscarMensaje(Mensajes Chat)
        {
            var results = Collection.Find(s => (s.UsuarioE == Chat.UsuarioE && s.UsuarioR == Chat.UsuarioR) && (s.Texto == Chat.Texto)).ToList();
            return results;
        }
        public List<Mensajes> BuscarMensajeGrupo(Mensajes Chat)
        {
            var results = Collection.Find(s => (s.UsuarioE == Chat.UsuarioE) && (s.GNombre == Chat.GNombre) && (s.Num == Chat.Num)).ToList();
            return results;
        }
        public List<Mensajes> Listatotal()
        {
            var results = Collection.Find(s => true).ToList();
            return results;
        }


    }
}
