using System;
using System.Collections.Generic;
using System.Text;

namespace OC2_P2_201800523
{
    class cosasGlobalesewe
    {
        public static int temp = 0;
        public static int etiqueta = 0;
        public static bool controlBreak = false;
        public static bool controlContinue = false;
        public static bool controlExit = false;
        public static string salida = "";
        static string args;

        public static void inicializar()
        {
            temp = 0;
            etiqueta = 0;
            controlBreak = false;
            controlContinue = false;
            controlExit = false;
            salida = "";
            Program.form.consola.Text = "";
        }
        public static string nuevoTemp(string argumento)
        {
            string temp1 = "t" + temp;
            temp++;
            temp1 += " = " + argumento + ";\n";
            salida += temp1;
            return "t" + (temp - 1);
        }
        public static string nuevoTemp()
        {
            string temp1 = "t" + temp;
            temp++;
            return temp1;
        }
        public static void concatenarAccion(string argumento)
        {
            salida += argumento + "\n";
        }

        public static string crearEtiqueta()
        {
            string etiqueta1 = "L" + etiqueta;
            etiqueta++;
            return etiqueta1;
        }

        public static void cocinar()
        {
            args = "double heap[1000000]; \ndouble stack[16000]; \n";
            args += "double sp; //Puntero del stack\n";
            args += "double hp; //Puntero del heap\n";
            args += declararTemporales();
            args += "main()\n{\n";
            salida = args + salida;  
            salida += "}";
        }


        static string declararTemporales()
        {
            string t = "double ";
            for(int i = 0; i < temp; i++)
            {
                t += "t" + i + ", ";
            }
            t = t.Remove(t.Length - 2);
            t += ";\n";
            return t;
        }
    }
}
