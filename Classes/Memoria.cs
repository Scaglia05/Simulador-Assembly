using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulador_Assembly_Final.Classes {
    public class Memoria {
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

        public List<string> MostrarEstadoMemoriaDados() {
            var resultado = new List<string>();

            if (memoria == null || !memoria.Any()) {
                resultado.Add("Memória vazia.");
                return resultado;
            }

            foreach (var par in memoria.OrderBy(e => e.Key)) {
                resultado.Add($"Endereço {par.Key:D4}: {par.Value:X2}");
            }

            return resultado;
        }
    }
}
