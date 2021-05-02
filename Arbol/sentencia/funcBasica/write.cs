using OC2_P2_201800523.tablaSimbolos;
using System;
using System.Collections.Generic;
using System.Text;
using OC2_P2_201800523.Arbol.Expresion;
using OC2_P2_201800523.AST;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;


namespace OC2_P2_201800523.Arbol.sentencia.funcBasica
{
    class write : nodo
    {
        public write(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public write(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            string argumento = "";
            string temp;
            //string array, pointer;

            
            if (node.ChildNodes.Count == 1)//Write de un termino
            {


                expresion expr = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(0));
                resultado res = expr.traducir(ref tablaActual, ambito, "", "", xd);

                if (res.simbolo != null)
                {
                   
                    if (res.tipo == terminales.rinteger || res.tipo == terminales.rreal || res.tipo == terminales.rboolean)
                    {

                        

                        if (res.tipo == terminales.rinteger)
                        {
                            argumento += "printf(\"%d\", (int)" + res.valor + ");";
                        }
                        else if (res.tipo == terminales.rreal)
                        {

                            argumento += "printf(\"%f\", (double)" + res.valor + ");";
                        }
                        else//SUMAS RESTAS Y DEMAS
                        {
                            argumento += "t3 = " + res.valor + " ;";
                            argumento += "booleanoCadena();";
                        }
                    }
                    else//Cadena o char
                    {
                        argumento = "t0 = " + res.simbolo.direccion + ";\n";
                        argumento += "imprimirLn();\n";
                    }
                    
                }
                else
                {

                    if (res.tipo == terminales.rstring || res.tipo == terminales.rchar)
                    {
                        
                        if (res.argumento != null)
                        {
                            cosasGlobalesewe.concatenarAccion(res.argumento);

                            argumento += "t0 = " + res.valor + ";\n";
                            argumento += "imprimirLn();\n";

                        }
                        else
                        {
                            temp = cosasGlobalesewe.nuevoTemp();
                            argumento += temp + " = hp;\n";
                            foreach (char caracter in res.valor)
                            {
                                argumento += "heap[(int)hp] = " + (int)caracter + ";\n";
                                argumento += "hp = hp + 1;\n";
                            }

                            argumento += "heap[(int)hp] = " + "-1" + ";\n";
                            argumento += "hp = hp + 1;\n";
                            argumento += "t0 = " + temp + ";\n";
                            argumento += "imprimirLn();\n";
                        }

                        
                    }
                    else
                    {
                        if (res.tipo.ToString() == "numero")
                        {
                            if (int.TryParse(res.valor, out int i) == true)
                            {
                                argumento += "printf(\"%d\", (int)" + res.valor + ");\n";
                            }
                            else if (double.TryParse(res.valor, out double j) == true)
                            {

                                argumento += "printf(\"%f\", (double)" + res.valor + ");\n";
                            }
                            else//SUMAS RESTAS Y DEMAS
                            {
                                argumento += "printf(\"%f\", " + res.valor + ");\n";
                            }

                        }
                        else if (res.tipo == "true" || res.tipo == "false")
                        {
                            argumento += "t3 = " + res.valor + " ;\n";
                            argumento += "booleanoCadena();\n";
                        }
                        else
                        {
                            if (res.argumento != null)
                            {
                                if (res.tipo == terminales.and || res.tipo == terminales.or || res.tipo == terminales.not)
                                    cosasGlobalesewe.concatenarAccion(res.argumento);
                                argumento += "t3 = " + res.valor + " ;\n";
                                argumento += "booleanoCadena();\n";
                            }
                        }
                    }
                }
                cosasGlobalesewe.concatenarAccion(argumento);
            }
            else
            {
                LinkedList<expresion> listaExpresiones = new LinkedList<expresion>();

                parametrosWrite par = new parametrosWrite(noterminales.PARAMETROSWRITELN, node);
                par.nuevoParametro(listaExpresiones);

                argumento = "";
                foreach (var a in listaExpresiones)
                {
                    resultado res = a.traducir(ref tablaActual, ambito, verdadero, falso, xd);
                    if (res.simbolo != null)
                    {
                        if (res.tipo == terminales.rinteger || res.tipo == terminales.rreal || res.tipo == terminales.rboolean)
                        {
                           

                            if (res.tipo == terminales.rinteger)
                            {
                                argumento += "printf(\"%d\", (int)" + res.valor + ");";
                            }
                            else if (res.tipo == terminales.rreal)
                            {

                                argumento += "printf(\"%f\", (double)" + res.valor + ");";
                            }
                            else//SUMAS RESTAS Y DEMAS
                            {
                                argumento += "t3 = " + res.valor + " ;";
                                argumento += "booleanoCadena();";
                            }
                        }
                        else // Cadena o char
                        {
                            argumento += "t0 = " + res.simbolo.direccion + ";\n";
                            argumento += "imprimirLn();\n";
                        }
                    }
                    else
                    {

                        if (res.tipo == terminales.rstring || res.tipo == terminales.rchar)
                        {
                            if (res.argumento != null)
                            {
                                cosasGlobalesewe.concatenarAccion(res.argumento);

                                argumento += "t0 = " + res.valor + ";\n";
                                argumento += "imprimirLn();\n";

                            }
                            else
                            {
                                temp = cosasGlobalesewe.nuevoTemp();
                                argumento += temp + " = hp;\n";
                                foreach (char caracter in res.valor)
                                {
                                    argumento += "heap[(int)hp] = " + (int)caracter + ";\n";
                                    argumento += "hp = hp + 1;\n";
                                }

                                argumento += "heap[(int)hp] = " + "-1" + ";\n";
                                argumento += "hp = hp + 1;\n";
                                argumento += "t0 = " + temp + ";\n";
                                argumento += "imprimirLn();\n";
                            }
                        }
                        else
                        {
                            if (res.tipo.ToString() == "numero")
                            {
                                if (int.TryParse(res.valor, out int i) == true)
                                {
                                    argumento += "printf(\"%d\", (int)" + res.valor + ");\n";
                                }
                                else if (double.TryParse(res.valor, out double j) == true)
                                {

                                    argumento += "printf(\"%f\", (double)" + res.valor + ");\n";
                                }
                                else//SUMAS RESTAS Y DEMAS
                                {
                                    argumento += "printf(\"%f\", " + res.valor + ");\n";
                                }

                            }
                            else if (res.tipo == "true" || res.tipo == "false")
                            {
                                argumento += "t3 = " + res.valor + " ;\n";
                                argumento += "booleanoCadena();\n";
                            }
                            else
                            {
                                if (res.argumento != null)
                                {
                                    if (res.tipo == terminales.and || res.tipo == terminales.or || res.tipo == terminales.not)
                                        cosasGlobalesewe.concatenarAccion(res.argumento);
                                    argumento += "t3 = " + res.valor + " ;\n";
                                    argumento += "booleanoCadena();\n";
                                }
                            }
                        }
                    }
                    

                }
                cosasGlobalesewe.concatenarAccion(argumento);
                //expresion laultima = new expresion(noterminales.EXPRESION, node.ChildNodes.ElementAt(2));
                //resultado res1 = laultima.Ejecutar();
                //cadenaSalida += res1.getValor();
            }


            return new resultado();
        }
    }
}
