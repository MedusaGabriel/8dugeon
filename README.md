dotnet run
# 8Dungeon

RPG em desenvolvimento com duas implementações paralelas: uma versão de console baseada em texto e um protótipo com interface em Unity.

## Visão Geral do Repositório

| Diretório | Descrição |
| --- | --- |
| `legacy_console/StudyProjetoMiniRpg` | Versão original em C# com interface de console e combate por texto. |
| `unity-8dungeon/8Dungeon` | Projeto Unity 6.0 (6000.3.0f1) com gameplay híbrido de exploração em grade e combate narrativo. |
| `C#Study` | Exercícios e protótipos isolados usados durante o estudo de C#. |

## Versão Console (`legacy_console/StudyProjetoMiniRpg`)

### Fluxo do jogo
1. `Program.cs` carrega as narrações em JSON, instancia um inimigo aleatório através de `EnemyFactory` e cria o herói com `Hero.CriarHeroi()`.
2. `BattleSystem` delega a execução para `BattleOrchestrator`, responsável por intermediar entrada do jogador, narrações e regras de turno.
3. A cada rodada o `PlayerInput` interpreta textos livres mapeando sinônimos para ações (`atacar`, `defender`, `analisar`, `fugir`).
4. `TurnExecutor` executa a ação escolhida, consulta `HeroNarrationRepository` para textos dinâmicos e chama a `EnemyAI` para a resposta do inimigo.
5. `PositionSystem` controla a distância entre herói e inimigo (baseada em distância de Chebyshev), permitindo decisões de alcance e movimentos táticos futuros.

### Principais componentes
- **Battle**: `BattleOrchestrator`, `TurnExecutor`, `EnemyAI`, `PositionSystem` e `BattleUI` compõem o loop de combate, lidando com narrações, cálculo de dano, alcance e stamina do inimigo.
- **Heroes** e **Enemies**: classes de suporte (`Hero`, `Enemy`, `HeroClassConfig`, `EnemyFactory`, etc.) carregam estatísticas de arquivos JSON e aplicam regras de criação, dano e defesa.
- **Narrations**: `HeroNarrationRepository` e `NarrationRepository` (static) fornecem textos temáticos para eventos como aparição, ataque, dano e morte, enriquecendo a narrativa.
- **Player**: `PlayerInput` e `HeroInputSynonymsRepository` reforçam a fantasia de parser textual aceitando diferentes verbos e estilos de comando.

### Dados
- Arquivos em `Data/` (`hero_classes.json`, `enemy_classes.json`, `hero_narrations.json`, `enemy_narrations.json`, etc.) definem atributos, falas e sinônimos das entidades.
- Ajuste ou adicione JSONs para expandir classes, inimigos e repertório de narrativas sem alterar código.

### Executando
1. Instale o SDK .NET mais recente (o build atual foi gerado com `net10.0`; use um SDK compatível ou ajuste o `TargetFramework` ao recriar o `.csproj`).
2. Abra a solução `8dugeon.sln` no Visual Studio e atualize o caminho do projeto se necessário (o arquivo `.csproj` não está versionado; crie um projeto console dentro de `legacy_console/StudyProjetoMiniRpg` apontando para os fontes existentes).
3. Com o projeto configurado, execute `dotnet run --project legacy_console/StudyProjetoMiniRpg` para iniciar o jogo no console.

## Versão Unity (`unity-8dungeon/8Dungeon`)

### Requisitos
- Unity 6.0.3f1 (`m_EditorVersion: 6000.3.0f1`).
- Pacotes padrão listados em `Packages/manifest.json` (Input System, TextMesh Pro, URP, etc.).

### Gameplay atual
1. **Criação do herói**: `GameController` solicita nome e classe; `HeroFactory` carrega `HeroClassData` de `Resources/HeroClasses` e monta instâncias de `HeroStats`.
2. **Exploração**: `ExplorationManager` move o herói em uma grade cartesiana, gerencia paredes iniciais e posiciona inimigos ativos; `GridViewController` renderiza o tabuleiro no UI.
3. **Encontros**: quando ocorre um encontro aleatório ou um inimigo alcança o jogador, `BattleSystem` é inicializado com probabilidades configuráveis (desvio, contra-ataque, bloqueio perfeito).
4. **Combate**: ações de jogador são interpretadas por `CommandParser`; `BattleSystem` coordena ataques, defesas, fugas e análise (que revela atributos completos), chamando `NarrationController` para mensagens dinâmicas. O resultado de cada rodada é encapsulado em `BattleRoundResult` para atualizar UI e fluxo.
5. **Narração e dados**: resources em `Assets/Resources/Data` e `Assets/Resources/Narrations` alimentam `NarrationController`, `EnemyNarrationRepository` e versões Unity dos repositórios de narração.

### Como testar
1. Abra o diretório `unity-8dungeon/8Dungeon` na Unity Hub com a versão 6000.3.0f1.
2. Carregue a cena `Assets/Scenes/Layout V.1.unity` (contém UI de exploração e protótipo de batalha).
3. Pressione Play, informe nome/classe, use comandos de exploração (ex.: `andar 2 passos`) e execute ações de batalha na UI (`atacar`, `defender`, `analisar`, `fugir`).
4. Ajuste probabilidades de encontro e combate diretamente no inspetor do `GameController` para testar comportamentos diferentes.

## Próximos Passos Sugeridos
- Versionar o `.csproj` da versão console e alinhar o `TargetFramework` com um SDK estável.
- Sincronizar o repertório de narrações entre console e Unity para reutilizar conteúdo.
- Expadir o sistema de exploração Unity com interações de sala, loot e eventos não combativos.
- Implementar habilidades de classe especiais (já descritas em `BattleUI.ExibirInformacoesHeroi`) tanto no console quanto na Unity.