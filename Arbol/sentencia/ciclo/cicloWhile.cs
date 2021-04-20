using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.Arbol.Expresion;
using OC2_P2_201800523.AST;
using System.Collections.Generic;
using System;

namespace OC2_P2_201800523.Arbol.sentencia
{
    class cicloWhile : nodo
    {
        public cicloWhile(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public cicloWhile(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            resultado res;
            string argumento;
            string temporal;
            string temp = "";
            string array, pointer;


            string tempCondicion;
            string tempCiclo;
            string tempSalida = "";


            expresion izquierda;
            expresion derecha;
            resultado resIzq;
            resultado resDer;
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



            tempCondicion = cosasGlobalesewe.crearEtiqueta();
            tempCiclo = cosasGlobalesewe.crearEtiqueta();
            tempSalida = cosasGlobalesewe.crearEtiqueta();
            argumento = "/*INICIO DE WHILE*/\n";

            argumento += tempCondicion + ":\n";
            cosasGlobalesewe.concatenarAccion(argumento);
            expresion exp = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(1));
            res = exp.traducir(ref tablaActual, ambito, verdadero, falso, xd);

            if (res.argumento != null)
            {
                cosasGlobalesewe.concatenarAccion(res.argumento);
            }


            argumento = "if(" + res.valor + ") goto " + tempCiclo + ";\n"
                + "goto " + tempSalida + ";\n"
                + tempCiclo + ":";

            cosasGlobalesewe.concatenarAccion(argumento);

            hacerTraduccion(node.ChildNodes.ElementAt(4), ref tablaActual, ambito, tempCondicion, tempSalida, xd);

            argumento = "goto " + tempCondicion + ";\n"
                + tempSalida + ":\n";
            argumento += "/*FIN DE WHILE*/";
            cosasGlobalesewe.concatenarAccion(argumento);



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
