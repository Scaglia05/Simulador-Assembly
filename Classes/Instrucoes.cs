using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Simulador_Assembly_Final.Classes {
    public class Instrucoes {

        public static Dictionary<string, int> ParseWordsToArray(string filePath, int TipoI, int TipoJ, int TipoR, decimal tempoClockUnicoSegundos, MemoriaInstrucao memoriaInstrucao) {
            var resultado = new Dictionary<string, int>();
            var linhas = File.ReadAllLines(filePath);
            int endereco = 0;

            // Atualizar os ciclos antes de processar as instruções
            TabelaInstrucoes.AtualizarCiclos(TipoR, TipoI, TipoJ);
            Simulador simulador = new Simulador();

            foreach (var linha in linhas) {
                string linhaLimpa = linha.Split('#')[0].Trim(); // Remove comentários

                if (string.IsNullOrEmpty(linhaLimpa))
                    continue;

                var (instrucao, operandos) = ParseInstrucao(linhaLimpa); // Usa o parser robusto
                                                                         // Converte a instrução para hexadecimal
                string conversaoHexa = simulador.ConverterInstrucaoParaHexadecimal(instrucao, operandos);

                // Converte o hexadecimal string (ex: "0x20100001") para int
                int valorDecimal = Convert.ToInt32(conversaoHexa, 16);

                // Escreve na memória de instruções como uma palavra (4 bytes)
                memoriaInstrucao.EscreverPalavra(endereco, valorDecimal);

                endereco += 4; // próximo endereço

                if (string.IsNullOrEmpty(instrucao))
                    continue;

                // Verifica o tipo da instrução
                if (TabelaInstrucoes.Instrucoes.TryGetValue(instrucao, out var tipo)) {
                    // Ajuste no cálculo dos ciclos usando os valores atualizados
                    int ciclos = tipo.Item2;  // Agora, tipo.Item2 tem o número de ciclos correto (atualizado)

                    // Formatar a chave com a instrução e operandos
                    string chaveFormatada = $"{instrucao} ({string.Join(", ", operandos)})";

                    // Adicionar ao dicionário
                    resultado[$"{linhaLimpa} ==> {chaveFormatada}"] = ciclos;

                    //// Aguarda o tempo equivalente aos ciclos (tempoClockUnicoSegundos é o tempo por ciclo)
                    //int delayMs = (int)(tempoClockUnicoSegundos * 1000m * ciclos);  // Multiplica pelo número de ciclos
                    //if (delayMs > 0)
                    //    Thread.Sleep(delayMs);

                } else {
                    // Instrução não reconhecida
                    resultado[$"{linhaLimpa} ==> {linhaLimpa}"] = 1;
                }
            }

            return resultado;
        }


        public static (string instrucao, List<string> Operands) ParseInstrucao(string linha) {
            linha = linha.Split('#')[0].Trim();  // Etapa 1: Remover comentários
            if (string.IsNullOrWhiteSpace(linha))
                return (null, new List<string>());

            // Etapa 2: Separar a instrução e operandos
            string[] partes = linha.Split(new[] { ' ', '\t' }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length == 0)
                return (null, new List<string>());

            string instrucao = partes[0].ToLower();
            List<string> operands = new List<string>();

            if (partes.Length > 1) {
                // Etapa 3: Separar os operandos por vírgula ou ponto
                string[] brutos = partes[1].Split(new[] { ',', '.' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var bruto in brutos) {
                    string op = bruto.Trim();

                    // Etapa 4: Verificação para operandos entre parênteses
                    if (op.Contains('(') && op.Contains(')')) {
                        int indexAbre = op.IndexOf('(');
                        int indexFecha = op.IndexOf(')');

                        if (indexAbre >= 0 && indexFecha > indexAbre) {
                            string offset = op.Substring(0, indexAbre);  // Separar offset
                            string baseReg = op.Substring(indexAbre + 1, indexFecha - indexAbre - 1);  // Separar base

                            if (!string.IsNullOrWhiteSpace(offset))
                                operands.Add(offset);  // Adicionar offset

                            if (!string.IsNullOrWhiteSpace(baseReg))
                                operands.Add(baseReg);  // Adicionar base
                        } else {
                            operands.Add(op);  // Se não conseguir separar corretamente, adicionar o valor bruto
                        }
                    } else {
                        operands.Add(op);  // Adicionar o operando se não houver parênteses
                    }
                }
            }

            return (instrucao, operands);
        }


        public void Executar(string instrucao, List<string> Operands, Dictionary<string, int> registradores, Memoria memoria, Dictionary<string, int> labels, int pc, Dictionary<string, int> ciclosInstrucoes, decimal tempoClockUnicoSegundos, MemoriaInstrucao memoriaInstrucao) {
            // Totalização de instrução
            Totalizador.TotalInstrucoes++;
            Simulador simulador = new Simulador();

            // Verifica se a instrução existe no dicionário de ciclos e pega o número de ciclos
            if (TabelaInstrucoes.Instrucoes.TryGetValue(instrucao, out var tipoEValores)) {
                // O Item2 da tupla contém o número de ciclos
                int ciclos = tipoEValores.Item2;

                // Totaliza os ciclos
                Totalizador.TotalCiclos += ciclos;

                // Calcula o tempo gasto para a instrução (tempo por ciclo multiplicado pelos ciclos)
                decimal tempoInstrucaoSegundos = tempoClockUnicoSegundos * ciclos;

                // Atualiza o tempo total de execução
                Totalizador.TempoTotalSegundos += tempoInstrucaoSegundos;

                // Você pode imprimir ou monitorar o tempo total a cada execução, se necessário
                //Console.WriteLine($"Instrução: {instrucao}, Ciclos: {ciclos}, Tempo gasto: {tempoInstrucaoSegundos} segundos, Tempo Total: {Totalizador.TempoTotalSegundos} segundos");


                // Converte a instrução para binário e hexadecimal e exibe
                string instrucaoBinario = simulador.ConverterInstrucaoParaBinario(instrucao, Operands);
                string instrucaoHexadecimal = simulador.ConverterInstrucaoParaHexadecimal(instrucao, Operands);

                // Mostra o estado dos registradores e da memória para cada iteração
                MostrarEstadoRegistradoresEMemoria(registradores, memoria, instrucaoHexadecimal, memoriaInstrucao);
                //Console.WriteLine($"Instrução Binária: {instrucaoBinario}");
                //Console.WriteLine($"Instrução Hexadecimal: {instrucaoHexadecimal}");

            } else {
                //Console.WriteLine($"Aviso: Instrução '{instrucao}' não tem ciclo definido. Ignorada no totalizador.");
            }

            switch (instrucao) {
                case "add":
                    registradores[Operands[0]] = registradores[Operands[1]] + registradores[Operands[2]];
                    break;

                case "sub":
                    registradores[Operands[0]] = registradores[Operands[1]] - registradores[Operands[2]];
                    break;

                case "addi":
                    registradores[Operands[0]] = registradores[Operands[1]] + int.Parse(Operands[2]);
                    break;

                case "lw":
                    if (Operands.Count < 3)
                        throw new ArgumentException("Operandos insuficientes para 'lw'. Esperado: destino, offset, base");

                    string destino = Operands[0];
                    int offset = int.Parse(Operands[1]);
                    string baseReg = Operands[2];
                    int endereco = registradores[baseReg] + offset;

                    int valor = memoria.LerPalavra(endereco);
                    //Console.WriteLine($"[lw] Carregando memória[{endereco}] = {valor} em {destino}");

                    registradores[destino] = valor;
                    break;

                case "sw":
                    memoria.EscreverPalavra(registradores[Operands[2]] + int.Parse(Operands[1]), registradores[Operands[0]]);
                    break;

                case "lh":
                    registradores[Operands[0]] = memoria.LerMeiaPalavra(registradores[Operands[2]] + int.Parse(Operands[1]));
                    break;

                case "sh":
                    memoria.EscreverMeiaPalavra(registradores[Operands[2]] + int.Parse(Operands[1]), registradores[Operands[0]]);
                    break;

                case "lb":
                    registradores[Operands[0]] = memoria.LerByte(registradores[Operands[2]] + int.Parse(Operands[1]));
                    break;

                case "sb":
                    memoria.EscreverByte(registradores[Operands[2]] + int.Parse(Operands[1]), registradores[Operands[0]]);
                    break;

                case "and":
                    registradores[Operands[0]] = registradores[Operands[1]] & registradores[Operands[2]];
                    break;

                case "or":
                    registradores[Operands[0]] = registradores[Operands[1]] | registradores[Operands[2]];
                    break;

                case "nor":
                    registradores[Operands[0]] = ~(registradores[Operands[1]] | registradores[Operands[2]]);
                    break;

                case "andi":
                    registradores[Operands[0]] = registradores[Operands[1]] & int.Parse(Operands[2]);
                    break;

                case "ori":
                    registradores[Operands[0]] = registradores[Operands[1]] | int.Parse(Operands[2]);
                    break;

                case "sll":
                    registradores[Operands[0]] = registradores[Operands[1]] << int.Parse(Operands[2]);
                    break;

                case "srl":
                    registradores[Operands[0]] = (int)((uint)registradores[Operands[1]] >> int.Parse(Operands[2]));
                    break;

                case "beq":
                    if (registradores[Operands[0]] == registradores[Operands[1]]) {
                        registradores["PC"] = ProcessarSalto(Operands, registradores["PC"], labels);
                    } else {
                        // Caso contrário, o PC é incrementado normalmente
                        registradores["PC"] = ++pc;
                    }
                    break;

                case "bne":
                    if (registradores[Operands[0]] != registradores[Operands[1]]) {
                        // Se os registradores forem diferentes, chama o método para processar o salto
                        registradores["PC"] = ProcessarSalto(Operands, registradores["PC"], labels);
                    } else {
                        // Caso contrário, o PC é incrementado normalmente
                        registradores["PC"] = ++pc;
                    }
                    break;

                case "slt":
                    registradores[Operands[0]] = registradores[Operands[1]] < registradores[Operands[2]] ? 1 : 0;
                    break;

                case "sltu":
                    registradores[Operands[0]] = (uint)registradores[Operands[1]] < (uint)registradores[Operands[2]] ? 1 : 0;
                    break;

                case "slti":
                    registradores[Operands[0]] = registradores[Operands[1]] < int.Parse(Operands[2]) ? 1 : 0;
                    break;

                case "sltiu":
                    registradores[Operands[0]] = (uint)registradores[Operands[1]] < (uint)int.Parse(Operands[2]) ? 1 : 0;
                    break;

                case "j":
                    // Verifique se o rótulo existe no dicionário
                    if (labels.TryGetValue(Operands[0], out int linhaSalto)) {
                        registradores["PC"] = linhaSalto; // Atualiza o PC para a linha do rótulo
                    } else {
                        //Console.WriteLine($"Erro: Rótulo '{Operands[0]}' não encontrado.");
                    }
                    break;

                case "jr":
                    registradores["PC"] = registradores[Operands[0]];
                    break;

                case "jal":
                    registradores["$ra"] = registradores["PC"] + 4;
                    registradores["PC"] = int.Parse(Operands[0]);
                    break;

                default:
                    throw new Exception($"Instrução '{instrucao}' não reconhecida.");
            }
        }

        public static int ProcessarSalto(List<string> operandos, int pc, Dictionary<string, int> labels) {
            string label = operandos.LastOrDefault();  // Pega o último operando, que é a label
            if (labels.ContainsKey(label)) {
                return labels[label];  // Retorna o endereço da label de destino
            } else {
                //Console.WriteLine($"Erro: Label '{label}' não encontrada.");
                return pc + 4;  // Retorna o próximo endereço de PC (caso não encontre a label)
            }
        }


        // Método para mostrar o estado dos registradores e da memória
        public void MostrarEstadoRegistradoresEMemoria(Dictionary<string, int> registradores, Memoria memoria, string instrucaoHexadecimal, MemoriaInstrucao memoriaInstrucao) {
            //Console.WriteLine($"PC: {instrucaoHexadecimal}");

            //Console.WriteLine("Estado dos Registradores:");
            foreach (var registrador in registradores) {
                //Console.WriteLine($"  {registrador.Key}: {registrador.Value}");
            }

            //Console.WriteLine("Estado da Memória:");
            memoria.MostrarEstadoMemoriaDados();  // Mostra os primeiros 16 endereços de memória, você pode ajustar conforme necessário
            memoriaInstrucao.ObterEstadoMemoriaDados();

        }


        public string ObterHexadecimalLinha(string? linhaAtual) {

            if (linhaAtual is null)
                return string.Empty;

            string linhaLimpa = linhaAtual.Split('#')[0].Trim();
            if (string.IsNullOrWhiteSpace(linhaLimpa))
                return string.Empty;

            var (instrucao, operandos) = ParseInstrucao(linhaLimpa);
            Simulador simulador = new Simulador();
            string conversaoHexa = simulador.ConverterInstrucaoParaHexadecimal(instrucao, operandos);

            return conversaoHexa;
        }

    }
}
