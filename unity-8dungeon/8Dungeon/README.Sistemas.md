# 8Dungeon - Sistemas Principais

## Controle do Fluxo

### GameController (`Assets/Scripts/GameController.cs`)
- Mantém a máquina de estados (`HeroName`, `HeroClass`, `Exploration`, `EnemyIntro`, `PlayerTurn`, `BattleEnd`).
- Atualiza textos do UI (TMP) e direciona comandos ao subsistema adequado.
- Instancia `ExplorationManager` e `BattleSystem` com parâmetros configurados no inspetor.
- Escuta o input do jogador através de `TMP_InputField.onSubmit` e distribui o texto para parsers específicos.

**Para adicionar novo estado**
1. Acrescente o valor ao enum `GameState`.
2. Crie um método `Enter<Estado>()` seguindo o padrão existente.
3. Atualize `ProcessCommand` para tratar o novo estado.

## Exploração

### ExplorationManager (`Assets/Scripts/Exploration/ExplorationManager.cs`)
- Armazena a posição do jogador e coleções de inimigos ativos/obstáculos.
- Limita passos por comando (`MaxStepsPerMove`) e evita atravessar paredes.
- Sorteia encontros com base em `encounterChancePerStep` e aproxima inimigos existentes.
- Retorna `ExplorationMoveResult` com indicadores de bloqueio, encontro e posição.

### GridViewController (`Assets/Scripts/UI/GridViewController.cs`)
- Constrói dinamicamente uma grade de `Image` UI para representar o mapa ao redor do jogador.
- Pinta cores distintas para herói, inimigos e paredes a partir das coleções informadas.

**Para adicionar novas interações de exploração**
1. Estenda `ExplorationCommandParser` com novos verbos/direções.
2. Adicione campos ou métodos a `ExplorationMoveResult` caso necessite retornar dados adicionais.
3. Atualize o `GameController` para interpretar o novo resultado e disparar feedback visual.

## Combate

### BattleController (`Assets/Scripts/Battle/BattleController.cs`)
- Recebe referências de `HeroStats`, `EnemyStats` e repassa ações para o `BattleSystem`.
- Serve como ponto único para acionar turnos a partir do `GameController`.

### BattleSystem (`Assets/Scripts/Battle/BattleSystem.cs`)
- Processa ações do jogador (atacar, defender, analisar, fugir).
- Controla probabilidades de esquiva, contra-ataque e bloqueio perfeito definidas via inspetor.
- Calcula dano usando ataque/defesa e variação aleatória.
- Chama o `NarrationController` para narrar eventos (ataques, dano, morte, etc.).
- Retorna `BattleRoundResult` indicando mensagem montada e flags de término/morte.

**Para adicionar nova ação de combate**
1. Insira a ação no enum `AcaoJogador` e atualize `CommandParser` para reconhecê-la.
2. Implemente método correspondente em `BattleSystem`, encapsulando o fluxo completo.
3. Ajuste `GameController.HandlePlayerTurnInput` para chamar o novo método e tratar o resultado.

## Entidades

### HeroStats (`Assets/Scripts/Heroes/HeroStats.cs`)
- Estrutura de dados com atributos atuais e máximos do herói.
- Campo `ClassKey` usado para buscar narrações temáticas.

### HeroFactory (`Assets/Scripts/Heroes/HeroFactory.cs`)
- Carrega `HeroClassData` de `Resources/HeroClasses` para preencher `HeroStats`.
- Usa cache interno para evitar leituras repetidas.
- Cria instância padrão se o asset não for encontrado.

### EnemyStats (`Assets/Scripts/Enemies/EnemyStats.cs`) e EnemyFactory
- `EnemyStats` guarda atributos, alcances e probabilidades específicas.
- `EnemyFactory` escolhe `EnemyTypeData` de `Resources/Enemies`, exclui arquétipos `Default` para sorteios e produz instâncias prontas para batalha.

**Para adicionar novas classes ou inimigos**
1. Crie novos ScriptableObjects (`HeroClassData` ou `EnemyTypeData`) no menu Assets > Create.
2. Salve-os dentro da pasta `Resources` correspondente para que as fábricas carreguem automaticamente.
3. Ajuste narrações ou fatos adicionais conforme necessário (veja seção Narrativa).

## Narrativa

### NarrationController (`Assets/Scripts/Narration/NarrationController.cs`)
- Singleton que mantém repositórios de falas.
- Expõe métodos `Say` e `SayEnemy` para registrar textos em fila.
- Pode notificar listeners (como `NarrationFeed`) para atualizar a UI.

### HeroNarrationRepository (`Assets/Scripts/Narration/HeroNarrationRepository.cs`)
- Lê `hero_narrations.json` (em `Resources/Data`).
- Retorna falas específicas por classe e evento com fallback para padrões.

### EnemyNarrationRepository e modelos relacionados
- Fornecem linhas de aparição, ataque e morte para cada arquétipo inimigo.
- `EnemyNarrationsLoader` carrega JSONs e entrega dicionários prontos para uso.

**Para adicionar novas linhas narrativas**
1. Atualize os JSONs em `Assets/Resources/Data` seguindo a mesma estrutura.
2. Se criar novos eventos, estenda os enums (`NarrationEvent`, `EnemyNarrationEvent`) e ajuste os métodos do `NarrationController` que executam as buscas.

## Comandos

### CommandParser (`Assets/Scripts/Commands/CommandParser.cs`)
- Normaliza texto (trim, lowercase) e identifica palavras-chave.
- Mapeia prefixos para ações (`atac`, `defen`, `fug`, `analis`).

### ExplorationCommandParser (`Assets/Scripts/Commands/ExplorationCommandParser.cs`)
- Analisa frases como "andar 2 passos" para extrair direção e quantidade.
- Limita passos ao valor máximo definido pelo `ExplorationManager`.

**Para aceitar novos comandos de texto**
1. Adicione entradas correspondentes no parser apropriado.
2. Retorne feedback amigável quando um termo não for reconhecido, seguindo os padrões existentes.
3. Atualize o `GameController` para lidar com o novo resultado (por exemplo, mudança de prompt ou disparo de animações).
