using System.Text;
using UnityEngine;
using TMPro;

public enum GameState
{
    HeroName,
    HeroClass,
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


    [Header("Chances de Batalha")]
    [Range(0f, 1f)] public float enemyDodgeChance = 0.2f;
    [Range(0f, 1f)] public float enemyCounterChanceOnDodge = 0.5f;
    [Range(0f, 1f)] public float playerPerfectBlockChance = 0.6f;

    private GameState currentState;
    private HeroStats hero;
    private EnemyStats enemy;
    private BattleController battleController;
    private readonly StringBuilder narrationBuffer = new StringBuilder();
    private bool narrationSubscribed;

    private void Awake()
    {
        if (commandInput != null)
        {
            commandInput.onSubmit.AddListener(HandleCommandSubmitted);
        }

        TrySubscribeToNarration();
    }


    private void Start()
    {
        Random.InitState(System.Environment.TickCount);
        hero = null;
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
        SetPrompt("Digite o nome do seu herói:");
        ClearAndFocusInput();
    }

    private void EnterHeroClassState()
    {
        currentState = GameState.HeroClass;
        SetNarration($"Herói: {pendingHeroName}");
        SetPrompt("Digite a classe do seu herói:");
        ClearAndFocusInput();
    }

    private void EnterEnemyIntroState()
    {
        currentState = GameState.EnemyIntro;

        enemy = CreateRandomEnemy();

        ClearNarration();
        AppendNarration($"Herói criado: {hero.Name} ({hero.Class}).");

        battleController = new BattleController(
            hero,
            enemy,
            enemyDodgeChance,
            enemyCounterChanceOnDodge,
            playerPerfectBlockChance
        );

        SetPrompt("Pressione Enter para iniciar a batalha.");
        ClearAndFocusInput();

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

        SetPrompt("O que você faz? (atacar | defender | fugir | analisar)");
        ClearAndFocusInput();
        UpdateStatusUI();
    }

    private void HandlePlayerTurnInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            SetPrompt("Comando vazio. Digite atacar, defender, fugir ou analisar.");
            ClearAndFocusInput();
            return;
        }

        AcaoJogador acao = CommandParser.ParsePlayerAction(input);

        // Se a ação não é uma das conhecidas, trata como inválida
        if (acao != AcaoJogador.Atacar &&
            acao != AcaoJogador.Defender &&
            acao != AcaoJogador.Analisar &&
            acao != AcaoJogador.Fugir)
        {
            SetPrompt("Comando não reconhecido. Use atacar, defender, fugir ou analisar.");
            ClearAndFocusInput();
            return;
        }

        ClearNarration();

        // Agora delega para o BattleController
        BattleRoundResult result = battleController.ExecutePlayerAction(acao);

        ApplyBattleResult(result);
    }


    private void ApplyBattleResult(BattleRoundResult result)
    {
        if (result == null) return;

        UpdateStatusUI();


        if (result.BattleEnded)
        {
            currentState = GameState.BattleEnd;
            AppendNarration(result.Message);
            SetPrompt("Fim da batalha.");
            ClearAndFocusInput(false);
        }
        else
        {
            currentState = GameState.PlayerTurn;
            AppendNarration(result.Message);
            SetPrompt("O que você faz? (atacar | defender | fugir | analisar)");
            ClearAndFocusInput();
        }
    }

    #endregion

    #region HANDLERS INICIAIS

    private void HandleHeroNameInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            SetPrompt("Nome vazio é proibido. Digite um nome para o seu herói:");
            ClearAndFocusInput();
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
        EnterEnemyIntroState();
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

    private void SetPrompt(string text)
    {
        if (promptText != null)
        {
            promptText.text = text;
        }
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
        ClearNarration();
        AppendNarration(text);
    }

    private void AppendNarration(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        if (narrationBuffer.Length > 0)
        {
            narrationBuffer.Append("\n\n");
        }

        narrationBuffer.Append(text);

        if (narrationText != null)
        {
            narrationText.text = narrationBuffer.ToString();
        }
    }

    private void ClearNarration()
    {
        narrationBuffer.Clear();

        if (narrationText != null)
        {
            narrationText.text = string.Empty;
        }
    }


    #endregion
}
