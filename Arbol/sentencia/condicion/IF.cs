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
    class IF : nodo
    {
        public IF(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public IF(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            string argumento;
            resultado res;
            string tempVerdadero = "";
            string tempFalso = "";
            string tempSiguiente = "";
            string tempEtiqueta = "";
            if (node.ChildNodes.Count == 4) // else normalito
            {

                hacerTraduccion(node.ChildNodes.ElementAt(2), ref tablaActual, ambito, verdadero, falso, xd);
                
                //hacerEjecucion(node.ChildNodes.ElementAt(2));
            }
            else //else if
            {
                /*Condicion del if*/
                ParseTreeNode expresion = node.ChildNodes.ElementAt(2);
                expresion expr = new expresion(noterminales.EXPRESION, expresion);
                res = expr.traducir(ref tablaActual, ambito, verdadero, falso, xd);

                if (res.argumento != null)
                {
                    cosasGlobalesewe.concatenarAccion(res.argumento);
                }

                tempVerdadero = cosasGlobalesewe.crearEtiqueta();
                tempFalso = cosasGlobalesewe.crearEtiqueta();


                argumento = "/*INICIA DECLARACION DE IF COMPUESTO EWE*/\n";
                argumento += "if(" + res.valor + ") goto " + tempVerdadero + ";\n" + "goto " + tempFalso + ";\n";
                argumento += tempVerdadero + ":";
                cosasGlobalesewe.concatenarAccion(argumento);
                hacerTraduccion(node.ChildNodes.ElementAt(5), ref tablaActual, ambito, verdadero, falso, xd);
                argumento = "goto " + verdadero + ";\n"; //SALIDA DE LA CADENA IF ELSE
                argumento += tempFalso + ":\n";

                cosasGlobalesewe.concatenarAccion(argumento);
                if(node.ChildNodes.ElementAt(7).ChildNodes.Count != 0)
                {
                    
                    ParseTreeNode elseif = node.ChildNodes.ElementAt(7);
                    condicion.IF siguienteelseif = new condicion.IF(noterminales.ELSEIF, elseif);
                    siguienteelseif.traducir(ref tablaActual, ambito, verdadero, falso, xd);
                }
                

                
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
