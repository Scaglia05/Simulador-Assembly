### 🚀 Simulador MIPS

Simulador completo da arquitetura MIPS que permite interpretar, executar e analisar código assembly MIPS detalhadamente. Executa passo a passo, simulando o comportamento real da CPU, incluindo registradores, memória, controle de saltos e cálculo do tempo baseado nos ciclos de clock.

---
### 📄 Formato do arquivo de código MIPS

O simulador aceita um arquivo texto contendo o código assembly MIPS seguindo as regras abaixo:

- Cada instrução deve estar em uma linha separada.
- Comentários iniciam com `#` e todo o conteúdo após `#` na linha é ignorado.
- Instruções e operandos são separados por espaços ou tabs.
- Operandos múltiplos são separados por vírgulas `,` ou pontos `...`.
- Endereços com offset devem usar o formato `offset(base)`, por exemplo: `lw $t0, 4($sp)`.
- Labels (rótulos) devem estar em linhas próprias, terminando com `:`, e são usados para saltos.

Exemplo de arquivo:

```asm
main:
    add $t0, $t1, $t2    # Soma $t1 e $t2 e guarda em $t0
    lw  $t3, 4($sp)      # Carrega da memória em $sp+4 para $t3
    beq $t0, $t3, end    # Se iguais, pula para end
    sub $t0, $t0, $t1
end:
    sw  $t0, 0($sp)
````

---

### ⚙️ Funcionalidades

▶️ **Execução passo a passo**  
Permite avançar a execução do código assembly MIPS linha a linha, facilitando o aprendizado e depuração.

🔀 **Suporte a labels e saltos**  
Trata corretamente instruções de desvio condicionais e incondicionais (`j`, `beq`, `bne`, etc.), controlando o fluxo do programa.

⏳ **Simulação realista de ciclos de clock**  
Cada instrução consome ciclos baseados na configuração, simulando o tempo real gasto no processador.

🧮 **Visualização dinâmica**  
Exibe em tempo real o conteúdo atualizado dos registradores e da memória durante a execução.

📝 **Parsing robusto**  
Interpreta corretamente instruções, operandos, comentários (`#`) e labels, garantindo o funcionamento adequado do simulador.

🔄 **Controle manual de execução**  
Usuário avança manualmente cada passo e aguarda a execução automática com delays configuráveis (Consumo de clock/Intrução).

💾 **Conversão de instruções**  
Converte as instruções para suas representações em binário e hexadecimal, facilitando análise e validação.

📊 **Monitoramento detalhado**  
Exibe o estado completo da CPU e memória após cada instrução, incluindo contadores de ciclos e tempo acumulado.

---

### 🛠️ Requisitos

Para executar o simulador MIPS, certifique-se de ter o seguinte ambiente configurado:

