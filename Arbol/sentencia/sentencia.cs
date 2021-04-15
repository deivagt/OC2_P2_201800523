using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.Arbol.Expresion;
using OC2_P2_201800523.AST;
using System.Collections.Generic;
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

            switch (palabraClave.Token.Text)
            {
                case "if":
                    if (node.ChildNodes.Count == 7) // if normalito
                    {
                        cosasGlobalesewe.concatenarAccion("IF\n");
                        /*Condicion del if*/
                        ParseTreeNode expresion = node.ChildNodes.ElementAt(1);
                        expresion expr = new expresion(noterminales.EXPRESION, expresion);
                        res = expr.traducir(ref tablaActual, ambito,verdadero,falso,xd);
                        //if (res.getValor() == "true")
                        //{
                        //    hacerEjecucion(node.ChildNodes.ElementAt(4));
                        //}
                        //else

                        //{
                        //    if (res.getValor() != "false")
                        //    {

                        //    }
                        //    else
                        //    {
                        //        //ERROR
                        //    }
                        //}
                    }
                    else //if else
                    {
                        cosasGlobalesewe.concatenarAccion("IF else\n");
                        //ParseTreeNode expresion = node.ChildNodes.ElementAt(1);
                        //expresion expr = new expresion(noterminales.EXPRESION, expresion);
                        //res = expr.Ejecutar();
                        //if (res.getValor() == "true")
                        //{
                        //    hacerEjecucion(node.ChildNodes.ElementAt(4));
                        //}
                        //else if (res.getValor() == "false")
                        //{
                        //    ParseTreeNode elseif = node.ChildNodes.ElementAt(6);
                        //    condicion.IF siguienteelseif = new condicion.IF(noterminales.ELSEIF, elseif);
                        //    siguienteelseif.Ejecutar();
                        //}
                        //else
                        //{
                        //    //Error
                        //}
                    }

                    return new resultado();

                case "write":
                    ParseTreeNode paramWrite = node.ChildNodes.ElementAt(2);
                    funcBasica.write write = new funcBasica.write(noterminales.PARAMETROSWRITELN, paramWrite);
                    write.traducir(ref tablaActual, ambito, verdadero, falso, xd);
                    return new resultado();

                case "writeln":
                    ParseTreeNode paramWriteln = node.ChildNodes.ElementAt(2);
                    funcBasica.write writeln = new funcBasica.write(noterminales.PARAMETROSWRITELN, paramWriteln);
                    writeln.traducir(ref tablaActual, ambito, verdadero, falso, xd);
                    cosasGlobalesewe.concatenarAccion("printf(\"\\n\"); ");
                    return new resultado();
            }
            return new resultado();

        }
    }
}
