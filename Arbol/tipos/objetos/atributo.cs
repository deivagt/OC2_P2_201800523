using System;
using System.Collections.Generic;
using System.Text;
using OC2_P2_201800523.Arbol.tipos.arreglos;

namespace OC2_P2_201800523.Arbol.tipos.objetos
{
    class atributo
    {
        public string id;
        public int pos;
        public string tipo;
        public bool esCrudo;
        public bool esArray;
        public string direccion;
        public atributo interno;
        public LinkedList<atributo> listaAtributos;
        public LinkedList<index> listaIndex;
        public atributo(string id, string tipo)
        {
            this.id = id;
            this.tipo = tipo;
            if(tipo == "string" || tipo == "char" || tipo == "integer" || tipo == "real" || tipo == "boolean")
            {
                this.esCrudo = true;
            }
            else
            {
                this.esCrudo = false;
            }
            this.esArray = false;
        }

        public atributo buscarAtributo(string id)
        {

            id = id.ToLower();
            foreach (var a in listaAtributos)
            {
                if (a.id == id)
                {
                    return a;
                }

            }
            return null;
        }
    }
}
