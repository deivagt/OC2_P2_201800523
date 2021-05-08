using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;
using Irony.Ast;
using OC2_P2_201800523.AST;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.Arbol.tipos.arreglos;
using OC2_P2_201800523.Arbol.tipos.objetos;
//using OC2_P2_201800523.Arbol.Ejecucion.Funcion_Procedimiento;

namespace OC2_P2_201800523.tablaSimbolos
{
    class simbolo
    {

        public string ambito;
        public string categoria;
        public int contador;
        public string id;
        public string tipo;
        public string direccion;
        public int fila;
        public string temporal;
        public int columna;
        public bool esConstante;
        public bool esTipo;
        public LinkedList<parametroCustom> listaParam;
        public ParseTreeNode nodo;
        public bool esArray;
        public atributo estrAtrArray;



        /*Para arreglos*/
        public LinkedList<index> listaIndex;

        /*Para objetos*/
        public LinkedList<atributo> listaAtributos;

        public simbolo(string ambito, string id, string tipo, string direccion, int fila, int columna, string categoria)
        {
            this.ambito = ambito;
            this.id = id;
            this.tipo = tipo;
            this.categoria = categoria;
            this.direccion = direccion;
            this.fila = fila;
            this.columna = columna;
            this.esConstante = false;
        }

        public simbolo(string ambito, string id, string tipo, string direccion, int fila, int columna, string categoria, LinkedList<parametroCustom> listaParam)
        {
            this.ambito = ambito;
            this.id = id;
            this.tipo = tipo;
            this.categoria = categoria;
            this.direccion = direccion;
            this.fila = fila;
            this.columna = columna;
            this.esConstante = false;
            this.listaParam = listaParam;
        }

        public simbolo(string ambito, string id, string tipo, int fila, int columna, string categoria, LinkedList<index> listaIndex, bool esTipo)
        {
            this.ambito = ambito;
            this.id = id;
            this.tipo = tipo;
            this.categoria = categoria;
            this.fila = fila;
            this.columna = columna;
            this.esConstante = false;
            this.listaIndex = listaIndex;
            this.esTipo = esTipo;
        }

        public simbolo(string ambito, string id, string tipo, string direccion, int fila, int columna, string categoria, LinkedList<index> listaIndex, bool esArray)
        {
            this.ambito = ambito;
            this.id = id;
            this.tipo = tipo;
            this.categoria = categoria;
            this.direccion = direccion;
            this.fila = fila;
            this.columna = columna;
            this.esConstante = false;
            this.listaIndex = listaIndex;
            this.esArray = esArray;
        }

        public simbolo(string ambito, string id, string tipo, int fila, int columna, string categoria, LinkedList<atributo> listaAtributos, bool esTipo)
        {
            this.ambito = ambito;
            this.id = id;
            this.tipo = tipo;
            this.categoria = categoria;
            this.fila = fila;
            this.columna = columna;
            this.esConstante = false;
            this.listaAtributos = listaAtributos;
            this.esTipo = esTipo;
        }
        public simbolo(string ambito, string id, string tipo, string direccion, int fila, int columna, string categoria, LinkedList<atributo> listaAtributos, bool esArray)
        {
            this.ambito = ambito;
            this.id = id;
            this.tipo = tipo;
            this.categoria = categoria;
            this.direccion = direccion;
            this.fila = fila;
            this.columna = columna;
            this.esConstante = false;
            this.listaAtributos = listaAtributos;
            this.esArray = esArray;
        }
        public simbolo(string ambito, string id, string tipo, string direccion,string categoria, LinkedList<atributo> listaAtributos )
        {
            this.ambito = ambito;
            this.id = id;
            this.tipo = tipo;
            
            this.direccion = direccion;
            this.categoria = categoria;
            this.esConstante = false;
           
            this.listaAtributos = listaAtributos;
            
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
