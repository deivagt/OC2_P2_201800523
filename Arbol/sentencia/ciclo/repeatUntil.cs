using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.Arbol.Expresion;
using OC2_P2_201800523.AST;
using System.Collections.Generic;
using System;

namespace OC2_P2_201800523.Arbol.sentencia.ciclo
{
    class repeatUntil :nodo
    {
        public repeatUntil(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public repeatUntil(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            resultado res;
            string argumento;
            string temporal;
            string temp = "";
            //string array, pointer;


            string tempCondicion;
            string tempCiclo;
            string tempSalida = "";


            expresion izquierda;
            expresion derecha;
            resultado resIzq;
            resultado resDer;
            



            tempCiclo = cosasGlobalesewe.crearEtiqueta();
            tempCondicion = cosasGlobalesewe.crearEtiqueta();
            
            tempSalida = cosasGlobalesewe.crearEtiqueta();
            argumento = "/*INICIO DE REPEAT UNTIL*/\n";
            argumento += tempCiclo + ":";
            cosasGlobalesewe.concatenarAccion(argumento);
            hacerTraduccion(node.ChildNodes.ElementAt(1), ref tablaActual, ambito, tempCondicion, tempSalida, xd);

            argumento = tempCondicion + ":";
            cosasGlobalesewe.concatenarAccion(argumento);

            expresion exp = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(3));
            res = exp.traducir(ref tablaActual, ambito, "", "", xd);

            if (res.argumento != null)
            {
                cosasGlobalesewe.concatenarAccion(res.argumento);
            }

            temp = res.valor;

            if (res.simbolo != null)
            {
                temp = cosasGlobalesewe.nuevoTemp("stack" + "[(int)" + res.valor + "]");
            }

            argumento = "if(" + temp + ") goto " + tempSalida + ";\n"
                + "goto " + tempCiclo + ";\n"
                 + tempSalida + ":\n"
            + "/*FIN DE REPEAT UNTIL*/";    
            
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
