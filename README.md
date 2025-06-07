### 🚀 Simulador MIPS

Um simulador completo da arquitetura MIPS que permite interpretar, executar e analisar código assembly MIPS de forma detalhada, passo a passo, simulando o comportamento real da CPU, incluindo registradores, memória, saltos e cálculo de tempo baseado nos ciclos do clock.

---
### 📄 Formato do arquivo de código MIPS

O simulador recebe como entrada um arquivo texto com o código assembly MIPS.
Padrões e convenções:
Cada instrução deve estar em uma linha separada.
Comentários iniciam com # e são ignorados (tudo após # na linha).
Instruções e operandos separados por espaços ou tabs.
Operandos múltiplos separados por vírgulas , ou pontos ...
Endereços com offset usam o formato offset(base), ex: lw $t0, 4($sp).
Labels (rótulos) são linhas próprias, terminadas com :, usados para saltos.

``main:
    add $t0, $t1, $t2   # Soma $t1 e $t2 e guarda em $t0
    lw  $t3, 4($sp)     # Carrega da memória em $sp+4 para $t3
    beq $t0, $t3, end   # Se iguais, pula para end
    sub $t0, $t0, $t1
end:
    sw  $t0, 0($sp)``
    
---

### ⚙️ Funcionalidades

▶️ Execução passo a passo do código assembly MIPS
🔀 Suporte a labels e saltos condicionais/incondicionais (j, beq, bne, etc)
⏳ Simulação de ciclos de clock para cálculo de tempo realista
🧮 Visualização dinâmica dos registradores e memória
📝 Parsing robusto de instruções, operandos, comentários e labels
🔄 Avanço manual ou automático da execução
💾 Conversão de instruções para binário e hexadecimal para validação
📊 Monitoramento do estado da CPU e memória após cada instrução

---

###🛠️ Requisitos

.NET 6.0 ou superior
Visual Studio 2022 ou editor C# compatível
Arquivo teste com instruções
---
### 🚀 Como utilizar

Pré-requisitos
Ambiente .NET configurado (Visual Studio, .NET CLI)
Código MIPS em arquivo .asm seguindo o padrão
Passo a passo
Preparar o código MIPS
Escreva seu código assembly em um arquivo texto com o formato explicado.
Carregar código no simulador
Use o método ParseWordsToArray para ler o arquivo e preparar a memória.
Executar instruções
Itere pela memória de instruções, executando linha a linha com o método Executar.
Monitorar estado
Após cada passo, visualize registradores, memória, instrução em binário/hex e tempo acumulado.

Exemplo básico (C#):

``Memoria memoria = new Memoria();
MemoriaInstrucao memoriaInstrucao = new MemoriaInstrucao();
var registradores = InicializarRegistradores();
var labels = CarregarLabels(codigoAssembly);
double tempoClock = 1.0 / 1_000_000; // 1 MHz = 1 microssegundo por ciclo
var instrucoesECiclos = ParseWordsToArray("programa.asm", TipoI, TipoJ, TipoR, tempoClock, memoriaInstrucao);
int pc = 0;
bool executar = true;
while (executar) {
    var (instrucao, operandos) = ParseInstrucao(ObterLinhaDaMemoria(memoriaInstrucao, pc));
    Executar(instrucao, operandos, registradores, memoria, labels, pc, ciclosInstrucoes, tempoClock, memoriaInstrucao, simuladorObj, false);
    pc = registradores["PC"];
    if (pc >= fimDoPrograma) executar = false;
}``

---

### 📂 Inserção manual x leitura de arquivo

Inserção manual: Pode-se passar o código diretamente como string, linha a linha, respeitando o formato.
Leitura do arquivo: Processa o arquivo linha a linha, ignorando comentários e vazios, separando instruções e operandos para execução.

---

###🧩 Como o parser interpreta as instruções 

Remove comentários (#)
Divide linha em instrução e operandos
Separa operandos por vírgula ou ponto
Trata operandos com offset e base (offset(base))
Labels são identificados e armazenados para controle de saltos

---

### 🏗️ Como foi implementado?

Arquitetura Geral
Parser: interpreta o código MIPS em instruções e operandos
Simulador: executa as instruções e atualiza o estado da CPU e memória
Tabela de Instruções: contém ciclos e características de cada instrução
Memória: abstração para leitura e escrita simuladas
Controladores: contabilizam ciclos, instruções e tempo de execução
Métodos principais
Executar: executa lógica da instrução e atualiza o estado
ParseInstrucao: extrai instrução e operandos do texto
ParseWordsToArray: carrega e prepara a memória do programa
Detalhes
Suporte a instruções R, I e J (add, sub, lw, sw, beq, j, etc)
Atualização correta do PC, incluindo saltos e desvios
Contabilização dos ciclos por instrução e cálculo do tempo total
Tratamento de exceções para instruções inválidas

---

### 🧑‍💻 Como usar
Passos rápidos
Clone o repositório
Abra o projeto em Visual Studio ou editor preferido
Prepare seu código assembly seguindo o padrão
Execute o simulador e avance passo a passo
Observe registradores, memória, PC e tempo

---
### ⚠️ Dicas importantes e cuidados

Use instruções MIPS válidas e operandos corretos
Declare todos os labels usados
Evite acessar endereços de memória não inicializados
Comentários devem iniciar com # e estar fora da instrução

---

### 🤝 Contribuições

Contribuições são bem-vindas!
Faça fork do projeto
Crie uma branch para sua feature: git checkout -b minha-feature
Faça commit das alterações: git commit -m 'Minha feature'
Envie para o repositório: git push origin minha-feature
Abra um Pull Request
