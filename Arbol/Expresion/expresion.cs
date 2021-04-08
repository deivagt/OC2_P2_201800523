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
            string temp;

            string tempVerdadero = "";
            string tempFalso = "";
            string tempSiguiente = "";
            string tempSalida = "";
            string tempEtiqueta = "";

            if (node.ChildNodes.Count == 1)
            {
                ParseTreeNode salida = node.ChildNodes.ElementAt(0);
                if (salida.Term.ToString() == "numero")
                {

                    if (int.TryParse(salida.Token.Text, out int i) == true)
                    {
                        res = new resultado(salida.Term.ToString(), int.Parse(salida.Token.Text));

                        return res;
                    }
                    else
                    {

                        res = new resultado(salida.Term.ToString(), double.Parse(salida.Token.Text));
                        return res;
                    }

                }
                else if (salida.Term.ToString() == "cadena")
                {
                    //string retorno = salida.Token.Text.Replace("'", "");
                    //if (retorno.Length == 1)
                    //{
                    //    return new resultado(terminales.rchar, retorno);
                    //}
                    //else
                    //{
                    //    return new resultado(terminales.rstring, retorno);
                    //}

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
                //if (node.ChildNodes.ElementAt(0).Term.ToString() != "-")//UMINUS
                //{

                //    expresion derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(1));
                //    resultado resDer = derecha.Ejecutar();
                //    resultado res = new resultado(terminales.numero, -resDer.getNumero());
                //    return res;
                //}
                //else//NOT
                //{
                //    expresion derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(1));
                //    resultado resDer = derecha.Ejecutar();
                //    resultado res = new resultado(terminales.numero, (!resDer.getBooleano()).ToString());
                //    return res;
                //}
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
                    expresion izquierda;
                    expresion derecha;
                    resultado resIzq;
                    resultado resDer;

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
                        //case "and":
                        //    if (resIzq.getBooleano() == true && resDer.getBooleano() == true)
                        //    {
                        //        return new resultado(terminales.rtrue, "true");
                        //    }
                        //    else
                        //    {
                        //        return new resultado(terminales.rfalse, "false");
                        //    }
                        //case "or":
                        //    if (resIzq.getBooleano() == false && resDer.getBooleano() == false)
                        //    {
                        //        return new resultado(terminales.rfalse, "false");
                        //    }
                        //    else
                        //    {
                        //        return new resultado(terminales.rtrue, "true");
                        //    }
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
                                
                                if(resIzq.tipo != terminales.and && resIzq.tipo != terminales.or && resIzq.tipo != terminales.not)
                                {
                                    argumento = resIzq.tipo +"if(" + resIzq.valor + ") goto " + tempSiguiente + ";\n" + "goto " + tempFalso + ";\n";

                                }

                                argumento += tempSiguiente + ":\n" + "";
                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + resDer.valor + ") goto " + tempVerdadero + ";\n" + "goto " + tempFalso + ";\n";

                                }

                                argumento += tempVerdadero + ":\n";
                                argumento += "t" + cosasGlobalesewe.temp + " = 1" + ";\n";
                                argumento += "goto " + tempSalida + ";\n" + tempFalso + ":\n";
                                argumento += "t" + cosasGlobalesewe.temp + " = 0" + ";\n";
                                argumento += tempSalida + ":\n";

                                cosasGlobalesewe.temp++;
                                cosasGlobalesewe.concatenarAccion(argumento);
                            }
                            else //Previo al ultimo AND OR NOT
                            {
                                argumento = "";
                                if (resIzq.tipo != terminales.and && resIzq.tipo != terminales.or && resIzq.tipo != terminales.not)
                                {
                                    argumento = "if(" + resIzq.valor + ") goto " + tempSiguiente + ";\n" + "goto " + falso + ";\n";
                                }


                                argumento += tempSiguiente + ":\n" + "";
                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + resDer.valor + ") goto " + verdadero + ";\n" + "goto " + falso + ";";
                                }
                                cosasGlobalesewe.concatenarAccion(argumento);
                            }


                            return new resultado(terminales.and, tempEtiqueta);
                        //case "OR":
                        //    if (resIzq.getBooleano() == false && resDer.getBooleano() == false)
                        //    {
                        //        return new resultado(terminales.rfalse, "false");
                        //    }
                        //    else
                        //    {
                        //        return new resultado(terminales.rtrue, "true");
                        //    }
                        case "=":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " == " + resDer.valor;
                            return new resultado(terminales.numero, argumento);
                        case "<>":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " != " + resDer.valor;
                            return new resultado(terminales.numero, argumento);
                        case "<":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " < " + resDer.valor;
                            return new resultado(terminales.numero, argumento);
                        case ">":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " > " + resDer.valor;
                            return new resultado(terminales.numero, argumento);
                        case "<=":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " <= " + resDer.valor;
                            return new resultado(terminales.numero, argumento);
                        case ">=":
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " >= " + resDer.valor;
                            return new resultado(terminales.numero, argumento);

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