- **.NET 6.0** (ou superior) instalado no seu sistema.
- **Visual Studio 2022** (ou outro editor compatível com C#) para compilar e rodar o projeto.
- **Arquivo texto** contendo as instruções MIPS que serão simuladas, seguindo o formato padrão especificado.

---
### 🚀 Como Utilizar o Simulador MIPS

#### Pré-requisitos
- Ambiente .NET instalado e configurado (Visual Studio, .NET CLI ou similar).
- Arquivo texto contendo o código MIPS, formatado conforme o padrão esperado.

#### Passo a Passo para Uso

1. **Preparar o Código MIPS**  
   Crie um arquivo texto com seu código assembly MIPS. Respeite o formato padrão:  
   - Cada instrução em uma linha separada.  
   - Comentários iniciados com `#`.  
   - Labels terminam com `:` em linhas próprias.  
   - Opcionalmente, a primeira linha pode conter a configuração `Config_CPU` para definir clock e ciclos, caso não utilize inserção manual.

2. **Carregar o Código no Simulador**  
   Abra ou carregue o arquivo texto com o código. Se houver a linha `Config_CPU` no arquivo, o simulador lerá automaticamente as configurações. Caso contrário, insira os parâmetros manualmente via interface.

3. **Executar as Instruções**  
   Inicie a simulação e execute as instruções linha a linha, manualmente ou automaticamente.

4. **Monitorar o Estado do Simulador**  
   Após cada instrução, observe:  
   - Valores dos registradores.  
   - Conteúdo da memória.  
   - Instrução em binário e hexadecimal.  
   - Tempo acumulado baseado nos ciclos e clock configurados.

Este fluxo permite entender o funcionamento interno da CPU MIPS e depurar seu código assembly com facilidade.

---

### 📂 Inserção manual x leitura de arquivo

O simulador oferece duas formas para configurar a simulação:

Inserção Manual:
Quando ativada, libera campos para o usuário inserir parâmetros personalizados (clock, ciclos) diretamente na interface, dispensando a necessidade de configurações no arquivo.

Leitura de Arquivo:
Quando desativada, o simulador exige que a primeira linha do arquivo contenha uma configuração no formato Config_CPU, com parâmetros como frequência do clock e ciclos para tipos de instruções. 
Formato: 

``Config_CPU = [10ghz,i=1,j=1,r=1]``

Essa linha deve especificar a frequência do clock (com unidade kHz, MHz ou GHz) e o número de ciclos para cada tipo de instrução (i, j e r). Após validar essa configuração, o simulador processa o arquivo linha a linha, ignorando comentários e linhas vazias, separando instruções e identificando labels para saltos.
O simulador valida esses parâmetros antes da execução e prepara os registradores, memória e instruções para a simulação. Caso falte a configuração ou haja erro no formato, a execução é interrompida com mensagem de erro.

--- 

## 🏗️ Como foi implementado?

### Arquitetura Geral
- **Parser:** responsável por interpretar o código MIPS, identificando instruções e seus operandos.  
- **Simulador:** centraliza a execução das instruções, gerenciando o estado dos registradores, da memória de dados e instruções, além de controlar o fluxo do programa (PC).  
- **Tabela de Instruções:** mantém o mapeamento das instruções com seus tipos (R, I, J) e os ciclos de clock associados a cada tipo, facilitando a decodificação e execução.  
- **Memória (Dados e Instruções):** abstrações que simulam a memória do sistema, permitindo leitura e escrita de bytes, meia palavra e palavra completa, além de manipulação ordenada dos dados armazenados.  
- **Totalizador:** controlador global que contabiliza o total de instruções executadas, ciclos consumidos e tempo total estimado da simulação.  

### Métodos principais
- `ConverterInstrucaoParaBinario`: converte uma instrução MIPS textual e seus operandos para sua representação binária, suportando os formatos R, I e J.  
- `MontarTipoR/I/J`: auxiliares para montar a instrução binária conforme o tipo da instrução.  
- `ObterOpcode` e `ObterFunct`: fornecem o código opcode e função (funct) necessários para montar a instrução.  
- `ObterTempoCicloEmSegundos` e `ObterTempoTotalEstimado`: calculam o tempo consumido pela simulação com base na frequência do clock e no total de ciclos.  
- `Memoria.LerPalavra/EscreverPalavra`, `MemoriaInstrucao.LerPalavra/EscreverPalavra`: leitura e escrita simulada de dados e instruções na memória.  
- `TabelaInstrucoes.AtualizarCiclos`: atualiza os ciclos por tipo de instrução, permitindo ajuste dinâmico da simulação.  

### Detalhes
- Suporte completo aos formatos de instrução R, I e J, incluindo as principais operações (add, sub, lw, sw, beq, j, entre outras).  
- Controle rigoroso do PC, com suporte a desvios e saltos, respeitando os efeitos de cada instrução.  
- Implementação detalhada da memória, com manipulação byte a byte e agrupamento por palavras, permitindo visualização do estado da memória em formato hexadecimal.  
- Contabilização precisa dos ciclos de clock por instrução, com cálculo estimado do tempo total de execução baseado na frequência do clock configurada.  
- Uso de coleções e dicionários para manter registradores, labels, e o conteúdo da memória, garantindo eficiência e clareza no acesso aos dados.  
- Tratamento de instruções inválidas ou não reconhecidas retornando valores padrão para evitar falhas durante a simulação.  

---
### 🧑‍💻 Como usar

Passos rápidos:  
1. Clone o repositório  
2. Abra o projeto no Visual Studio ou editor de sua preferência  
3. Prepare seu código assembly seguindo o padrão definido  
4. Execute o simulador e avance passo a passo  
5. Observe o estado dos registradores, memória, PC e tempo de execução  

---

 ### 📋 Gerenciamento do Projeto

Você pode acompanhar o progresso e as tarefas do projeto no Kanban do Trello:

[Kanban do Projeto no Trello](https://trello.com/invite/b/680d30e40f94bc3bee2bc17a/ATTI6dbf5b171dd271bebf91144524f120bb58A3D19C/arq-comp-compilador-assembly)

---
### ⚠️ Dicas importantes e cuidados

- Utilize instruções MIPS válidas e operandos corretos  
- Declare todos os labels que forem utilizados  
- Evite acessar endereços de memória não inicializados  
- Comentários devem começar com `#` e estar fora da instrução  

---

### 🤝 Contribuições

Contribuições são sempre bem-vindas!  
1. Faça um fork do projeto  
2. Crie uma branch para sua feature: `git checkout -b minha-feature`  
3. Faça commit das alterações: `git commit -m 'Minha feature'`  
4. Envie para o repositório: `git push origin minha-feature`  
5. Abra um Pull Request para análise  
