using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
        static LinkedList<string> funcionesC3D;

        public static void inicializar()
        {
            temp = 4;
            etiqueta = 0;
            controlBreak = false;
            controlContinue = false;
            controlExit = false;
            salida = "";
            Program.form.consola.Text = "";
            funcionesC3D = new LinkedList<string>();
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
            args = "#include <stdio.h>\n";
            args += "float heap[1000000]; \nfloat stack[16000]; \n";
            args += "float sp; //Puntero del stack\n";
            args += "float hp; //Puntero del heap\n";
            args += declararTemporales();
            args += cocinarWriteln();
            args += booleanoCadena();
            args += concatenacionUwU();
            args += cocinarFunciones();
            args += "void main()\n{\n";
            salida = args + salida;  
            salida += "return;\n}";
        }

        static string cocinarFunciones()
        {
            string retorno = "";
            foreach(var funcion in funcionesC3D)
            {
                retorno += funcion;
            }
            return retorno;
        }

        
        static string declararTemporales()
        {
           
            string t = "float ";
            for(int i = 0; i < temp; i++)
            {
                t += "t" + i + ", ";
            }
            t = t.Remove(t.Length - 2);
            t += ";\n";
            return t;
        }
        static string cocinarWriteln() //t0, t0, t2
        {
            string argumentos = "void imprimirLn(){\n"
            + "L0:\n"
            + "if(heap[(int)t0] != -1) goto L1;\n"
            + "goto L4;\n"
            + "L1:\n"
            + "if(heap[(int)t0]>=299) goto L2;\n"
            + "if(heap[(int)t0]<-1) goto L3;\n"
            + "printf(\"%c\", (char)heap[(int)t0]);\n"
            + "t0 = t0 + 1;\n"
            + "goto L0;\n"
            + "L2:\n"
            + "t0 = heap[(int)t0];\n"
            + "t2 = t0 - 300;\n"
            + "printf(\"%f\",t0);\n"
            + "t0 = t0 + 1;\n"
            + "goto L0;\n"
            + "L3:\n"
            + "t0 = heap[(int)t0];\n"
            + "printf(\"%f\",t0);\n"
            + "t0 = t0 + 1;\n"
            + "goto L0;\n"
            + "L4:\n"
            + "return;\n"
            + "}\n";
            return argumentos;
            
        }
        static string booleanoCadena() //t3
        {
            string argumento = "void booleanoCadena(){\n"
                + "L0:\n"
                + "if(t3 == 1) goto L1;\n"
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
        static string concatenacionUwU()
        {
            /*ANTES DE LLAMAR A ESTA FUNCION GUARDAR EN UN TEMPORAL EL INICIO DE LA NUEVA CADENA*/
            string argumento = "/*t0 y t2 indica un inicio de cadena*/\nvoid concatenacion(){\n"
                + "L0:\n"
                + "if(heap[(int)t0] == -1) goto L2;\n"
                + "goto L1;\n"
                + "L1:\n"
                + "t1 = heap[(int)t0];\n"
                + "/*Guardar Caracter*/\n"
                + "heap[(int)hp] = t1;\n"
                + "hp = hp + 1;\n"
                + "t0 = t0 + 1;\n"
                + "goto L0;\n"
                + "L2:\n"
                + "if(heap[(int)t2] == -1) goto L4;\n"
                + "goto L3;\n"
                + "L3:\n"
                + "t1 = heap[(int)t2];\n"
                + "/*Guardar Caracter*/\n"
                + "heap[(int)hp] = t1;\n"
                + "hp = hp + 1;\n"
                + "t2 = t2 + 1;\n"
                + "goto L1;\n"
                + "L4:\n"
                + "/*Finalizar Cadena*/\n"
                + "heap[(int)hp] = -1;\n"
                + "hp = hp + 1;\n"
                + "return;\n"
                + "}\n";
            return argumento;
        }
    }
}
