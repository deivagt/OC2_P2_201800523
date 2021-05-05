using System;
using System.Collections.Generic;
using System.Text;

namespace OC2_P2_201800523.Arbol.tipos.objetos
{
    class atributo
    {
        public string id;
        public string tipo;
        public bool esCrudo;
        public string direccion;
        public atributo interno;
        public LinkedList<atributo> listaAtributos;
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
