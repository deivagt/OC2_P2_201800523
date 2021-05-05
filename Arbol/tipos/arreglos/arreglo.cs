using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.AST;

namespace OC2_P2_201800523.Arbol.tipos.arreglos
{
    // id + igual + rarray + abrir_corchete + INDEXADO + cerrar_corchete + rof + TIPO + punto_coma + DECLTIPOS
    class arreglo : nodo
    {
        public arreglo(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public arreglo(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {
            
            LinkedList<index> listaIndex = new LinkedList<index>();
            indexado nuevaIndex = new indexado(noterminales.INDEXADO, node.ChildNodes.ElementAt(4));
            nuevaIndex.nuevaTraduccion(listaIndex);

            ParseTreeNode id = node.ChildNodes.ElementAt(0);
            string tipo = node.ChildNodes.ElementAt(7).ChildNodes.ElementAt(0).Token.Text;
            int fila = id.Token.Location.Line;
            int columna = id.Token.Location.Column;

            simbolo nuevotipo = new simbolo(ambito, id.Token.Text, tipo, fila+1, columna+1, "array", listaIndex, true);
            tablaActual.agregarSimbolo(nuevotipo);

            return new resultado();
        }

        
    }
}
