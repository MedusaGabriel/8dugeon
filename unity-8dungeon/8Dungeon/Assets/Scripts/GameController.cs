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
    public TMP_Text messageText;
    public TMP_InputField commandInput;
    public TMP_Text heroStatusText;
    public TMP_Text enemyStatusText;


    [Header("Chances de Batalha")]
    [Range(0f, 1f)] public float enemyDodgeChance = 0.2f;
    [Range(0f, 1f)] public float enemyCounterChanceOnDodge = 0.5f;
    [Range(0f, 1f)] public float playerPerfectBlockChance = 0.6f;

    private GameState currentState;
    private HeroStats hero;
    private EnemyStats enemy;
    private BattleSystem battleSystem;

    private void Start()
    {
        hero = null;
        EnterHeroNameState();
    }

    #region ESTADOS INICIAIS (CRIAÇÃO DO HERÓI)

    private void EnterHeroNameState()
    {
        currentState = GameState.HeroName;
        messageText.text = "Digite o nome do seu herói:";
        commandInput.text = "";
        commandInput.ActivateInputField();
    }

    private void EnterHeroClassState()
    {
        currentState = GameState.HeroClass;
        messageText.text = "Digite a classe do seu herói (Mago ou Guerreiro):";
        commandInput.text = "";
        commandInput.ActivateInputField();
    }

    private void EnterEnemyIntroState()
    {
        currentState = GameState.EnemyIntro;

        enemy = CreateRandomEnemy();

        // Cria o sistema de batalha com herói, inimigo e chances
        battleSystem = new BattleSystem(
            hero,
            enemy,
            enemyDodgeChance,
            enemyCounterChanceOnDodge,
            playerPerfectBlockChance
        );

        messageText.text =
            $"Herói criado!\n" +
            $"Nome: {hero.Name}\n" +
            $"Classe: {hero.Class}\n" +
            $"HP: {hero.CurrentHp}/{hero.MaxHp}\n\n" +
            $"Um inimigo apareceu!\n" +
            $"É um {enemy.Name}.\n" +
            $"HP: {enemy.CurrentHp}/{enemy.MaxHp}\n\n" +
            "Pressione Confirmar para iniciar a batalha.";

        commandInput.text = "";
        commandInput.DeactivateInputField();

        UpdateStatusUI();
    }

    #endregion

    #region LOOP PRINCIPAL: INPUT DO JOGADOR

    public void OnConfirmButtonPressed()
    {
        string text = commandInput.text.Trim();

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

        messageText.text =
            $"Inimigo: {enemy.Name} HP {enemy.CurrentHp}/{enemy.MaxHp}\n" +
            $"Herói: {hero.Name} HP {hero.CurrentHp}/{hero.MaxHp}\n\n" +
            "O que você faz? (atacar / defender / fugir)";

        commandInput.text = "";
        commandInput.ActivateInputField();
        UpdateStatusUI();
    }

    private void HandlePlayerTurnInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            messageText.text += "\n\nComando vazio. Escreva algo (atacar / defender / fugir).";
            commandInput.text = "";
            commandInput.ActivateInputField();
            return;
        }

        AcaoJogador acao = CommandParser.ParsePlayerAction(input);
        BattleRoundResult result = null;

        switch (acao)
        {
            case AcaoJogador.Atacar:
                result = battleSystem.PlayerAttack();
                ApplyBattleResult(result);
                break;

            case AcaoJogador.Defender:
                result = battleSystem.PlayerDefend();
                ApplyBattleResult(result);
                break;

            case AcaoJogador.Fugir:
                result = battleSystem.PlayerFlee();
                ApplyBattleResult(result);
                break;

            default:
                messageText.text += "\n\nComando não reconhecido. Tente: atacar, defender ou fugir.";
                commandInput.text = "";
                commandInput.ActivateInputField();
                break;
        }
    }

    private void ApplyBattleResult(BattleRoundResult result)
    {
        if (result == null) return;

        UpdateStatusUI();


        if (result.BattleEnded)
        {
            currentState = GameState.BattleEnd;
            messageText.text = result.Message + "\n\n(Fim da batalha.)";
            commandInput.text = "";
            commandInput.DeactivateInputField();
        }
        else
        {
            currentState = GameState.PlayerTurn;
            messageText.text = result.Message + "\n\nO que você faz? (atacar / defender / fugir)";
            commandInput.text = "";
            commandInput.ActivateInputField();
        }
    }

    #endregion

    #region HANDLERS INICIAIS

    private void HandleHeroNameInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            messageText.text = "Nome vazio é proibido. Digite um nome para o seu herói:";
            commandInput.text = "";
            commandInput.ActivateInputField();
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
        if (heroStatusText != null && hero != null)
        {
            heroStatusText.text =
                $"{hero.Name}\n" +
                $"Classe: {hero.Class}\n" +
                $"HP: {hero.CurrentHp}/{hero.MaxHp}";
        }

        if (enemyStatusText != null)
        {
            if (enemy != null)
            {
                enemyStatusText.text =
                    $"{enemy.Name}\n" +
                    $"HP: {enemy.CurrentHp}/{enemy.MaxHp}";
            }
            else
            {
                enemyStatusText.text = "";
            }
        }
    }


    #endregion
}
