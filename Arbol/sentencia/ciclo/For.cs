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
    class For : nodo
    {
        public For(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public For(string tipo, ParseTreeNode node) : base(tipo, node) { }

        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            resultado res;
            string argumento;
            string tempIncremento;
            string temp, temp1;
            //string array, pointer;


            string tempCondicion;
            string tempCiclo;
            string tempSalida = "";

            expresion izquierda;
            expresion derecha;
            resultado resIzq;
            resultado resDer;



            simbolo variable = tablaActual.buscar(node.ChildNodes.ElementAt(1).Token.Text, ambito);
            if (variable == null)
            {
                return new resultado();
            }



            tempCondicion = cosasGlobalesewe.crearEtiqueta();
            tempCiclo = cosasGlobalesewe.crearEtiqueta();
            tempIncremento = cosasGlobalesewe.crearEtiqueta();
            tempSalida = cosasGlobalesewe.crearEtiqueta();
            //PASO 1: ASIGNAR VARIABLE


            argumento = "/*INICIO DE FOR*/";

            cosasGlobalesewe.concatenarAccion(argumento);

            /*Crear codigo de asignacion de valor*/
            temp = cosasGlobalesewe.nuevoTemp();
            expresion expresionInicial = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(3));
            resultado resExprIni = expresionInicial.traducir(ref tablaActual, ambito, "", "", xd);
            if (resExprIni.argumento != null)
            {
                cosasGlobalesewe.concatenarAccion(resExprIni.argumento);
            }
            temp = resExprIni.valor;


            #region Codigo de asignar            


            argumento = "stack" + "[(int)" + variable.direccion + "] = " + temp + ";";
            cosasGlobalesewe.concatenarAccion(argumento);

            #endregion
            //FIN DE LA ASIGNACION

            //CODIGO DE LA CONDICION
            expresion exp = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(5));
            res = exp.traducir(ref tablaActual, ambito, "", "", xd);

            if (res.argumento != null)
            {
                cosasGlobalesewe.concatenarAccion(res.argumento);
            }
            temp1 = res.valor;


            argumento = tempCondicion + ":\n";
            temp = cosasGlobalesewe.nuevoTemp();
            cosasGlobalesewe.concatenarAccion(argumento);


            
            //EL incremento ewe
            if (node.ChildNodes.ElementAt(4).Token.Text == "to")
            {
                argumento = temp + " = " + "stack" + "[(int)" + variable.direccion + "];\n"

               + "if(" + temp + "<=" + temp1 + ") goto " + tempCiclo + ";\n"
           + "goto " + tempSalida + ";\n"

           + tempIncremento + ":";
                cosasGlobalesewe.concatenarAccion(argumento);
                argumento = "stack" + "[(int)" + variable.direccion + "]" + " = " + temp + " + 1;\n"
           + "goto " + tempCondicion + ";\n"
           + tempCiclo + ":";
            }
            else
            {
                argumento = temp + " = " + "stack" + "[(int)" + variable.direccion + "];\n"

               + "if(" + temp + ">=" + temp1 + ") goto " + tempCiclo + ";\n"
           + "goto " + tempSalida + ";\n"
           + tempIncremento + ":";
                cosasGlobalesewe.concatenarAccion(argumento);
                argumento = "stack" + "[(int)" + variable.direccion + "]" + " = " + temp + " - 1;\n"
            + "goto " + tempCondicion + ";\n"
            + tempCiclo + ":";
            }

            cosasGlobalesewe.concatenarAccion(argumento);
            cosasGlobalesewe.concatenarAccion("/*INICIO COSAS FOR*****************/");
            hacerTraduccion(node.ChildNodes.ElementAt(8), ref tablaActual, ambito, tempIncremento, tempSalida, xd);
            cosasGlobalesewe.concatenarAccion("/*FIN COSAS FOR*******************/");

            argumento = "goto " + tempIncremento + ";\n"
               + tempSalida + ":\n";
            argumento += "/*FIN DE FOR*/";
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
