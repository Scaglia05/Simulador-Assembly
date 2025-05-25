# Simulador de Instruções MIPS com Controle de Clock em C#

Este projeto é um simulador didático para execução passo a passo de programas escritos em linguagem assembly MIPS. Ele simula o ciclo de clock, a execução das instruções e o funcionamento básico dos registradores, memória e labels.

---

## Sumário

- [Descrição](#descrição)
- [Funcionalidades](#funcionalidades)
- [Requisitos](#requisitos)
- [Como usar](#como-usar)
- [Detalhes do código e arquitetura](#detalhes-do-código-e-arquitetura)
- [O que evitar](#o-que-evitar)
- [Contribuindo](#contribuindo)
- [Licença](#licença)
- [Contato](#contato)

---

## Descrição

Este simulador permite que você execute código assembly MIPS linha a linha, visualizando o estado dos registradores, memória e ponteiros. Ele respeita os ciclos por instrução para simular o tempo de clock, facilitando o entendimento do funcionamento interno de um processador RISC.

---

## Funcionalidades

- Execução passo a passo do código assembly MIPS
- Suporte a labels, saltos condicionais e incondicionais (`j`, `b`, etc.)
- Simulação de ciclos de clock por instrução (delay controlado)
- Visualização do conteúdo dos registradores e memória em tempo real
- Parsing de instruções e operandos, com tratamento de comentários e labels
- Interface para avançar a execução manualmente ou automática

---

## Requisitos

- .NET 6.0 ou superior (para executar a aplicação C#)
- Visual Studio 2022 (ou editor compatível com C#)
- Conhecimento básico em linguagem assembly MIPS (recomendado)
  
---

## Como usar

**Clone o repositório:**
Abra o projeto no Visual Studio ou editor preferido.
Configure o arquivo de entrada:
Abra o arquivo onde o código assembly é carregado (exemplo: LinhasPrograma)
Insira seu código assembly MIPS seguindo o formato padrão:
Cada linha deve ser uma instrução válida
Labels devem terminar com :
Comentários iniciam com #

Exemplo:
addi $t0, $zero, 0       # inicializa $t0 com 0
Loop:
add  $t0, $t0, $t2       # soma 2 ao acumulador
bne  $t3, $t4, Loop      # repete até contar 10

Execute o simulador:

Use a interface para avançar a execução passo a passo
Observe os valores dos registradores, PC e memória atualizados a cada ciclo
Verifique mensagens de erro caso alguma instrução seja inválida ou label não encontrada
Compreensão dos controles:
O simulador respeita o tempo de clock definido, simulando ciclos por instrução
Você pode avançar manualmente ou aguardar a execução automática com delays
Labels e saltos são interpretados corretamente para controlar o fluxo do programa

Detalhes do código e arquitetura
Controle de clock:
O método ExecutarClock() controla o ciclo da execução, decrementando os ciclos restantes por instrução e aguardando delay entre eles.

Parsing de instruções:
As linhas do programa são parseadas para extrair instrução e operandos, ignorando comentários e labels.

Execução de instruções:
O método Executar() recebe instrução, operandos, registradores e memória, atualizando o estado conforme a lógica da instrução.

Gerenciamento de memória:
Memória e registradores são representados por estruturas que simulam os endereços e valores reais.

Labels e saltos:
Labels são armazenados em um dicionário e usados para controlar o fluxo de execução com instruções j e b.

O que evitar
Instruções inválidas:
Certifique-se de usar instruções MIPS válidas e operandos corretos para evitar erros de parsing.
Labels não declaradas:
Não use labels que não foram definidas, isso causará erro de execução.
Acesso fora dos limites da memória:
Evite acessar posições de memória que não foram inicializadas, pode causar erros ou comportamento inesperado.
Comentários e linhas vazias:
Comentários devem começar com # e não podem estar dentro da instrução (deve estar após o código). Linhas vazias são ignoradas.
Contribuindo
Contribuições são muito bem-vindas!
Fork o projeto
Crie uma branch para sua feature (git checkout -b minha-feature)
Faça commit das suas alterações (git commit -m 'Minha feature')
Faça push para o branch (git push origin minha-feature)
Abra um Pull Request no repositório original


