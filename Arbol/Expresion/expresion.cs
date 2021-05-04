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
            //string array, pointer;


            string tempIzquierdo;
            string tempDerecho;
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

                    string inicio = cosasGlobalesewe.nuevoTemp();
                    argumento = inicio + " = hp;\n";
                    foreach (char caracter in retorno)
                    {
                        argumento += "heap[(int)hp] = " + (int)caracter + ";\n";
                        argumento += "hp = hp + 1;\n";
                    }
                    argumento += "heap[(int)hp] = " + "-1" + ";\n";
                    argumento += "hp = hp + 1;\n";
                    cosasGlobalesewe.concatenarAccion(argumento);
                    if (retorno.Length == 1)
                    {
                        return new resultado(terminales.rchar, inicio);
                    }
                    else
                    {
                        return new resultado(terminales.rstring, inicio);
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
                    simbolo a = tablaActual.buscar(salida.Token.Text, ambito);
                    if (a != null)
                    {
                        temp = cosasGlobalesewe.nuevoTemp("stack" + "[(int)" + a.direccion + "]");
                        if (a.tipo == "integer")
                        {

                            return new resultado(terminales.rinteger, temp, a);
                        }
                        else if (a.tipo == "real")
                        {
                            return new resultado(terminales.rreal, temp, a);
                        }

                        else if (a.tipo == "string")
                        {
                            return new resultado(terminales.rstring, temp, a);
                        }
                        else if (a.tipo == "char")
                        {
                            return new resultado(terminales.rchar, temp, a);
                        }
                        else if (a.tipo == "boolean")
                        {
                            return new resultado(terminales.rboolean, temp, a);
                        }
                        else
                        {
                            return new resultado(a.tipo, temp, a);
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

                    tempDerecho = resDer.valor;
                    if (resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resDer.tipo == terminales.rboolean)
                    {
                        tempDerecho = cosasGlobalesewe.nuevoTemp("stack" + "[(int)" + resDer.valor + "]");
                    }
                    argumento = tempEtiqueta + " = - " + tempDerecho + ";\n";
                    cosasGlobalesewe.concatenarAccion(argumento);
                    return new resultado(terminales.numero, tempEtiqueta, argumento);

                }
                else//NOT
                {

                    tempVerdadero = cosasGlobalesewe.crearEtiqueta();
                    tempFalso = cosasGlobalesewe.crearEtiqueta();
                    tempSalida = cosasGlobalesewe.crearEtiqueta();
                    derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(1));

                    resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                    tempDerecho = resDer.valor;

                    if (resDer.tipo == terminales.rboolean)
                    {
                        tempDerecho = cosasGlobalesewe.nuevoTemp("stack" + "[(int)" + resDer.valor + "]");
                    }


                    argumento = "if(" + tempDerecho + ") goto " + tempVerdadero + ";\n" + "goto " + tempFalso + ";\n";

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

                            if ((resIzq.tipo == terminales.rstring || resIzq.tipo == terminales.rchar))
                            {
                                if (resDer.tipo == terminales.rstring || resDer.tipo == terminales.rchar)
                                {
                                    argumento = "";
                                    string inicioNuevaCadena = cosasGlobalesewe.nuevoTemp();
                                    string inicioIzq, inicioDer;

                                    inicioIzq = resIzq.valor;

                                    inicioDer = resDer.valor;



                                    argumento += inicioNuevaCadena + " = hp;\n";
                                    argumento += "t0 = " + inicioIzq + ";\n";
                                    argumento += "t2 = " + inicioDer + ";\n";

                                    argumento += "concatenacion();\n";
                                    return new resultado(terminales.rstring, inicioNuevaCadena, argumento);

                                }
                                else
                                {
                                    Program.form.richTextBox5.Text += "Error Sintactico: No se pueden concatenar tipos distintos\n";
                                    return new resultado(terminales.rstring, "");
                                }


                            }

                            else if (resIzq.tipo == terminales.rinteger || resIzq.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean || resIzq.tipo == terminales.numero || resIzq.tipo == terminales.rtrue || resIzq.tipo == terminales.rfalse)
                            {
                                if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse || resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)
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
                                //Error
                            }


                            return new resultado();
                        #endregion
                        case "-":
                            #region Resta
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            if (resIzq.tipo == terminales.rinteger || resIzq.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean || resIzq.tipo == terminales.numero || resIzq.tipo == terminales.rtrue || resIzq.tipo == terminales.rfalse)
                            {
                                if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse || resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)
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
                                //Error
                            }


                            return new resultado();
                        #endregion
                        case "*":
                            #region Multiplicacion
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            if (resIzq.tipo == terminales.rinteger || resIzq.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean || resIzq.tipo == terminales.numero || resIzq.tipo == terminales.rtrue || resIzq.tipo == terminales.rfalse)
                            {
                                if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse || resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)
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
                                //Error
                            }


                            return new resultado();
                        #endregion

                        case "/":
                            #region Division1
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            if (resIzq.tipo == terminales.rinteger || resIzq.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean || resIzq.tipo == terminales.numero || resIzq.tipo == terminales.rtrue || resIzq.tipo == terminales.rfalse)
                            {
                                if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse || resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)
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
                                //Error
                            }


                            return new resultado();
                        #endregion
                        case "mod":
                            #region DivisionMod
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            if (resIzq.tipo == terminales.rinteger || resIzq.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean || resIzq.tipo == terminales.numero || resIzq.tipo == terminales.rtrue || resIzq.tipo == terminales.rfalse)
                            {
                                if (resDer.tipo == terminales.numero || resDer.tipo == terminales.rtrue || resDer.tipo == terminales.rfalse || resDer.tipo == terminales.rinteger || resDer.tipo == terminales.rreal || resIzq.tipo == terminales.rboolean)
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
                                //Error
                            }

                            return new resultado();
                        #endregion


                        case "AND":
                            #region AND
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

                            tempIzquierdo = resIzq.valor;
                            tempDerecho = resDer.valor;

                            if (resIzq.tipo == terminales.rboolean)
                            {

                                tempIzquierdo = cosasGlobalesewe.nuevoTemp("stack" + "[(int)" + resIzq.valor + "]");
                            }

                            if (resDer.tipo == terminales.rboolean)
                            {
                                tempDerecho = cosasGlobalesewe.nuevoTemp("stack" + "[(int)" + resDer.valor + "]");
                            }


                            if (verdadero == "" && falso == "") //ULTIMO AND OR NOT
                            {
                                argumento = "";

                                tempSalida = cosasGlobalesewe.crearEtiqueta();

                                if (resIzq.tipo != terminales.and && resIzq.tipo != terminales.or && resIzq.tipo != terminales.not)
                                {
                                    argumento = "if(" + tempIzquierdo + ") goto " + tempSiguiente + ";\n" + "goto " + tempFalso + ";\n";

                                }
                                argumento += resIzq.argumento;
                                argumento += tempSiguiente + ":\n" + "";
                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + tempDerecho + ") goto " + tempVerdadero + ";\n" + "goto " + tempFalso + ";\n";

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
                                    argumento = "if(" + tempIzquierdo + ") goto " + tempSiguiente + ";\n" + "goto " + falso + ";\n";
                                }

                                argumento += resIzq.argumento;
                                argumento += tempSiguiente + ":\n" + "";
                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + tempDerecho + ") goto " + verdadero + ";\n" + "goto " + falso + ";\n";

                                }
                                argumento += resDer.argumento;
                                //cosasGlobalesewe.concatenarAccion(argumento);
                            }

                            return new resultado(terminales.and, temp, argumento);
                        #endregion
                        case "OR":
                            #region OR
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

                            tempIzquierdo = resIzq.valor;
                            tempDerecho = resDer.valor;
                            if (resIzq.tipo == terminales.rboolean)
                            {

                                tempIzquierdo = cosasGlobalesewe.nuevoTemp("stack" + "[(int)" + resIzq.valor + "]");
                            }

                            if (resDer.tipo == terminales.rboolean)
                            {
                                tempDerecho = cosasGlobalesewe.nuevoTemp("stack" + "[(int)" + resDer.valor + "]");
                            }

                            if (verdadero == "" && falso == "") //ULTIMO AND OR NOT
                            {
                                argumento = "";

                                tempSalida = cosasGlobalesewe.crearEtiqueta();

                                if (resIzq.tipo != terminales.and && resIzq.tipo != terminales.or && resIzq.tipo != terminales.not)
                                {
                                    argumento = "if(" + tempIzquierdo + ") goto " + tempVerdadero + ";\n" + "goto " + tempSiguiente + ";\n";

                                }
                                argumento += resIzq.argumento;
                                argumento += tempSiguiente + ":\n" + "";
                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + tempDerecho + ") goto " + tempVerdadero + ";\n" + "goto " + tempFalso + ";\n";

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
                                    argumento = "if(" + tempIzquierdo + ") goto " + verdadero + ";\n" + "goto " + tempSiguiente + ";\n";
                                }

                                argumento += resIzq.argumento + ";\n";
                                argumento += tempSiguiente + ":\n" + "";

                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + tempDerecho + ") goto " + verdadero + ";\n" + "goto " + falso + ";\n";
                                }
                                argumento += resDer.argumento;
                                //cosasGlobalesewe.concatenarAccion(argumento);
                            }

                            return new resultado(terminales.or, temp, argumento);
                        #endregion
                        case "and":
                            #region and
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

                            tempIzquierdo = resIzq.valor;
                            tempDerecho = resDer.valor;
                            if (resIzq.tipo == terminales.rboolean)
                            {

                                tempIzquierdo = cosasGlobalesewe.nuevoTemp("stack" + "[(int)" + resIzq.valor + "]");
                            }

                            if (resDer.tipo == terminales.rboolean)
                            {
                                tempDerecho = cosasGlobalesewe.nuevoTemp("stack" + "[(int)" + resDer.valor + "]");
                            }

                            if (verdadero == "" && falso == "") //ULTIMO AND OR NOT
                            {
                                argumento = "";

                                tempSalida = cosasGlobalesewe.crearEtiqueta();

                                if (resIzq.tipo != terminales.and && resIzq.tipo != terminales.or && resIzq.tipo != terminales.not)
                                {
                                    argumento = "if(" + tempIzquierdo + ") goto " + tempSiguiente + ";\n" + "goto " + tempFalso + ";\n";

                                }
                                argumento += resIzq.argumento;
                                argumento += tempSiguiente + ":\n" + "";
                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + tempDerecho + ") goto " + tempVerdadero + ";\n" + "goto " + tempFalso + ";\n";

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
                                    argumento = "if(" + tempIzquierdo + ") goto " + tempSiguiente + ";\n" + "goto " + falso + ";\n";
                                }

                                argumento += resIzq.argumento;
                                argumento += tempSiguiente + ":\n" + "";
                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + tempDerecho + ") goto " + verdadero + ";\n" + "goto " + falso + ";\n";

                                }
                                argumento += resDer.argumento;
                                //cosasGlobalesewe.concatenarAccion(argumento);
                            }

                            return new resultado(terminales.and, temp, argumento);
                        #endregion
                        case "or":
                            #region or
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

                            tempIzquierdo = resIzq.valor;
                            tempDerecho = resDer.valor;
                            if (resIzq.tipo == terminales.rboolean)
                            {

                                tempIzquierdo = cosasGlobalesewe.nuevoTemp("stack" + "[(int)" + resIzq.valor + "]");
                            }

                            if (resDer.tipo == terminales.rboolean)
                            {
                                tempDerecho = cosasGlobalesewe.nuevoTemp("stack" + "[(int)" + resDer.valor + "]");
                            }
                            if (verdadero == "" && falso == "") //ULTIMO AND OR NOT
                            {
                                argumento = "";

                                tempSalida = cosasGlobalesewe.crearEtiqueta();

                                if (resIzq.tipo != terminales.and && resIzq.tipo != terminales.or && resIzq.tipo != terminales.not)
                                {
                                    argumento = "if(" + tempIzquierdo + ") goto " + tempVerdadero + ";\n" + "goto " + tempSiguiente + ";\n";

                                }
                                argumento += resIzq.argumento;
                                argumento += tempSiguiente + ":\n" + "";
                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + tempIzquierdo + ") goto " + tempVerdadero + ";\n" + "goto " + tempFalso + ";\n";

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
                                    argumento = "if(" + tempIzquierdo + ") goto " + verdadero + ";\n" + "goto " + tempSiguiente + ";\n";
                                }

                                argumento += resIzq.argumento + ";\n";
                                argumento += tempSiguiente + ":\n" + "";

                                if (resDer.tipo != terminales.and && resDer.tipo != terminales.or && resDer.tipo != terminales.not)
                                {
                                    argumento += "if(" + tempDerecho + ") goto " + verdadero + ";\n" + "goto " + falso + ";\n";
                                }
                                argumento += resDer.argumento;
                            }

                            return new resultado(terminales.or, temp, argumento);
                        #endregion
                        case "=":
                            #region igual
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");
                            argumento = resIzq.valor + " == " + resDer.valor;
                            temp = cosasGlobalesewe.nuevoTemp(argumento);
                            return new resultado(terminales.numero, temp);
                        #endregion
                        case "<>":
                            #region distinto
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " != " + resDer.valor;
                            temp = cosasGlobalesewe.nuevoTemp(argumento);
                            return new resultado(terminales.numero, temp);

                        #endregion
                        case "<":
                            #region menor
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " < " + resDer.valor;
                            temp = cosasGlobalesewe.nuevoTemp(argumento);
                            return new resultado(terminales.numero, temp);
                        #endregion
                        case ">":
                            #region mayor
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " > " + resDer.valor;
                            temp = cosasGlobalesewe.nuevoTemp(argumento);
                            return new resultado(terminales.numero, temp);
                        #endregion
                        case "<=":
                            #region menorIgual
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " <= " + resDer.valor;
                            temp = cosasGlobalesewe.nuevoTemp(argumento);
                            return new resultado(terminales.numero, temp);
                        #endregion
                        case ">=":
                            #region mayorIgual
                            izquierda = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                            derecha = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));

                            resIzq = izquierda.traducir(ref tablaActual, ambito, "", "", "");
                            resDer = derecha.traducir(ref tablaActual, ambito, "", "", "");

                            argumento = resIzq.valor + " >= " + resDer.valor;
                            temp = cosasGlobalesewe.nuevoTemp(argumento);
                            return new resultado(terminales.numero, temp);
                            #endregion

                    }
                }
            }
            else
            {
                if (node.ChildNodes.ElementAt(1).Token.Text == "(")// Function
                {
                    LinkedList<expresion> listaParam = new LinkedList<expresion>();
                    string id = node.ChildNodes.ElementAt(0).Token.Text;
                    funcProce.parametros lp = new funcProce.parametros(noterminales.PARAMETROS, node.ChildNodes.ElementAt(2));
                    if (node.ChildNodes.ElementAt(2).ChildNodes.Count != 0)
                    {
                        lp.nuevaTraduccion(listaParam);
                    }

                    argumento = "";
                    simbolo fn = tablaActual.buscarFuncion(id);
                    if (fn != null)
                    {
                        string tempResetear = cosasGlobalesewe.nuevoTemp();
                        argumento += tempResetear + " = sp;\n";
                        if (fn.listaParam.Count != 0)
                        {

                            string tempInterno;
                            LinkedList<parametroCustom> listaParamFunc = fn.listaParam;
                            for (int i = 0; i <= listaParam.Count - 1; i++)
                            {
                                resultado respuesta = listaParam.ElementAt(i).traducir(ref tablaActual, ambito, "", "", xd);
                                parametroCustom comparador = listaParamFunc.ElementAt(i);

                                temp = cosasGlobalesewe.nuevoTemp();

                                if (respuesta.argumento != null)
                                {
                                    cosasGlobalesewe.concatenarAccion(respuesta.argumento);
                                }



                                if (respuesta.tipo == terminales.rinteger || respuesta.tipo == terminales.rreal || respuesta.tipo == terminales.numero || respuesta.tipo == terminales.rboolean)
                                {
                                    if (comparador.tipo == terminales.rinteger || comparador.tipo == terminales.rreal || comparador.tipo == terminales.numero || comparador.tipo == terminales.rboolean)
                                    {


                                        argumento += temp + " = " + tempResetear + " + " + (i + 1) + ";\n";
                                        argumento += "stack[(int)" + temp + "] = " + respuesta.valor + ";\n";

                                    }
                                    else
                                    {
                                        //Error
                                    }
                                }
                                else if (respuesta.tipo == terminales.rstring || respuesta.tipo == terminales.rchar)
                                {
                                    if (comparador.tipo == terminales.rstring || comparador.tipo == terminales.rchar)
                                    {
                                        string otroTempPuta = cosasGlobalesewe.nuevoTemp();
                                        if (respuesta.simbolo != null)
                                        {
                                            argumento += temp + " = " + tempResetear + " + " + (i + 1) + ";\n";
                                            argumento += "stack[(int)" + temp + "] = " + respuesta.valor + ";\n";
                                        }
                                        else
                                        {

                                            argumento += otroTempPuta + " = hp;\n";
                                            foreach (char caracter in respuesta.valor)
                                            {
                                                argumento += "heap[(int)hp] = " + (int)caracter + ";\n";
                                                argumento += "hp = hp + 1;\n";
                                            }

                                            argumento += "heap[(int)hp] = " + "-1" + ";\n";
                                            argumento += "hp = hp + 1;\n";
                                            argumento += temp + " = " + tempResetear + " + " + (i + 1) + ";\n";
                                            argumento += "stack[(int)" + temp + "] = " + otroTempPuta + ";\n";
                                        }
                                    }
                                }
                                else
                                {
                                    //Error
                                }
                            }
                        }
                        else
                        {
                            //Error
                        }

                        argumento += id + "();\n";
                        argumento += "sp = " + tempResetear + ";\n";
                        cosasGlobalesewe.concatenarAccion(argumento);
                        temp = cosasGlobalesewe.nuevoTemp("stack[(int)" + fn.direccion + "]");
                        return new resultado(fn.tipo, temp, fn);
                    }
                }
                else if (node.ChildNodes.ElementAt(1).Token.Text == "[")
                {
                    //ARRAY
                    argumento = "";
                    ParseTreeNode id = node.ChildNodes.ElementAt(0);
                    simbolo a = tablaActual.buscar(id.Token.Text, ambito);
                    string retorno = "";
                    if (a != null)
                    {

                        LinkedList<resultado> listaPos = new LinkedList<resultado>();
                        ParseTreeNode auxi = node.ChildNodes.ElementAt(2);
                        resultado posicion = null;
                        while (auxi != null)
                        {
                            if (auxi.ChildNodes.Count == 3)
                            {
                                expresion thisExpr = new expresion(noterminales.EXPRESION, auxi.ChildNodes.ElementAt(2));
                                posicion = thisExpr.traducir(ref tablaActual, ambito, "", "", "");
                                listaPos.AddFirst(posicion);
                                auxi = auxi.ChildNodes.ElementAt(0);
                            }
                            else if (auxi.ChildNodes.Count == 1)
                            {
                                expresion thisExpr = new expresion(noterminales.EXPRESION, auxi.ChildNodes.ElementAt(0));
                                posicion = thisExpr.traducir(ref tablaActual, ambito, "", "", "");
                                listaPos.AddFirst(posicion);
                                auxi = null;
                            }
                        }


                        if (listaPos.Count == 1)//C3D para un array normalito
                        {
                            temp = cosasGlobalesewe.nuevoTemp();
                            temporal = cosasGlobalesewe.nuevoTemp();
                            retorno = cosasGlobalesewe.nuevoTemp();
                            argumento += temp + " = " + "stack[(int)" + a.direccion + "];\n";
                            argumento += temporal + " = " + temp + " + " + listaPos.ElementAt(0).valor + ";\n";
                            argumento += temporal + " = " + temporal + " + " + 1 + ";\n";
                            argumento += retorno + " = heap[(int)" + temporal + "];\n";
                            cosasGlobalesewe.concatenarAccion(argumento);
                        }
                        else//Matriz y demás
                        {
                            int contador = 0;
                            foreach (var pos in listaPos)
                            {
                                if (contador == 0)
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();
                                    temporal = cosasGlobalesewe.nuevoTemp();
                                    retorno = cosasGlobalesewe.nuevoTemp();
                                    argumento += temp + " = " + "stack[(int)" + a.direccion + "];\n";
                                    argumento += temporal + " = " + temp + " + " + pos.valor + ";\n";
                                    argumento += temporal + " = " + temporal + " + " + 1 + ";\n";
                                    argumento += retorno + " = heap[(int)" + temporal + "];\n";
                                }
                                else
                                {
                                    temporal = cosasGlobalesewe.nuevoTemp();
                                    argumento += temporal + " = " + retorno + " + " + pos.valor + ";\n";
                                    argumento += temporal + " = " + temporal + " + " + 1 + ";\n";
                                    retorno = cosasGlobalesewe.nuevoTemp();
                                    argumento += retorno + " = heap[(int)" + temporal + "];\n";
                                }
                                contador++;
                            }
                            cosasGlobalesewe.concatenarAccion(argumento);
                        }

                        if (a.tipo == "integer")
                        {
                            return new resultado(terminales.rinteger, retorno, a, true);
                        }
                        else if (a.tipo == "real")
                        {
                            return new resultado(terminales.rreal, retorno, a, true);
                        }
                        else if (a.tipo == "string")
                        {
                            return new resultado(terminales.rstring, retorno, a, true);
                        }
                        else if (a.tipo == "char")
                        {
                            return new resultado(terminales.rchar, retorno, a, true);
                        }
                        else if (a.tipo == "boolean")
                        {
                            return new resultado(terminales.rboolean, retorno, a, true);
                        }
                        else
                        {

                            simbolo tipoCustom = tablaActual.buscarTipo(a.tipo);
                            return new resultado(tipoCustom.tipo, retorno, a, true);
                        }
                    }

                }

                return new resultado();
            }

            return new resultado();
        }
    }
}
