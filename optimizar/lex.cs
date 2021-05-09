using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;
using OC2_P2_201800523.AST;

namespace OC2_P2_201800523.optimizar
{
    class lex : Grammar
    {
        public lex() : base(caseSensitive: false)
        {

            /*Terminales*/

            IdentifierTerminal id = new IdentifierTerminal(terminales.id);
            NumberLiteral numero = new NumberLiteral(terminales.numero, NumberOptions.AllowSign);
            var igual = ToTerm(terminales.igual);
            var dosigual = ToTerm("==");
            var menos = ToTerm(terminales.menos);
            var uminus = ToTerm(terminales.uminus);
            var mas = ToTerm(terminales.mas);
            var por = ToTerm(terminales.por);
            var barra_div = ToTerm(terminales.barra_div);
            var distinto = ToTerm("!=");
            var menor = ToTerm(terminales.menor);
            var menor_igual = ToTerm(terminales.menor_igual);
            var mayor = ToTerm(terminales.mayor);
            var mayor_igual = ToTerm(terminales.mayor_igual);
            var cadena = new StringLiteral(terminales.cadena, "\"");
            var coma = ToTerm(terminales.coma);

            var punto_coma = ToTerm(terminales.punto_coma);

            var rif = ToTerm(terminales.rif);
            var dos_puntos = ToTerm(terminales.dos_puntos);
            var abrir_parentesis = ToTerm(terminales.abrir_parentesis);
            var cerrar_parentesis = ToTerm(terminales.cerrar_parentesis);
            var abrir_corchete = ToTerm(terminales.abrir_corchete);
            var cerrar_corchete = ToTerm(terminales.cerrar_corchete);
            var rint = ToTerm("int");
            var rgoto = ToTerm("goto");
            var rprint = ToTerm("print");
            var rprintf = ToTerm("printf");
            var comentarioUL = new CommentTerminal(terminales.comentarioUL, "//", new[] { "\n" });
            var comentarioMLTipo1 = new CommentTerminal(terminales.comentarioMLTipo1, "/*", new[] { "*/" });
            NonGrammarTerminals.Add(comentarioUL);
            NonGrammarTerminals.Add(comentarioMLTipo1);


            NonTerminal INI = new NonTerminal(noterminales.INI);
            NonTerminal COSAS = new NonTerminal(noterminales.COSAS);
            NonTerminal SIGNOS = new NonTerminal("SIGNOS");
            NonTerminal EXPRESION = new NonTerminal(noterminales.EXPRESION);
            NonTerminal PRINT = new NonTerminal("PRINT");
            NonTerminal INSTRUCCION = new NonTerminal(noterminales.INSTRUCCION);

            /*Leer C3D*/

            INI.Rule = COSAS
                ;

            COSAS.Rule = COSAS + INSTRUCCION
                | INSTRUCCION
                
                ;

            INSTRUCCION.Rule = EXPRESION + igual + EXPRESION + punto_coma
                | id + abrir_parentesis + cerrar_parentesis + punto_coma
                | rif + abrir_parentesis + EXPRESION + cerrar_parentesis + rgoto + id + punto_coma 
                | rif + abrir_parentesis + EXPRESION + cerrar_parentesis + rgoto + id + punto_coma + rgoto + id + punto_coma
                | rgoto + id + punto_coma//Llamada etiqueta
                | id + dos_puntos//Etiqueta
                | rprint + abrir_parentesis + PRINT + cerrar_parentesis + punto_coma
                | rprintf + abrir_parentesis + PRINT + cerrar_parentesis + punto_coma
                | id + punto_coma
                | punto_coma
                

                ;
            PRINT.Rule = cadena
             | cadena + coma + EXPRESION
             | cadena + coma + abrir_parentesis + id + cerrar_parentesis + EXPRESION
             ;


            EXPRESION.Rule = EXPRESION + SIGNOS + EXPRESION
                | id
                | numero
                | id + abrir_corchete + abrir_parentesis + id + cerrar_parentesis + EXPRESION + cerrar_corchete
                | uminus + EXPRESION + PreferShiftHere()
                ;

            SIGNOS.Rule = mas
                | menos
                | por
                | barra_div
                | distinto
                | menor_igual
                | mayor_igual
                | menor
                | mayor
                | dosigual
                ;








            this.Root = INI;

            this.RegisterOperators(5, Associativity.Left, terminales.uminus);
            this.RegisterOperators(4, Associativity.Left, terminales.not);
            this.RegisterOperators(3, Associativity.Left, terminales.por, terminales.barra_div, terminales.div, terminales.mod, terminales.and);
            this.RegisterOperators(2, Associativity.Left, terminales.mas, terminales.menos, terminales.or);
            this.RegisterOperators(1, Associativity.Left, terminales.distinto, terminales.menor, terminales.menor_igual,
                terminales.mayor, terminales.mayor_igual, terminales.rin, terminales.igual);
            this.RegisterOperators(0, Associativity.Left, terminales.igual);
            AddToNoReportGroup(punto_coma);
        }
    }
}
