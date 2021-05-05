using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.Arbol.tipos.arreglos;
using OC2_P2_201800523.Arbol.tipos.objetos;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.AST;

namespace OC2_P2_201800523.Arbol.tipos
{
    
    class types : nodo
    {
        public types(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public types(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            if (node.ChildNodes.Count == 10) //Arreglo
            {
                arreglo nuevoArreglo = new arreglo(terminales.rarray, node);
                nuevoArreglo.traducir(ref tablaActual, ambito, verdadero, falso, xd);
            }else if(node.ChildNodes.Count == 6)//Object
            {
                objeto nuevoObjeto = new objeto(terminales.robject, node);
                nuevoObjeto.traducir(ref tablaActual, ambito, verdadero, falso, xd);
            }
            return new resultado();
        }
    }
}
