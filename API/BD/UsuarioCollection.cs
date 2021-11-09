using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using API.Models;

namespace API.BD
{
    public class UsuarioCollection
    {
        internal MongoDB repositorio = new MongoDB();
        private IMongoCollection<UsuariosModel> Collection;
        public UsuarioCollection()
        {
            Collection = repositorio.db.GetCollection<UsuariosModel>("Usuarios");
        }
        public string insertar(UsuariosModel Usuarioinfo)
        {
            try
            {
                Collection.InsertOneAsync(Usuarioinfo);
                return "Agregado con exito";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public UsuariosModel Buscar(UsuariosModel Usuarioinfo) {
            var filter = Builders<UsuariosModel>.Filter.Eq(s => s.Usuario, Usuarioinfo.Usuario);
            var results = Collection.Find(filter).ToList();
            UsuariosModel retornar = new UsuariosModel();
            retornar = results[0];
            return retornar;
        }

        public void delete(UsuariosModel Usuario)
        {
            var filter = Builders<UsuariosModel>.Filter.Eq(s => s.Usuario, Usuario.Usuario);
            Collection.DeleteOneAsync(filter);
        }
    }
}
