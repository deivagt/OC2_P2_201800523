using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;
using Irony.Ast;
using OC2_P2_201800523.AST;
//using OC2_P2_201800523.Arbol.Ejecucion.Funcion_Procedimiento;

namespace OC2_P2_201800523.tablaSimbolos
{
    class simbolo
    {


        public string ambito;
        public string id;
        public string tipo;
        public string valor;
        public string defaultVal;
        public int fila;
        public int columna;
        public bool esFuncion;
        public bool esTipo;
        public bool esAtributo;
        public bool esConstante;
        public string categoria;


        public ParseTreeNode nodo;
        public LinkedList<simbolo> atributos;
        //public objetoFuncion fn;

        public simbolo(string ambito, string id, string tipo, string valor, int fila, int columna, ParseTreeNode nodo)
        {
            
            this.ambito = ambito;
            this.id = id;
            this.tipo = tipo;
            this.valor = valor;
            this.fila = fila;
            this.columna = columna;
            this.nodo = nodo;
        }
        public simbolo(string ambito, string id, string tipo, string valor, int fila, int columna) //variable inicializada
        {
            this.ambito = ambito;
            this.id = id;
            this.tipo = tipo;
            this.valor = valor;
            this.fila = fila;
            this.columna = columna;
            this.defaultVal = "";
            this.categoria = "var";
        }
        public simbolo(string ambito, string id, string tipo, int fila, int columna)  //variable sin inicializar con valores por defecto
        {
            this.ambito = ambito;
            this.id = id;
            this.tipo = tipo;
          
            this.fila = fila;
            this.columna = columna;
            this.defaultVal = "";
            this.categoria = "var";

            if (tipo == terminales.rstring)
            {
                this.valor = "";
            } else if (tipo == terminales.rinteger)
            {
                this.valor = "0";
            }
            else if (tipo == terminales.rreal)
            {
                double a = 0.0000000000000000000000000000000000000000000000;
                this.valor = a.ToString();
            }
            else if (tipo == terminales.rchar)
            {
                this.valor = "";
            }
            else if (tipo == terminales.rboolean)
            {
                this.valor = "false";
            }
            else if (tipo == terminales.id)
            {
                this.valor = manejadorArbol.tabladeSimbolos.buscar(tipo);
            }
        }

        public simbolo(bool esAtributo, string ambito, string id, string tipo, int fila, int columna)  //atributo
        {
            this.esAtributo = esAtributo;
            this.ambito = ambito;
            this.id = id;
            this.tipo = tipo;
           
            this.fila = fila;
            this.columna = columna;
            this.defaultVal = "";
            this.categoria = "atr";

            if (tipo == terminales.rstring)
            {
                this.valor = "";
            }
            else if (tipo == terminales.rinteger)
            {
                this.valor = "0";
            }
            else if (tipo == terminales.rreal)
            {
                double a = 0.0000000000000000000000000000000000000000000000;
                this.valor = a.ToString();
            }
            else if (tipo == terminales.rchar)
            {
                this.valor = "";
            }
            else if (tipo == terminales.rboolean)
            {
                this.valor = "false";
            }
            else if (tipo == terminales.id)
            {
                this.valor = manejadorArbol.tabladeSimbolos.buscar(tipo);
            }
        }
        public simbolo(string ambito, string id, string tipo, int fila, int columna, bool esTipo) //Tipo
        {
            this.ambito = ambito;
            this.id = id;
            this.tipo = tipo;
            this.fila = fila;
            this.columna = columna;
            this.esTipo = true;
            this.valor = "";
            this.categoria = "type";

            if (tipo == terminales.rstring)
            {
                this.defaultVal = "";
            }
            else if (tipo == terminales.rinteger)
            {
                this.defaultVal = "0";
            }
            else if (tipo == terminales.rreal)
            {
                double a = 0.0000000000000000000000000000000000000000000000;
                this.defaultVal = a.ToString();
            }
            else if (tipo == terminales.rchar)
            {
                this.defaultVal = "";
            }
            else if (tipo == terminales.rboolean)
            {
                this.defaultVal = "false";
            }
            else if (tipo == terminales.id)
            {
                this.defaultVal = manejadorArbol.tabladeSimbolos.buscar(tipo);
            }
        }

        public simbolo(string ambito, string id, string valor, string tipo, int fila, int columna, bool esTipo, bool esConstante) //Constante
        {
            this.ambito = ambito;
            this.id = id;
            this.fila = fila;
            this.columna = columna;
            this.tipo = tipo;
            this.esTipo = false;
            this.esConstante = true;
            this.valor = valor;
            this.categoria = "const";          
           
        }

        public simbolo(string ambito, string id,  int fila, int columna, LinkedList<simbolo> atributos) //Objeto
        {
            this.ambito = ambito;
            this.id = id;
            this.tipo = "object";
            this.valor = "object";
            this.fila = fila;
            this.columna = columna;
            this.defaultVal = "";
            this.categoria = "obj";
            this.atributos = atributos;
        }

        //public simbolo(string ambito, string id, int fila, int columna, objetoFuncion fn) //Proceimiento
        //{
        //    this.ambito = ambito;
        //    this.id = id;
        //    this.tipo = "null";
        //    this.valor = "null";
        //    this.fila = fila;
        //    this.columna = columna;
        //    this.defaultVal = "";
        //    this.categoria = "func";
        //    this.fn = fn;
        //}

        //public simbolo(string ambito, string id, int fila, int columna,string tipo, objetoFuncion fn) //Funcion
        //{
        //    this.ambito = ambito;
        //    this.id = id;
        //    this.tipo = tipo;
        //    this.valor = "null";
        //    this.fila = fila;
        //    this.columna = columna;
        //    this.defaultVal = "";
        //    this.categoria = "func";
        //    this.fn = fn;
        //}


    }
}
