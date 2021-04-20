using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;
using Irony.Ast;

namespace OC2_P2_201800523.AST
{
    class Gramatica : Grammar
    {
        public Gramatica() : base(caseSensitive: false )
        {         
            #region Declaracion de Terminales
            IdentifierTerminal id = new IdentifierTerminal(terminales.id);
            NumberLiteral numero = new NumberLiteral(terminales.numero,NumberOptions.AllowSign);
            var program = ToTerm(terminales.program);
            var var = ToTerm(terminales.var);
            var rconst = ToTerm(terminales.rconst);
            var rtype = ToTerm(terminales.rtype);
            var function = ToTerm(terminales.function);
            var procedure = ToTerm(terminales.procedure);
            var begin = ToTerm(terminales.begin);
            var end = ToTerm(terminales.end);
            var not = ToTerm(terminales.not);
            var div = ToTerm(terminales.div);
            var mod = ToTerm(terminales.mod);
            var and = ToTerm(terminales.and);
            var or = ToTerm(terminales.or);
            var rin = ToTerm(terminales.rin);
            var rtrue = ToTerm(terminales.rtrue);
            var rfalse = ToTerm(terminales.rfalse);
            var cadena = new StringLiteral(terminales.cadena, "\'");
            var rarray = ToTerm(terminales.rarray);
            var rthen = ToTerm(terminales.rthen);
            var rif = ToTerm(terminales.rif);
            var relse = ToTerm(terminales.relse);
            var rcase = ToTerm(terminales.rcase);
            var rof = ToTerm(terminales.rof);
            var writeln = ToTerm(terminales.writeln);
            var rwhile = ToTerm(terminales.rwhile);
            var rdo = ToTerm(terminales.rdo);
            var rfor = ToTerm(terminales.rfor);
            var to = ToTerm(terminales.to);
            var repeat = ToTerm(terminales.repeat);
            var until = ToTerm(terminales.until);
            var rbreak = ToTerm(terminales.rbreak);
            var rcontinue = ToTerm(terminales.rcontinue);
            var robject = ToTerm(terminales.robject);
            var write = ToTerm(terminales.write);
            var exit = ToTerm(terminales.exit);
            var graficar_ts = ToTerm(terminales.graficar_ts);

            var comentarioUL = new CommentTerminal(terminales.comentarioUL, "//", new[] { "\n" });
            var comentarioMLTipo1 = new CommentTerminal(terminales.comentarioMLTipo1, "(*", new[] { "*)" });
            var comentarioMLTipo2 = new CommentTerminal(terminales.comentarioMLTipo2, "{", new[] { "}" });
            
            /*TIPOS*/
            var rstring = ToTerm(terminales.rstring);
            var rinteger = ToTerm(terminales.rinteger);
            var rreal = ToTerm(terminales.rreal);
            var rchar = ToTerm(terminales.rchar);
            var rboolean = ToTerm(terminales.rboolean);
            var rvoid = ToTerm(terminales.rvoid);

            /*SIMBOLOS*/
            var punto_coma = ToTerm(terminales.punto_coma);
            var punto = ToTerm(terminales.punto);
            var uses = ToTerm(terminales.uses);
            var coma = ToTerm(terminales.coma);
            var abrir_parentesis = ToTerm(terminales.abrir_parentesis);
            var cerrar_parentesis = ToTerm(terminales.cerrar_parentesis);
            var abrir_corchete = ToTerm(terminales.abrir_corchete);
            var cerrar_corchete = ToTerm(terminales.cerrar_corchete);
            var dos_puntos_igual = ToTerm(terminales.dos_puntos_igual);
            var dos_puntos = ToTerm(terminales.dos_puntos);
            var igual = ToTerm(terminales.igual);

            var menos = ToTerm(terminales.menos);
            var uminus = ToTerm(terminales.uminus);
            var mas = ToTerm(terminales.mas);
            var por = ToTerm(terminales.por);
            var barra_div = ToTerm(terminales.barra_div);
            var distinto = ToTerm(terminales.distinto);
            var menor = ToTerm(terminales.menor);
            var menor_igual = ToTerm(terminales.menor_igual);
            var mayor = ToTerm(terminales.mayor);
            var mayor_igual = ToTerm(terminales.mayor_igual);
            var dospunticos = ToTerm(terminales.dospunticos);


            #region Otros
            NonGrammarTerminals.Add(comentarioUL);
            NonGrammarTerminals.Add(comentarioMLTipo1);
            NonGrammarTerminals.Add(comentarioMLTipo2);
            #endregion


            #endregion

            #region No terminales
            NonTerminal INI = new NonTerminal(noterminales.INI);
            NonTerminal COSAS = new NonTerminal(noterminales.COSAS);
            NonTerminal USES = new NonTerminal(noterminales.USES);
            NonTerminal OTRO_USES = new NonTerminal(noterminales.OTRO_USES);

            NonTerminal CUERPO_PROGRAMA = new NonTerminal(noterminales.CUERPO_PROGRAMA);

            NonTerminal INSTRUCCIONES = new NonTerminal(noterminales.INSTRUCCIONES);
            NonTerminal INSTRUCCION = new NonTerminal(noterminales.INSTRUCCION);

            NonTerminal VARIABLE = new NonTerminal(noterminales.VARIABLE);
            NonTerminal OTRA_DECL_VARIABLE = new NonTerminal(noterminales.OTRA_DECL_VARIABLE);

            NonTerminal CONSTANTE = new NonTerminal(noterminales.CONSTANTE);
            NonTerminal OTRA_CONSTANTE = new NonTerminal(noterminales.OTRA_CONSTANTE);

            NonTerminal INDEXADO = new NonTerminal(noterminales.INDEXADO);

            NonTerminal DECLTIPOS = new NonTerminal(noterminales.DECLTIPOS);
            NonTerminal DECLVARIOST = new NonTerminal(noterminales.DECLVARIOST);

            NonTerminal TIPO = new NonTerminal(noterminales.TIPO);
            NonTerminal TRETORNO = new NonTerminal(noterminales.TRETORNO);

            NonTerminal FUNCION_O_PROCEDIMIENTO = new NonTerminal(noterminales.FUNCION_O_PROCEDIMIENTO);
            NonTerminal FUNCION = new NonTerminal(noterminales.FUNCION);
            NonTerminal OTRA_FUNCION = new NonTerminal(noterminales.OTRA_FUNCION);

            NonTerminal SENTENCIAS = new NonTerminal(noterminales.SENTENCIAS);
            NonTerminal SENTENCIA = new NonTerminal(noterminales.SENTENCIA);
            NonTerminal EXPRESION = new NonTerminal(noterminales.EXPRESION);

            NonTerminal CASOS = new NonTerminal(noterminales.CASOS);
            NonTerminal CASO = new NonTerminal(noterminales.CASO);

            NonTerminal PARAMETROS = new NonTerminal(noterminales.PARAMETROS);

            NonTerminal VALOR_REFERENCIA = new NonTerminal(noterminales.VALOR_REFERENCIA);
            NonTerminal VALOR = new NonTerminal(noterminales.VALOR);
            NonTerminal REFERENCIA = new NonTerminal(noterminales.REFERENCIA);

            NonTerminal PARAMETROSWRITELN = new NonTerminal(noterminales.PARAMETROSWRITELN);
            NonTerminal DECLARACIONATRIBUTOS = new NonTerminal(noterminales.DECLARACIONATRIBUTOS);
            NonTerminal OTRADECLARACIONATRIBUTOS = new NonTerminal(noterminales.OTRADECLARACIONATRIBUTOS);

            NonTerminal ELSEIF = new NonTerminal(noterminales.ELSEIF);
            NonTerminal DECLARARENFUNC = new NonTerminal(noterminales.DECLARARENFUNC);


            //Control


            // NonTerminal PARAMETROS = new NonTerminal(noterminales.PARAMETROS);

            #endregion

            #region DEFINICION OWO
            
            INI.Rule = COSAS
                ;

            COSAS.Rule = program + id + punto_coma +USES + INSTRUCCIONES + CUERPO_PROGRAMA
                ;
            COSAS.ErrorRule = SyntaxError + Eof;

            CUERPO_PROGRAMA.Rule = begin + SENTENCIAS + end + punto
                ;

            CUERPO_PROGRAMA.ErrorRule = SyntaxError + Eof
                ;

            USES.Rule = uses + id + punto_coma
                | uses + id + coma + OTRO_USES
                | Empty                
                ;

            OTRO_USES.Rule = id + coma + OTRO_USES
                | id + punto_coma
                ;

            INSTRUCCIONES.Rule = INSTRUCCIONES + INSTRUCCION
                | INSTRUCCION
                | Empty
                ;

            INSTRUCCION.Rule = FUNCION
                | var + VARIABLE
                | rtype + DECLTIPOS
                | rconst + CONSTANTE
                ;

            INSTRUCCION.ErrorRule = SyntaxError + punto_coma;

            CONSTANTE.Rule = id +dos_puntos + TIPO  + igual + EXPRESION + punto_coma + CONSTANTE
                | Empty
                ;

            CONSTANTE.ErrorRule = SyntaxError + punto_coma;

            VARIABLE.Rule = id + dos_puntos + TIPO + igual + EXPRESION + punto_coma + VARIABLE 
                | id + dos_puntos + TIPO + punto_coma + VARIABLE
                | OTRA_DECL_VARIABLE + dos_puntos + TIPO + punto_coma + VARIABLE         
                | Empty
                ;

            OTRA_DECL_VARIABLE.Rule = OTRA_DECL_VARIABLE + coma + id
                | id
                ;

            DECLTIPOS.Rule =
                 DECLVARIOST + igual + TIPO + punto_coma + DECLTIPOS
                | id + igual + TIPO + punto_coma + DECLTIPOS
                | id + igual + robject + var + DECLARACIONATRIBUTOS + end + punto_coma
                | id + igual + rarray + abrir_corchete + INDEXADO + cerrar_corchete + rof + TIPO + punto_coma + DECLTIPOS               
                | Empty
                ;

            DECLTIPOS.ErrorRule = SyntaxError + punto_coma
                ;

            DECLVARIOST.Rule = DECLVARIOST + coma + id
                | id
                ;

            DECLARACIONATRIBUTOS.Rule = id + dos_puntos + TIPO + punto_coma + DECLARACIONATRIBUTOS
                | OTRADECLARACIONATRIBUTOS + dos_puntos + TIPO + punto_coma + DECLARACIONATRIBUTOS
                | Empty                 
                ;

            OTRADECLARACIONATRIBUTOS.Rule = OTRADECLARACIONATRIBUTOS +coma + id
                | id
                ;

            INDEXADO.Rule = INDEXADO + coma + EXPRESION + dospunticos + EXPRESION
                | EXPRESION + dospunticos + EXPRESION
                ;

           

            FUNCION.Rule =
                 procedure + id + abrir_parentesis + cerrar_parentesis + punto_coma + DECLARARENFUNC + OTRA_FUNCION + begin + SENTENCIAS + end + punto_coma
                | procedure + id + abrir_parentesis + VALOR_REFERENCIA + cerrar_parentesis + punto_coma + DECLARARENFUNC + OTRA_FUNCION + begin + SENTENCIAS + end + punto_coma
                | function + id + abrir_parentesis + cerrar_parentesis + dos_puntos + TRETORNO + punto_coma + DECLARARENFUNC + OTRA_FUNCION + begin + SENTENCIAS + end + punto_coma
                | function + id + abrir_parentesis + VALOR_REFERENCIA + cerrar_parentesis + dos_puntos + TRETORNO + punto_coma  + DECLARARENFUNC + OTRA_FUNCION + begin + SENTENCIAS + end + punto_coma
                            
                ;
            OTRA_FUNCION.Rule = FUNCION
                | Empty
                ;

            DECLARARENFUNC.Rule = DECLARARENFUNC + var + VARIABLE
                | var + VARIABLE
                | Empty
                ;

            VALOR_REFERENCIA.Rule = var + REFERENCIA /*VALORES POR REFERENCIA*/
                | VALOR /*VALORES POR VALOR*/
                ;

            VALOR.Rule = id + dos_puntos + TIPO + punto_coma + VALOR_REFERENCIA
                | id + coma + VALOR
                | id + dos_puntos + TIPO                
                ;

            REFERENCIA.Rule = id + dos_puntos + TIPO + punto_coma + VALOR_REFERENCIA
                | id + coma + REFERENCIA
                | id + dos_puntos + TIPO
                ;


            

            SENTENCIAS.Rule = SENTENCIAS + SENTENCIA
                | SENTENCIA
                |Empty
                ;

            SENTENCIA.Rule = id + dos_puntos_igual + EXPRESION + punto_coma////
                | id + abrir_parentesis + cerrar_parentesis + punto_coma //LLAMADA
                | id + abrir_parentesis +PARAMETROS+ cerrar_parentesis + punto_coma //LLAMADA
                | rif + EXPRESION + rthen + begin + SENTENCIAS + end + punto_coma////
                | rif + EXPRESION + rthen + begin + SENTENCIAS + end + PreferShiftHere() + ELSEIF + punto_coma////
                | rcase + EXPRESION + rof + CASOS + end + punto_coma
                | rcase + EXPRESION + rof + CASOS +  PreferShiftHere() + relse + begin + SENTENCIAS + end + punto_coma + end + punto_coma
                | rwhile + EXPRESION + rdo + begin + SENTENCIAS + end + punto_coma////
                | rfor + id + dos_puntos_igual + EXPRESION + to + EXPRESION + rdo + begin + SENTENCIAS + end + punto_coma
                | repeat + begin + SENTENCIAS + end + until + EXPRESION + punto_coma
                | repeat + SENTENCIAS + until + EXPRESION + punto_coma
                | writeln  + punto_coma
                | write + punto_coma
                | write + abrir_parentesis + PARAMETROSWRITELN + cerrar_parentesis + punto_coma////
                | writeln + abrir_parentesis+ PARAMETROSWRITELN + cerrar_parentesis + punto_coma////
                | rbreak + punto_coma
                | rcontinue + punto_coma
                | exit + abrir_parentesis + EXPRESION + cerrar_parentesis + punto_coma
                | graficar_ts + abrir_parentesis+ cerrar_parentesis + punto_coma
                ;

            ELSEIF.Rule = relse + rif + EXPRESION + rthen + begin + SENTENCIAS + end  + ELSEIF
                | relse + begin + SENTENCIAS + end
                | Empty
                ;

            SENTENCIA.ErrorRule = SyntaxError+ punto_coma
                ;

            PARAMETROSWRITELN.Rule = PARAMETROSWRITELN + coma + EXPRESION
                | EXPRESION
                ;

            CASOS.Rule = CASOS + CASO
                | CASO
                ;

            CASO.Rule = EXPRESION + dos_puntos + begin + SENTENCIAS + end + punto_coma
                ;            

            EXPRESION.Rule = uminus + EXPRESION + PreferShiftHere()
                | not + EXPRESION
                | EXPRESION + por + EXPRESION
                | EXPRESION + div + EXPRESION
                | EXPRESION + barra_div + EXPRESION
                | EXPRESION + mod + EXPRESION
                | EXPRESION + and + EXPRESION 
                | EXPRESION + mas + EXPRESION
                | EXPRESION + menos + EXPRESION
                | EXPRESION + or + EXPRESION 
                | EXPRESION + igual + EXPRESION 
                | EXPRESION + distinto + EXPRESION
                | EXPRESION + menor + EXPRESION 
                | EXPRESION + menor_igual + EXPRESION 
                | EXPRESION + mayor +EXPRESION 
                | EXPRESION + mayor_igual + EXPRESION 
                | id                
                | id + PreferShiftHere() +abrir_parentesis  + cerrar_parentesis//
                | numero
                | cadena                
                | rtrue
                | rfalse
                | abrir_parentesis + EXPRESION + cerrar_parentesis                
                ;           

            PARAMETROS.Rule = PARAMETROS + coma + EXPRESION
                | EXPRESION
                ;

            TIPO.Rule = rstring
                | rinteger
                | rreal
                | rchar
                | rboolean
                | id
                ;

            TRETORNO.Rule = rstring
                | rinteger
                | rreal
                | rchar
                | rboolean
                | rvoid
                | id
                ;

           
            #endregion            
            this.Root = INI;
            this.RegisterOperators(5, Associativity.Left, terminales.uminus);
            this.RegisterOperators(4, Associativity.Left, terminales.not);
            this.RegisterOperators(3, Associativity.Left,terminales.por, terminales.barra_div, terminales.div, terminales.mod, terminales.and);
            this.RegisterOperators(2, Associativity.Left,terminales.mas, terminales.menos, terminales.or);
            this.RegisterOperators(1, Associativity.Left,terminales.distinto, terminales.menor, terminales.menor_igual,
                terminales.mayor, terminales.mayor_igual, terminales.rin, terminales.igual);
            this.RegisterOperators(0, Associativity.Left, terminales.igual);
            AddToNoReportGroup(punto_coma);
        }
    }
}
