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
            string temporal;
            string temp = "";
            string array, pointer;

            string tempVerdadero = "";
            string tempFalso = "";
            string tempSiguiente = "";
            string tempSalida = "";
            string tempEtiqueta = "";

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
                        return new resultado(terminales.rstring, retorno);
                    }

                }
                else if (salida.Term.ToString() == "true" || salida.Term.ToString() == "false")
                {

                    if (salida.Term.ToString() == "true")
                    {
                        return new resultado(terminales.rtrue, "1");
                    }
                    else
                    {
                        return new resultado(terminales.rfalse, "0");
                    }

                }
                else
                {
                    simbolo a = tablaActual.buscar(salida.Token.Text);
                    if (a != null)
                    {
                        if (a.tipo == "integer")
                        {

                            return new resultado(terminales.rinteger, a.direccion);
                        }
                        else if (a.tipo == "real")
                        {
                            return new resultado(terminales.rreal, a.direccion);
                        }

                        else if (a.tipo == "string")
                        {
                            return new resultado(terminales.rstring, a.direccion);
                        }
                        else if (a.tipo == "char")
                        {
                            return new resultado(terminales.rchar, a.direccion);
                        }
                        else if (a.tipo == "boolean")
                        {
                            return new resultado(terminales.rboolean, a.direccion);
                        }
                        else
                        {
                            return new resultado("etc", a.direccion);
                        }
                    }



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
                            #region SUMA
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            if (resIzq.tipo == terminales.rstring || resIzq.tipo == terminales.rchar || resDer.tipo == terminales.rstring || resDer.tipo == terminales.rchar)
                            {
                                /*Concatenar cadena*/
                                return new resultado(terminales.rstring, "xd");
                            }

                            else if (resIzq.tipo == terminales.rinteger || resIzq.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//a+a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();
                                    temporal = cosasGlobalesewe.nuevoTemp();
                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");
                                    cosasGlobalesewe.concatenarAccion(temporal + " = " + array + "[(int)" + resDer.valor + "];");
                                    argumento = temp + " + " + temporal;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//a+num
                                {

                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");

                                    argumento = temp + " + " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }

                            else if (resIzq.tipo == terminales.numero || resIzq.tipo == terminales.rtrue || resIzq.tipo == terminales.rfalse)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//num+a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resDer.valor + "];");

                                    argumento = resIzq.valor + " + " + temp;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);

                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//num + num
                                {
                                    argumento = resIzq.valor + " + " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }
                            else
                            {
                                //ERROR
                            }

                            return new resultado();
                        #endregion
                        case "-":
                            #region Resta
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            if (resIzq.tipo == terminales.rinteger || resIzq.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//a-a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();
                                    temporal = cosasGlobalesewe.nuevoTemp();
                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");
                                    cosasGlobalesewe.concatenarAccion(temporal + " = " + array + "[(int)" + resDer.valor + "];");
                                    argumento = temp + " - " + temporal;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//a-num
                                {

                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");

                                    argumento = temp + " - " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }

                            else if (resIzq.tipo == terminales.numero || resIzq.tipo == terminales.rtrue || resIzq.tipo == terminales.rfalse)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//num-a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resDer.valor + "];");

                                    argumento = resIzq.valor + " - " + temp;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);

                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//num - num
                                {
                                    argumento = resIzq.valor + " - " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }
                            else
                            {
                                //ERROR
                            }

                            return new resultado();
                        #endregion
                        case "*":
                            #region Multiplicacion
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            if (resIzq.tipo == terminales.rinteger || resIzq.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//a*a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();
                                    temporal = cosasGlobalesewe.nuevoTemp();
                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");
                                    cosasGlobalesewe.concatenarAccion(temporal + " = " + array + "[(int)" + resDer.valor + "];");
                                    argumento = temp + " * " + temporal;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//a*num
                                {

                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");

                                    argumento = temp + " * " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }

                            else if (resIzq.tipo == terminales.numero || resIzq.tipo == terminales.rtrue || resIzq.tipo == terminales.rfalse)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//num*a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resDer.valor + "];");

                                    argumento = resIzq.valor + " * " + temp;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);

                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//num * num
                                {
                                    argumento = resIzq.valor + " * " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }
                            else
                            {
                                //ERROR
                            }

                            return new resultado();
                        #endregion

                        case "/":
                            #region Division1
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            if (resIzq.tipo == terminales.rinteger || resIzq.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//a/a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();
                                    temporal = cosasGlobalesewe.nuevoTemp();
                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");
                                    cosasGlobalesewe.concatenarAccion(temporal + " = " + array + "[(int)" + resDer.valor + "];");
                                    argumento = temp + " / " + temporal;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//a/num
                                {

                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");

                                    argumento = temp + " / " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }

                            else if (resIzq.tipo == terminales.numero || resIzq.tipo == terminales.rtrue || resIzq.tipo == terminales.rfalse)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//num/a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resDer.valor + "];");

                                    argumento = resIzq.valor + " / " + temp;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);

                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//num / num
                                {
                                    argumento = resIzq.valor + " / " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }
                            else
                            {
                                //ERROR
                            }

                            return new resultado();
                        #endregion
                        case "mod":
                            #region DivisionMod
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            if (resIzq.tipo == terminales.rinteger || resIzq.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//a%a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();
                                    temporal = cosasGlobalesewe.nuevoTemp();
                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");
                                    cosasGlobalesewe.concatenarAccion(temporal + " = " + array + "[(int)" + resDer.valor + "];");
                                    argumento = temp + " % " + temporal;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//a%num
                                {

                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");

                                    argumento = temp + " % " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }

                            else if (resIzq.tipo == terminales.numero || resIzq.tipo == terminales.rtrue || resIzq.tipo == terminales.rfalse)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//num%a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resDer.valor + "];");

                                    argumento = resIzq.valor + " % " + temp;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);

                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//num % num
                                {
                                    argumento = resIzq.valor + " % " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }
                            else
                            {
                                //ERROR
                            }

                            return new resultado();
                        #endregion


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
                            }

                            return new resultado(terminales.or, temp, argumento);
                        case "=":
                            #region igual
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            if (resIzq.tipo == terminales.rinteger || resIzq.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//a==a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();
                                    temporal = cosasGlobalesewe.nuevoTemp();
                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");
                                    cosasGlobalesewe.concatenarAccion(temporal + " = " + array + "[(int)" + resDer.valor + "];");
                                    argumento = temp + " == " + temporal;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//a==num
                                {

                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");

                                    argumento = temp + " == " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }

                            else if (resIzq.tipo == terminales.numero || resIzq.tipo == terminales.rtrue || resIzq.tipo == terminales.rfalse)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//num==a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resDer.valor + "];");

                                    argumento = resIzq.valor + " == " + temp;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);

                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//num == num
                                {
                                    argumento = resIzq.valor + " == " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }
                            else
                            {
                                //ERROR
                            }

                            return new resultado();
                        #endregion
                        case "<>":
                            #region distinto
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            if (resIzq.tipo == terminales.rinteger || resIzq.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//a<>a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();
                                    temporal = cosasGlobalesewe.nuevoTemp();
                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");
                                    cosasGlobalesewe.concatenarAccion(temporal + " = " + array + "[(int)" + resDer.valor + "];");
                                    argumento = temp + " <> " + temporal;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//a<>num
                                {

                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");

                                    argumento = temp + " <> " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }

                            else if (resIzq.tipo == terminales.numero || resIzq.tipo == terminales.rtrue || resIzq.tipo == terminales.rfalse)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//num<>a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resDer.valor + "];");

                                    argumento = resIzq.valor + " <> " + temp;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);

                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//num <> num
                                {
                                    argumento = resIzq.valor + " <> " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }
                            else
                            {
                                //ERROR
                            }

                            return new resultado();
                        #endregion
                        case "<":
                            #region menor
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            if (resIzq.tipo == terminales.rinteger || resIzq.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//a<a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();
                                    temporal = cosasGlobalesewe.nuevoTemp();
                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");
                                    cosasGlobalesewe.concatenarAccion(temporal + " = " + array + "[(int)" + resDer.valor + "];");
                                    argumento = temp + " < " + temporal;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//a<num
                                {

                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");

                                    argumento = temp + " < " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }

                            else if (resIzq.tipo == terminales.numero || resIzq.tipo == terminales.rtrue || resIzq.tipo == terminales.rfalse)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//num<a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resDer.valor + "];");

                                    argumento = resIzq.valor + " < " + temp;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);

                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//num < num
                                {
                                    argumento = resIzq.valor + " < " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }
                            else
                            {
                                //ERROR
                            }

                            return new resultado();
                        #endregion
                        case ">":
                            #region mayor
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            if (resIzq.tipo == terminales.rinteger || resIzq.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//a>a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();
                                    temporal = cosasGlobalesewe.nuevoTemp();
                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");
                                    cosasGlobalesewe.concatenarAccion(temporal + " = " + array + "[(int)" + resDer.valor + "];");
                                    argumento = temp + " > " + temporal;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//a>num
                                {

                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");

                                    argumento = temp + " > " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }

                            else if (resIzq.tipo == terminales.numero || resIzq.tipo == terminales.rtrue || resIzq.tipo == terminales.rfalse)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//num>a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resDer.valor + "];");

                                    argumento = resIzq.valor + " > " + temp;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);

                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//num > num
                                {
                                    argumento = resIzq.valor + " > " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }
                            else
                            {
                                //ERROR
                            }

                            return new resultado();
                        #endregion
                        case "<=":
                            #region menorIgual
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            if (resIzq.tipo == terminales.rinteger || resIzq.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//a<=a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();
                                    temporal = cosasGlobalesewe.nuevoTemp();
                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");
                                    cosasGlobalesewe.concatenarAccion(temporal + " = " + array + "[(int)" + resDer.valor + "];");
                                    argumento = temp + " <= " + temporal;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//a<=num
                                {

                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");

                                    argumento = temp + " <= " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }

                            else if (resIzq.tipo == terminales.numero || resIzq.tipo == terminales.rtrue || resIzq.tipo == terminales.rfalse)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//num<=a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resDer.valor + "];");

                                    argumento = resIzq.valor + " <= " + temp;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);

                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//num <= num
                                {
                                    argumento = resIzq.valor + " <= " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }
                            else
                            {
                                //ERROR
                            }

                            return new resultado();
                        #endregion
                        case ">=":
                            #region mayorIgual
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            if (resIzq.tipo == terminales.rinteger || resIzq.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//a>=a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();
                                    temporal = cosasGlobalesewe.nuevoTemp();
                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");
                                    cosasGlobalesewe.concatenarAccion(temporal + " = " + array + "[(int)" + resDer.valor + "];");
                                    argumento = temp + " >= " + temporal;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//a>=num
                                {

                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resIzq.valor + "];");

                                    argumento = temp + " >= " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }

                            else if (resIzq.tipo == terminales.numero || resIzq.tipo == terminales.rtrue || resIzq.tipo == terminales.rfalse)
                            {
                                if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)//num>=a
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();

                                    cosasGlobalesewe.concatenarAccion(temp + " = " + array + "[(int)" + resDer.valor + "];");

                                    argumento = resIzq.valor + " >= " + temp;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);

                                    return new resultado(terminales.numero, temp);
                                }
                                else if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse)//num >= num
                                {
                                    argumento = resIzq.valor + " >= " + resDer.valor;
                                    temp = cosasGlobalesewe.nuevoTemp(argumento);
                                    return new resultado(terminales.numero, temp);
                                }
                                else
                                {
                                    //ERROR
                                }
                            }
                            else
                            {
                                //ERROR
                            }

                            return new resultado();
                            #endregion

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
