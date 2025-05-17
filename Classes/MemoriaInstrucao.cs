using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador_Assembly_Teste01.Classes {
    public class MemoriaInstrucao {
        private readonly Dictionary<int, byte> memoria = new();

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

        public void MostrarEstadoMemoriaDados() {
            if (memoria == null || !memoria.Any()) {
                Console.WriteLine("Memória vazia.");
                return;
            }

            foreach (var grupo in memoria.OrderBy(p => p.Key).GroupBy(p => p.Key / 4)) {
                int enderecoBase = grupo.First().Key / 4 * 4;
                byte[] bytes = new byte[4];

                for (int i = 0; i < 4; i++) {
                    memoria.TryGetValue(enderecoBase + i, out bytes[i]);
                }

                int palavra = BitConverter.ToInt32(bytes, 0);
                Console.WriteLine($"Endereço {enderecoBase:D4}: 0x{palavra:X8}");
            }
        }
    }
}

