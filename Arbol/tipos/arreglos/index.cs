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
    class index
    {
        public int inicio;
        public int final;
        public index(int inicio, int final)
        {
            this.inicio = inicio;
            this.final = final;
        }
    }
}
