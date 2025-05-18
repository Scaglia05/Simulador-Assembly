using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulador_Assembly_Final.Classes {
    public class TabelaInstrucoes {
        public Dictionary<string, int> registradores = new();

        public static Dictionary<string, (string, int)> Instrucoes = new()
        {
            { "add",  ("R", 0) }, { "sub",  ("R", 0) }, { "and",  ("R", 0) },
            { "or",   ("R", 0) }, { "nor",  ("R", 0) }, { "sll",  ("R", 0) },
            { "srl",  ("R", 0) }, { "jr",   ("R", 0) }, { "slt",  ("R", 0) },
            { "sltu", ("R", 0) }, { "addi", ("I", 0) }, { "andi", ("I", 0) },
            { "ori",  ("I", 0) }, { "lw",   ("I", 0) }, { "sw",   ("I", 0) },
            { "lh",   ("I", 0) }, { "sh",   ("I", 0) }, { "lb",   ("I", 0) },
            { "sb",   ("I", 0) }, { "beq",  ("I", 0) }, { "bne",  ("I", 0) },
            { "slti", ("I", 0) }, { "sltiu",("I", 0) }, { "j",    ("J", 0) },
            { "jal",  ("J", 0) }
        };

        public static class Registradores {
            public static Dictionary<string, int> CriarRegistradores() {
                return new Dictionary<string, int>
                {
                    { "$zero", 0 },
                    { "$v0", 0 }, { "$v1", 0 },
                    { "$a0", 0 }, { "$a1", 0 }, { "$a2", 0 }, { "$a3", 0 },
                    { "$t0", 0 }, { "$t1", 0 }, { "$t2", 0 }, { "$t3", 0 },
                    { "$t4", 0 }, { "$t5", 0 }, { "$t6", 0 }, { "$t7", 0 },
                    { "$s0", 0 }, { "$s1", 0 }, { "$s2", 0 }, { "$s3", 0 },
                    { "$s4", 0 }, { "$s5", 0 }, { "$s6", 0 }, { "$s7", 0 },
                    { "$t8", 0 }, { "$t9", 0 },
                    { "$gp", 0 }, { "$sp", 0 }, { "$fp", 0 }, { "$ra", 0 },
                    { "PC", 0 }
                };
            }
        }

        public static void AtualizarCiclos(int TipoR, int TipoI, int TipoJ) {
            // Atualiza os ciclos no dicionário para cada tipo de instrução
            foreach (var chave in Instrucoes.Keys.ToList()) {
                var tipoAtual = Instrucoes[chave].Item1;  // Tipo: "R", "I", "J"

                // Substitui o valor de ciclos com base no tipo
                if (tipoAtual == "R") {
                    Instrucoes[chave] = ("R", TipoR);  // Atualiza para o valor de ciclos para tipo "R"
                } else if (tipoAtual == "I") {
                    Instrucoes[chave] = ("I", TipoI);  // Atualiza para o valor de ciclos para tipo "I"
                } else if (tipoAtual == "J") {
                    Instrucoes[chave] = ("J", TipoJ);  // Atualiza para o valor de ciclos para tipo "J"
                }
            }
        }

    }

}
