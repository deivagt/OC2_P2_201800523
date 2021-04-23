using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
//using OC2_P2_201800523.Arbol.Ejecucion.Funcion_Procedimiento;

namespace OC2_P2_201800523.tablaSimbolos 
{
    class tabla :ICloneable
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

        public simbolo buscar(string id)
        {
            id = id.ToLower();
            foreach (var a in tablaSimbolos)
            {
                if (a.id == id)
                {
                    return a;
                }
            }
            return null;
        }
        public object Clone()
        {
            return MemberwiseClone();
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
