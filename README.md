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
