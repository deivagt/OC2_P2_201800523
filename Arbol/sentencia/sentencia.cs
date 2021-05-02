using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.Arbol.Expresion;
using OC2_P2_201800523.AST;
using System.Collections.Generic;
using OC2_P2_201800523.Arbol.sentencia.ciclo;
using OC2_P2_201800523.Arbol.sentencia.condicion;
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
            //string array, pointer;




            switch (palabraClave.Token.Text)
            {
                case "if":
                    if (node.ChildNodes.Count == 7) // if normalito
                    {

                        /*Condicion del if*/
                        ParseTreeNode expresion = node.ChildNodes.ElementAt(1);
                        expresion expr = new expresion(noterminales.EXPRESION, expresion);
                        res = expr.traducir(ref tablaActual, ambito, "", "", xd);


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
                        argumento = "goto " + tempSalida + ";\n";
                        argumento += tempFalso + ":" + tempSalida + ":\n";


                        cosasGlobalesewe.concatenarAccion(argumento);
                        argumento = "/*Finaliza DECLARACION DE IF SIMPLE*/";
                        cosasGlobalesewe.concatenarAccion(argumento);

                    }
                    else //if else
                    {

                        /*Condicion del if*/
                        ParseTreeNode expresion = node.ChildNodes.ElementAt(1);
                        expresion expr = new expresion(noterminales.EXPRESION, expresion);
                        res = expr.traducir(ref tablaActual, ambito, "", "", xd);

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
                        siguienteelseif.traducir(ref tablaActual, ambito, tempSalida, falso, xd);

                        argumento = tempSalida + ":";
                        cosasGlobalesewe.concatenarAccion(argumento);
                        argumento = "/*Finaliza DECLARACION DE IF COMPUESTO EWE*/";
                        cosasGlobalesewe.concatenarAccion(argumento);

                    }

                    return new resultado();

                case "write":
                    ParseTreeNode paramWrite = node.ChildNodes.ElementAt(2);
                    funcBasica.write write = new funcBasica.write(noterminales.PARAMETROSWRITELN, paramWrite);
                    write.traducir(ref tablaActual, ambito, "", "", xd);
                    return new resultado();


                case "writeln":
                    ParseTreeNode paramWriteln = node.ChildNodes.ElementAt(2);
                    funcBasica.write writeln = new funcBasica.write(noterminales.PARAMETROSWRITELN, paramWriteln);
                    writeln.traducir(ref tablaActual, ambito, "", "", xd);
                    cosasGlobalesewe.concatenarAccion("printf(\"\\n\"); ");
                    return new resultado();

                case "while":
                    cicloWhile ciclowhile = new cicloWhile("WHILE", node);
                    ciclowhile.traducir(ref tablaActual, ambito, verdadero, falso, xd);
                    return new resultado();

                case "repeat":
                    repeatUntil ciclorepeat = new repeatUntil("REPEAT", node);
                    ciclorepeat.traducir(ref tablaActual, ambito, verdadero, falso, xd);
                    return new resultado();

                case "case":
                    casos casos = new casos("CASOS", node);
                    casos.traducir(ref tablaActual, ambito, verdadero, falso, xd);
                    return new resultado();

                case "for":
                    For FOR = new For("FOR", node);
                    FOR.traducir(ref tablaActual, ambito, verdadero, falso, xd);
                    return new resultado();

                case "continue":
                    cosasGlobalesewe.concatenarAccion("goto " + verdadero + ";");
                    return new resultado();

                case "break":
                    cosasGlobalesewe.concatenarAccion("goto " + falso + ";");
                    return new resultado();

                
                case "Exit":
                    string[] partido = xd.Split(';');
                    if (node.ChildNodes.Count == 4)// No return
                    {

                    }
                    else
                    {
                        
                        expresion exp = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));
                        res = exp.traducir(ref tablaActual, ambito, "", "", xd);
                        argumento = "";
                        if(res.argumento != null)
                        {
                            argumento = res.argumento;
                        }

                        
                            argumento += "stack" + "[(int)" + partido[1] + "] = " + res.valor + ";";
                            cosasGlobalesewe.concatenarAccion(argumento);
                        
                        
                    }
                    cosasGlobalesewe.concatenarAccion("goto " + partido[0] + ";");

                    return new resultado();


                default:
                    if (palabraClave.Term.ToString() == "id")
                    {
                        if (node.ChildNodes.ElementAt(1).Token.Text == ":=")//Asignacion
                        {
                            simbolo variable = tablaActual.buscar(palabraClave.Token.Text, ambito);
                            if (variable != null)
                            {
                                expresion exp = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));
                                res = exp.traducir(ref tablaActual, ambito, "", "", xd);

                                if (res.argumento != null)
                                {
                                    cosasGlobalesewe.concatenarAccion(res.argumento);
                                }                                
                                if (variable.tipo == terminales.rinteger || variable.tipo == terminales.rreal || variable.tipo == terminales.rboolean)
                                {
                                    //TIPOS CONCUERDAN
                                    argumento = "stack" + "[(int)" + variable.direccion + "] = " + res.valor + ";";
                                    cosasGlobalesewe.concatenarAccion(argumento);
                                    //manejadorArbol.imprimirTabla();
                                }
                                else
                                {
                                    if (variable.tipo == terminales.rstring || res.tipo == terminales.rchar)
                                    {
                                        variable.direccion = res.valor;
                                    }
                                }
                            }
                            else
                            {
                                //ERROR
                            }
                        }
                        else 
                        {
                            LinkedList<expresion> listaParam = new LinkedList<expresion>();
                            string id = node.ChildNodes.ElementAt(0).Token.Text;
                            funcProce.parametros lp = new funcProce.parametros(noterminales.PARAMETROS, node.ChildNodes.ElementAt(2));
                            if(node.ChildNodes.ElementAt(2).ChildNodes.Count != 0)
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
                                    string temp;
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
                                                if (respuesta.simbolo != null)// para variables
                                                {

                                                    if (comparador.porValor == true)//En el parametro se asigna el valor
                                                    {
                                                        tempInterno = cosasGlobalesewe.nuevoTemp();
                                                        argumento += temp + " = " + tempResetear + " + " + (i + 1) + ";\n";
                                                        argumento += tempInterno + " = " + "stack" + "[(int)" + respuesta.valor + "];\n";
                                                        argumento += "stack[(int)" + temp + "] = " + tempInterno + ";\n";

                                                    }
                                                    else
                                                    {

                                                        argumento += temp + " = " + tempResetear + " + " + (i + 1) + ";\n";
                                                        argumento += "stack[(int)" + temp + "] = " + respuesta.valor + ";\n";
                                                    }
                                                }
                                                else
                                                {
                                                    argumento += temp + " = " + tempResetear + " + " + (i + 1) + ";\n";
                                                    argumento += "stack[(int)" + temp + "] = " + respuesta.valor + ";\n";
                                                }
                                            }
                                            else
                                            {

                                            }
                                        }
                                        else if (respuesta.tipo == terminales.rstring || respuesta.tipo == terminales.rchar)
                                        {
                                            if (comparador.tipo == terminales.rstring || comparador.tipo == terminales.rchar)
                                            {
                                                string otroTempPuta = cosasGlobalesewe.nuevoTemp();
                                                if(respuesta.simbolo != null)
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
                            }
                        }
                    }
                    return new resultado();
            }
        }
        void determinarTipo(resultado res, simbolo variable)
        {
            if (res.tipo == "numero")
            {
                if (int.TryParse(res.valor, out int i) == true)
                {
                    res.tipo = "integer";
                }
                else
                {
                    res.tipo = "real";
                }
            }
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
