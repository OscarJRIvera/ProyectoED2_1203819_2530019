using System;
using System.Collections.Generic;
using System.Text;

namespace CIFRADO
{
    public interface ICIFRADO
    {
        public void Cifrar(String Ruta, String Ruta2, String ValorL);
        public void Decifrar(String Ruta, String Ruta2, String ValorL);
        public String Cifrar(String Valor,int llave);
        public String Decifrar(String Valor, int llave);
    }
}
