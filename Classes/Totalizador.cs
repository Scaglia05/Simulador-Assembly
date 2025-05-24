using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulador_Assembly_Final.Classes {
    public static class Totalizador {
        public static int TotalInstrucoes { get; set; }
        public static int TotalCiclos { get; set; } = 0;
        //public static double TempoTotalSegundos => TotalCiclos * ClockSegundos;
        public static double TempoTotalSegundos { get; set; }
        public static string TempoTotalSegundosTeste { get; set; }

        public static void Reset() {
            TotalInstrucoes = 0;
            TotalCiclos = 0;
        }

    }
}
