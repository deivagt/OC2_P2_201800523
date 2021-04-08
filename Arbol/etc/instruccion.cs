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
    class instruccion : nodo
    {
        public instruccion(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public instruccion(string tipo, ParseTreeNode node) : base(tipo, node) { }

        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            //if(node.ChildNodes.Count == 1) //Funcion
            //{
            //    Funcion_Procedimiento.Procedimiento_Funcion fn = new Funcion_Procedimiento.Procedimiento_Funcion(noterminales.FUNCION, node.ChildNodes.ElementAt(0));
            //    fn.Ejecutar();
            //}
            //else //Los demas jjjjj
            //{
            //    if(node.ChildNodes.ElementAt(0).Token.Text == "var")
            //    {
            //        variables.variable variable = new variables.variable(noterminales.VARIABLE, node.ChildNodes.ElementAt(1));
            //        variable.Ejecutar();
            //    }
            //    else if (node.ChildNodes.ElementAt(0).Token.Text == "const")
            //    {
            //        constantes.constante constante = new constantes.constante(noterminales.CONSTANTE, node.ChildNodes.ElementAt(1));
            //        constante.Ejecutar();
            //    }
            //    else if (node.ChildNodes.ElementAt(0).Token.Text == "type")
            //    {
            //        Tipos.decltipos tipo = new Tipos.decltipos(noterminales.DECLTIPOS, node.ChildNodes.ElementAt(1));
            //        tipo.Ejecutar();
            //    }
            //}

            return new resultado();
        }

       
    }
}
