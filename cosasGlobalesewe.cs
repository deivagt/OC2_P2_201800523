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
            args += declararVariablesInternas();
            args += cocinarWriteln();
            args += booleanoCadena();
            args += "void main()\n{\n";
            salida = args + salida;  
            salida += "}";
        }

        

        static string declararVariablesInternas()
        {
            int temp = 5;
            string salida = "double ";
            for (int i = 0; i < temp; i++)
            {
                salida += "N" + i + ", ";
            }
            salida = salida.Remove(salida.Length - 2);
            salida += ";\n";
            return salida;
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
        static string cocinarWriteln() //N0, N1, N2
        {
            string argumentos = "void imprimirLn(){\n"
            + "L0:\n"
            + "if(heap[(int)N0] != -1) goto L1;\n"
            + "goto L4;\n"
            + "L1:\n"
            + "if(heap[(int)N0]>=299) goto L2;\n"
            + "if(heap[(int)N0]<-1) goto L3;\n"
            + "printf(\"%c\", (char)heap[(int)N0]);\n"
            + "N0 = N0 + 1;\n"
            + "goto L0;\n"
            + "L2:\n"
            + "N1 = heap[(int)N0];\n"
            + "N2 = N1 - 300;\n"
            + "printf(\"%f\",N1);\n"
            + "N0 = N0 + 1;\n"
            + "goto L0;\n"
            + "L3:\n"
            + "N1 = heap[(int)N0];\n"
            + "printf(\"%f\",N1);\n"
            + "N0 = N0 + 1;\n"
            + "goto L0;\n"
            + "L4:\n"
            + "return;\n"
            + "}\n";
            return argumentos;
            
        }
        static string booleanoCadena() //N3
        {
            string argumento = "void booleanoCadena(){\n"
                + "L0:\n"
                + "if(N3 == 1) goto L1;\n"
                + "goto L2;\n"
                + "L1:\n"
                + "printf(\"%c\",(char)116);\n"
                + "printf(\"%c\",(char)114);\n"
                + "printf(\"%c\",(char)117);\n"
                + "printf(\"%c\",(char)101);\n"
                + "goto L3;\n"
                + "L2:\n"
                + "printf(\"%c\",(char)102);\n"
                + "printf(\"%c\",(char)97);\n"
                + "printf(\"%c\",(char)108);\n"
                + "printf(\"%c\",(char)115);\n"
                + "printf(\"%c\",(char)101);\n"
                + "L3:\n"
                + "return;\n"
                + "}\n";

            return argumento;
        }
    }
}
