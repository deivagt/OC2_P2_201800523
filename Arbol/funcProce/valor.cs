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
    class valor :nodo
    {
        public valor(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public valor(string tipo, ParseTreeNode node) : base(tipo, node) { }

        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            throw new NotImplementedException();
        }

        public void nuevaTraduccion(LinkedList<ParseTreeNode> listaVar)
        {
            if (node.ChildNodes.Count != 1)
            {
                valor siguiente = new valor(noterminales.VALOR, node.ChildNodes.ElementAt(0));
                siguiente.nuevaTraduccion(listaVar);
                listaVar.AddLast(node.ChildNodes.ElementAt(2));
            }
            else
            {
                listaVar.AddLast(node.ChildNodes.ElementAt(0));
            }

        }
    }
}
