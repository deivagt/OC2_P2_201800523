using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.AST;

namespace OC2_P2_201800523.Arbol.funcProce
{
    /*
     FUNCION.Rule =
       procedure + id + abrir_parentesis + VALOR_REFERENCIA + cerrar_parentesis + punto_coma + DECLARARENFUNC + OTRA_FUNCION + begin + SENTENCIAS + end + punto_coma
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

            if (tipoAccion == "function")
            {
                ParseTreeNode id = node.ChildNodes.ElementAt(1);
                if (tablaActual.buscar(id.Token.Text) != null)
                {
                    /*Error*/
                }
                else
                {
                    /*Crear simbolo*/
                    ParseTreeNode tipo = node.ChildNodes.ElementAt(6);
                    string temp = cosasGlobalesewe.nuevoTemp();
                    int fila = id.Token.Location.Line;
                    int columna = id.Token.Location.Column;
                    string eltipo = tipo.ChildNodes.ElementAt(0).Token.Text;

                    /*El trucazo de los parametros*/
                    listaParam = new LinkedList<parametroCustom>();
                    valorReferencia preparacionParam = new valorReferencia(noterminales.VALOR_REFERENCIA, node.ChildNodes.ElementAt(3));
                    preparacionParam.nuevaTraduccion(listaParam);

                    simbolo nuevaFuncion = new simbolo(ambito, id.Token.Text, eltipo, temp, fila, columna, "funcion",listaParam);
                    tablaActual.agregarSimbolo(nuevaFuncion);

                    /*Declarar los parametros*/
                    argumento += "/*Inicia declaracion de parametros*/\n";
                    foreach (var a in listaParam)
                    {
                        System.Diagnostics.Debug.WriteLine(a.id + " esporvalor: " + a.porValor);
                    }

                }



            }
            else// procedure
            {
                ParseTreeNode id = node.ChildNodes.ElementAt(1);
                if (tablaActual.buscar(id.Token.Text) != null)
                {
                    /*Error*/
                }
                else
                {
                    /*Crear simbolo*/
                    ParseTreeNode tipo = node.ChildNodes.ElementAt(6);
                    string temp = cosasGlobalesewe.nuevoTemp();
                    int fila = id.Token.Location.Line;
                    int columna = id.Token.Location.Column;
                    string eltipo = tipo.ChildNodes.ElementAt(0).Token.Text;

                    /*El trucazo de los parametros*/
                    listaParam = new LinkedList<parametroCustom>();
                    valorReferencia preparacionParam = new valorReferencia(noterminales.VALOR_REFERENCIA, node.ChildNodes.ElementAt(3));
                    preparacionParam.nuevaTraduccion(listaParam);
                    //TEMP REPRESENTA LA ETIQUETA CON LA QUE SE GUERDARA EL RETORNO
                    simbolo nuevaFuncion = new simbolo(ambito, id.Token.Text, eltipo, temp, fila, columna, "procedimiento",listaParam);
                    tablaActual.agregarSimbolo(nuevaFuncion);
                    nuevaTabla = (tabla)tablaActual.Clone();

                    /*Declarar los parametros*/
                    argumento += "/*Inicia declaracion de parametros*/\n";
                    foreach (var a in listaParam)
                    {
                        string nuevoTemp = cosasGlobalesewe.nuevoTemp();

                    }




                }
            }

            return new resultado();
        }
    }
}
