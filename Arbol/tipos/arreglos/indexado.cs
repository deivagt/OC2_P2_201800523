using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.AST;

namespace OC2_P2_201800523.Arbol.tipos.arreglos
{
    class indexado:nodo
    {
        public indexado(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public indexado(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            throw new NotImplementedException();
        }
        public void nuevaTraduccion(LinkedList<index> lista)
        {
            if (node.ChildNodes.Count != 3)
            {
                indexado siguiente = new indexado(noterminales.INDEXADO, node.ChildNodes.ElementAt(0));
                siguiente.nuevaTraduccion(lista);

                int inicio = int.Parse(node.ChildNodes.ElementAt(2).Token.Text);
                int final = int.Parse(node.ChildNodes.ElementAt(4).Token.Text);
                index nuevoIndex = new index(inicio,final);
                lista.AddLast(nuevoIndex);
            }
            else
            {
                int inicio = int.Parse(node.ChildNodes.ElementAt(0).Token.Text);
                int final = int.Parse(node.ChildNodes.ElementAt(2).Token.Text);
                index nuevoIndex = new index(inicio, final);
                lista.AddLast(nuevoIndex);
            }
        }

        
    }
}
