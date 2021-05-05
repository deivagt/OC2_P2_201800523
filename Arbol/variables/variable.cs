using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.Arbol.Expresion;
using OC2_P2_201800523.Arbol.tipos;
using OC2_P2_201800523.Arbol.tipos.objetos;
using OC2_P2_201800523.AST;


namespace OC2_P2_201800523.Arbol.variables
{
    class variable : nodo
    {
        public variable(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public variable(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            string argumento = "";
            string temp;
            string otroOtroTemp;
            if (node.ChildNodes.Count == 5)
            {
                ParseTreeNode otraVariable = node.ChildNodes.ElementAt(4);

                if (node.ChildNodes.ElementAt(0).Term.ToString() == terminales.id)
                {
                    ParseTreeNode id = node.ChildNodes.ElementAt(0);
                    ParseTreeNode tipo = node.ChildNodes.ElementAt(2);

                    simbolo nuevoSimbolo;

                    int fila = id.Token.Location.Line;
                    int columna = id.Token.Location.Column;
                    string eltipo = tipo.ChildNodes.ElementAt(0).Token.Text;
                    string guardadoEtiqueta;
                    temp = cosasGlobalesewe.nuevoTemp();
                    string otroTemp;



                    /*Escribir en C3D*/

                    argumento = "/*EMPIEZA DECLARACION VARIABLE " + id.Token.Text + "*/\n";

                    if (eltipo == terminales.rstring || eltipo == terminales.rchar)
                    {
                        nuevoSimbolo = new simbolo(ambito, id.Token.Text, eltipo, temp, fila + 1, columna + 1, "variable");
                        tablaActual.agregarSimbolo(nuevoSimbolo);
                        argumento += temp + " = " + "hp" + ";\n";
                        argumento += "hp" + " = " + "hp" + " + 1;\n";
                        argumento += "heap" + "[(int)" + temp + "] = " + "-1" + ";\n";
                    }
                    else if (eltipo == terminales.rinteger)
                    {
                        nuevoSimbolo = new simbolo(ambito, id.Token.Text, eltipo, temp, fila + 1, columna + 1, "variable");
                        tablaActual.agregarSimbolo(nuevoSimbolo);
                        argumento += temp + " = " + "sp" + ";\n";
                        argumento += "sp" + " = " + "sp" + " + 1;\n";
                        argumento += "stack" + "[(int)" + temp + "] = " + "0" + ";\n";
                    }
                    else if (eltipo == terminales.rreal)
                    {
                        nuevoSimbolo = new simbolo(ambito, id.Token.Text, eltipo, temp, fila + 1, columna + 1, "variable");
                        tablaActual.agregarSimbolo(nuevoSimbolo);
                        argumento += temp + " = " + "sp" + ";\n";
                        argumento += "sp" + " = " + "sp" + " + 1;\n";
                        argumento += "stack" + "[(int)" + temp + "] = " + "0.0" + ";\n";
                    }
                    else if (eltipo == terminales.rboolean)
                    {
                        nuevoSimbolo = new simbolo(ambito, id.Token.Text, eltipo, temp, fila + 1, columna + 1, "variable");
                        tablaActual.agregarSimbolo(nuevoSimbolo);
                        argumento += temp + " = " + "sp" + ";\n";
                        argumento += "sp" + " = " + "sp" + " + 1;\n";
                        argumento += "stack" + "[(int)" + temp + "] = " + "0" + ";\n";
                    }
                    else// Tipo Custom
                    {
                        simbolo tipoCustom = tablaActual.buscarTipo(eltipo);
                        if (tipoCustom.categoria == "array")

                        {
                            #region array
                            string salidaCadenas = cosasGlobalesewe.nuevoTemp("hp");
                            argumento += "heap[(int)" + salidaCadenas + "] = -1;\n";
                            argumento += "hp" + " = " + "hp" + " + 1;\n";
                            tipos.arreglos.arreglo newArr = new tipos.arreglos.arreglo(terminales.rarray, node);
                            nuevoSimbolo = new simbolo(ambito, id.Token.Text, eltipo, temp, fila + 1, columna + 1, "variable", tipoCustom.listaIndex, true);
                            tablaActual.agregarSimbolo(nuevoSimbolo);
                            if (tipoCustom.listaIndex.Count == 1)
                            {
                                argumento += temp + " = " + "sp" + ";\n";
                                argumento += "sp" + " = " + "sp" + " + 1;\n";
                                argumento += "stack" + "[(int)" + temp + "] = " + "hp" + ";\n";



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
                                    else
                                    {
                                        argumento += "heap[(int)" + temporal + "] = " + salidaCadenas + ";\n";

                                    }
                                }


                            }
                            else
                            {
                                argumento += temp + " = " + "sp" + ";\n";
                                argumento += "sp" + " = " + "sp" + " + 1;\n";
                                argumento += "stack" + "[(int)" + temp + "] = " + "hp" + ";\n";


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
                                                else
                                                {
                                                    argumento += "heap[(int)" + temporal + "] = " + salidaCadenas + ";\n";

                                                }
                                            }
                                        }

                                    }



                                }
                            }
                            #endregion
                        }
                        else if (tipoCustom.categoria == "objeto")
                        {
                            #region objeto
                            LinkedList<simbolo> listaAtributos = new LinkedList<simbolo>();
                            /*salida para objetos*/
                            string salidaCadenas = cosasGlobalesewe.nuevoTemp("hp");
                            argumento += "heap[(int)" + salidaCadenas + "] = -1;\n";
                            argumento += "hp" + " = " + "hp" + " + 1;\n";
                            /*Declarar posicion Objeto*/
                            argumento += temp + " = " + "sp" + ";\n";
                            argumento += "sp" + " = " + "sp" + " + 1;\n";
                            argumento += "stack" + "[(int)" + temp + "] = " + "hp" + ";\n";
                            //bool repetir = false;


                            if (tipoCustom != null)
                            {

                                if (tipoCustom.listaAtributos != null)
                                {


                                    for (int i = 0; i < tipoCustom.listaAtributos.Count; i++)//Crear Temporales
                                    {
                                        temp = cosasGlobalesewe.nuevoTemp();
                                        argumento += temp + " = " + "hp" + ";\n";
                                        argumento += "hp" + " = " + "hp" + " + 1;\n";
                                        tipoCustom.listaAtributos.ElementAt(i).direccion = temp;

                                    }


                                    int contador = 0;
                                    foreach (var atributo in tipoCustom.listaAtributos)
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

                                            tipoCustom.listaAtributos.ElementAt(contador).listaAtributos = atributoComplejo.listaAtributos;


                                        }


                                        nuevoSimbolo = new simbolo("", atributo.id, atributo.tipo, atributo.direccion, fila + 1, columna + 1, "atributo");
                                        listaAtributos.AddLast(nuevoSimbolo);
                                        contador++;
                                    }
                                }
                                else//Array
                                {

                                }
                            }


