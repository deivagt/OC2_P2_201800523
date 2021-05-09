using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using OC2_P2_201800523.tablaSimbolos;
using OC2_P2_201800523.AST;

namespace OC2_P2_201800523.optimizar
{
    class analizadorC3D

    {
        string aux = "";
        int cg = 0;
        public ParseTreeNode raiz;
        public void analisis(string entrada)
        {
            LinkedList<string> listaGlobal = new LinkedList<string>();
            LinkedList<string> ListaAnalisis = new LinkedList<string>();
            LinkedList<string> ListaSalida = new LinkedList<string>();

            Program.form.consola.Text = "";
            Program.form.richTextBox3.Text = "";
            Program.form.richTextBox4.Text = "";
            Program.form.consolaErrores.Text = "";
            Program.form.consolaOPT.Text = "";
            cosasGlobalesewe.inicializar();
            bool guardar = false;
            string auxGlobal = "";
            string auxAnalisis = "";
            int regla = 3;
            string auxiliar = "";
            string auxi = entrada;

            while (regla <= 15)
            {
                try
                {
                    listaGlobal = new LinkedList<string>();
                    ListaAnalisis = new LinkedList<string>();
                    ListaSalida = new LinkedList<string>();

                    foreach (char caracter in auxi)
                    {
                        if (caracter == '{')
                        {

                            guardar = true;
                            listaGlobal.AddLast(auxGlobal);
                            listaGlobal.AddLast("{\n");
                            auxGlobal = "";
                            continue;

                        }
                        else if (caracter == '}')
                        {
                            guardar = false;
                            ListaAnalisis.AddLast(auxAnalisis);
                            listaGlobal.AddLast("}\n");
                            auxAnalisis = "";
                            continue;
                        }

                        if (guardar == true)
                        {
                            auxAnalisis += caracter;
                        }
                        else
                        {
                            auxGlobal += caracter;
                        }
                    }

                    int contador = 0;
                    foreach (string ent in ListaAnalisis)
                    {
                        auxiliar = ent;
                        lex gram = new lex();
                        LanguageData leng = new LanguageData(gram);
                        Parser parser = new Parser(leng);
                        ParseTree arbol = parser.Parse(ent);
                        raiz = arbol.Root;

                        gram = new lex();
                        leng = new LanguageData(gram);
                        parser = new Parser(leng);
                        arbol = parser.Parse(auxiliar);
                        try
                        {
                            if (arbol.Root != null)
                            {

                                raiz = arbol.Root;
                                try
                                {
                                    cg = 0;
                                    recorrer(raiz, regla);
                                }
                                catch (StackOverflowException a)
                                {

                                }

                                System.Diagnostics.Debug.WriteLine(aux);
                                ListaSalida.AddLast(aux);
                                aux = "";

                            }

                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e);
                        }
                        contador++;
                    }

                    auxi = merge(listaGlobal, ListaSalida);
                    regla++;
                }
                catch (Exception e)
                {
                    break;
                }

            }

            Program.form.consola.Text = auxi;

            foreach (string sal in ListaSalida)
            {
                System.Diagnostics.Debug.WriteLine(sal);
            }



        }
        public void recorrer(ParseTreeNode nodo)
        {
            foreach (var hijo in nodo.ChildNodes)
            {
                System.Diagnostics.Debug.WriteLine(hijo.Term);
                recorrer(hijo);
            }

        }

        string merge(LinkedList<string> listaGlobal, LinkedList<string> listaSalida)
        {
            string salida = "";
            foreach (string lG in listaGlobal)
            {
                salida += lG;
                if (lG == "{\n")
                {
                    string last = listaSalida.First();
                    listaSalida.RemoveFirst();
                    salida += last;
                }
            }
            return salida;
        }

