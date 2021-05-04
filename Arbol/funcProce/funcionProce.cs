using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.AST;
using OC2_P2_201800523.Arbol.sentencia;

namespace OC2_P2_201800523.Arbol.funcProce
{
    /*
     FUNCION.Rule =
       
       | function + id + abrir_parentesis + VALOR_REFERENCIA + cerrar_parentesis + dos_puntos + TRETORNO + punto_coma + DECLARARENFUNC + OTRA_FUNCION + begin + SENTENCIAS + end + punto_coma
       ;
    */
    class funcionProce : nodo
    {
        public funcionProce(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public funcionProce(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            string argumento = "";
            string tipoAccion = node.ChildNodes.ElementAt(0).Token.Text;
            LinkedList<parametroCustom> listaParam;
            tabla nuevaTabla;

            string respaldo = cosasGlobalesewe.tomarSalida();

            if (tipoAccion == "function")
            {
                ParseTreeNode id = node.ChildNodes.ElementAt(1);
                if (tablaActual.buscar(id.Token.Text, ambito) != null)
                {
                    /*Error*/
                }
                else
                {
                    /*Crear simbolo*/
                    string puntero = cosasGlobalesewe.nuevoTemp();
                    string etiquetaRetorno = cosasGlobalesewe.crearEtiqueta();
                    int fila = id.Token.Location.Line;
                    int columna = id.Token.Location.Column;

                    /*El trucazo de los parametros*/
                    listaParam = new LinkedList<parametroCustom>();
                    valorReferencia preparacionParam = new valorReferencia(noterminales.VALOR_REFERENCIA, node.ChildNodes.ElementAt(3));
                    preparacionParam.nuevaTraduccion(listaParam);
                    string tipo = node.ChildNodes.ElementAt(6).ChildNodes.ElementAt(0).Token.Text;
                    //TEMP REPRESENTA LA ETIQUETA CON LA QUE SE GUERDARA EL RETORNO
                    simbolo nuevaFuncion = new simbolo(ambito, id.Token.Text, tipo, puntero, fila, columna, "funcion", listaParam);
                    tablaActual.agregarSimbolo(nuevaFuncion);
                    nuevaTabla = tablaActual.Clone();


                    /*Inicio de funcion*/
                    argumento += "void " + id.Token.Text + "(){\n";
                    /*Declarar los parametros*/
                    argumento += "/*Inicia declaracion de parametros*/\n";

                    argumento += puntero + " = sp;\n";
                    int contador = 1;
                    foreach (var a in listaParam)
                    {
                        argumento += "/*Se declara un param*/\n";
                        if (a.porValor == true)
                        {
                            string nuevoTemp = cosasGlobalesewe.nuevoTemp();
                            /*Declarar la posicion del parametro*/
                            argumento += nuevoTemp + " = " + puntero + " + " + contador + ";\n";
                            argumento += "sp = sp + 1;\n";
                            simbolo nuevoSimbolo = new simbolo(id.Token.Text, a.id, a.tipo, nuevoTemp, 0, 0, "parametro");
                            a.direccion = nuevoTemp;
                            nuevaTabla.agregarSimbolo(nuevoSimbolo);
                        }
                        else
                        {
                            string otraCosa = cosasGlobalesewe.nuevoTemp();
                            string nuevoTemp = cosasGlobalesewe.nuevoTemp();
                            /*Declarar la posicion del parametro*/
                            argumento += otraCosa + " = " + puntero + " + " + contador + ";\n";

                            argumento += nuevoTemp + " = " + "stack" + "[(int)" + otraCosa + "];\n";
                            argumento += "sp = sp + 1;\n";

                            simbolo nuevoSimbolo = new simbolo(id.Token.Text, a.id, a.tipo, nuevoTemp, 0, 0, "parametro");
                            a.direccion = nuevoTemp;
                            nuevaTabla.agregarSimbolo(nuevoSimbolo);
                        }

                        argumento += "/*Se acaba un param*/\n";
                        contador++;
                    }

                    if (listaParam.Count != 0)
                    {
                        argumento += "sp = sp + 1;\n";
                    }
                    argumento += "/*Finaliza declaracion de parametros*/\n";

                    argumento += "/*Declaracion de variables internas*/\n";
                    LinkedList<variables.variable> listVar = new LinkedList<variables.variable>();
                    declararEnFunc nuevaDecl = new declararEnFunc(noterminales.DECLARARENFUNC, node.ChildNodes.ElementAt(8));
                    nuevaDecl.nuevaTraduccion(listVar);
                    foreach(var variable in listVar)
                    {                        
                        variable.traducir(ref nuevaTabla, id.Token.Text, verdadero, falso, xd);
                    }
                   
                    argumento += "/*Finaliza declaracion de variables internas*/\n";

                    hacerTraduccion(node.ChildNodes.ElementAt(11), ref nuevaTabla, id.Token.Text, "", "", etiquetaRetorno+";"+puntero);
                    argumento += cosasGlobalesewe.tomarSalida();
                    argumento += etiquetaRetorno + ":\n" + "return;\n}\n";
                    tablaActual.restaurar(nuevaTabla);
                    cosasGlobalesewe.funcionesC3D.AddLast(argumento);
                }
            }
            else// procedure
            /*procedure + id + abrir_parentesis + VALOR_REFERENCIA + cerrar_parentesis + punto_coma + DECLARARENFUNC + OTRA_FUNCION + begin + SENTENCIAS + end + punto_coma*/
            {
                ParseTreeNode id = node.ChildNodes.ElementAt(1);
                if (tablaActual.buscar(id.Token.Text, ambito) != null)
                {
                    /*Error*/
                }
                else
                {
                    /*Crear simbolo*/
                    string puntero = cosasGlobalesewe.nuevoTemp();
                    string etiquetaRetorno = cosasGlobalesewe.crearEtiqueta();
                    int fila = id.Token.Location.Line;
                    int columna = id.Token.Location.Column;

                    /*El trucazo de los parametros*/
                    listaParam = new LinkedList<parametroCustom>();
                    valorReferencia preparacionParam = new valorReferencia(noterminales.VALOR_REFERENCIA, node.ChildNodes.ElementAt(3));
                    preparacionParam.nuevaTraduccion(listaParam);
                    //TEMP REPRESENTA LA ETIQUETA CON LA QUE SE GUERDARA EL RETORNO
                    simbolo nuevoProcedimiento = new simbolo(ambito, id.Token.Text, "void", puntero, fila, columna, "procedimiento", listaParam);
                    tablaActual.agregarSimbolo(nuevoProcedimiento);
                    nuevaTabla = tablaActual.Clone();


                    /*Inicio de funcion*/
                    argumento += "void " + id.Token.Text + "(){\n";
                    /*Declarar los parametros*/
                    argumento += "/*Inicia declaracion de parametros*/\n";

                    argumento += puntero + " = sp;\n";
                    int contador = 1;
                    foreach (var a in listaParam)
                    {
                        argumento += "/*Se declara un param*/\n";
                        if (a.porValor == true)
                        {
                            string nuevoTemp = cosasGlobalesewe.nuevoTemp();
                            /*Declarar la posicion del parametro*/
                            argumento += nuevoTemp + " = " + puntero + " + " + contador + ";\n";
                            argumento += "sp = sp + 1;\n";
                            simbolo nuevoSimbolo = new simbolo(id.Token.Text, a.id, a.tipo, nuevoTemp, 0, 0, "parametro");
                            a.direccion = nuevoTemp;
                            nuevaTabla.agregarSimbolo(nuevoSimbolo);
                        }
                        else
                        {
                            string otraCosa = cosasGlobalesewe.nuevoTemp();
                            string nuevoTemp = cosasGlobalesewe.nuevoTemp();
                            /*Declarar la posicion del parametro*/
                            argumento += otraCosa + " = " + puntero + " + " + contador + ";\n";

                            argumento += nuevoTemp + " = " + "stack" + "[(int)" + otraCosa + "];\n";
                            argumento += "sp = sp + 1;\n";

                            simbolo nuevoSimbolo = new simbolo(id.Token.Text, a.id, a.tipo, nuevoTemp, 0, 0, "parametro");
                            a.direccion = nuevoTemp;
                            nuevaTabla.agregarSimbolo(nuevoSimbolo);
                        }

                        argumento += "/*Se acaba un param*/\n";
                        contador++;
                    }

                    if (listaParam.Count != 0)
                    {
                        argumento += "sp = sp + 1;\n";
                    }
                    argumento += "/*Finaliza declaracion de parametros*/\n";

                    argumento += "/*Declaracion de variables internas*/\n";
                    LinkedList<variables.variable> listVar = new LinkedList<variables.variable>();
                    declararEnFunc nuevaDecl = new declararEnFunc(noterminales.DECLARARENFUNC, node.ChildNodes.ElementAt(6));
                    nuevaDecl.nuevaTraduccion(listVar);
                    foreach (var variable in listVar)
                    {
                        variable.traducir(ref nuevaTabla, id.Token.Text, verdadero, falso, xd);
                    }

                    argumento += "/*Finaliza declaracion de variables internas*/\n";
                    hacerTraduccion(node.ChildNodes.ElementAt(9), ref nuevaTabla, id.Token.Text, "", "", etiquetaRetorno);
                    argumento += cosasGlobalesewe.tomarSalida();
                    argumento += etiquetaRetorno + ":\n" + "return;\n}\n";
                    tablaActual.restaurar(nuevaTabla);
                    cosasGlobalesewe.funcionesC3D.AddLast(argumento);

                }
            }
            cosasGlobalesewe.concatenarAccion(respaldo);
            return new resultado();
        }

        void hacerTraduccion(ParseTreeNode lstSent, ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            if (lstSent.ChildNodes.Count != 0)
            {
                LinkedList<sentencia.sentencia> listaSentencias = new LinkedList<sentencia.sentencia>();
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
