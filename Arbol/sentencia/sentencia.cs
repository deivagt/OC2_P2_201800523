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
    class sentencia : nodo
    {
        public sentencia(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public sentencia(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            resultado res;
            ParseTreeNode palabraClave = node.ChildNodes.ElementAt(0);
            string argumento;

            string tempVerdadero = "";
            string tempFalso = "";
            string tempSiguiente = "";
            string tempSalida = "";
            string tempEtiqueta = "";

            switch (palabraClave.Token.Text)
            {
                case "if":
                    if (node.ChildNodes.Count == 7) // if normalito
                    {

                        /*Condicion del if*/
                        ParseTreeNode expresion = node.ChildNodes.ElementAt(1);
                        expresion expr = new expresion(noterminales.EXPRESION, expresion);
                        res = expr.traducir(ref tablaActual, ambito, verdadero, falso, xd);


                        tempVerdadero = cosasGlobalesewe.crearEtiqueta();
                        tempFalso = cosasGlobalesewe.crearEtiqueta();
                        tempSalida = cosasGlobalesewe.crearEtiqueta();

                        if (res.argumento != null)
                        {
                            cosasGlobalesewe.concatenarAccion(res.argumento);
                        }

                        argumento = "/*INICIA DECLARACION DE IF SIMPLE*/\n";
                        argumento += "if(" + res.valor + ") goto " + tempVerdadero + ";\n" + "goto " + tempFalso + ";\n";
                        argumento += tempVerdadero + ":";
                        cosasGlobalesewe.concatenarAccion(argumento);
                        hacerTraduccion(node.ChildNodes.ElementAt(4), ref tablaActual, ambito, verdadero, falso, xd);
                        argumento = "goto "+tempSalida+";\n";
                        argumento += tempFalso + ":"+ tempSalida + ":\n";
                        

                        cosasGlobalesewe.concatenarAccion(argumento);
                        argumento = "/*Finaliza DECLARACION DE IF SIMPLE*/";
                        cosasGlobalesewe.concatenarAccion(argumento);

                        //if (res.getValor() == "true")
                        //{
                        //    hacerEjecucion(node.ChildNodes.ElementAt(4));
                        //}
                        //else

                        //{
                        //    if (res.getValor() != "false")
                        //    {

                        //    }
                        //    else
                        //    {
                        //        //ERROR
                        //    }
                        //}
                    }
                    else //if else
                    {

                        /*Condicion del if*/
                        ParseTreeNode expresion = node.ChildNodes.ElementAt(1);
                        expresion expr = new expresion(noterminales.EXPRESION, expresion);
                        res = expr.traducir(ref tablaActual, ambito, verdadero, falso, xd);

                        if (res.argumento != null)
                        {
                            cosasGlobalesewe.concatenarAccion(res.argumento);
                        }

                        tempVerdadero = cosasGlobalesewe.crearEtiqueta();
                        tempFalso = cosasGlobalesewe.crearEtiqueta();
                        tempSalida = cosasGlobalesewe.crearEtiqueta();


                        argumento = "/*INICIA DECLARACION DE IF COMPUESTO EWE*/\n";
                        argumento += "if(" + res.valor + ") goto " + tempVerdadero + ";\n" + "goto " + tempFalso + ";\n";
                        argumento += tempVerdadero + ":";
                        cosasGlobalesewe.concatenarAccion(argumento);
                        
                        hacerTraduccion(node.ChildNodes.ElementAt(4), ref tablaActual, ambito, verdadero, falso, xd);
                        argumento = "goto " + tempSalida + ";\n";
                        argumento += tempFalso + ":\n";

                        cosasGlobalesewe.concatenarAccion(argumento);

                        /*ELSE IF*/
                        ParseTreeNode elseif = node.ChildNodes.ElementAt(6);
                        condicion.IF siguienteelseif = new condicion.IF(noterminales.ELSEIF, elseif);
                        siguienteelseif.traducir(ref tablaActual, ambito,tempSalida,falso, xd);

                        argumento = tempSalida + ":";
                        cosasGlobalesewe.concatenarAccion(argumento);
                        argumento = "/*Finaliza DECLARACION DE IF COMPUESTO EWE*/";
                        cosasGlobalesewe.concatenarAccion(argumento);
                       
                    }

                    return new resultado();

                case "write":
                    ParseTreeNode paramWrite = node.ChildNodes.ElementAt(2);
                    funcBasica.write write = new funcBasica.write(noterminales.PARAMETROSWRITELN, paramWrite);
                    write.traducir(ref tablaActual, ambito, verdadero, falso, xd);
                    return new resultado();

                case "writeln":
                    ParseTreeNode paramWriteln = node.ChildNodes.ElementAt(2);
                    funcBasica.write writeln = new funcBasica.write(noterminales.PARAMETROSWRITELN, paramWriteln);
                    writeln.traducir(ref tablaActual, ambito, verdadero, falso, xd);
                    cosasGlobalesewe.concatenarAccion("printf(\"\\n\"); ");
                    return new resultado();
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
