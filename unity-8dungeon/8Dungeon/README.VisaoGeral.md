# 8Dungeon - Visão Geral

## Propósito do Projeto

8Dungeon é um protótipo de RPG focado em combate narrativo. O jogo combina exploração em uma grade cartesiana simples com batalhas por turnos descritas por textos dinâmicos, permitindo ao jogador tomar decisões estratégicas por meio de comandos digitados.

## Arquitetura em Camadas

- **Controle (Controllers)**: coordena o fluxo geral e interações de interface. O `GameController` centraliza estados do jogo e direciona chamadas para os sistemas especializados.
- **Sistemas (Systems)**: encapsulam regras de domínio, como combate (`BattleSystem`) e exploração (`ExplorationManager`).
- **Entidades e Dados (Data/Models)**: representam heróis, inimigos e configurações carregadas de ScriptableObjects ou JSON.
- **Serviços (Services)**: oferecem funcionalidades transversais, como o `NarrationController`, responsável por recuperar falas temáticas.
- **Fábricas (Factories)**: instanciam entidades interpretando dados serializados para manter a criação desacoplada da lógica de jogo.

## Estrutura de Pastas Essenciais

| Pasta | Conteúdo |
| --- | --- |
| `Assets/Scripts/GameController.cs` | Máquina de estados principal e ligação com a UI. |
| `Assets/Scripts/Battle/` | Regras de combate, orquestração de turnos e modelos de resultado. |
| `Assets/Scripts/Exploration/` | Controle da movimentação em grade e detecção de encontros. |
| `Assets/Scripts/Heroes/` | Estruturas e fábrica de heróis baseadas em ScriptableObjects. |
| `Assets/Scripts/Enemies/` | Estruturas e fábrica de inimigos, incluindo arquétipos. |
| `Assets/Scripts/Narration/` | Repositórios, controladores e modelos de narrativa. |
| `Assets/Scripts/Commands/` | Conversão de texto digitado em ações de jogo. |
| `Assets/Resources/` | Dados compartilhados carregados em runtime (classes, inimigos, narrações). |
| `Assets/Scenes/Layout V.1.unity` | Cena de trabalho que combina exploração, UI e combate. |

## Fluxo Geral de Execução

1. **Setup**: ao iniciar a cena, o `GameController` entra no estado de criação do herói e configura serviços como o `NarrationController`.
2. **Criação do Herói**: dados digitados são convertidos em um `HeroStats` via `HeroFactory`, que consulta `HeroClassData` em `Resources/HeroClasses`.
3. **Exploração**: o `ExplorationManager` atualiza a posição do herói, avalia obstáculos e sorteia encontros. O `GridViewController` reflete o estado na interface.
4. **Encontro**: quando um inimigo é detectado, o `EnemyFactory` cria um `EnemyStats` e o `BattleSystem` é instanciado com parâmetros de combate definidos no inspetor.
5. **Batalha**: o jogador digita comandos interpretados pelo `CommandParser`. O `BattleSystem` calcula resultados, chama narrativas e atualiza o `GameController` com um `BattleRoundResult`.
6. **Retorno**: assim que a batalha termina, o fluxo volta para a exploração ou encerra o jogo conforme o resultado.

## Como Estender a Estrutura

- **Novas cenas**: reutilize o `GameController` na hierarquia e arraste os mesmos prefabs/UI já conectados. Configure as referências públicas no inspetor para manter o fluxo intacto.
- **Novos dados**: adicione assets em `Resources` seguindo os formatos existentes. As fábricas usam `Resources.LoadAll`, então novos arquivos compatíveis são carregados automaticamente.
- **Serviços adicionais**: crie novos *Singletons* ou *ScriptableObjects* e injete referências no `GameController` para disponibilizá-los durante os estados do jogo.
