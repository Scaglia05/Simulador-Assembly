using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Compilador_Assembly_Teste01.Classes;
using static Compilador_Assembly_Teste01.Classes.TabelaInstrucoes;

public class ResultadoSimulacao {
    public int TotalInstrucoes { get; set; }
    public int TotalCiclos { get; set; }
    public decimal TempoTotalSegundos { get; set; }
}

public class SimuladorService {
    public async Task<ResultadoSimulacao> ExecutarSimulacao(
        string filePath, decimal clockMHz, int ciclosR, int ciclosI, int ciclosJ) {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            throw new FileNotFoundException("Arquivo não encontrado.");

        if (clockMHz <= 0 || ciclosR <= 0 || ciclosI <= 0 || ciclosJ <= 0)
            throw new ArgumentException("Parâmetros inválidos.");

        // Calcula o tempo de um ciclo de clock em segundos
        decimal tempoClockUnicoSegundos = 1m / (clockMHz * 1_000_000m);

        // Zera totalizadores estáticos
        Totalizador.TotalInstrucoes = 0;
        Totalizador.TotalCiclos = 0;
        Totalizador.TempoTotalSegundos = 0;

        // Inicializa objetos do simulador
        MemoriaInstrucao memoriaInstrucao = new();
        Memoria memoria = new();
        var labels = new Dictionary<string, int>();
        var registradores = Registradores.CriarRegistradores();

        // Obtém o dicionário com os ciclos de cada instrução
        var ciclosInstrucoes = Compilador_Assembly_Teste01.Classes.Instrucoes.ParseWordsToArray(filePath, ciclosI, ciclosJ, ciclosR, tempoClockUnicoSegundos, memoriaInstrucao);

        var linhasPrograma = File.ReadAllLines(filePath);

        // Identifica labels
        for (int i = 0; i < linhasPrograma.Length; i++) {
            var linha = linhasPrograma[i].Trim();
            if (string.IsNullOrWhiteSpace(linha) || linha.StartsWith("#")) continue;

            if (linha.EndsWith(":")) {
                string nomeLabel = linha[..^1].Trim();
                labels[nomeLabel] = i;
            }
        }

        Instrucoes instrucoes = new();
        int pc = 0;
        while (pc < linhasPrograma.Length) {
            var linha = linhasPrograma[pc].Trim();
            if (string.IsNullOrWhiteSpace(linha) || linha.StartsWith("#") || linha.EndsWith(":")) {
                pc++;
                continue;
            }

            var (instrucao, operandos) = Compilador_Assembly_Teste01.Classes.Instrucoes.ParseInstrucao(linha);
            if (!string.IsNullOrEmpty(instrucao) && operandos != null) {
                Totalizador.TotalInstrucoes++;
                if (ciclosInstrucoes.TryGetValue(instrucao, out int ciclosInstrucao))
                    Totalizador.TotalCiclos += ciclosInstrucao;

                instrucoes.Executar(instrucao, operandos, registradores, memoria, labels, pc, ciclosInstrucoes, tempoClockUnicoSegundos, memoriaInstrucao);

                // Aguarda o tempo simulado da instrução (ajustável)
                if (TabelaInstrucoes.Instrucoes.TryGetValue(instrucao, out var dicInstrucoes)) {
                    int ciclos = dicInstrucoes.Item2;
                    decimal tempoInstrucaoSegundos = ciclos * tempoClockUnicoSegundos;
                    await Task.Delay((int)(tempoInstrucaoSegundos * 1000));
                }

                // Controle do PC para saltos
                if (instrucao.StartsWith("j")) {
                    string label = operandos.FirstOrDefault();
                    if (labels.ContainsKey(label))
                        pc = labels[label];
                    else
                        throw new Exception($"Label {label} não encontrada.");
                } else if (instrucao.StartsWith("b")) {
                    pc = registradores["PC"];
                } else {
                    pc++;
                }
            } else {
                throw new Exception($"Erro ao processar a linha: {linha}");
            }
        }

        Totalizador.TempoTotalSegundos = Totalizador.TotalCiclos * tempoClockUnicoSegundos;

        // Prepara o resultado para retorno
        var resultado = new ResultadoSimulacao {
            TotalInstrucoes = Totalizador.TotalInstrucoes,
            TotalCiclos = Totalizador.TotalCiclos,
            TempoTotalSegundos = Totalizador.TempoTotalSegundos
        };

        return resultado;
    }
}