        public void recorrer(ParseTreeNode nodo, int regla)
        {

            bool control;


            foreach (var hijo in nodo.ChildNodes)
            {

                switch (regla)
                {
                    case 3:
                        if (hijo.ChildNodes.Count == 10)
                        {
                            if (hijo.ChildNodes.ElementAt(2).ChildNodes.Count == 3)
                            {
                                control = R3(hijo);
                                if (control == true)
                                {
                                    concatenar(hijo);
                                    recorrer(hijo, 3);
                                }
                            }
                            else
                            {
                                concatenar(hijo);
                                recorrer(hijo, 3);
                            }
                        }
                        else
                        {
                            concatenar(hijo);
                            recorrer(hijo, 3);
                        }
                        continue;
                    case 4:
                        if (hijo.ChildNodes.Count == 10)
                        {
                            if (hijo.ChildNodes.ElementAt(2).ChildNodes.Count == 3)
                            {
                                control = R4(hijo);
                                if (control == true)
                                {
                                    concatenar(hijo);
                                    recorrer(hijo, 4);
                                }
                            }
                            else
                            {
                                concatenar(hijo);
                                recorrer(hijo, 4);
                            }
                        }
                        else
                        {
                            concatenar(hijo);
                            recorrer(hijo, 4);
                        }
                        continue;
                    case 6:
                        if (hijo.ChildNodes.Count == 4)
                        {
                            if (hijo.ChildNodes.ElementAt(0).Term.ToString() == "EXPRESION")
                            {
                                string comp = hijo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Term.ToString();
                                if (comp == "id")
                                {
                                    comp = hijo.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).Term.ToString();
                                    if (comp == "EXPRESION")
                                    {
                                        control = R6(hijo);
                                        if (control == true)
                                        {
                                            concatenar(hijo);
                                            recorrer(hijo, 6);
                                        }

                                    }
                                    else
                                    {
                                        concatenar(hijo);
                                        recorrer(hijo, 6);
                                    }
                                }
                                else
                                {
                                    concatenar(hijo);
                                    recorrer(hijo, 6);
                                }
                            }
                            else
                            {
                                concatenar(hijo);
                                recorrer(hijo, 6);
                            }
                        }
                        else
                        {
                            concatenar(hijo);
                            recorrer(hijo, 6);
                        }


                        continue;
                    case 7:
                        if (hijo.ChildNodes.Count == 4)
                        {

                            if (hijo.ChildNodes.ElementAt(0).Term.ToString() == "EXPRESION")
                            {
                                string comp = hijo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Term.ToString();
                                if (comp == "id")
                                {
                                    comp = hijo.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).Term.ToString();
                                    if (comp == "EXPRESION")
                                    {
                                        control = R7(hijo);
                                        if (control == true)
                                        {
                                            concatenar(hijo);
                                            recorrer(hijo, 7);
                                        }

                                    }
                                    else
                                    {
                                        concatenar(hijo);
                                        recorrer(hijo, 7);
                                    }
                                }
                                else
                                {
                                    concatenar(hijo);
                                    recorrer(hijo, 7);
                                }
                            }
                            else
                            {
                                concatenar(hijo);
                                recorrer(hijo, 7);
                            }
                        }
                        else
                        {
                            concatenar(hijo);
                            recorrer(hijo, 7);
                        }


                        continue;
                    case 8:
                        if (hijo.ChildNodes.Count == 4)
                        {
                            if (hijo.ChildNodes.ElementAt(0).Term.ToString() == "EXPRESION")
                            {
                                string comp = hijo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Term.ToString();
                                if (comp == "id")
                                {
                                    comp = hijo.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).Term.ToString();
                                    if (comp == "EXPRESION")
                                    {
                                        control = R8(hijo);
                                        if (control == true)
                                        {
                                            concatenar(hijo);
                                            recorrer(hijo, 8);
                                        }

                                    }
                                    else
                                    {
                                        concatenar(hijo);
                                        recorrer(hijo, 8);
                                    }
                                }
                                else
                                {
                                    concatenar(hijo);
                                    recorrer(hijo, 8);
                                }
                            }
                            else
                            {
                                concatenar(hijo);
                                recorrer(hijo, 8);
                            }
                        }
                        else
                        {
                            concatenar(hijo);
                            recorrer(hijo, 8);
                        }


