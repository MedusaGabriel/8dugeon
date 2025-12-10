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
    [Header("UI")]
    public TMP_Text messageText;
    public TMP_InputField commandInput;

    [Header("Chances do Inimigo")]
    [Range(0f, 1f)] public float enemyDodgeChance = 0.2f;              // 20% de chance de desviar
    [Range(0f, 1f)] public float enemyCounterChanceOnDodge = 0.5f;      // 50% de chance de contra-atacar após desvio

    private GameState currentState;
    private HeroStats hero;
    private EnemyStats enemy;

    private void Start()
    {
        hero = new HeroStats();
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
    }

    #endregion

    #region LOOP PRINCIPAL: INPUT DO JOGADOR

    // Ligado ao botão "Confirmar"
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
                // Depois podemos usar aqui para reiniciar a batalha.
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

        AcaoJogador acao = ParsePlayerAction(input);

        switch (acao)
        {
            case AcaoJogador.Atacar:
                ResolvePlayerAttack();
                break;

            case AcaoJogador.Defender:
                ResolvePlayerDefend();
                break;

            case AcaoJogador.Fugir:
                ResolvePlayerFlee();
                break;

            default:
                messageText.text += "\n\nComando não reconhecido. Tente: atacar, defender ou fugir.";
                commandInput.text = "";
                commandInput.ActivateInputField();
                break;
        }
    }

    #endregion

    #region RESOLUÇÃO DAS AÇÕES

    private void ResolvePlayerAttack()
    {
        // 1) Tenta desviar
        bool dodged = EnemyDodgedAttack();

        if (dodged)
        {
            string msg =
                $"Você atacou o {enemy.Name}, mas ele DESVIOU do seu ataque!\n" +
                $"HP do inimigo: {enemy.CurrentHp}/{enemy.MaxHp}";

            // 2) Após desviar, chance de contra-atacar
            bool counter = EnemyCounterAttackOnDodge();

            if (counter)
            {
                msg += $"\n\nO {enemy.Name} aproveita a abertura e contra-ataca!";
                // Reaproveitamos a lógica de ataque inimigo, sem defesa do jogador
                ResolveEnemyAttack(msg, playerDefending: false);
            }
            else
            {
                // Só desviou, sem contra-ataque. Volta para o turno do jogador.
                messageText.text = msg + "\n\nO que você faz? (atacar / defender / fugir)";
                commandInput.text = "";
                commandInput.ActivateInputField();
                currentState = GameState.PlayerTurn;
            }
        }
        else
        {
            // Não desviou: leva dano normal
            int damage = CalculateDamage(hero.Attack, enemy.Defense);
            enemy.CurrentHp = Mathf.Max(0, enemy.CurrentHp - damage);

            string msg =
                $"Você atacou o {enemy.Name} e causou {damage} de dano.\n" +
                $"HP do inimigo: {enemy.CurrentHp}/{enemy.MaxHp}";

            if (enemy.CurrentHp <= 0)
            {
                msg += "\n\nO inimigo foi derrotado! Você venceu a batalha.";
                EnterBattleEnd(msg);
            }
            else
            {
                // Inimigo ainda está vivo, turno dele (sem defesa do jogador)
                ResolveEnemyAttack(msg, playerDefending: false);
            }
        }
    }

    private void ResolvePlayerDefend()
    {
        string msg =
            $"Você se prepara para defender o ataque do {enemy.Name}...";

        ResolveEnemyAttack(msg, playerDefending: true);
    }

    private void ResolveEnemyAttack(string previousMessage, bool playerDefending)
    {
        int damage;
        string defenseText = "";

        if (playerDefending)
        {
            // Chance de defesa bem-sucedida: por exemplo, 60%
            float roll = Random.value; // 0.0 a 1.0

            if (roll < 0.6f)
            {
                // Defesa perfeita: nenhum dano
                damage = 0;
                defenseText = "Você defendeu completamente o ataque e não sofreu dano!";
            }
            else
            {
                // Defesa parcial: reduz o dano pela metade
                int fullDamage = CalculateDamage(enemy.Attack, hero.Defense);
                damage = Mathf.RoundToInt(fullDamage * 0.5f);
                if (damage < 1) damage = 1;
                defenseText = $"Você conseguiu reduzir o dano pela metade, mas ainda sofreu {damage} de dano.";
            }
        }
        else
        {
            // Sem defesa
            damage = CalculateDamage(enemy.Attack, hero.Defense);
        }

        hero.CurrentHp = Mathf.Max(0, hero.CurrentHp - damage);

        string msg = previousMessage + "\n\n";

        if (playerDefending)
        {
            msg += $"{enemy.Name} atacou!\n{defenseText}\n";
        }
        else
        {
            msg += $"O {enemy.Name} atacou você e causou {damage} de dano.\n";
        }

        msg += $"Seu HP: {hero.CurrentHp}/{hero.MaxHp}";

        if (hero.CurrentHp <= 0)
        {
            msg += "\n\nVocê foi derrotado.";
            EnterBattleEnd(msg);
        }
        else
        {
            // Volta para o turno do jogador
            messageText.text = msg + "\n\nO que você faz? (atacar / defender / fugir)";
            commandInput.text = "";
            commandInput.ActivateInputField();
            currentState = GameState.PlayerTurn;
        }
    }

    private void ResolvePlayerFlee()
    {
        string msg =
            "Você decidiu fugir da batalha.\n" +
            "Você escapa em segurança, mas a luta termina aqui.";

        EnterBattleEnd(msg);
    }

    private void EnterBattleEnd(string finalMessage)
    {
        currentState = GameState.BattleEnd;
        messageText.text = finalMessage + "\n\n(Fim da batalha.)";
        commandInput.text = "";
        commandInput.DeactivateInputField();
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

        hero.Name = input;
        EnterHeroClassState();
    }

    private void HandleHeroClassInput(string input)
    {
        HeroClass heroClass;

        if (!TryParseHeroClass(input, out heroClass))
        {
            heroClass = HeroClass.Default;
        }

        hero.ApplyClass(heroClass);
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
        float roll = Random.value;

        if (roll < 0.5f)
        {
            return new EnemyStats(EnemyType.Slime);
        }
        else
        {
            return new EnemyStats(EnemyType.Esqueleto);
        }
    }

    private AcaoJogador ParsePlayerAction(string input)
    {
        string lower = input.ToLower();

        if (lower.Contains("atac"))
            return AcaoJogador.Atacar;

        if (lower.Contains("defen"))
            return AcaoJogador.Defender;

        if (lower.Contains("fug") || lower.Contains("sair"))
            return AcaoJogador.Fugir;

        return AcaoJogador.Invalida;
    }

    private int CalculateDamage(int attack, int defense)
    {
        // Dano base simples: ataque - (defesa * 0.2), com variação aleatória
        float defenseFactor = 1f - (defense * 0.02f); // cada ponto de defesa reduz 2% do ataque
        defenseFactor = Mathf.Clamp(defenseFactor, 0.5f, 1.0f); // não deixar reduzir demais

        float raw = attack * defenseFactor;

        // Variação leve para não ficar sempre igual
        float variation = Random.Range(0.8f, 1.2f);
        int finalDamage = Mathf.RoundToInt(raw * variation);

        if (finalDamage < 1)
            finalDamage = 1;

        return finalDamage;
    }

    private bool EnemyDodgedAttack()
    {
        float roll = Random.value;
        return roll < enemyDodgeChance;
    }

    private bool EnemyCounterAttackOnDodge()
    {
        float roll = Random.value;
        return roll < enemyCounterChanceOnDodge;
    }

    #endregion
}
