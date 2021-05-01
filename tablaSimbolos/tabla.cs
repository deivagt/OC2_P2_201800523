using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
//using OC2_P2_201800523.Arbol.Ejecucion.Funcion_Procedimiento;

namespace OC2_P2_201800523.tablaSimbolos 
{
    class tabla 
    {
        LinkedList<simbolo> tablaSimbolos;

        public tabla()
        {
            this.tablaSimbolos = new LinkedList<simbolo>();
        }

        public void agregarSimbolo(simbolo simbolo)
        {
            simbolo.id = simbolo.id.ToLower();
            this.tablaSimbolos.AddLast(simbolo);
        }

        public LinkedList<simbolo> getTabla()
        {
            return this.tablaSimbolos;
        }

        public string buscarDireccion(string id)
        {
            id = id.ToLower();
            foreach (var a in tablaSimbolos)
            {
                if (a.id == id)
                {
                    return a.direccion;
                }
            }
            return null;
        }

        public simbolo buscar(string id,string ambito)
        {
            id = id.ToLower();
            foreach (var a in tablaSimbolos)
            {
                if (a.id == id && a.ambito == ambito)
                {
                    return a;
                }
            }

            foreach (var a in tablaSimbolos)
            {
                if (a.id == id && a.ambito == "global")
                {
                    return a;
                }
            }
            return null;
        }

        public void restaurar (tabla tablaNueva)
        {
            for(int i = 0; i <= this.tablaSimbolos.Count - 1; i++)
            {
                if(this.tablaSimbolos.ElementAt(i).direccion != tablaNueva.tablaSimbolos.ElementAt(i).direccion)
                {
                    this.tablaSimbolos.ElementAt(i).direccion = tablaNueva.tablaSimbolos.ElementAt(i).direccion;
                }
            }
        }
        public tabla Clone()
        {
            LinkedList<simbolo> nuevosSimbolos = new LinkedList<simbolo>();
            tabla nuevaTabla = new tabla();
            foreach(var simbolo in this.tablaSimbolos)
            {
                nuevosSimbolos.AddLast(simbolo);
            }
            nuevaTabla.tablaSimbolos = nuevosSimbolos;
            return nuevaTabla;
        }

        public simbolo buscarFuncion(string id)
        {

            id = id.ToLower();
            foreach (var a in tablaSimbolos)
            {
                if (a.categoria == "funcion" || a.categoria == "procedimiento")
                {
                    if (a.id == id)
                    {
                        return a;
                    }
                }

            }
            return null;
        }

        public simbolo buscarTipo(string id)
        {

            id = id.ToLower();
            foreach (var a in tablaSimbolos)
            {
                if (a.esTipo == true)
                {
                    if (a.id == id)
                    {
                        return a;
                    }
                }

            }
            return null;
        }
        //public simbolo buscarSimbolo(string id, string ambito)
        //{

        //    id = id.ToLower();
        //    foreach (var a in tablaSimbolos)
        //    {
        //        if (a.categoria == "var" || a.categoria == "const")
        //        {
        //            if (a.id == id)
        //            {
        //                if (a.ambito == ambito || a.ambito == "global")
        //                {
        //                    return a;
        //                }

        //            }
        //        }

        //    }
        //    return null;
        //}
        //public simbolo buscarFuncion(string id)
        //{

        //    id = id.ToLower();
        //    foreach (var a in tablaSimbolos)
        //    {
        //        if (a.categoria == "func")
        //        {
        //            if (a.id == id)
        //            {
        //                return a;
        //            }
        //        }

        //    }
        //    return null;
        //}
    }
}
