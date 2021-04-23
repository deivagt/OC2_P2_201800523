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
    class valorReferencia : nodo
    {
        public valorReferencia(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public valorReferencia(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {            
            throw new NotImplementedException();
        }

        public void nuevaTraduccion(LinkedList<parametroCustom> listaParam)
        {
            bool esPorValor;
            LinkedList<ParseTreeNode> listaVar = new LinkedList<ParseTreeNode>();
            valor variables;
            valorReferencia otra;
            string tipo;
            if (node.ChildNodes.Count == 6)//VALOR_REFERENCIA + punto_coma + var + VALOR+ dos_puntos + TIPO  /*VALORES POR REFERENCIA*/
            {
                esPorValor = false;
                otra = new valorReferencia(noterminales.VALOR_REFERENCIA, node.ChildNodes.ElementAt(0));
                otra.nuevaTraduccion(listaParam);

                variables = new valor(noterminales.VALOR, node.ChildNodes.ElementAt(3));
                variables.nuevaTraduccion(listaVar);
                tipo = node.ChildNodes.ElementAt(5).Token.Text;
                foreach (var Var in listaVar)
                {
                    string id = Var.Token.Text;
                    listaParam.AddLast(new parametroCustom(id, esPorValor,tipo));
                }
            }
            else if (node.ChildNodes.Count == 5)//VALOR_REFERENCIA + punto_coma + VALOR+ dos_puntos + TIPO  /*VALORES POR VALOR*/     
            {
                esPorValor = true;
                otra = new valorReferencia(noterminales.VALOR_REFERENCIA, node.ChildNodes.ElementAt(0));
                otra.nuevaTraduccion(listaParam);

                variables = new valor(noterminales.VALOR, node.ChildNodes.ElementAt(2));
                variables.nuevaTraduccion(listaVar);
                tipo = node.ChildNodes.ElementAt(4).Token.Text;
                foreach (var Var in listaVar)
                {
                    string id = Var.Token.Text;
                    
                    listaParam.AddLast(new parametroCustom(id, esPorValor, tipo));
                }
                

            }
            else if (node.ChildNodes.Count == 4)//var + VALOR+ dos_puntos + TIPO /*POR REFERENCIA*/ 
            {
                esPorValor = false;
                variables = new valor(noterminales.VALOR, node.ChildNodes.ElementAt(1));
                variables.nuevaTraduccion(listaVar);
                tipo = node.ChildNodes.ElementAt(3).Token.Text;
                foreach (var Var in listaVar)
                {
                    string id = Var.Token.Text;
                    listaParam.AddLast(new parametroCustom(id, esPorValor,tipo));
                }
            }
            else // VALOR + dos_puntos + TIPO /*POR VALOR*/ 
            {
                esPorValor = true;
                variables = new valor(noterminales.VALOR, node.ChildNodes.ElementAt(0));
                variables.nuevaTraduccion(listaVar);
                tipo = node.ChildNodes.ElementAt(2).Token.Text;
                foreach (var Var in listaVar)
                {
                    string id = Var.Token.Text;
                    listaParam.AddLast(new parametroCustom(id, esPorValor,tipo));
                }
            }
        }
    }
}