                            /*Guardar en tabla de simbolos*/
                            nuevoSimbolo = new simbolo(ambito, id.Token.Text, eltipo, temp, fila + 1, columna + 1, "objeto", tipoCustom.listaAtributos, true);
                            tablaActual.agregarSimbolo(nuevoSimbolo);
                            #endregion
                        }
                    }

                    argumento += "/*FINALIZA DECLARACION VARIABLE " + id.Token.Text + "*/";
                    /*Agregar a la salida*/
                    cosasGlobalesewe.concatenarAccion(argumento);

                    if (otraVariable.ChildNodes.Count != 0)
                    {
                        variable otraVar = new variable(noterminales.VARIABLE, otraVariable);
                        otraVar.traducir(ref tablaActual, ambito, "", "", "");
                    }

                }
                else
                {
                    LinkedList<ParseTreeNode> listaVar = new LinkedList<ParseTreeNode>();
                    ParseTreeNode ids = node.ChildNodes.ElementAt(0);
                    ParseTreeNode tipo = node.ChildNodes.ElementAt(2);
                    simbolo nuevoSimbolo;
                    otra_decl_variable variasVariables = new otra_decl_variable(noterminales.OTRA_DECL_VARIABLE, ids);

                    variasVariables.nuevaTraduccion(listaVar);


                    foreach (var id in listaVar)
                    {
                        int fila = id.Token.Location.Line;
                        int columna = id.Token.Location.Column;



                        if (tipo.ChildNodes.Count == 1)
                        {
                            string eltipo = tipo.ChildNodes.ElementAt(0).Token.Text;

                            temp = cosasGlobalesewe.nuevoTemp();


                            argumento = "/*EMPIEZA DECLARACION VARIABLE " + id.Token.Text + "*/\n";

                            if (eltipo == terminales.rstring || eltipo == terminales.rchar)
                            {
                                nuevoSimbolo = new simbolo(ambito, id.Token.Text, eltipo, temp, fila + 1, columna + 1, "variable");
                                tablaActual.agregarSimbolo(nuevoSimbolo);
                                argumento += temp + " = " + "hp" + ";\n";
                                argumento += "hp" + " = " + "hp" + " + 1;\n";
                                argumento += "heap" + "[(int)" + temp + "] = " + "-1" + ";\n";
                            }
                            else if (eltipo == terminales.rinteger)
                            {
                                nuevoSimbolo = new simbolo(ambito, id.Token.Text, eltipo, temp, fila + 1, columna + 1, "variable");
                                tablaActual.agregarSimbolo(nuevoSimbolo);
                                argumento += temp + " = " + "sp" + ";\n";
                                argumento += "sp" + " = " + "sp" + " + 1;\n";
                                argumento += "stack" + "[(int)" + temp + "] = " + "0" + ";\n";
                            }
                            else if (eltipo == terminales.rreal)
                            {
                                nuevoSimbolo = new simbolo(ambito, id.Token.Text, eltipo, temp, fila + 1, columna + 1, "variable");
                                tablaActual.agregarSimbolo(nuevoSimbolo);
                                argumento += temp + " = " + "sp" + ";\n";
                                argumento += "sp" + " = " + "sp" + " + 1;\n";
                                argumento += "stack" + "[(int)" + temp + "] = " + "0.0" + ";\n";
                            }
                            else if (eltipo == terminales.rboolean)
                            {
                                nuevoSimbolo = new simbolo(ambito, id.Token.Text, eltipo, temp, fila + 1, columna + 1, "variable");
                                tablaActual.agregarSimbolo(nuevoSimbolo);
                                argumento += temp + " = " + "sp" + ";\n";
                                argumento += "sp" + " = " + "sp" + " + 1;\n";
                                argumento += "stack" + "[(int)" + temp + "] = " + "0" + ";\n";
                            }

                            else// Tipo Custom
                            {
                                simbolo tipoCustom = tablaActual.buscarTipo(eltipo);
                                if (tipoCustom.categoria == "array")
                                {
                                    #region array
                                    string salidaCadenas = cosasGlobalesewe.nuevoTemp("hp");
                                    argumento += "heap[(int)" + salidaCadenas + "] = -1;\n";
                                    argumento += "hp" + " = " + "hp" + " + 1;\n";
                                    tipos.arreglos.arreglo newArr = new tipos.arreglos.arreglo(terminales.rarray, node);
                                    nuevoSimbolo = new simbolo(ambito, id.Token.Text, eltipo, temp, fila + 1, columna + 1, "variable", tipoCustom.listaIndex, true);
                                    tablaActual.agregarSimbolo(nuevoSimbolo);
                                    if (tipoCustom.listaIndex.Count == 1)
                                    {
                                        argumento += temp + " = " + "sp" + ";\n";
                                        argumento += "sp" + " = " + "sp" + " + 1;\n";
                                        argumento += "stack" + "[(int)" + temp + "] = " + "hp" + ";\n";



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
                                            else
                                            {
                                                argumento += "heap[(int)" + temporal + "] = " + salidaCadenas + ";\n";

                                            }
                                        }


                                    }
                                    else
                                    {
                                        argumento += temp + " = " + "sp" + ";\n";
                                        argumento += "sp" + " = " + "sp" + " + 1;\n";
                                        argumento += "stack" + "[(int)" + temp + "] = " + "hp" + ";\n";


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
                                                        else
                                                        {
                                                            argumento += "heap[(int)" + temporal + "] = " + salidaCadenas + ";\n";

                                                        }
                                                    }
                                                }

                                            }



                                        }
                                    }
                                    #endregion
                                }
                                else if (tipoCustom.categoria == "objeto")
                                {
                                    #region objeto
                                    LinkedList<simbolo> listaAtributos = new LinkedList<simbolo>();
                                    /*salida para objetos*/
                                    string salidaCadenas = cosasGlobalesewe.nuevoTemp("hp");
                                    argumento += "heap[(int)" + salidaCadenas + "] = -1;\n";
                                    argumento += "hp" + " = " + "hp" + " + 1;\n";
                                    /*Declarar posicion Objeto*/
                                    argumento += temp + " = " + "sp" + ";\n";
                                    argumento += "sp" + " = " + "sp" + " + 1;\n";
                                    argumento += "stack" + "[(int)" + temp + "] = " + "hp" + ";\n";
                                    //bool repetir = false;


                                    if (tipoCustom != null)
                                    {


                                        if (tipoCustom.listaAtributos != null)
                                        {


                                            for (int i = 0; i < tipoCustom.listaAtributos.Count; i++)//Crear Temporales
                                            {
                                                temp = cosasGlobalesewe.nuevoTemp();
                                                argumento += temp + " = " + "hp" + ";\n";
                                                argumento += "hp" + " = " + "hp" + " + 1;\n";
                                                tipoCustom.listaAtributos.ElementAt(i).direccion = temp;

                                            }


                                            int contador = 0;
                                            foreach (var atributo in tipoCustom.listaAtributos)
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

                                                    tipoCustom.listaAtributos.ElementAt(contador).listaAtributos = atributoComplejo.listaAtributos;


                                                }


                                                nuevoSimbolo = new simbolo("", atributo.id, atributo.tipo, atributo.direccion, fila + 1, columna + 1, "atributo");
                                                listaAtributos.AddLast(nuevoSimbolo);
                                                contador++;
                                            }
                                        }
                                        else//Array
                                        {

                                        }
                                    }


                                    /*Guardar en tabla de simbolos*/
                                    nuevoSimbolo = new simbolo(ambito, id.Token.Text, eltipo, temp, fila + 1, columna + 1, "objeto", tipoCustom.listaAtributos, true);
                                    tablaActual.agregarSimbolo(nuevoSimbolo);
                                    #endregion
                                }
                            }

                        }


                        argumento += "/*FINALIZA DECLARACION VARIABLE " + id.Token.Text + "*/";
                        /*Agregar a la salida*/
                        cosasGlobalesewe.concatenarAccion(argumento);
                    }

                }

            }
            else
            {

                ParseTreeNode id = node.ChildNodes.ElementAt(0);
                ParseTreeNode tipo = node.ChildNodes.ElementAt(2);
                ParseTreeNode expresion = node.ChildNodes.ElementAt(4);
                ParseTreeNode otraVariable = node.ChildNodes.ElementAt(6);

                expresion expr = new expresion(noterminales.EXPRESION, expresion);
                resultado res = expr.traducir(ref tablaActual, ambito, "", "", "");

                cosasGlobalesewe.concatenarAccion(res.argumento);
                //Creacion de simbolo
                int fila = id.Token.Location.Line;
                int columna = id.Token.Location.Column;
                string eltipo = tipo.ChildNodes.ElementAt(0).Token.Text;
                bool bandera = true;//Variable para la verificacion de tipos ewe

                if (bandera)
                {
                    temp = cosasGlobalesewe.nuevoTemp();

                    simbolo nuevoSimbolo = new simbolo(ambito, id.Token.Text, eltipo, temp, fila + 1, columna + 1, "variable");

                    tablaActual.agregarSimbolo(nuevoSimbolo);

                    /*Escribir en C3D*/

                    argumento = "/*EMPIEZA DECLARACION VARIABLE " + id.Token.Text + "*/\n";


                    argumento += temp + " = " + "sp" + ";\n";
                    argumento += "sp" + " = " + "sp" + " + 1;\n";
                    argumento += "stack" + "[(int)" + temp + "] = " + res.valor + ";\n";



                    argumento += "/*FINALIZA DECLARACION VARIABLE " + id.Token.Text + "*/";
                    /*Agregar a la salida*/
                    cosasGlobalesewe.concatenarAccion(argumento);

                    if (otraVariable.ChildNodes.Count != 0)
                    {
                        variable otraVar = new variable(noterminales.VARIABLE, otraVariable);
                        otraVar.traducir(ref tablaActual, ambito, "", "", "");
                    }

                }
                else
                {
                    //CAGASTE XD
                }

            }
            return new resultado();
        }

        atributo iniciarAtributos(ref tabla tablaActual, atributo atr, string temp, ref string argumento, string salidaCadenas)
        {
            
            string otroOtroTemp = "";
            atributo retorno = null ;


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
                                argumento += "heap[(int)" + tipoCustom.direccion + "] = 0;\n";
                            }
                            else if (tipoCustom.tipo == "string" || tipoCustom.tipo == "char")
                            {
                                argumento += "heap[(int)" + tipoCustom.direccion + "] = " + salidaCadenas + ";\n";
                            }
                            else//Un type xd
                            {
                                //ASUMIENDO QUE NO ES UN ARRAY
                                temp = cosasGlobalesewe.nuevoTemp();
                                argumento += "heap[(int)" + tipoCustom.direccion + "] = " + temp + ";\n";
                                //TA A MEDIAS
                                atributo atributoComplejo = iniciarAtributos(ref tablaActual, atributo, temp, ref argumento, salidaCadenas);

                                retorno.listaAtributos.ElementAt(contador).listaAtributos = atributoComplejo.listaAtributos;
                            }
                            contador++;
                            //if (tipoCustom.tipo == "integer" || tipoCustom.tipo == "real" || tipoCustom.tipo == "boolean")
                            //{
                            //    argumento += "heap[(int)" + temporal + "] = 0;\n";
                            //}
                            //else
                            //{
                            //    argumento += "heap[(int)" + temporal + "] = " + salidaCadenas + ";\n";

                            //}
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
                                        else
                                        {
                                            argumento += "heap[(int)" + temporal + "] = " + salidaCadenas + ";\n";

                                        }
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
