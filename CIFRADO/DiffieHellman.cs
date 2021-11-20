using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace CIFRADO
{
     public class DiffieHellman
    {
        public int P = 449;
        public int G = 11;
        public int Publickey(int secretrandom) 
        {
            BigInteger Valor = BigInteger.ModPow(G, (BigInteger)secretrandom, (BigInteger)P);
            int respuesta = (int)Valor;
            return respuesta;
        }
        public int SecretKey(int secretrandom, int Publickey)
        {
            BigInteger Valor = BigInteger.ModPow(Publickey, (BigInteger)secretrandom, (BigInteger)P);
            int respuesta = (int)Valor;
            return respuesta;
        }
    }
}
