using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Parsing;
using OC2_P2_201800523.AST;
using OC2_P2_201800523.tablaSimbolos;

namespace OC2_P2_201800523.Arbol
{
    class ini : nodo
    {
        public ini(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public ini(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            variables.variable a = new variables.variable(noterminales.VARIABLE, node.ChildNodes.ElementAt(0));
            //etc.cosas a = new etc.cosas(noterminales.COSAS, node.ChildNodes.ElementAt(0));            
            resultado res = a.traducir(ref tablaActual, ambito, "", "", "");
            return res;
        }
    }
}
