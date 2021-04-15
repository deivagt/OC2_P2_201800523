
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.Arbol.Expresion;
using OC2_P2_201800523.AST;
using System.Collections.Generic;

namespace OC2_P2_201800523.Arbol.sentencia
{
    class sentencias : nodo
    {
        public sentencias(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public sentencias(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            throw new System.NotImplementedException();
        }

        public void nuevaTraduccion(LinkedList<sentencia> lista)
        {
            if (node.ChildNodes.Count == 2)
            {
                sentencias siguiente = new sentencias(noterminales.SENTENCIAS, node.ChildNodes.ElementAt(0));
                siguiente.nuevaTraduccion(lista);

                sentencia ins = new sentencia(noterminales.SENTENCIA, node.ChildNodes.ElementAt(1));
                lista.AddLast(ins);
            }
            else if (node.ChildNodes.Count == 1)
            {
                sentencia ins = new sentencia(noterminales.INSTRUCCION, node.ChildNodes.ElementAt(0));
                lista.AddLast(ins);
            }
            else
            {
                //XD
            }
        }

        
    }
}
