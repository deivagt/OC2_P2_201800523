using System;
using System.Collections.Generic;
using System.Text;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;

namespace OC2_P2_201800523.Arbol
{
    abstract class nodo
    {
        public string type;                //Tipo del Nodo
        public string value;            //Valor del Nodo
        public ParseTreeNode node;      //Nodo original del arbol de irony

        public nodo(string type, string value, ParseTreeNode node) //Para terminales
        {
            this.type = type;
            this.value = value;
            this.node = node;
        }

        public nodo(string type, ParseTreeNode node) //Para no terminales
        {
            this.type = type;
            this.value = "";
            this.node = node;
        }

        public abstract resultado traducir(ref tabla tablaActual, string ambito, string verdadero,string falso, string xd);

       


    }
}
