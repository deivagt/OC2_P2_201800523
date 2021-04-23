using System;
using System.Collections.Generic;
using System.Text;

namespace OC2_P2_201800523.tablaSimbolos
{
    class parametroCustom
    {
        public string id;
        public bool porValor;
        public string tipo;

        public parametroCustom(string id, bool porValor, string tipo)
        {
            this.id = id;
            this.porValor = porValor;
            this.tipo = tipo;
        }
    }
}
