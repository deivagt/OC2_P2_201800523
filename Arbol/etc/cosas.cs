﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;

using OC2_P2_201800523.AST;

namespace OC2_P2_201800523.Arbol.etc
{
    class cosas : nodo
    {
        
        public cosas(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public cosas(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            ParseTreeNode instr = node.ChildNodes.ElementAt(4);
            if (instr.ChildNodes.Count != 0)
            {
                LinkedList<instruccion> instruccionesjjjj = new LinkedList<instruccion>();
                instrucciones listaInstrucciones = new instrucciones(noterminales.INSTRUCCIONES, instr);
                listaInstrucciones.nuevaTraduccion(instruccionesjjjj);

                foreach (var instruccion in instruccionesjjjj)
                {
                    instruccion.traducir(ref tablaActual, ambito,verdadero,falso,xd);
                }

            }

            ParseTreeNode main = node.ChildNodes.ElementAt(5);
            sentencia.cuerpo_programa ejecucionMain = new sentencia.cuerpo_programa(noterminales.CUERPO_PROGRAMA, main);
            ejecucionMain.traducir(ref tablaActual, ambito, verdadero, falso, xd);



            return new resultado();
        }

        
    }
}
