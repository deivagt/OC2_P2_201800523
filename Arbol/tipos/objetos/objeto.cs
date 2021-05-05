using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.AST;
namespace OC2_P2_201800523.Arbol.tipos.objetos
{
    class objeto: nodo
    {
        public objeto(string tipo, string valor, ParseTreeNode node) : base(tipo, valor, node) { }
        public objeto(string tipo, ParseTreeNode node) : base(tipo, node) { }
        public override resultado traducir(ref tabla tablaActual, string ambito, string verdadero, string falso, string xd)
        {

            LinkedList<atributo> listaPos = new LinkedList<atributo>();
            ParseTreeNode auxi = node.ChildNodes.ElementAt(3);
            atributo atr = null;
            while (auxi != null)
            {
                if (auxi.ChildNodes.Count == 2)
                {
                    ParseTreeNode nuevoAtr = auxi.ChildNodes.ElementAt(1);
                    atr = new atributo(nuevoAtr.ChildNodes.ElementAt(1).Token.Text, nuevoAtr.ChildNodes.ElementAt(3).ChildNodes.ElementAt(0).Token.Text);
                    
                    listaPos.AddFirst(atr);
                    auxi = auxi.ChildNodes.ElementAt(0);
                }
                else if (auxi.ChildNodes.Count == 1)
                {
                    ParseTreeNode nuevoAtr = auxi.ChildNodes.ElementAt(0);
                    atr = new atributo(nuevoAtr.ChildNodes.ElementAt(1).Token.Text, nuevoAtr.ChildNodes.ElementAt(3).ChildNodes.ElementAt(0).Token.Text);

                    listaPos.AddFirst(atr);
                    auxi = null;
                }
            }

            ParseTreeNode id = node.ChildNodes.ElementAt(0);
            int fila = id.Token.Location.Line;
            int columna = id.Token.Location.Column;

            simbolo nuevotipo = new simbolo(ambito, id.Token.Text, "object", fila+1, columna+1, "objeto", listaPos, true);
            tablaActual.agregarSimbolo(nuevotipo);

            return new resultado();
        }
    }
}