                        continue;
                    case 9:
                        if (hijo.ChildNodes.Count == 4)
                        {
                            if (hijo.ChildNodes.ElementAt(0).Term.ToString() == "EXPRESION")
                            {
                                string comp = hijo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Term.ToString();
                                if (comp == "id")
                                {
                                    comp = hijo.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).Term.ToString();
                                    if (comp == "EXPRESION")
                                    {
                                        control = R9(hijo);
                                        if (control == true)
                                        {
                                            concatenar(hijo);
                                            recorrer(hijo, 9);
                                        }

                                    }
                                    else
                                    {
                                        concatenar(hijo);
                                        recorrer(hijo, 9);
                                    }
                                }
                                else
                                {
                                    concatenar(hijo);
                                    recorrer(hijo, 9);
                                }
                            }
                            else
                            {
                                concatenar(hijo);
                                recorrer(hijo, 9);
                            }
                        }
                        else
                        {
                            concatenar(hijo);
                            recorrer(hijo, 9);
                        }


                        continue;

                    case 12:
                        if (hijo.ChildNodes.Count == 4)
                        {
                            if (hijo.ChildNodes.ElementAt(0).Term.ToString() == "EXPRESION")
                            {
                                string comp = hijo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Term.ToString();
                                if (comp == "id")
                                {
                                    comp = hijo.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).Term.ToString();
                                    if (comp == "EXPRESION")
                                    {
                                        control = R12(hijo);
                                        if (control == true)
                                        {
                                            concatenar(hijo);
                                            recorrer(hijo, 12);
                                        }
                                        else
                                        {

                                        }

                                    }
                                    else
                                    {
                                        concatenar(hijo);
                                        recorrer(hijo, 12);
                                    }
                                }
                                else
                                {
                                    concatenar(hijo);
                                    recorrer(hijo, 12);
                                }
                            }
                            else
                            {
                                concatenar(hijo);
                                recorrer(hijo, 12);
                            }
                        }
                        else
                        {
                            concatenar(hijo);
                            recorrer(hijo, 12);
                        }


                        continue;
                    case 13:
                        if (hijo.ChildNodes.Count == 4)
                        {
                            if (hijo.ChildNodes.ElementAt(0).Term.ToString() == "EXPRESION")
                            {
                                string comp = hijo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Term.ToString();
                                if (comp == "id")
                                {
                                    comp = hijo.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).Term.ToString();
                                    if (comp == "EXPRESION")
                                    {
                                        control = R13(hijo);
                                        if (control == true)
                                        {
                                            concatenar(hijo);
                                            recorrer(hijo, 13);
                                        }
                                        else
                                        {




                                        }

                                    }
                                    else
                                    {
                                        concatenar(hijo);
                                        recorrer(hijo, 13);
                                    }
                                }
                                else
                                {
                                    concatenar(hijo);
                                    recorrer(hijo, 13);
                                }
                            }
                            else
                            {
                                concatenar(hijo);
                                recorrer(hijo, 13);
                            }
                        }
                        else
                        {
                            concatenar(hijo);
                            recorrer(hijo, 13);
                        }


                        continue;
                    case 14:
                        if (hijo.ChildNodes.Count == 4)
                        {
                            if (hijo.ChildNodes.ElementAt(0).Term.ToString() == "EXPRESION")
                            {
                                string comp = hijo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Term.ToString();
                                if (comp == "id")
                                {
                                    comp = hijo.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).Term.ToString();
                                    if (comp == "EXPRESION")
                                    {
                                        control = R14(hijo);
                                        if (control == true)
                                        {
                                            concatenar(hijo);
                                            recorrer(hijo, 14);
                                        }
                                        else
                                        {

                                        }

                                    }
                                    else
                                    {
                                        concatenar(hijo);
                                        recorrer(hijo, 14);
                                    }
                                }
                                else
                                {
                                    concatenar(hijo);
                                    recorrer(hijo, 14);
                                }
                            }
                            else
                            {
                                concatenar(hijo);
                                recorrer(hijo, 14);
                            }
                        }
                        else
                        {
                            concatenar(hijo);
                            recorrer(hijo, 14);
                        }
                        continue;
                    case 15:
                        if (hijo.ChildNodes.Count == 4)
                        {
                            if (hijo.ChildNodes.ElementAt(0).Term.ToString() == "EXPRESION")
                            {
                                string comp = hijo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Term.ToString();
                                if (comp == "id")
                                {
                                    comp = hijo.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).Term.ToString();
                                    if (comp == "EXPRESION")
                                    {
                                        control = R15(hijo);
                                        if (control == true)
                                        {
                                            concatenar(hijo);
                                            recorrer(hijo, 15);
                                        }
                                        else
                                        {

                                        }

                                    }
                                    else
                                    {
                                        concatenar(hijo);
                                        recorrer(hijo, 15);
                                    }
                                }
                                else
                                {
                                    concatenar(hijo);
                                    recorrer(hijo, 15);
                                }
                            }
                            else
                            {
                                concatenar(hijo);
                                recorrer(hijo, 15);
                            }
                        }
                        else
                        {
                            concatenar(hijo);
                            recorrer(hijo, 15);
                        }
                        continue;
                    case 16:
                        if (hijo.ChildNodes.Count == 4)
                        {
                            if (hijo.ChildNodes.ElementAt(0).Term.ToString() == "EXPRESION")
                            {
                                string comp = hijo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Term.ToString();
                                if (comp == "id")
                                {
                                    comp = hijo.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).Term.ToString();
                                    if (comp == "EXPRESION")
                                    {
                                        control = R16(hijo);
                                        if (control == true)
                                        {
                                            concatenar(hijo);
                                            recorrer(hijo, 16);
                                        }
                                        else
                                        {

                                        }

                                    }
                                    else
                                    {
                                        concatenar(hijo);
                                        recorrer(hijo, 16);
                                    }
                                }
                                else
                                {
                                    concatenar(hijo);
                                    recorrer(hijo, 16);
                                }
                            }
                            else
                            {
                                concatenar(hijo);
                                recorrer(hijo, 16);
                            }
                        }
                        else
                        {
                            concatenar(hijo);
                            recorrer(hijo, 16);
                        }
                        continue;
                    default:
                        concatenar(hijo);
                        recorrer(hijo, regla);
                        continue;
                }
                cg++;
            }

        }
        void concatenar(ParseTreeNode nodo)
        {
            if (nodo.Token != null)
            {
                if (nodo.Token.Text != "")
                {
                    aux += nodo.Token.Text;
                }

                if (nodo.Token.Text == ";" || nodo.Token.Text == ":")
                {
                    aux += "\n";
                }
                if (nodo.Token.Text == ")" || nodo.Token.Text == "goto")
                {
                    aux += " ";
                }
            }
        }
        bool R3(ParseTreeNode nodo)
        {
            // rif + abrir_parentesis + EXPRESION + cerrar_parentesis + rgoto + id + punto_coma + rgoto + id + punto_coma

            ParseTreeNode operacion = nodo.ChildNodes.ElementAt(2);
            ParseTreeNode izq = operacion.ChildNodes.ElementAt(0);
            ParseTreeNode simbolo = operacion.ChildNodes.ElementAt(1);
            ParseTreeNode der = operacion.ChildNodes.ElementAt(2);

            ParseTreeNode etTrue = nodo.ChildNodes.ElementAt(5);
            ParseTreeNode etFalse = nodo.ChildNodes.ElementAt(8);
            if (izq.ChildNodes.ElementAt(0).Term.ToString() == "numero" && der.ChildNodes.ElementAt(0).Term.ToString() == "numero")
            {
                if (simbolo.ChildNodes.ElementAt(0).Token.Text == "==")
                {
                    if (izq.ChildNodes.ElementAt(0).Token.Text == der.ChildNodes.ElementAt(0).Token.Text)
                    {
                        Program.form.consolaOPT.Text += "Se aplicó la Regla 3\n";
                        Program.form.consolaOPT.Text += "Tipo de Optimización: Mirilla\n";
                        Program.form.consolaOPT.Text += "Código agredado: " + "\n";
                        Program.form.consolaOPT.Text += "Código eliminado: " + "if(" + izq.ChildNodes.ElementAt(0).Token.Text + "==" + der.ChildNodes.ElementAt(0).Token.Text + ") goto " + etTrue.Token.Text + "\n" + "goto " + etFalse.Token.Text + "\n";
                        
                        Program.form.consolaOPT.Text += "Fila: "+nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Line+"\n";
                        Program.form.consolaOPT.Text += "-----------------\n";
                        aux += "goto " + etTrue.Token.Text + ";\n";
                        return false;
                    }

                }
            }

            return true;

        }
        bool R4(ParseTreeNode nodo)
        {
            // rif + abrir_parentesis + EXPRESION + cerrar_parentesis + rgoto + id + punto_coma + rgoto + id + punto_coma

            ParseTreeNode operacion = nodo.ChildNodes.ElementAt(2);
            ParseTreeNode izq = operacion.ChildNodes.ElementAt(0);
            ParseTreeNode simbolo = operacion.ChildNodes.ElementAt(1);
            ParseTreeNode der = operacion.ChildNodes.ElementAt(2);

            ParseTreeNode etTrue = nodo.ChildNodes.ElementAt(5);
            ParseTreeNode etFalse = nodo.ChildNodes.ElementAt(8);
            if (izq.ChildNodes.ElementAt(0).Term.ToString() == "numero" && der.ChildNodes.ElementAt(0).Term.ToString() == "numero")
            {
                if (simbolo.ChildNodes.ElementAt(0).Token.Text == "==")
                {
                    if (izq.ChildNodes.ElementAt(0).Token.Text != der.ChildNodes.ElementAt(0).Token.Text)
                    {
                        Program.form.consolaOPT.Text += "Se aplicó la Regla 4\n";
                        Program.form.consolaOPT.Text += "Tipo de Optimización: Mirilla\n";
                        Program.form.consolaOPT.Text += "Código agredado: " + "\n";
                        Program.form.consolaOPT.Text += "Código eliminado: " + "if(" + izq.ChildNodes.ElementAt(0).Token.Text + "==" + der.ChildNodes.ElementAt(0).Token.Text + ") goto " + etTrue.Token.Text + "\n";
                        Program.form.consolaOPT.Text += "Fila: " + nodo.ChildNodes.ElementAt(0).Token.Location.Line + "\n";
                        Program.form.consolaOPT.Text += "-----------------\n";
                        aux += "goto " + etFalse.Token.Text + ";\n";
                        return false;
                    }
                }
            }

            return true;

        }
        bool R6(ParseTreeNode nodo)
        {
            ParseTreeNode id = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0);

            ParseTreeNode operacion = nodo.ChildNodes.ElementAt(2);
            ParseTreeNode izq = operacion.ChildNodes.ElementAt(0);
            ParseTreeNode simbolo = operacion.ChildNodes.ElementAt(1);
            ParseTreeNode der = operacion.ChildNodes.ElementAt(2);
            if (simbolo.ChildNodes.ElementAt(0).Token.Text == "+" && der.ChildNodes.ElementAt(0).Token.Text == "0")
            {
                if (id.Token.Text == izq.ChildNodes.ElementAt(0).Token.Text)
                {
                    Program.form.consolaOPT.Text += "Se aplicó la Regla 6\n";
                    Program.form.consolaOPT.Text += "Tipo de Optimización: Mirilla\n";
                    Program.form.consolaOPT.Text += "Código agredado: " + "\n";
                    Program.form.consolaOPT.Text += "Código eliminado: " + id.Token.Text + " = " + id.Token.Text + simbolo.ChildNodes.ElementAt(0).Token.Text + "0" + "\n";
                    Program.form.consolaOPT.Text += "Fila: " + id.Token.Location.Line + "\n";
                    Program.form.consolaOPT.Text += "-----------------\n";
                    return false;
                }
            }
            else if (simbolo.ChildNodes.ElementAt(0).Token.Text == "+" && izq.ChildNodes.ElementAt(0).Token.Text == "0")
            {
                if (id.Token.Text == der.ChildNodes.ElementAt(0).Token.Text)
                {
                    Program.form.consolaOPT.Text += "Se aplicó la Regla 6\n";
                    Program.form.consolaOPT.Text += "Tipo de Optimización: Mirilla\n";
                    Program.form.consolaOPT.Text += "Código agredado: " + "\n";
                    Program.form.consolaOPT.Text += "Código eliminado: " + id.Token.Text + " = " + "0" + simbolo.ChildNodes.ElementAt(0).Token.Text + id.Token.Text + "\n";
                    Program.form.consolaOPT.Text += "Fila: " + id.Token.Location.Line + "\n";
                    Program.form.consolaOPT.Text += "-----------------\n";
                    return false;
                }
            }

            return true;

        }

        bool R7(ParseTreeNode nodo)
        {
            ParseTreeNode id = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0);

            ParseTreeNode operacion = nodo.ChildNodes.ElementAt(2);
            ParseTreeNode izq = operacion.ChildNodes.ElementAt(0);
            ParseTreeNode simbolo = operacion.ChildNodes.ElementAt(1);
            ParseTreeNode der = operacion.ChildNodes.ElementAt(2);
            if (simbolo.ChildNodes.ElementAt(0).Token.Text == "-" && der.ChildNodes.ElementAt(0).Token.Text == "0")
            {
                if (id.Token.Text == izq.ChildNodes.ElementAt(0).Token.Text)
                {
                    Program.form.consolaOPT.Text += "Se aplicó la Regla 7\n";
                    Program.form.consolaOPT.Text += "Tipo de Optimización: Mirilla\n";
                    Program.form.consolaOPT.Text += "Código agredado: " + "\n";
                    Program.form.consolaOPT.Text += "Código eliminado: " + id.Token.Text + " = " + id.Token.Text + simbolo.ChildNodes.ElementAt(0).Token.Text + "0" + "\n";
                    Program.form.consolaOPT.Text += "Fila: " + id.Token.Location.Line + "\n";
                    Program.form.consolaOPT.Text += "-----------------\n";
                    return false;
                }


            }

            return true;

        }
        bool R8(ParseTreeNode nodo)
        {
            ParseTreeNode id = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0);

            ParseTreeNode operacion = nodo.ChildNodes.ElementAt(2);
            ParseTreeNode izq = operacion.ChildNodes.ElementAt(0);
            ParseTreeNode simbolo = operacion.ChildNodes.ElementAt(1);
            ParseTreeNode der = operacion.ChildNodes.ElementAt(2);
            if (simbolo.ChildNodes.ElementAt(0).Token.Text == "*" && der.ChildNodes.ElementAt(0).Token.Text == "1")
            {
                if (id.Token.Text == izq.ChildNodes.ElementAt(0).Token.Text)
                {
                    Program.form.consolaOPT.Text += "Se aplicó la Regla 8\n";
                    Program.form.consolaOPT.Text += "Tipo de Optimización: Mirilla\n";
                    Program.form.consolaOPT.Text += "Código agredado: " + "\n";
                    Program.form.consolaOPT.Text += "Código eliminado: " + id.Token.Text + " = " + id.Token.Text + simbolo.ChildNodes.ElementAt(0).Token.Text + "1" + "\n";
                    Program.form.consolaOPT.Text += "Fila: " + id.Token.Location.Line + "\n";
                    Program.form.consolaOPT.Text += "-----------------\n";
                    return false;
                }
            }
            else if (simbolo.ChildNodes.ElementAt(0).Token.Text == "*" && izq.ChildNodes.ElementAt(0).Token.Text == "1")
            {
                if (id.Token.Text == der.ChildNodes.ElementAt(0).Token.Text)
                {
                    Program.form.consolaOPT.Text += "Se aplicó la Regla 8\n";
                    Program.form.consolaOPT.Text += "Tipo de Optimización: Mirilla\n";
                    Program.form.consolaOPT.Text += "Código agredado: " + "\n";
                    Program.form.consolaOPT.Text += "Código eliminado: " + id.Token.Text + " = " + "1" + simbolo.ChildNodes.ElementAt(0).Token.Text + id.Token.Text + "\n";
                    Program.form.consolaOPT.Text += "Fila: " + id.Token.Location.Line + "\n";
                    Program.form.consolaOPT.Text += "-----------------\n";
                    return false;
                }
            }

            return true;

        }
        bool R9(ParseTreeNode nodo)
        {
            ParseTreeNode id = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0);

            ParseTreeNode operacion = nodo.ChildNodes.ElementAt(2);
            ParseTreeNode izq = operacion.ChildNodes.ElementAt(0);
            ParseTreeNode simbolo = operacion.ChildNodes.ElementAt(1);
            ParseTreeNode der = operacion.ChildNodes.ElementAt(2);
            if (simbolo.ChildNodes.ElementAt(0).Token.Text == "/" && der.ChildNodes.ElementAt(0).Token.Text == "1")
            {
                if (id.Token.Text == izq.ChildNodes.ElementAt(0).Token.Text)
                {
                    Program.form.consolaOPT.Text += "Se aplicó la Regla 9\n";
                    Program.form.consolaOPT.Text += "Tipo de Optimización: Mirilla\n";
                    Program.form.consolaOPT.Text += "Código agredado: " + "\n";
                    Program.form.consolaOPT.Text += "Código eliminado: " + id.Token.Text + " = " + id.Token.Text + simbolo.ChildNodes.ElementAt(0).Token.Text + "1" + "\n";
                    Program.form.consolaOPT.Text += "Fila: " + id.Token.Location.Line + "\n";
                    Program.form.consolaOPT.Text += "-----------------\n";
                    return false;
                }
            }

            return true;

        }
        bool R12(ParseTreeNode nodo)
        {
            ParseTreeNode id = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0);

            ParseTreeNode operacion = nodo.ChildNodes.ElementAt(2);
            ParseTreeNode izq = operacion.ChildNodes.ElementAt(0);
            ParseTreeNode simbolo = operacion.ChildNodes.ElementAt(1);
            ParseTreeNode der = operacion.ChildNodes.ElementAt(2);

            if (simbolo.ChildNodes.ElementAt(0).Token.Text == "*" && der.ChildNodes.ElementAt(0).Token.Text == "1")
            {
                ParseTreeNode idIzq = izq.ChildNodes.ElementAt(0);
                Program.form.consolaOPT.Text += "Se aplicó la Regla 12\n";
                Program.form.consolaOPT.Text += "Tipo de Optimización: Mirilla\n";
                Program.form.consolaOPT.Text += "Código agredado: " + id.Token.Text + " = " + idIzq.Token.Text + "\n";
                Program.form.consolaOPT.Text += "Código eliminado: " + id.Token.Text + " = " + idIzq.Token.Text + simbolo.ChildNodes.ElementAt(0).Token.Text + "1" + "\n";
                Program.form.consolaOPT.Text += "Fila: " + id.Token.Location.Line + "\n";
                Program.form.consolaOPT.Text += "-----------------\n";

                aux += id.Token.Text + " = " + idIzq.Token.Text + ";\n";


                return false;

            }
            else if (simbolo.ChildNodes.ElementAt(0).Token.Text == "*" && izq.ChildNodes.ElementAt(0).Token.Text == "1")
            {
                ParseTreeNode idDer = der.ChildNodes.ElementAt(0);
                Program.form.consolaOPT.Text += "Se aplicó la Regla 12\n";
                Program.form.consolaOPT.Text += "Tipo de Optimización: Mirilla\n";
                Program.form.consolaOPT.Text += "Código agredado: " + id.Token.Text + " = " + idDer.Token.Text + "\n";
                Program.form.consolaOPT.Text += "Código eliminado: " + id.Token.Text + " = " + "1" + simbolo.ChildNodes.ElementAt(0).Token.Text + idDer.Token.Text + "\n";
                Program.form.consolaOPT.Text += "Fila: " + id.Token.Location.Line + "\n";
                Program.form.consolaOPT.Text += "-----------------\n";
                aux += id.Token.Text + " = " + idDer.Token.Text + ";\n";
                return false;

            }

            return true;

        }

        bool R13(ParseTreeNode nodo)
        {
            ParseTreeNode id = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0);

            ParseTreeNode operacion = nodo.ChildNodes.ElementAt(2);
            ParseTreeNode izq = operacion.ChildNodes.ElementAt(0);
            ParseTreeNode simbolo = operacion.ChildNodes.ElementAt(1);
            ParseTreeNode der = operacion.ChildNodes.ElementAt(2);
            if (simbolo.ChildNodes.ElementAt(0).Token.Text == "/" && der.ChildNodes.ElementAt(0).Token.Text == "1")
            {
                ParseTreeNode idIzq = izq.ChildNodes.ElementAt(0);
                Program.form.consolaOPT.Text += "Se aplicó la Regla 13\n";
                Program.form.consolaOPT.Text += "Tipo de Optimización: Mirilla\n";
                Program.form.consolaOPT.Text += "Código agredado: " + id.Token.Text + " = " + idIzq.Token.Text + "\n";
                Program.form.consolaOPT.Text += "Código eliminado: " + id.Token.Text + " = " + idIzq.Token.Text + simbolo.ChildNodes.ElementAt(0).Token.Text + "1" + "\n";
                Program.form.consolaOPT.Text += "Fila: " + id.Token.Location.Line + "\n";
                Program.form.consolaOPT.Text += "-----------------\n";
                aux += id.Token.Text + " = " + idIzq.Token.Text + ";\n";
                return false;

            }

            return true;

        }

        bool R14(ParseTreeNode nodo)
        {
            ParseTreeNode id = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0);

            ParseTreeNode operacion = nodo.ChildNodes.ElementAt(2);
            ParseTreeNode izq = operacion.ChildNodes.ElementAt(0);
            ParseTreeNode simbolo = operacion.ChildNodes.ElementAt(1);
            ParseTreeNode der = operacion.ChildNodes.ElementAt(2);

            if (simbolo.ChildNodes.ElementAt(0).Token.Text == "*" && der.ChildNodes.ElementAt(0).Token.Text == "2")
            {
                ParseTreeNode idIzq = izq.ChildNodes.ElementAt(0);
                Program.form.consolaOPT.Text += "Se aplicó la Regla 14\n";
                Program.form.consolaOPT.Text += "Tipo de Optimización: Mirilla\n";
                Program.form.consolaOPT.Text += "Código agredado: " + id.Token.Text + " = " + idIzq.Token.Text + " + " + idIzq.Token.Text + "\n";
                Program.form.consolaOPT.Text += "Código eliminado: " + id.Token.Text + " = " + idIzq.Token.Text + simbolo.ChildNodes.ElementAt(0).Token.Text + "2" + "\n";
                Program.form.consolaOPT.Text += "Fila: " + id.Token.Location.Line + "\n";
                Program.form.consolaOPT.Text += "-----------------\n";

                aux += id.Token.Text + " = " + idIzq.Token.Text + " + " + idIzq.Token.Text + ";\n";


                return false;

            }
            else if (simbolo.ChildNodes.ElementAt(0).Token.Text == "*" && izq.ChildNodes.ElementAt(0).Token.Text == "2")
            {
                ParseTreeNode idDer = der.ChildNodes.ElementAt(0);
                Program.form.consolaOPT.Text += "Se aplicó la Regla 14\n";
                Program.form.consolaOPT.Text += "Tipo de Optimización: Mirilla\n";
                Program.form.consolaOPT.Text += "Código agredado: " + id.Token.Text + " = " + idDer.Token.Text + " + " + idDer.Token.Text + "\n";
                Program.form.consolaOPT.Text += "Código eliminado: " + id.Token.Text + " = " + "2" + simbolo.ChildNodes.ElementAt(0).Token.Text + idDer.Token.Text + "\n";
                Program.form.consolaOPT.Text += "Fila: " + id.Token.Location.Line + "\n";
                Program.form.consolaOPT.Text += "-----------------\n";
                aux += id.Token.Text + " = " + idDer.Token.Text + " + " + idDer.Token.Text + ";\n";
                return false;

            }

            return true;

        }
        bool R15(ParseTreeNode nodo)
        {
            ParseTreeNode id = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0);

            ParseTreeNode operacion = nodo.ChildNodes.ElementAt(2);
            ParseTreeNode izq = operacion.ChildNodes.ElementAt(0);
            ParseTreeNode simbolo = operacion.ChildNodes.ElementAt(1);
            ParseTreeNode der = operacion.ChildNodes.ElementAt(2);

            if (simbolo.ChildNodes.ElementAt(0).Token.Text == "*" && der.ChildNodes.ElementAt(0).Token.Text == "0")
            {
                ParseTreeNode idIzq = izq.ChildNodes.ElementAt(0);
                Program.form.consolaOPT.Text += "Se aplicó la Regla 15\n";
                Program.form.consolaOPT.Text += "Tipo de Optimización: Mirilla\n";
                Program.form.consolaOPT.Text += "Código agredado: " + id.Token.Text + " = " + "0" + "\n";
                Program.form.consolaOPT.Text += "Código eliminado: " + id.Token.Text + " = " + idIzq.Token.Text + simbolo.ChildNodes.ElementAt(0).Token.Text + "0" + "\n";
                Program.form.consolaOPT.Text += "Fila: " + id.Token.Location.Line + "\n";
                Program.form.consolaOPT.Text += "-----------------\n";

                aux += id.Token.Text + " = " + "0" + ";\n";


                return false;

            }
            else if (simbolo.ChildNodes.ElementAt(0).Token.Text == "*" && izq.ChildNodes.ElementAt(0).Token.Text == "0")
            {
                ParseTreeNode idDer = der.ChildNodes.ElementAt(0);
                Program.form.consolaOPT.Text += "Se aplicó la Regla 15\n";
                Program.form.consolaOPT.Text += "Tipo de Optimización: Mirilla\n";
                Program.form.consolaOPT.Text += "Código agredado: " + id.Token.Text + " = " + "0" + "\n";
                Program.form.consolaOPT.Text += "Código eliminado: " + id.Token.Text + " = " + "0" + simbolo.ChildNodes.ElementAt(0).Token.Text + idDer.Token.Text + "\n";
                Program.form.consolaOPT.Text += "Fila: " + id.Token.Location.Line + "\n";
                Program.form.consolaOPT.Text += "-----------------\n";
                aux += id.Token.Text + " = " + "0" + ";\n";
                return false;

            }

            return true;

        }

        bool R16(ParseTreeNode nodo)
        {
            ParseTreeNode id = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0);

            ParseTreeNode operacion = nodo.ChildNodes.ElementAt(2);
            ParseTreeNode izq = operacion.ChildNodes.ElementAt(0);
            ParseTreeNode simbolo = operacion.ChildNodes.ElementAt(1);
            ParseTreeNode der = operacion.ChildNodes.ElementAt(2);
            if (simbolo.ChildNodes.ElementAt(0).Token.Text == "/" && izq.ChildNodes.ElementAt(0).Token.Text == "0")
            {

                ParseTreeNode idDer = der.ChildNodes.ElementAt(0);
                Program.form.consolaOPT.Text += "Se aplicó la Regla 16\n";
                Program.form.consolaOPT.Text += "Tipo de Optimización: Mirilla\n";
                Program.form.consolaOPT.Text += "Código agredado: " + id.Token.Text + " = " + "0" + "\n";
                Program.form.consolaOPT.Text += "Código eliminado: " + id.Token.Text + " = " + "0" + simbolo.ChildNodes.ElementAt(0).Token.Text + idDer.Token.Text + "\n";
                Program.form.consolaOPT.Text += "Fila: " + id.Token.Location.Line + "\n";
                Program.form.consolaOPT.Text += "-----------------\n";
                aux += id.Token.Text + " = " + "0" + ";\n";
                return false;

            }

            return true;

        }

    }
}
