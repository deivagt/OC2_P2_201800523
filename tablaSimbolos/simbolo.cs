using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;
using Irony.Ast;
using OC2_P2_201800523.AST;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.Arbol.tipos.arreglos;
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
      


        /*Para arreglos*/
        public LinkedList<index> listaIndex;

        public simbolo(string ambito, string id, string tipo, string direccion, int fila, int columna,string categoria )
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

        public simbolo(string ambito, string id, string tipo, int fila, int columna, string categoria, LinkedList<index> listaIndex,bool esTipo)
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

        public simbolo(string ambito, string id, string tipo, string direccion, int fila, int columna, string categoria, LinkedList<index> listaIndex,bool esArray)
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

    }
}
