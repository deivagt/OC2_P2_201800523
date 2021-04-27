using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.AST;
using OC2_P2_201800523.Arbol.sentencia;
using OC2_P2_201800523.Arbol.Expresion;

namespace OC2_P2_201800523.Arbol.funcProce
{
    class parametros:nodo
    {
        public parametros(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public parametros(string tipo, ParseTreeNode node) : base(tipo, node) { }

        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            throw new NotImplementedException();
        }
        public void nuevaTraduccion(LinkedList<expresion> lista)
        {
            if (node.ChildNodes.Count != 1)
            {
                parametros siguiente = new parametros(noterminales.OTRA_DECL_VARIABLE, node.ChildNodes.ElementAt(0));
                siguiente.nuevaTraduccion(lista);
                expresion expresion = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));
                lista.AddLast(expresion);
            }
            else
            {
                expresion expresion = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                lista.AddLast(expresion);
            }
        }
    }
}
