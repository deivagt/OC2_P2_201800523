using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.AST;

namespace OC2_P2_201800523.Arbol.Expresion
{
    class expresion : nodo
    {
        public expresion(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public expresion(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            resultado res;
            string argumento;
            string temp = "";

            string tempVerdadero = "";
            string tempFalso = "";
            string tempSiguiente = "";
            string tempSalida = "";
            string tempEtiqueta = "";

            expresion izquierda;
            expresion derecha;
            resultado resIzq;
            resultado resDer;

            if (node.ChildNodes.Count == 1)
            {
                ParseTreeNode salida = node.ChildNodes.ElementAt(0);
                if (salida.Term.ToString() == "numero")
                {

                    if (int.TryParse(salida.Token.Text, out int i) == true)
                    {
                        res = new resultado(terminales.numero, int.Parse(salida.Token.Text));

                        return res;
                    }
                    else
                    {

                        res = new resultado(terminales.numero, double.Parse(salida.Token.Text));
                        return res;
                    }

                }
                else if (salida.Term.ToString() == "cadena")
                {
                    string retorno = salida.Token.Text.Replace("'", "");
                    if (retorno.Length == 1)
                    {
                        return new resultado(terminales.rchar, retorno);
                    }
                    else
                    {
                        return new resultado(terminales.cadena, retorno);
                    }

                }
                else if (salida.Term.ToString() == "true" || salida.Term.ToString() == "false")
                {

                    if (salida.Term.ToString() == "true")
                    {
                        return new resultado(salida.Term.ToString(), "1");
                    }
                    else
                    {
                        return new resultado(salida.Term.ToString(), "0");
                    }

                }
                else // id
                {
                    //if (salida.Token.Text == "true" || salida.Token.Text == "false")
                    //{
                    //    return new resultado(terminales.rboolean, salida.Token.Text);
                    //}
                    //else
                    //{
                    //    simbolo a = manejadorArbol.tabladeSimbolos.buscarSimbolo(salida.Token.Text);
                    //    if (a != null)
                    //    {
                    //        if (a.tipo == "integer" || a.tipo == "real")
                    //        {
                    //            return new resultado(terminales.numero, a.valor);
                    //        }
                    //        else
                    //        {
                    //            return new resultado(a.tipo, a.valor);
                    //        }

                    //    }
                    //}


                    return new resultado();
                }

            }
            else if (node.ChildNodes.Count == 2)
            {
                if (node.ChildNodes.ElementAt(0).Term.ToString() == "-")//UMINUS
                {
                    tempEtiqueta = cosasGlobalesewe.nuevoTemp();
                    derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(1));
                    resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                    argumento = tempEtiqueta + " = - " + resDer.valor + ";\n"; 

                    return new resultado(terminales.numero, tempEtiqueta, argumento);
                    
                }
                else//NOT
                {

                    tempVerdadero = cosasGlobalesewe.crearEtiqueta();
                    tempFalso = cosasGlobalesewe.crearEtiqueta();
                    tempSalida = cosasGlobalesewe.crearEtiqueta();
                    derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(1));

                    resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                    argumento = "if(" + resDer.valor + ") goto " + tempVerdadero + ";\n" + "goto " + tempFalso + ";\n";

                    temp = cosasGlobalesewe.nuevoTemp();

                    argumento += tempVerdadero + ":\n";
                    argumento += temp + " = 0" + ";\n";
                    argumento += "goto " + tempSalida + ";\n" + tempFalso + ":\n";
                    argumento += temp + " = 1" + ";\n";
                    argumento += tempSalida + ":\n";

                    //cosasGlobalesewe.concatenarAccion(argumento);
                    return new resultado(terminales.not, temp, argumento);

                }
            }
            else if (node.ChildNodes.Count == 3 && (node.ChildNodes.ElementAt(0).Term.ToString() == "EXPRESION" || node.ChildNodes.ElementAt(0).Term.ToString() == "("))
            {

                if (node.ChildNodes.ElementAt(0).Term.ToString() != "EXPRESION") /*CASO DEL PARENTESIS*/
                {
                    expresion centro = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(1));
                    res = centro.traducir(ref tablaActual, ambito, "", "", "");
                    return res;

                }
                else
                {

                    ParseTreeNode salida = node.ChildNodes.ElementAt(1);

                    switch (salida.Token.Text)
                    {
                        case "+":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " + " + resDer.valor;
                            temp = cosasGlobalesewe.nuevoTemp(argumento);
                            return new resultado(terminales.numero, temp);
                        case "-":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " - " + resDer.valor;
                            temp = cosasGlobalesewe.nuevoTemp(argumento);
                            return new resultado(terminales.numero, temp);
                        case "*":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " * " + resDer.valor;
                            temp = cosasGlobalesewe.nuevoTemp(argumento);
                            return new resultado(terminales.numero, temp);

                        case "/":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " / " + resDer.valor;
                            temp = cosasGlobalesewe.nuevoTemp(argumento);
                            return new resultado(terminales.numero, temp);
                        case "mod":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " % " + resDer.valor;
                            temp = cosasGlobalesewe.nuevoTemp(argumento);
                            return new resultado(terminales.numero, temp);


                        case "AND":
                            if (verdadero != "" && falso != "")
                            {
                                tempFalso = falso;
                                tempVerdadero = verdadero;

                            }
                            else
                            {
                                tempVerdadero = cosasGlobalesewe.crearEtiqueta();
                                tempFalso = cosasGlobalesewe.crearEtiqueta();
                            }
                            tempSiguiente = cosasGlobalesewe.crearEtiqueta();

                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, tempSiguiente, tempFalso, "");
                            resDer = derecha.traducir(ref tablaActual, ambito, tempVerdadero, tempFalso, "");

                            if (verdadero == "" && falso == "") //ULTIMO AND OR NOT
                            {
                                argumento = "";

                                tempSalida = cosasGlobalesewe.crearEtiqueta();

                                if (resIzq.tipo != terminales.and && resIzq.tipo != terminales.or && resIzq.tipo != terminales.not)
                                {
                                    argumento = "if(" + resIzq.valor + ") goto " + tempSiguiente + ";\n" + "goto " + tempFalso + ";\n";

                                }
                                argumento += resIzq.argumento;
                                argumento += tempSiguiente + ":\n" + "";
                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + resDer.valor + ") goto " + tempVerdadero + ";\n" + "goto " + tempFalso + ";\n";

                                }
                                argumento += resDer.argumento;
                                temp = cosasGlobalesewe.nuevoTemp();
                                argumento += tempVerdadero + ":\n";
                                argumento += temp + " = 1" + ";\n";
                                argumento += "goto " + tempSalida + ";\n" + tempFalso + ":\n";
                                argumento += temp + " = 0" + ";\n";
                                argumento += tempSalida + ":\n";

                                cosasGlobalesewe.temp++;
                                //cosasGlobalesewe.concatenarAccion(argumento);
                            }
                            else //Previo al ultimo AND OR NOT
                            {
                                argumento = "";
                                if (resIzq.tipo != terminales.and && resIzq.tipo != terminales.or && resIzq.tipo != terminales.not)
                                {
                                    argumento = "if(" + resIzq.valor + ") goto " + tempSiguiente + ";\n" + "goto " + falso + ";\n";
                                }

                                argumento += resIzq.argumento;
                                argumento += tempSiguiente + ":\n" + "";
                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + resDer.valor + ") goto " + verdadero + ";\n" + "goto " + falso + ";\n";

                                }
                                argumento += resDer.argumento;
                                //cosasGlobalesewe.concatenarAccion(argumento);
                            }

