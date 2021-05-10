using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.AST;
using OC2_P2_201800523.Arbol.sentencia;
using OC2_P2_201800523.Arbol.tipos.objetos;

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

            string temp;
            if (tipoAccion == "function")
            {
                ParseTreeNode id = node.ChildNodes.ElementAt(1);
                if (tablaActual.buscar(id.Token.Text, ambito) != null)
                {
                    /*Error*/
                }
                else
                {
                    LinkedList<simbolo> listaAtributos = new LinkedList<simbolo>();
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
                            simbolo nuevoSimbolo;

                            if (a.tipo == "integer" || a.tipo == "real" || a.tipo == "string" || a.tipo == "boolean" || a.tipo == "char")
                            {
                                nuevoSimbolo = new simbolo(id.Token.Text, a.id, a.tipo, nuevoTemp, 0, 0, "parametro");
                                a.direccion = nuevoTemp;
                            }
                            else
                            {
                                simbolo tipoCustom = tablaActual.buscarTipo(a.tipo);
                                nuevoSimbolo = new simbolo(id.Token.Text, a.id, a.tipo, nuevoTemp, "parametro", tipoCustom.listaAtributos);
                                a.direccion = nuevoTemp;
                            }

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
                            simbolo nuevoSimbolo;

                            if (a.tipo == "integer" || a.tipo == "real" || a.tipo == "string" || a.tipo == "boolean" || a.tipo == "char")
                            {
                                nuevoSimbolo = new simbolo(id.Token.Text, a.id, a.tipo, nuevoTemp, 0, 0, "parametro");
                                a.direccion = nuevoTemp;
                            }
                            else
                            {
                                simbolo tipoCustom = tablaActual.buscarTipo(a.tipo);
                                nuevoSimbolo = new simbolo(id.Token.Text, a.id, a.tipo, nuevoTemp, "parametro", tipoCustom.listaAtributos);
                                nuevoSimbolo.listaIndex = tipoCustom.listaIndex;
                                a.direccion = nuevoTemp;
                            }

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
                    foreach (var variable in listVar)
                    {
                        variable.traducir(ref nuevaTabla, id.Token.Text, verdadero, falso, xd);
                    }

                    argumento += "/*Finaliza declaracion de variables internas*/\n";

                    hacerTraduccion(node.ChildNodes.ElementAt(11), ref nuevaTabla, id.Token.Text, "", "", etiquetaRetorno + ";" + puntero);
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
                            simbolo nuevoSimbolo;

                            if (a.tipo == "integer" || a.tipo == "real" || a.tipo == "string" || a.tipo == "boolean" || a.tipo == "char")
                            {
                                nuevoSimbolo = new simbolo(id.Token.Text, a.id, a.tipo, nuevoTemp, 0, 0, "parametro");
                                a.direccion = nuevoTemp;
                            }
                            else
                            {
                                simbolo tipoCustom = tablaActual.buscarTipo(a.tipo);
                                nuevoSimbolo = new simbolo(id.Token.Text, a.id, a.tipo, nuevoTemp, "parametro", tipoCustom.listaAtributos);
                                nuevoSimbolo.listaIndex = tipoCustom.listaIndex;
                                a.direccion = nuevoTemp;
                            }

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
                            simbolo nuevoSimbolo;

                            if (a.tipo == "integer" || a.tipo == "real" || a.tipo == "string" || a.tipo == "boolean" || a.tipo == "char")
                            {
                                nuevoSimbolo = new simbolo(id.Token.Text, a.id, a.tipo, nuevoTemp, 0, 0, "parametro");
                                a.direccion = nuevoTemp;
                            }
                            else
                            {
                                simbolo tipoCustom = tablaActual.buscarTipo(a.tipo);
                                nuevoSimbolo = new simbolo(id.Token.Text, a.id, a.tipo, nuevoTemp, "parametro", tipoCustom.listaAtributos);
                                a.direccion = nuevoTemp;
                            }

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

        atributo iniciarAtributos(ref tabla tablaActual, atributo atr, string temp, ref string argumento, string salidaCadenas)
        {

            string otroOtroTemp = "";
            atributo retorno = null;


            simbolo tipoCustom = tablaActual.buscarTipo(atr.tipo);
            LinkedList<simbolo> listaAtributos = new LinkedList<simbolo>();

            /*Declarar posicion Objeto*/
            argumento += "/*Inicio Atributo complejo*/\n";

            //bool repetir = false;

            if (tipoCustom != null)
            {

                retorno = atr;
                retorno.direccion = temp;
                retorno.listaAtributos = tipoCustom.listaAtributos;

                if (retorno.listaAtributos != null)
                {


                    for (int i = 0; i < retorno.listaAtributos.Count; i++)//Crear Temporales
                    {

                        argumento += temp + " = " + "hp" + ";\n";
                        argumento += "hp" + " = " + "hp" + " + 1;\n";
                        retorno.listaAtributos.ElementAt(i).direccion = temp;
                        temp = cosasGlobalesewe.nuevoTemp();
                    }

                    int contador = 0;

                    foreach (var atributo in retorno.listaAtributos)
                    {

                        if (atributo.tipo == "integer" || atributo.tipo == "real" || atributo.tipo == "boolean")
                        {
                            argumento += "heap[(int)" + atributo.direccion + "] = 0;\n";
                        }
                        else if (atributo.tipo == "string" || atributo.tipo == "char")
                        {
                            argumento += "heap[(int)" + atributo.direccion + "] = " + salidaCadenas + ";\n";
                        }
                        else
                        {
                            temp = cosasGlobalesewe.nuevoTemp();
                            argumento += "heap[(int)" + atributo.direccion + "] = " + temp + ";\n";

                            atributo atributoComplejo = iniciarAtributos(ref tablaActual, atributo, temp, ref argumento, salidaCadenas);

                            retorno.listaAtributos.ElementAt(contador).listaAtributos = atributoComplejo.listaAtributos;
                        }
                        contador++;
                    }
                }
                else//Array
                {

                    tipos.arreglos.arreglo newArr = new tipos.arreglos.arreglo(terminales.rarray, node);
                    argumento += "heap" + "[(int)" + temp + "] = " + "hp" + ";\n";
                    if (tipoCustom.listaIndex.Count == 1)
                    {
                        LinkedList<tipos.arreglos.index> listaActual = tipoCustom.listaIndex;
                        int inicio = tipoCustom.listaIndex.ElementAt(0).inicio;
                        int final = tipoCustom.listaIndex.ElementAt(0).final;
                        int tamanioArray = final - inicio + 1;


                        LinkedList<string> temporalesEwe = new LinkedList<string>();
                        for (int i = 0; i <= tamanioArray; i++)//Crear Temporales
                        {
                            temp = cosasGlobalesewe.nuevoTemp();
                            argumento += temp + " = " + "hp" + ";\n";
                            argumento += "hp" + " = " + "hp" + " + 1;\n";
                            temporalesEwe.AddLast(temp);
                        }
                        bool asignartamanio = true;
                        simbolo esteAtr;
                        atributo otroatr;
                        int contador = 0;
                        foreach (var temporal in temporalesEwe)
                        {
                            if (asignartamanio == true)
                            {
                                argumento += "heap[(int)" + temporal + "] = " + tamanioArray + ";\n";
                                asignartamanio = false;
                                continue;
                            }
                            if (tipoCustom.tipo == "integer" || tipoCustom.tipo == "real" || tipoCustom.tipo == "boolean")
                            {
                                argumento += "heap[(int)" + temporal + "] = 0;\n";
                            }
                            else if (tipoCustom.tipo == "string" || tipoCustom.tipo == "char")
                            {
                                argumento += "heap[(int)" + temporal + "] = " + salidaCadenas + ";\n";
                            }
                            else//Un type xd
                            {
                                //ASUMIENDO QUE NO ES UN ARRAY
                                temp = cosasGlobalesewe.nuevoTemp();
                                argumento += "/*ACCION ARRAY*/\n";
                                argumento += "heap[(int)" + temporal + "] = " + temp + ";\n";
                                //TA A MEDIAS
                                esteAtr = tablaActual.buscarTipo(tipoCustom.tipo);//Busca el tipo del array
                                otroatr = new atributo(esteAtr.id, esteAtr.id);
                                otroatr.direccion = temporal;
                                atributo atributoComplejo = iniciarAtributos(ref tablaActual, otroatr, temp, ref argumento, salidaCadenas);
                                retorno.esArray = true;
                                retorno.listaIndex = listaActual;
                                retorno.listaAtributos = atributoComplejo.listaAtributos;
                            }
                            contador++;


                        }


                    }
                    else
                    {



                        LinkedList<tipos.arreglos.index> listaActual = tipoCustom.listaIndex;
                        int multiplicador = 1;
                        for (int j = 0; j < listaActual.Count; j++)
                        {
                            if (j < listaActual.Count - 1)
                            {

                                int inicio = listaActual.ElementAt(j).inicio;
                                int final = listaActual.ElementAt(j).final;
                                int tamanioArray = final - inicio + 1;
                                /*ASUMIENDO 2D*/
                                int inicioSiguiente = listaActual.ElementAt(j + 1).inicio;
                                int finalSiguiente = listaActual.ElementAt(j + 1).final;
                                int tamanioSig = finalSiguiente - inicioSiguiente + 1;
                                multiplicador = multiplicador * tamanioArray;


                                string temphp = cosasGlobalesewe.nuevoTemp();
                                argumento += temphp + " = hp;\n";
                                LinkedList<string> temporalesEwe = new LinkedList<string>();
                                for (int i = 0; i <= tamanioArray; i++)//Crear Temporales
                                {
                                    temp = cosasGlobalesewe.nuevoTemp();
                                    argumento += temp + " = " + "hp" + ";\n";
                                    argumento += "hp" + " = " + "hp" + " + 1;\n";
                                    temporalesEwe.AddLast(temp);
                                }
                                bool asignartamanio = true;
                                int contador = 0;
                                foreach (var temporal in temporalesEwe)
                                {
                                    if (asignartamanio == true)
                                    {
                                        argumento += "heap[(int)" + temporal + "] = " + tamanioArray + ";\n";
                                        asignartamanio = false;
                                        continue;
                                    }
                                    int result = ((tamanioArray + (tamanioSig + 1) * contador) + 1);
                                    argumento += "heap[(int)" + temporal + "] = " + temphp + " + " + result + ";\n";
                                    contador++;
                                }
                            }
                            else
                            {
                                int inicio = listaActual.ElementAt(j).inicio;
                                int final = listaActual.ElementAt(j).final;
                                int tamanioArray = final - inicio + 1;
                                /*ASUMIENDO 2D*/



                                for (int k = 0; k < multiplicador; k++)
                                {

                                    LinkedList<string> temporalesEwe = new LinkedList<string>();
                                    for (int i = 0; i <= tamanioArray; i++)//Crear Temporales
                                    {
                                        temp = cosasGlobalesewe.nuevoTemp();
                                        argumento += temp + " = " + "hp" + ";\n";
                                        argumento += "hp" + " = " + "hp" + " + 1;\n";
                                        temporalesEwe.AddLast(temp);
                                    }
                                    bool asignartamanio = true;
                                    simbolo esteAtr;
                                    atributo otroatr;
                                    int contador = 0;
                                    foreach (var temporal in temporalesEwe)
                                    {
                                        if (asignartamanio == true)
                                        {
                                            argumento += "heap[(int)" + temporal + "] = " + tamanioArray + ";\n";
                                            asignartamanio = false;
                                            continue;
                                        }
                                        if (tipoCustom.tipo == "integer" || tipoCustom.tipo == "real" || tipoCustom.tipo == "boolean")
                                        {
                                            argumento += "heap[(int)" + temporal + "] = 0;\n";
                                        }
                                        else if (tipoCustom.tipo == "string" || tipoCustom.tipo == "char")
                                        {
                                            argumento += "heap[(int)" + temporal + "] = " + salidaCadenas + ";\n";
                                        }
                                        else//Un type xd
                                        {
                                            //ASUMIENDO QUE NO ES UN ARRAY
                                            temp = cosasGlobalesewe.nuevoTemp();
                                            argumento += "/*ACCION ARRAY*/\n";
                                            argumento += "heap[(int)" + temporal + "] = " + temp + ";\n";
                                            //TA A MEDIAS
                                            esteAtr = tablaActual.buscarTipo(tipoCustom.tipo);//Busca el tipo del array
                                            otroatr = new atributo(esteAtr.id, esteAtr.id);
                                            otroatr.direccion = temporal;
                                            atributo atributoComplejo = iniciarAtributos(ref tablaActual, otroatr, temp, ref argumento, salidaCadenas);
                                            retorno.esArray = true;
                                            retorno.listaIndex = listaActual;
                                            retorno.listaAtributos = atributoComplejo.listaAtributos;
                                        }
                                        contador++;

                                    }
                                }

                            }



                        }
                    }
                }
            }
            argumento += "/*Final Atributo complejo*/\n";


            return retorno;
        }
    }
}
