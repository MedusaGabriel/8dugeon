using System.Text;
using UnityEngine;
using TMPro;

public enum GameState
{
    HeroName,
    HeroClass,
    Exploration,
    EnemyIntro,
    PlayerTurn,
    BattleEnd
}

public class GameController : MonoBehaviour
{
    private string pendingHeroName;

    [Header("UI")]
    public TMP_Text narrationText;
    public TMP_Text promptText;
    public TMP_InputField commandInput;
    public TMP_Text heroStatusText;
    [SerializeField] private GridViewController gridView;

    private const string PlayerTurnPrompt = "O que você faz? (atacar | defender | fugir | analisar)";


    [Header("Exploração")]
    [Range(0f, 1f)] public float encounterChancePerStep = 0.25f;
    [SerializeField] private Vector2Int[] initialObstacles = new Vector2Int[]
    {
        new Vector2Int(2, 0),
        new Vector2Int(-2, 1),
        new Vector2Int(0, 2),
        new Vector2Int(1, -2)
    };

    [Header("Chances de Batalha")]
    [Range(0f, 1f)] public float enemyDodgeChance = 0.2f;
    [Range(0f, 1f)] public float enemyCounterChanceOnDodge = 0.5f;
    [Range(0f, 1f)] public float playerPerfectBlockChance = 0.6f;

    private GameState currentState;
    private HeroStats hero;
    private EnemyStats enemy;
    private BattleSystem battleSystem;
    private ExplorationManager explorationManager;
    private Vector2Int? pendingEncounterPosition;
    private NarrationFeed narrationFeed;
    private bool narrationSubscribed;

    private void Awake()
    {
        if (commandInput != null)
        {
            commandInput.onSubmit.AddListener(HandleCommandSubmitted);
        }

        narrationFeed = new NarrationFeed(narrationText);
        TrySubscribeToNarration();
    }


    private void Start()
    {
        Random.InitState(System.Environment.TickCount);
        hero = null;
        explorationManager = new ExplorationManager(encounterChancePerStep);
        TrySubscribeToNarration();
        EnterHeroNameState();
    }

    private void OnDestroy()
    {
        if (commandInput != null)
        {
            commandInput.onSubmit.RemoveListener(HandleCommandSubmitted);
        }

        if (narrationSubscribed)
        {
            var narrationController = NarrationController.Instance;
            if (narrationController != null)
            {
                narrationController.UnregisterListener(HandleNarrationEvent);
            }

            narrationSubscribed = false;
        }
    }

    #region ESTADOS INICIAIS (CRIAÇÃO DO HERÓI)

    private void EnterHeroNameState()
    {
        currentState = GameState.HeroName;
        SetNarration("Bem-vindo ao 8Dungeon!");
        ShowPrompt("Digite o nome do seu herói:");
    }

    private void EnterHeroClassState()
    {
        currentState = GameState.HeroClass;
        SetNarration($"Herói: {pendingHeroName}");
        ShowPrompt("Digite a classe do seu herói:");
    }

    private void EnterExplorationState(bool initialEntry = false)
    {
        currentState = GameState.Exploration;

        if (explorationManager == null)
        {
            explorationManager = new ExplorationManager(encounterChancePerStep);
        }

        if (initialEntry)
        {
            ApplyInitialObstacles();
            SetNarration("Você desperta nos corredores da dungeon, pronto para explorar.");
        }
        else
        {
            AppendNarration("Você retoma a exploração pelos corredores.");
        }

        UpdateExplorationView();
        ShowPrompt("Como você avança? (ex: andar 2 passos)");
    }

    private void EnterEnemyIntroState(string introMessage = null)
    {
        currentState = GameState.EnemyIntro;

        if (!string.IsNullOrWhiteSpace(introMessage))
        {
            AppendNarration(introMessage);
        }

        ShowPrompt("Pressione Enter para iniciar a batalha.");

        UpdateStatusUI();
    }


    #endregion

    #region LOOP PRINCIPAL: INPUT DO JOGADOR

    private void HandleCommandSubmitted(string rawInput)
    {
        ProcessCommand(rawInput);
    }

    private void ProcessCommand(string rawInput)
    {
        string text = rawInput?.Trim() ?? string.Empty;

        switch (currentState)
        {
            case GameState.HeroName:
                HandleHeroNameInput(text);
                break;

            case GameState.HeroClass:
                HandleHeroClassInput(text);
                break;

            case GameState.Exploration:
                HandleExplorationInput(text);
                break;

            case GameState.EnemyIntro:
                EnterPlayerTurnState();
                break;

            case GameState.PlayerTurn:
                HandlePlayerTurnInput(text);
                break;

            case GameState.BattleEnd:
                // Futuro: reiniciar a batalha ou voltar ao menu
                break;
        }
    }

    private void EnterPlayerTurnState()
    {
        currentState = GameState.PlayerTurn;

        SetNarration("A criatura observa seus movimentos, aguardando sua ação.");

        ShowPrompt(PlayerTurnPrompt);
        UpdateStatusUI();
    }