                            return new resultado(terminales.and, temp, argumento);

                        case "OR":

                            if (verdadero != "" && falso != "")
                            {
                                tempFalso = falso;
                                tempVerdadero = verdadero;

                            }
                            else
                            {
                                tempVerdadero = cosasGlobalesewe.crearEtiqueta();
                                tempFalso = cosasGlobalesewe.crearEtiqueta();
                            }
                            tempSiguiente = cosasGlobalesewe.crearEtiqueta();


                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, tempVerdadero, tempSiguiente, "");
                            resDer = derecha.traducir(ref tablaActual, ambito, tempVerdadero, tempFalso, "");

                            if (verdadero == "" && falso == "") //ULTIMO AND OR NOT
                            {
                                argumento = "";

                                tempSalida = cosasGlobalesewe.crearEtiqueta();

                                if (resIzq.tipo != terminales.and && resIzq.tipo != terminales.or && resIzq.tipo != terminales.not)
                                {
                                    argumento = "if(" + resIzq.valor + ") goto " + tempVerdadero + ";\n" + "goto " + tempSiguiente + ";\n";

                                }
                                argumento += resIzq.argumento;
                                argumento += tempSiguiente + ":\n" + "";
                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + resDer.valor + ") goto " + tempVerdadero + ";\n" + "goto " + tempFalso + ";\n";

                                }
                                argumento += resDer.argumento;
                                temp = cosasGlobalesewe.nuevoTemp();
                                argumento += tempVerdadero + ":\n";
                                argumento += temp + " = 1" + ";\n";
                                argumento += "goto " + tempSalida + ";\n" + tempFalso + ":\n";
                                argumento += temp + " = 0" + ";\n";
                                argumento += tempSalida + ":\n";

                                cosasGlobalesewe.temp++;
                                //cosasGlobalesewe.concatenarAccion(argumento);
                            }
                            else //Previo al ultimo AND OR NOT
                            {
                                argumento = "";
                                if (resIzq.tipo != terminales.and && resIzq.tipo != terminales.or && resIzq.tipo != terminales.not)
                                {
                                    argumento = "if(" + resIzq.valor + ") goto " + verdadero + ";\n" + "goto " + tempSiguiente + ";\n";
                                }

                                argumento += resIzq.argumento + ";\n";
                                argumento += tempSiguiente + ":\n" + "";

                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + resDer.valor + ") goto " + verdadero + ";\n" + "goto " + falso + ";\n";
                                }
                                argumento += resDer.argumento;
                                //cosasGlobalesewe.concatenarAccion(argumento);
                            }

                            return new resultado(terminales.or, temp, argumento);

                        case "and":

                            if (verdadero != "" && falso != "")
                            {
                                tempFalso = falso;
                                tempVerdadero = verdadero;

                            }
                            else
                            {
                                tempVerdadero = cosasGlobalesewe.crearEtiqueta();
                                tempFalso = cosasGlobalesewe.crearEtiqueta();
                            }
                            tempSiguiente = cosasGlobalesewe.crearEtiqueta();

                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, tempSiguiente, tempFalso, "");
                            resDer = derecha.traducir(ref tablaActual, ambito, tempVerdadero, tempFalso, "");

                            if (verdadero == "" && falso == "") //ULTIMO AND OR NOT
                            {
                                argumento = "";

                                tempSalida = cosasGlobalesewe.crearEtiqueta();

                                if (resIzq.tipo != terminales.and && resIzq.tipo != terminales.or && resIzq.tipo != terminales.not)
                                {
                                    argumento = "if(" + resIzq.valor + ") goto " + tempSiguiente + ";\n" + "goto " + tempFalso + ";\n";

                                }
                                argumento += resIzq.argumento ;
                                argumento += tempSiguiente + ":\n" + "";
                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + resDer.valor + ") goto " + tempVerdadero + ";\n" + "goto " + tempFalso + ";\n";

                                }
                                argumento += resDer.argumento ;
                                temp = cosasGlobalesewe.nuevoTemp();
                                argumento += tempVerdadero + ":\n";
                                argumento += temp + " = 1" + ";\n";
                                argumento += "goto " + tempSalida + ";\n" + tempFalso + ":\n";
                                argumento += temp + " = 0" + ";\n";
                                argumento += tempSalida + ":\n";

                                cosasGlobalesewe.temp++;
                                //cosasGlobalesewe.concatenarAccion(argumento);
                            }
                            else //Previo al ultimo AND OR NOT
                            {
                                argumento = "";
                                if (resIzq.tipo != terminales.and && resIzq.tipo != terminales.or && resIzq.tipo != terminales.not)
                                {
                                    argumento = "if(" + resIzq.valor + ") goto " + tempSiguiente + ";\n" + "goto " + falso + ";\n";
                                }

                                argumento += resIzq.argumento ;
                                argumento += tempSiguiente + ":\n" + "";
                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + resDer.valor + ") goto " + verdadero + ";\n" + "goto " + falso + ";\n";

                                }
                                argumento += resDer.argumento ;
                                //cosasGlobalesewe.concatenarAccion(argumento);
                            }

                            return new resultado(terminales.and, temp, argumento);
                        case "or":

                            if (verdadero != "" && falso != "")
                            {
                                tempFalso = falso;
                                tempVerdadero = verdadero;

                            }
                            else
                            {
                                tempVerdadero = cosasGlobalesewe.crearEtiqueta();
                                tempFalso = cosasGlobalesewe.crearEtiqueta();
                            }
                            tempSiguiente = cosasGlobalesewe.crearEtiqueta();


                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, tempVerdadero, tempSiguiente, "");
                            resDer = derecha.traducir(ref tablaActual, ambito, tempVerdadero, tempFalso, "");

                            if (verdadero == "" && falso == "") //ULTIMO AND OR NOT
                            {
                                argumento = "";

                                tempSalida = cosasGlobalesewe.crearEtiqueta();

                                if (resIzq.tipo != terminales.and && resIzq.tipo != terminales.or && resIzq.tipo != terminales.not)
                                {
                                    argumento = "if(" + resIzq.valor + ") goto " + tempVerdadero + ";\n" + "goto " + tempSiguiente + ";\n";

                                }
                                argumento += resIzq.argumento ;
                                argumento += tempSiguiente + ":\n" + "";
                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + resDer.valor + ") goto " + tempVerdadero + ";\n" + "goto " + tempFalso + ";\n";

                                }
                                argumento += resDer.argumento ;
                                temp = cosasGlobalesewe.nuevoTemp();
                                argumento += tempVerdadero + ":\n";
                                argumento += temp + " = 1" + ";\n";
                                argumento += "goto " + tempSalida + ";\n" + tempFalso + ":\n";
                                argumento += temp + " = 0" + ";\n";
                                argumento += tempSalida + ":\n";

                                cosasGlobalesewe.temp++;
                                //cosasGlobalesewe.concatenarAccion(argumento);
                            }
                            else //Previo al ultimo AND OR NOT
                            {
                                argumento = "";
                                if (resIzq.tipo != terminales.and && resIzq.tipo != terminales.or && resIzq.tipo != terminales.not)
                                {
                                    argumento = "if(" + resIzq.valor + ") goto " + verdadero + ";\n" + "goto " + tempSiguiente + ";\n";
                                }

                                argumento += resIzq.argumento + ";\n";
                                argumento += tempSiguiente + ":\n" + "";

                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + resDer.valor + ") goto " + verdadero + ";\n" + "goto " + falso + ";\n";
                                }
                                argumento += resDer.argumento ;
                                //cosasGlobalesewe.concatenarAccion(argumento);
                            }

                            return new resultado(terminales.or, temp,argumento);
                        case "=":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " == " + resDer.valor;
                            return new resultado(terminales.igual, argumento);
                        case "<>":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " != " + resDer.valor;
                            return new resultado(terminales.distinto, argumento);
                        case "<":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " < " + resDer.valor;
                            return new resultado(terminales.menor, argumento);
                        case ">":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " > " + resDer.valor;
                            return new resultado(terminales.mayor, argumento);
                        case "<=":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " <= " + resDer.valor;
                            return new resultado(terminales.menor_igual, argumento);
                        case ">=":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " >= " + resDer.valor;
                            return new resultado(terminales.mayor_igual, argumento);

                    }
                }
            }
            else
            {
                //simbolo s = manejadorArbol.tabladeSimbolos.buscarFuncion(node.ChildNodes.ElementAt(0).Token.Text);
                //if (s != null)
                //{
                //    manejadorArbol.ambitoActual = s.id;
                //    Funcion_Procedimiento.objetoFuncion of = s.fn;
                //    hacerEjecucion(of.lstSent);
                //    if (manejadorArbol.controlExit == true)
                //    {
                //        manejadorArbol.controlExit = false;
                //    }

                //    manejadorArbol.ambitoActual = "global";

                //    return new resultado(s.tipo, s.valor);
                //}


            }

            return new resultado();
        }


    }
}
