using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.Arbol.Expresion;
using OC2_P2_201800523.AST;


namespace OC2_P2_201800523.Arbol.variables
{
    class variable : nodo
    {
        public variable(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public variable(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            string argumento = "";
            if (node.ChildNodes.Count == 5)
            {
                //ParseTreeNode otraVariable = node.ChildNodes.ElementAt(4);

                //if (node.ChildNodes.ElementAt(0).Term.ToString() == terminales.id)
                //{
                //    ParseTreeNode id = node.ChildNodes.ElementAt(0);
                //    ParseTreeNode tipo = node.ChildNodes.ElementAt(2);

                //    int fila = id.Token.Location.Line;
                //    int columna = id.Token.Location.Column;
                //    string eltipo = tipo.ChildNodes.ElementAt(0).Token.Text;
                //    simbolo nuevoSimbolo = new simbolo(manejadorArbol.ambitoActual, id.Token.Text, eltipo, fila + 1, columna + 1);
                //    manejadorArbol.tabladeSimbolos.agregarSimbolo(nuevoSimbolo);
                //}
                //else
                //{
                //    LinkedList<ParseTreeNode> listaVar = new LinkedList<ParseTreeNode>();
                //    ParseTreeNode ids = node.ChildNodes.ElementAt(0);
                //    ParseTreeNode tipo = node.ChildNodes.ElementAt(2);

                //    otra_decl_variable variasVariables = new otra_decl_variable(noterminales.OTRA_DECL_VARIABLE, ids);

                //    variasVariables.nuevaEjecucion(listaVar);

                    
                     
                //     foreach (var a in listaVar)
                //    {
                //        int fila = a.Token.Location.Line;
                //        int columna = a.Token.Location.Column;
                //        string eltipo = tipo.ChildNodes.ElementAt(0).Token.Text;
                //        simbolo nuevoSimbolo = new simbolo(manejadorArbol.ambitoActual, a.Token.Text, eltipo, fila + 1, columna + 1);
                //        manejadorArbol.tabladeSimbolos.agregarSimbolo(nuevoSimbolo);
                //    }
                    
                //}
                //if (otraVariable.ChildNodes.Count != 0)
                //{
                //    variable otraVar = new variable(noterminales.VARIABLE, otraVariable);
                //    otraVar.Ejecutar();
                //}
            }
            else
            {

                ParseTreeNode id = node.ChildNodes.ElementAt(0);
                ParseTreeNode tipo = node.ChildNodes.ElementAt(2);
                ParseTreeNode expresion = node.ChildNodes.ElementAt(4);
                ParseTreeNode otraVariable = node.ChildNodes.ElementAt(6);

                expresion expr = new expresion(noterminales.EXPRESION, expresion);
                resultado res = expr.traducir(ref tablaActual, ambito, "","","");

                cosasGlobalesewe.concatenarAccion(res.argumento);
                //Creacion de simbolo
                int fila = id.Token.Location.Line;
                int columna = id.Token.Location.Column;
                string eltipo = tipo.ChildNodes.ElementAt(0).Token.Text;

                string temp = cosasGlobalesewe.nuevoTemp();

                simbolo nuevoSimbolo = new simbolo(ambito, id.Token.Text, eltipo, temp, fila + 1, columna + 1);

                tablaActual.agregarSimbolo(nuevoSimbolo);

                /*Escribir en C3D*/
                if(ambito == "global")//Escribir en heap
                {
                    argumento = temp + " = hp;\n"; 
                    argumento += "hp = hp + 1;\n";
                    argumento += "heap[(int)" + temp + "] = " + res.valor + ";\n";
                }
                else //Escribir en stack
                {

                }

                cosasGlobalesewe.concatenarAccion(argumento);
                if (otraVariable.ChildNodes.Count != 0)
                {
                    variable otraVar = new variable(noterminales.VARIABLE, otraVariable);
                    otraVar.traducir(ref tablaActual,ambito,"","","");
                }

            }
            return new resultado();
        }
    }
}
