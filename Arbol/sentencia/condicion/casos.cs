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
    class casos : nodo
    {
        public casos(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public casos(string tipo, ParseTreeNode node) : base(tipo, node) { }

        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            string argumento = "";
            resultado res;
            resultado res1;
            string tempVerdadero = "";
            string temp;
            string temp1;
            string tempFalso = "";
            string tempSiguiente = "";
            string tempSalida = "";
            string array, pointer;

            ParseTreeNode exprCase;
            expresion expCase;

            if (ambito == "global")//Escribir en heap
            {
                array = "heap";
                pointer = "hp";
            }
            else //Escribir en stack
            {
                array = "stack";
                pointer = "sp";

            }



            
            tempSalida = cosasGlobalesewe.crearEtiqueta();
      
            ParseTreeNode expresion = node.ChildNodes.ElementAt(1);
            expresion expr = new expresion(noterminales.EXPRESION, expresion);
            res = expr.traducir(ref tablaActual, ambito, "", "", xd);

            if (res.argumento != null)
            {
                cosasGlobalesewe.concatenarAccion(res.argumento);
            }

            temp = res.valor;

            if (res.simbolo != null)
            {
                temp = cosasGlobalesewe.nuevoTemp(array + "[(int)" + res.valor + "]");
            }
            LinkedList<ParseTreeNode> listaCasos = new LinkedList<ParseTreeNode>();
            ParseTreeNode uncaso = node.ChildNodes.ElementAt(3);
            caso variosCasos = new caso(noterminales.CASO, uncaso);
            variosCasos.nuevaTraduccion(listaCasos);

            

                foreach (var caso in listaCasos)
                {
                    tempVerdadero = cosasGlobalesewe.crearEtiqueta();
                    tempSiguiente = cosasGlobalesewe.crearEtiqueta();
                    exprCase = caso.ChildNodes.ElementAt(0);
                    expCase = new expresion(noterminales.EXPRESION, exprCase);
                    res1 = expCase.traducir(ref tablaActual, ambito, "", "", xd);
                    if (res1.argumento != null)
                    {
                        cosasGlobalesewe.concatenarAccion(res1.argumento);
                    }

                    temp1 = res1.valor;

                    if (res1.simbolo != null)
                    {
                        temp1 = cosasGlobalesewe.nuevoTemp(array + "[(int)" + res.valor + "]");
                    }

                    argumento = "if(" + temp + " == " + temp1 + ") goto " + tempVerdadero + ";\n"
                        + "goto " + tempSiguiente + ";\n"
                        + tempVerdadero + ":\n";

                    cosasGlobalesewe.concatenarAccion(argumento);

                    hacerTraduccion(caso.ChildNodes.ElementAt(3), ref tablaActual, ambito, tempSiguiente, tempSalida, xd);

                    argumento = "goto " + tempSalida + ";\n"
                        + tempSiguiente + ":";
                    cosasGlobalesewe.concatenarAccion(argumento);
                    

                }

            if (node.ChildNodes.Count == 6)//Case normalito
            {
                argumento = tempSalida + ":";
                cosasGlobalesewe.concatenarAccion(argumento);

            }
            else//case else
            {
                hacerTraduccion(node.ChildNodes.ElementAt(6), ref tablaActual, ambito, tempSiguiente, tempSalida, xd);
                argumento = tempSalida + ":";
                cosasGlobalesewe.concatenarAccion(argumento);
            }




            return new resultado();
        }

        void hacerTraduccion(ParseTreeNode lstSent, ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            if (lstSent.ChildNodes.Count != 0)
            {
                LinkedList<sentencia> listaSentencias = new LinkedList<sentencia>();
                sentencias sentencias = new sentencias(noterminales.SENTENCIAS, lstSent);
                sentencias.nuevaTraduccion(listaSentencias);

                foreach (var sentencia in listaSentencias)
                {
                    sentencia.traducir(ref tablaActual, ambito, verdadero, falso, xd);
                }

            }
        }
    }
}
