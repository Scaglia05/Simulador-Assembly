using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulador_Assembly_Final.Classes {
    public class MemoriaInstrucao {
        private readonly Dictionary<int, byte> memoria = new();
        public List<LinhaExecutada> LinhasExecutadas { get; set; } = new();


        public int LerPalavra(int endereco) {
            byte[] bytes = new byte[4];
            for (int i = 0; i < 4; i++) {
                bytes[i] = memoria.TryGetValue(endereco + i, out var b) ? b : (byte)0;
            }
            return BitConverter.ToInt32(bytes, 0);
        }

        public void EscreverPalavra(int endereco, int valor) {
            byte[] bytes = BitConverter.GetBytes(valor);
            for (int i = 0; i < 4; i++) {
                memoria[endereco + i] = bytes[i];
            }
        }

        public short LerMeiaPalavra(int endereco) {
            byte[] bytes = new byte[2];
            for (int i = 0; i < 2; i++) {
                bytes[i] = memoria.TryGetValue(endereco + i, out var b) ? b : (byte)0;
            }
            return BitConverter.ToInt16(bytes, 0);
        }

        public void EscreverMeiaPalavra(int endereco, int valor) {
            byte[] bytes = BitConverter.GetBytes((short)valor);
            for (int i = 0; i < 2; i++) {
                memoria[endereco + i] = bytes[i];
            }
        }

        public byte LerByte(int endereco) {
            return memoria.TryGetValue(endereco, out var b) ? b : (byte)0;
        }

        public void EscreverByte(int endereco, int valor) {
            memoria[endereco] = (byte)valor;
        }

        public List<string> ObterEstadoMemoriaDados() {
            var resultado = new List<string>();

            if (memoria == null || !memoria.Any())
                return resultado;

            foreach (var grupo in memoria.OrderBy(p => p.Key).GroupBy(p => p.Key / 4)) {
                int enderecoBase = grupo.First().Key / 4 * 4;
                byte[] bytes = new byte[4];

                for (int i = 0; i < 4; i++) {
                    memoria.TryGetValue(enderecoBase + i, out bytes[i]);
                }

                int palavra = BitConverter.ToInt32(bytes, 0);
                resultado.Add($"Endereço {enderecoBase:D4}: 0x{palavra:X8}");
            }

            return resultado;
        }
        public string ObterInstrucaoHexadecimal(string instrucao, int endereco) {
            Instrucoes instAssembly = new();
            if (string.IsNullOrEmpty(instrucao)) {
                return "  ";
            }
            var palavra = instAssembly.ObterHexadecimalLinha(instrucao);

            return $"Endereço {endereco:D4}: 0x{palavra:X8}";
        }
    }
}

