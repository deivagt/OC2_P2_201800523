using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;
using Irony.Ast;
using OC2_P2_201800523.Arbol;
using OC2_P2_201800523.AST;
//using OC2_P1_201800523.Arbol;
using OC2_P2_201800523.tablaSimbolos;
namespace OC2_P2_201800523
{

    class manejadorArbol
    {
       
        public string ambitoActual = "global";
        static ParseTreeNode raiz;
        tabla tabladeSimbolos;

        public static int contadorNodos = 0;

        public void iniciar(ParseTreeNode nuevaRaiz)
        {

            manejadorArbol.raiz = nuevaRaiz;
            tabladeSimbolos = new tabla();
            ambitoActual = "global";

        }

        public void traducir()
        {            
            ini a = new ini(noterminales.INI, manejadorArbol.raiz);
            resultado salida = a.traducir(ref tabladeSimbolos, ambitoActual, "", "", "");

        }

        //public void imprimirTabla()
        //{
        //    Program.form.richTextBox3.Text = "";
        //    foreach (var simbolo in manejadorArbol.tabladeSimbolos.getTabla())
        //    {
        //        Program.form.richTextBox3.AppendText("Simbolo:\n     Ámbito: " + simbolo.ambito + "\n     Nombre: " + simbolo.id + "\n     Tipo: "
        //            + simbolo.tipo + "\n     Valor: " + simbolo.valor + "\n     Fila: " + simbolo.fila + "\n     Columna; " + simbolo.columna + "\n");
        //        Program.form.richTextBox3.AppendText("-------------------------\n");
        //    }
        //}

        public  string graficar(ParseTreeNode raiz)
        {
            Program.form.richTextBox4.Text = "";
            manejadorArbol.contadorNodos = 0;
            Program.form.richTextBox4.AppendText("digraph g{\n");
            Program.form.richTextBox4.AppendText(
                    "nodo" + manejadorArbol.contadorNodos + "[label=\"" + raiz.Term.Name + "\"];\n"
                    );

            int temp = manejadorArbol.contadorNodos;
            manejadorArbol.contadorNodos++;
            foreach (var a in raiz.ChildNodes)
            {
                if (a.Token == null)
                {
                    Program.form.richTextBox4.AppendText(
                   "nodo" + manejadorArbol.contadorNodos + "[label=\"" + a.Term.Name + "\"];\n"
                   );

                    Program.form.richTextBox4.AppendText(
                        "nodo" + (temp) + "->" + "nodo" + (manejadorArbol.contadorNodos) + ";\n"
                        );
                    manejadorArbol.contadorNodos++;

                }
                else
                {
                    Program.form.richTextBox4.AppendText(
                  "nodo" + manejadorArbol.contadorNodos + "[label=\"" + a.Token.Text + "\"];\n"
                  );

                    Program.form.richTextBox4.AppendText(
                        "nodo" + (temp) + "->" + "nodo" + (manejadorArbol.contadorNodos) + ";\n"
                        );
                    manejadorArbol.contadorNodos++;
                }
                gr(a);
            }

            Program.form.richTextBox4.AppendText("}");
            string salida = "";
            return salida;
        }

         void gr(ParseTreeNode raiz)
        {
            int temp = manejadorArbol.contadorNodos - 1;
            foreach (var a in raiz.ChildNodes)
            {
                if (a.Token == null)
                {
                    Program.form.richTextBox4.AppendText(
                   "nodo" + manejadorArbol.contadorNodos + "[label=\"" + a.Term.Name + "\"];\n"
                   );

                    Program.form.richTextBox4.AppendText(
                        "nodo" + (temp) + "->" + "nodo" + (manejadorArbol.contadorNodos) + ";\n"
                        );
                    manejadorArbol.contadorNodos++;

                }
                else
                {
                    Program.form.richTextBox4.AppendText(
                  "nodo" + manejadorArbol.contadorNodos + "[label=\"" + a.Token.Text + "\"];\n"
                  );

                    Program.form.richTextBox4.AppendText(
                        "nodo" + (temp) + "->" + "nodo" + (manejadorArbol.contadorNodos) + ";\n"
                        );
                    manejadorArbol.contadorNodos++;
                }
                gr(a);
            }

        }


    }

}
