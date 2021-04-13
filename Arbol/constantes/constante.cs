using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.Arbol.Expresion;
using OC2_P2_201800523.AST;

namespace OC2_P2_201800523.Arbol.constantes
{
    class constante : nodo
    {
        public constante(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public constante(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            string argumento = "";
            string temp;
            //ParseTreeNode id = node.ChildNodes.ElementAt(0);
            //ParseTreeNode tipo = node.ChildNodes.ElementAt(2);
            //ParseTreeNode expresion = node.ChildNodes.ElementAt(4);
            //ParseTreeNode otraConstante = node.ChildNodes.ElementAt(6);

            //int fila = id.Token.Location.Line;
            //int columna = id.Token.Location.Column;

            //expresion expr = new expresion(noterminales.EXPRESION, expresion);
            //resultado res = expr.Ejecutar();
            //string eltipo = tipo.ChildNodes.ElementAt(0).Token.Text;
            //simbolo nuevoSimbolo = new simbolo(manejadorArbol.ambitoActual, id.Token.Text, res.getValor(),eltipo, fila + 1, columna + 1,false,true);
            //manejadorArbol.tabladeSimbolos.agregarSimbolo(nuevoSimbolo);

            //if (otraConstante.ChildNodes.Count != 0)
            //{
            //    constante otraVar = new constante(noterminales.CONSTANTE, otraConstante);
            //    otraVar.Ejecutar();
            //}
            ParseTreeNode id = node.ChildNodes.ElementAt(0);
            ParseTreeNode tipo = node.ChildNodes.ElementAt(2);
            ParseTreeNode expresion = node.ChildNodes.ElementAt(4);
            ParseTreeNode otraConstante = node.ChildNodes.ElementAt(6);

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

                simbolo nuevoSimbolo = new simbolo(ambito, id.Token.Text, eltipo, temp, fila + 1, columna + 1, true);

                tablaActual.agregarSimbolo(nuevoSimbolo);
                string array = "";
                string pointer = "";
                /*Escribir en C3D*/
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
                argumento = "/*EMPIEZA DECLARACION CONSTANTE " + id.Token.Text + "*/\n";
                argumento += temp + " = " + pointer + ";\n";

                if (res.tipo == terminales.cadena || res.tipo == terminales.rchar)
                {
                    //if (array != "heap") /*Hacer que el stack apunte al heap*/ //SIGUE EN DESARROLLO
                    //{
                    //    argumento += array + "[(int)" + temp + "] = " + "hp" + ";\n";
                    //}

                    foreach (char caracter in res.valor)
                    {
                        argumento += array + "[(int)" + pointer + "] = " + (int)caracter + ";\n";
                        argumento += pointer + " = " + pointer + " + 1;\n";
                    }

                    argumento += array + "[(int)" + pointer + "] = " + "-1" + ";\n";
                    argumento += pointer + " = " + pointer + " + 1;\n";
                }
                else
                {

                    argumento += pointer + " = " + pointer + " + 1;\n";
                    argumento += array + "[(int)" + temp + "] = " + res.valor + ";\n";

                }

                argumento += "/*FINALIZA DECLARACION CONSTANTE " + id.Token.Text + "*/";
                /*Agregar a la salida*/
                cosasGlobalesewe.concatenarAccion(argumento);

                if (otraConstante.ChildNodes.Count != 0)
                {
                    constante otraVar = new constante(noterminales.VARIABLE, otraConstante);
                    otraVar.traducir(ref tablaActual, ambito, "", "", "");
                }

            }
            return new resultado();
        }
    }
}
