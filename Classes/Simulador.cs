using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulador_Assembly_Final.Classes {
    public class Simulador {
        public string FilePath { get; set; }
        public decimal ClockMHz { get; set; }
        public int CyclesR { get; set; }
        public int CyclesI { get; set; }
        public int CyclesJ { get; set; }

        // Tempo de um único ciclo de clock em segundos
        public decimal TempoClockUnicoSegundos => 1m / (ClockMHz * 1_000_000m);

        // Tempo total acumulado em segundos (você pode somar com base nos ciclos executados)
        public decimal ClockTotalSegundos { get; set; }

        public Dictionary<string, int> Labels { get; set; } = new();
        public Dictionary<string, int> Registradores { get; set; } = TabelaInstrucoes.Registradores.CriarRegistradores();
        public Memoria Memoria { get; set; } = new();


        public static void AguardarTempo(decimal tempoEmSegundos) {
            int milissegundos = (int)(tempoEmSegundos * 1000);
            if (milissegundos > 0) {
                Thread.Sleep(milissegundos);
            }
        }


        // Conversão de registradores do estilo $t0, $s1, etc. para seus números
        private static readonly Dictionary<string, int> MapaRegistradores = new()
        {
            { "$zero", 0 }, { "$at", 1 },
            { "$v0", 2 }, { "$v1", 3 },
            { "$a0", 4 }, { "$a1", 5 }, { "$a2", 6 }, { "$a3", 7 },
            { "$t0", 8 }, { "$t1", 9 }, { "$t2", 10 }, { "$t3", 11 }, { "$t4", 12 }, { "$t5", 13 }, { "$t6", 14 }, { "$t7", 15 },
            { "$s0", 16 }, { "$s1", 17 }, { "$s2", 18 }, { "$s3", 19 }, { "$s4", 20 }, { "$s5", 21 }, { "$s6", 22 }, { "$s7", 23 },
            { "$t8", 24 }, { "$t9", 25 },
            { "$k0", 26 }, { "$k1", 27 },
            { "$gp", 28 }, { "$sp", 29 }, { "$fp", 30 }, { "$ra", 31 }
        };


        public string ConverterInstrucaoParaBinario(string instrucao, List<string> operandos) {
            if (!TabelaInstrucoes.Instrucoes.TryGetValue(instrucao, out var tipoEOpCode))
                return "00000000000000000000000000000000"; // desconhecido

            var tipo = tipoEOpCode.Item1; // Tipo da instrução: R, I, J
            var opCode = Simulador.ObterOpcode(instrucao); // você pode armazenar os opcodes em TabelaInstrucoes

            string binario = tipo switch {
                "R" => MontarTipoR(instrucao, operandos),
                "I" => MontarTipoI(opCode, operandos),
                "J" => MontarTipoJ(opCode, operandos),
                _ => "00000000000000000000000000000000"
            };

            return binario;
        }

        public string ConverterInstrucaoParaHexadecimal(string instrucao, List<string> operandos) {
            string binario = ConverterInstrucaoParaBinario(instrucao, operandos);
            string hexadecimal = $"0x{Convert.ToUInt32(binario, 2):X8}";
            return hexadecimal;
        }

        private string MontarTipoR(string instrucao, List<string> operandos) {
            // Exemplo de campos típicos do tipo R
            string op = "000000"; // opcode fixo para tipo R
            int rs = MapaRegistradores.GetValueOrDefault(operandos.Count > 1 ? operandos[1] : "$zero", 0);
            int rt = MapaRegistradores.GetValueOrDefault(operandos.Count > 2 ? operandos[2] : "$zero", 0);
            int rd = MapaRegistradores.GetValueOrDefault(operandos[0], 0);
            int shamt = 0;
            int funct = Simulador.ObterFunct(instrucao); // Precisa de mapeamento

            return $"{op}{ToBinary(rs, 5)}{ToBinary(rt, 5)}{ToBinary(rd, 5)}{ToBinary(shamt, 5)}{ToBinary(funct, 6)}";
        }

        private string MontarTipoI(string opCode, List<string> operandos) {
            int opcode = Convert.ToInt32(opCode, 2);

            int rs = MapaRegistradores.GetValueOrDefault(operandos.Count > 1 ? operandos[1] : "$zero", 0);
            int rt = MapaRegistradores.GetValueOrDefault(operandos[0], 0);
            int immediate = int.TryParse(operandos.Count > 2 ? operandos[2] : "0", out var valor) ? valor : 0;

            return $"{ToBinary(opcode, 6)}{ToBinary(rs, 5)}{ToBinary(rt, 5)}{ToBinary(immediate, 16)}";
        }

        private string MontarTipoJ(string opCode, List<string> operandos) {
            int opcode = Convert.ToInt32(opCode, 2);
            int address = int.TryParse(operandos[0], out var addr) ? addr : 0;

            return $"{ToBinary(opcode, 6)}{ToBinary(address, 26)}";
        }

        private string ToBinary(int value, int bits) {
            return Convert.ToString(value & ((1 << bits) - 1), 2).PadLeft(bits, '0');
        }

        public static string ObterOpcode(string instrucao) => instrucao switch {
            "lw" => "100011",
            "sw" => "101011",
            "lh" => "100001",
            "sh" => "101001",
            "lb" => "100000",
            "sb" => "101000",
            "beq" => "000100",
            "bne" => "000101",
            "addi" => "001000",
            "andi" => "001100",
            "ori" => "001101",
            "slti" => "001010",
            "sltiu" => "001011",
            "j" => "000010",
            "jal" => "000011",
            _ => "000000"
        };

        public static int ObterFunct(string instrucao) => instrucao switch {
            "add" => 32,
            "sub" => 34,
            "and" => 36,
            "or" => 37,
            "nor" => 39,
            "slt" => 42,
            "sltu" => 43,
            "sll" => 0,
            "srl" => 2,
            "jr" => 8,
            _ => 0
        };

    }

}
