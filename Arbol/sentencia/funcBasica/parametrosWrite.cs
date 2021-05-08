using OC2_P2_201800523.tablaSimbolos;
using System;
using System.Collections.Generic;
using System.Text;
using OC2_P2_201800523.Arbol.Expresion;
using OC2_P2_201800523.AST;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;

namespace OC2_P2_201800523.Arbol.sentencia.funcBasica
{
    class parametrosWrite : nodo
    {
        public parametrosWrite(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public parametrosWrite(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            throw new NotImplementedException();
        }

        public void nuevoParametro(LinkedList<expresion> lista)
        {
            if (node.ChildNodes.Count == 3)
            {
                parametrosWrite sigExpr = new parametrosWrite(noterminales.PARAMETROSWRITELN, node.ChildNodes.ElementAt(0));
                sigExpr.nuevoParametro(lista);

                expresion exp = new expresion(noterminales.SENTENCIA, node.ChildNodes.ElementAt(2));
                lista.AddLast(exp);
            }
            else if (node.ChildNodes.Count == 1)
            {
                expresion exp = new expresion(noterminales.SENTENCIA, node.ChildNodes.ElementAt(0));
                lista.AddLast(exp);
            }
            else
            {
                //XD
            }
        }
    }
}
