using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.AST;
namespace OC2_P2_201800523.Arbol.funcProce
{
    class declararEnFunc:nodo
    {

        public declararEnFunc(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public declararEnFunc(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {

            throw new NotImplementedException();
        }

        public void nuevaTraduccion(LinkedList<variables.variable> lista)
        {
            if (node.ChildNodes.Count == 3)
            {
                declararEnFunc siguiente = new declararEnFunc(noterminales.DECLARARENFUNC, node.ChildNodes.ElementAt(0));
                siguiente.nuevaTraduccion(lista);

                variables.variable ins = new variables.variable(noterminales.VARIABLE, node.ChildNodes.ElementAt(2));
                lista.AddLast(ins);
            }
            else if (node.ChildNodes.Count == 2)
            {
                variables.variable ins = new variables.variable(noterminales.VARIABLE, node.ChildNodes.ElementAt(1));
                lista.AddLast(ins);
            }
        }
    }
}
