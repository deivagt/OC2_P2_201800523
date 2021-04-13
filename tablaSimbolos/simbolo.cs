using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;
using Irony.Ast;
using OC2_P2_201800523.AST;
using OC2_P2_201800523.tablaSimbolos;
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
        public int columna;
        public bool esFuncion;
        public bool esTipo;
        public bool esAtributo;
        public bool esConstante;

        public ParseTreeNode nodo;

        public simbolo(string ambito, string id, string tipo, string direccion, int fila, int columna,string categoria)
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
        public simbolo(string ambito, string id, string tipo, string direccion, int fila, int columna, bool esConstante)//Constructor de constantes
        {
            this.ambito = ambito;
            this.id = id;
            this.tipo = tipo;
            this.categoria = "constante";
            this.direccion = direccion;
            this.fila = fila;
            this.columna = columna;
            this.esConstante = esConstante;
        }

    }
}
