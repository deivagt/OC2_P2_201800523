using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.Arbol.Expresion;
using OC2_P2_201800523.AST;
using System.Collections.Generic;
using System;

namespace OC2_P2_201800523.Arbol.sentencia.condicion
{
    class caso:nodo
    {
        public caso(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public caso(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public void nuevaTraduccion(LinkedList<ParseTreeNode> lista)
        {
            if (node.ChildNodes.Count != 1)
            {
                caso siguiente = new caso(noterminales.OTRA_DECL_VARIABLE, node.ChildNodes.ElementAt(0));
                siguiente.nuevaTraduccion(lista);
                lista.AddLast(node.ChildNodes.ElementAt(1));
            }
            else
            {
                lista.AddLast(node.ChildNodes.ElementAt(0));
            }
        }

        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            throw new NotImplementedException();
        }
    }
}
