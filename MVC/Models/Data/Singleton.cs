using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models.Data
{
    public class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public int verificarnum;
        public bool verificar;
        private Singleton()
        {
            verificarnum = 0;
            verificar = false;
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
