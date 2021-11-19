using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIFRADO;
namespace API.Models.Data
{
    public class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public ICIFRADO CifrarSDES;
        public DiffieHellman Llaves;
        private Singleton()
        {
            CifrarSDES = new SDES();
            Llaves = new DiffieHellman();
        }
        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
