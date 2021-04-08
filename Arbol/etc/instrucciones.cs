using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.AST;

namespace OC2_P2_201800523.Arbol.etc
{
    class instrucciones : nodo
    {
        public instrucciones(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public instrucciones(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {

            throw new NotImplementedException();
        }

        public void nuevaTraduccion(LinkedList<instruccion> lista)
        {
            if (node.ChildNodes.Count != 1)
            {
                instrucciones siguiente = new instrucciones(noterminales.INSTRUCCIONES, node.ChildNodes.ElementAt(0));
                siguiente.nuevaTraduccion(lista);

                instruccion ins = new instruccion(noterminales.INSTRUCCION, node.ChildNodes.ElementAt(1));
                lista.AddLast(ins);
            }
            else
            {
                instruccion ins = new instruccion(noterminales.INSTRUCCION, node.ChildNodes.ElementAt(0));
                lista.AddLast(ins);
            }
        }

        
    }
}