    private void HandlePlayerTurnInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            ShowPrompt("Comando vazio. Digite atacar, defender, fugir ou analisar.");
            return;
        }

        AcaoJogador acao = CommandParser.ParsePlayerAction(input);

        // Se a ação não é uma das conhecidas, trata como inválida
        if (acao != AcaoJogador.Atacar &&
            acao != AcaoJogador.Defender &&
            acao != AcaoJogador.Analisar &&
            acao != AcaoJogador.Fugir)
        {
            ShowPrompt("Comando não reconhecido. Use atacar, defender, fugir ou analisar.");
            return;
        }

        ClearNarration();

        // Agora delega direto ao BattleSystem
        BattleRoundResult result = ExecuteBattleAction(acao);

        ApplyBattleResult(result);
    }

    private void HandleExplorationInput(string input)
    {
        if (!ExplorationCommandParser.TryParseMove(input, out Vector2Int direction, out int steps, out string feedback))
        {
            ShowPrompt(feedback ?? "Não entendi quantos passos avançar. Tente novamente.");
            return;
        }

        ExplorationMoveResult moveResult = explorationManager.Move(direction, steps);

        AppendNarration(BuildExplorationNarration(moveResult));
        UpdateExplorationView();

        if (moveResult.ShouldStartBattle && moveResult.BattlePosition.HasValue)
        {
            string prompt = moveResult.EncounteredEnemy
                ? "Uma criatura bloqueia seu caminho! Pressione Enter para encará-la."
                : "O inimigo alcança você! Pressione Enter para reagir.";

            ShowPrompt(prompt);

            string introMessage = moveResult.EncounteredEnemy
                ? "Uma criatura bloqueia seu caminho, rosnando diante de você."
                : "O inimigo que o perseguia ataca sem aviso.";

            PrepareBattleFromExploration(moveResult.BattlePosition.Value, introMessage);
        }
        else
        {
            ShowPrompt("Como você avança? (ex: andar 2 passos)");
        }
    }

    private void PrepareBattleFromExploration(Vector2Int encounterPosition, string introMessage = null)
    {
        pendingEncounterPosition = encounterPosition;

        enemy = CreateRandomEnemy();

        battleSystem = new BattleSystem(
            hero,
            enemy,
            enemyDodgeChance,
            enemyCounterChanceOnDodge,
            playerPerfectBlockChance
        );

        string intro = string.IsNullOrWhiteSpace(introMessage)
            ? "A criatura encara você, pronta para lutar."
            : introMessage;

        EnterEnemyIntroState(intro);
    }

    private string BuildExplorationNarration(ExplorationMoveResult moveResult)
    {
        if (moveResult.StepsTaken <= 0)
        {
            return moveResult.BlockedByObstacle
                ? "Você tenta avançar, mas uma parede antiga bloqueia seu caminho."
                : "Você permanece atento, mas não sai do lugar.";
        }

        string message = moveResult.StepsTaken == 1
            ? "Você avança um único passo pelo corredor úmido."
            : $"Você avança {moveResult.StepsTaken} passos pelos corredores da dungeon.";

        if (moveResult.BlockedByObstacle)
        {
            message += "\nSeu avanço é interrompido por uma parede irregular de pedra.";
        }

        if (moveResult.EncounteredEnemy)
        {
            message += "\nUm som estranho ecoa à frente e uma presença hostil surge diante de você!";
        }
        else if (moveResult.ProximityTriggered)
        {
            message += "\nUma criatura próxima investe, tentando encurralá-lo!";
        }
        else
        {
            message += "\nO silêncio persiste enquanto você continua explorando.";
        }

        return message;
    }

    private BattleRoundResult ExecuteBattleAction(AcaoJogador acao)
    {
        if (battleSystem == null)
        {
            Debug.LogError("[GameController] BattleSystem não inicializado antes de executar ações.");
            return null;
        }

        switch (acao)
        {
            case AcaoJogador.Atacar:
                return battleSystem.PlayerAttack();
            case AcaoJogador.Defender:
                return battleSystem.PlayerDefend();
            case AcaoJogador.Fugir:
                return battleSystem.PlayerFlee();
            case AcaoJogador.Analisar:
                return battleSystem.PlayerAnalyze();
            default:
                Debug.LogError($"[GameController] Ação de jogador inválida: {acao}");
                return null;
        }
    }


    private void ApplyBattleResult(BattleRoundResult result)
    {
        if (result == null) return;

        UpdateStatusUI();


        if (result.BattleEnded)
        {
            AppendNarration(result.Message);
            if (result.PlayerDied)
            {
                currentState = GameState.BattleEnd;
                ShowPrompt("Fim da batalha.", false);
            }
            else
            {
                ReturnToExplorationAfterBattle();
            }
        }
        else
        {
            currentState = GameState.PlayerTurn;
            AppendNarration(result.Message);
            ShowPrompt(PlayerTurnPrompt);
        }
    }

    #endregion

    #region HANDLERS INICIAIS

    private void HandleHeroNameInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            ShowPrompt("Nome vazio é proibido. Digite um nome para o seu herói:");
            return;
        }

        pendingHeroName = input;
        EnterHeroClassState();
    }

    private void HandleHeroClassInput(string input)
    {
        HeroClass heroClass;

        if (!TryParseHeroClass(input, out heroClass))
        {
            heroClass = HeroClass.Default;
        }

        hero = HeroFactory.CreateHero(pendingHeroName, heroClass);
        hero.ClassKey = heroClass.ToString().ToLowerInvariant();
        explorationManager.Reset();
        EnterExplorationState(initialEntry: true);
    }

    #endregion

    #region UTILITÁRIOS

    private bool TryParseHeroClass(string input, out HeroClass heroClass)
    {
        string lower = input.ToLower();

        if (lower.Contains("mago"))
        {
            heroClass = HeroClass.Mago;
            return true;
        }

        if (lower.Contains("guerreiro") || lower.Contains("guerreir"))
        {
            heroClass = HeroClass.Guerreiro;
            return true;
        }

        heroClass = HeroClass.Default;
        return false;
    }

    private EnemyStats CreateRandomEnemy()
    {
        return EnemyFactory.CreateRandomEnemy();
    }
    private void UpdateStatusUI()
    {
        if (heroStatusText != null)
        {
            if (hero != null)
            {
                heroStatusText.text =
                    $"Nome: {hero.Name}\n" +
                    $"Classe: {hero.Class}\n" +
                    $"HP: {hero.CurrentHp}/{hero.MaxHp}\n" +
                    $"Defesa: {hero.Defense}\n" +
                    $"Ataque: {hero.Attack}\n" +
                    $"Descrição: {hero.ClassDescription}";
            }
            else
            {
                heroStatusText.text = string.Empty;
            }
        }
    }

    private void UpdateExplorationView()
    {
        if (gridView != null && explorationManager != null)
        {
            gridView.SetObstacles(explorationManager.Obstacles);
            gridView.Render(explorationManager.PlayerPosition, explorationManager.ActiveEnemies);
        }
    }

    private void ApplyInitialObstacles()
    {
        if (explorationManager == null)
        {
            return;
        }

        if (initialObstacles != null && initialObstacles.Length > 0)
        {
            explorationManager.SetObstacles(initialObstacles);
        }
        else
        {
            explorationManager.SetObstacles(null);
        }
    }

    private void ReturnToExplorationAfterBattle()
    {
        if (pendingEncounterPosition.HasValue && explorationManager != null)
        {
            explorationManager.RemoveEnemy(pendingEncounterPosition.Value);
            pendingEncounterPosition = null;
        }

        battleSystem = null;
        enemy = null;

        EnterExplorationState();
    }

    private void SetPrompt(string text)
    {
        if (promptText != null)
        {
            promptText.text = text;
        }
    }

    private void ShowPrompt(string text, bool enableInput = true)
    {
        SetPrompt(text);
        ClearAndFocusInput(enableInput);
    }

    private void ClearAndFocusInput(bool enableInput = true)
    {
        if (commandInput == null)
        {
            return;
        }

        commandInput.interactable = enableInput;
        commandInput.text = string.Empty;

        if (enableInput)
        {
            commandInput.ActivateInputField();
            commandInput.Select();
        }
        else
        {
            commandInput.DeactivateInputField();
        }
    }

    private void TrySubscribeToNarration()
    {
        if (narrationSubscribed)
        {
            return;
        }

        var narrationController = NarrationController.Instance;
        if (narrationController == null)
        {
            return;
        }

        narrationController.RegisterListener(HandleNarrationEvent);
        narrationSubscribed = true;
    }

    private void HandleNarrationEvent(string line)
    {
        AppendNarration(line);
    }

    private void SetNarration(string text)
    {
        narrationFeed?.Set(text);
    }

    private void AppendNarration(string text)
    {
        narrationFeed?.Append(text);
    }

    private void ClearNarration()
    {
        narrationFeed?.Clear();
    }


    #endregion

    private sealed class NarrationFeed
    {
        private readonly TMP_Text _target;
        private readonly StringBuilder _buffer = new StringBuilder();

        public NarrationFeed(TMP_Text target)
        {
            _target = target;
            Refresh();
        }

        public void Set(string text)
        {
            _buffer.Clear();
            AppendInternal(text, false);
        }

        public void Append(string text)
        {
            AppendInternal(text, true);
        }

        public void Clear()
        {
            _buffer.Clear();
            Refresh();
        }

        private void AppendInternal(string text, bool separate)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            if (separate && _buffer.Length > 0)
            {
                _buffer.Append("\n\n");
            }

            _buffer.Append(text);
            Refresh();
        }

        private void Refresh()
        {
            if (_target != null)
            {
                _target.text = _buffer.ToString();
            }
        }
    }
}
