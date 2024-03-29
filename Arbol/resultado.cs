﻿using OC2_P2_201800523.AST;
using System;
using System.Collections.Generic;
using System.Text;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;

namespace OC2_P2_201800523.Arbol
{
    class resultado
    {
        public string tipo;
        public string valor;
        public int temporal;
        public bool esArray;
        public string argumento;
        public simbolo simbolo;
        LinkedList<ParseTreeNode> nodo;


        public resultado(string tipo, string valor) //Para cadenas 
        {
            this.tipo = tipo;
            this.valor = valor;
        }
        public resultado(string tipo, string valor,simbolo simbolo) //Para cadenas 
        {
            this.tipo = tipo;
            this.valor = valor;
            this.simbolo = simbolo;
        }
        public resultado(string tipo, string valor, simbolo simbolo,bool esArray) //Para cadenas 
        {
            this.tipo = tipo;
            this.valor = valor;
            this.simbolo = simbolo;
            this.esArray = esArray;
        }
        public resultado(string tipo, string valor, string argumento, simbolo simbolo) //Otras cosas ewe
        {
            this.tipo = tipo;
            this.valor = valor;
            this.argumento = argumento;
            this.simbolo = simbolo;
        }
        public resultado(string tipo, string valor, string argumento) //Otras cosas ewe
        {
            this.tipo = tipo;
            this.valor = valor;
            this.argumento = argumento;
        }

        public resultado(string tipo, int valor) //Para numeros enteros 
        {
            this.tipo = tipo;
            this.valor = valor.ToString();
        }

        public resultado(string tipo, double valor) //Para numeros reales 
        {
            this.tipo = tipo;
            this.valor = valor.ToString();
        }

        public resultado(string tipo, ParseTreeNode nodo)
        {
            this.tipo = tipo;
            this.valor = valor.ToString();
        }
        public resultado()
        {
            this.tipo = "ERROR";
            this.valor = "";
        }

        public double getNumero()
        {
            if(tipo ==terminales.numero)
            {
                return double.Parse(this.valor);
            }
            else
            {
                return 0;
            }
        }
                

        public string getValor()
        {
            return this.valor;
        }

       

        public void setTemporal(int temporal)
        {
            this.temporal = temporal;
        }

        public bool getBooleano()
        {
            if(this.valor == "true" )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
