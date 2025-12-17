using UnityEngine;

public class BattleRoundResult
{
    public string Message;
    public bool BattleEnded;
    public bool PlayerDied;
    public bool EnemyDied;
}

public class BattleSystem
{
    private readonly HeroStats _hero;
    private readonly EnemyStats _enemy;

    private readonly float _enemyDodgeChance;
    private readonly float _enemyCounterChanceOnDodge;
    private readonly float _playerPerfectBlockChance;
    
    private bool _firstEncounter = true;
    private bool _enemyRevealed;

    private const string UnknownEnemySubject = "A criatura desconhecida";
    private const string UnknownEnemyObject = "a criatura desconhecida";

    public BattleSystem(
        HeroStats hero,
        EnemyStats enemy,
        float enemyDodgeChance,
        float enemyCounterChanceOnDodge,
        float playerPerfectBlockChance
    )
    {
        _hero = hero;
        _enemy = enemy;
        _enemyDodgeChance = Mathf.Clamp01(enemyDodgeChance);
        _enemyCounterChanceOnDodge = Mathf.Clamp01(enemyCounterChanceOnDodge);
        _playerPerfectBlockChance = Mathf.Clamp01(playerPerfectBlockChance);
        _enemyRevealed = false;
        
        // Narração ao encontrar o inimigo
        var narration = NarrationController.Instance;
        if (narration != null)
        {
            narration.Say(hero.ClassKey, NarrationEvent.EncounterStart);
            narration.SayEnemy(_enemy, _enemyRevealed, EnemyNarrationEvent.Appear, EnemyNarrationName);
        }
    }

    // ====== AÇÕES DO JOGADOR ======

    public BattleRoundResult PlayerAttack()
    {
        var narration = NarrationController.Instance;
        narration?.Say(_hero.ClassKey, NarrationEvent.PlayerAttack);
        
        var result = new BattleRoundResult();

        // 1) Tenta desviar
        bool dodged = EnemyDodgedAttack();

        if (dodged)
        {
            narration?.SayEnemy(_enemy, _enemyRevealed, EnemyNarrationEvent.Defend, EnemyNarrationName);

            string msg =
                $"Você atacou {EnemyObject}, mas o ataque foi desviado!";

            if (_enemyRevealed)
            {
                msg += $"\n{EnemySubject} permanece ileso ({_enemy.CurrentHp}/{_enemy.MaxHp} HP).";
            }
            else
            {
                msg += "\nA criatura continua sem revelar sua verdadeira forma.";
            }

            // 2) Pode contra-atacar
            bool counter = EnemyCounterAttackOnDodge();
            if (counter)
            {
                narration?.SayEnemy(_enemy, _enemyRevealed, EnemyNarrationEvent.Attack, EnemyNarrationName);
                msg += $"\n\n{EnemySubject} aproveita a abertura e contra-ataca!";
                string afterCounter = EnemyAttackInternal(msg, playerDefending: false, result: result);
                result.Message = afterCounter;
            }
            else
            {
                // Apenas desviou, sem contra-ataque
                result.Message = msg;
                result.BattleEnded = false;
                result.PlayerDied = false;
                result.EnemyDied = false;
            }

            return result;
        }
        else
        {
            // Não desviou: leva dano normal
            int damage = CalculateDamage(_hero.Attack, _enemy.Defense);
            _enemy.CurrentHp = Mathf.Max(0, _enemy.CurrentHp - damage);

            narration?.SayEnemy(_enemy, _enemyRevealed, EnemyNarrationEvent.Hurt, EnemyNarrationName);

            string msg =
                $"Você atacou {EnemyObject} e causou {damage} de dano.";

            if (_enemyRevealed)
            {
                msg += $"\n{EnemySubject} agora está com {_enemy.CurrentHp}/{_enemy.MaxHp} HP.";
            }
            else
            {
                msg += "\nMesmo ferida, a criatura ainda não se revela.";
            }

            if (_enemy.CurrentHp <= 0)
            {
                narration?.SayEnemy(_enemy, _enemyRevealed, EnemyNarrationEvent.Death, EnemyNarrationName);
                msg += "\n\nO inimigo foi derrotado! Você venceu a batalha.";
                result.Message = msg;
                result.BattleEnded = true;
                result.PlayerDied = false;
                result.EnemyDied = true;
                return result;
            }
            else
            {
                // Inimigo ainda está vivo, turno dele (sem defesa)
                string afterAttack = EnemyAttackInternal(msg, playerDefending: false, result: result);
                result.Message = afterAttack;
                return result;
            }
        }
    }

    public BattleRoundResult PlayerDefend()
    {
        // Narração ao defender
        NarrationController.Instance.Say(_hero.ClassKey, NarrationEvent.PlayerDefend);
        
        var result = new BattleRoundResult();

        string msg =
            $"Você se prepara para defender o ataque de {EnemyObject}...";

        string afterDefense = EnemyAttackInternal(msg, playerDefending: true, result: result);
        result.Message = afterDefense;
        return result;
    }
    
    public BattleRoundResult PlayerAnalyze()
    {
        // Narração ao analisar
        var narration = NarrationController.Instance;
        narration?.Say(_hero.ClassKey, NarrationEvent.PlayerAnalyze);
        
        var result = new BattleRoundResult();

        bool alreadyRevealed = _enemyRevealed;
        _enemyRevealed = true;

        if (!alreadyRevealed)
        {
            narration?.SayEnemy(_enemy, true, EnemyNarrationEvent.Reveal, _enemy.Name);
        }

        string intro = alreadyRevealed
            ? $"Você reavalia {_enemy.Name} em busca de novas fraquezas...\n\n"
            : "Você analisa a criatura desconhecida com toda atenção...\n\n";

        string details =
            $"Nome: {_enemy.Name}\n" +
            $"HP: {_enemy.CurrentHp}/{_enemy.MaxHp}\n" +
            $"Ataque: {_enemy.Attack}\n" +
            $"Defesa: {_enemy.Defense}\n\n";

        string msg = intro + details +
            $"{EnemySubject} aproveita a distração e ataca!";

        string afterAnalyze = EnemyAttackInternal(msg, playerDefending: false, result: result);
        result.Message = afterAnalyze;
        return result;
    }

    public BattleRoundResult PlayerFlee()
    {
        var result = new BattleRoundResult();

        string msg =
            "Você decidiu fugir da batalha.\n" +
            "Você escapa em segurança, mas a luta termina aqui.";

        result.Message = msg;
        result.BattleEnded = true;
        result.PlayerDied = false;
        result.EnemyDied = false;

        return result;
    }

    // ====== LÓGICA INTERNA DO ATAQUE DO INIMIGO ======

    private string EnemyAttackInternal(string previousMessage, bool playerDefending, BattleRoundResult result)
    {
        int damage;
        string defenseText = "";
        var narration = NarrationController.Instance;

        if (playerDefending)
        {
            float roll = Random.value;

            if (roll < _playerPerfectBlockChance)
            {
                damage = 0;
                defenseText = "Você defendeu completamente o ataque e não sofreu dano!";
            }
            else
            {
                int fullDamage = CalculateDamage(_enemy.Attack, _hero.Defense);
                damage = Mathf.RoundToInt(fullDamage * 0.5f);
                if (damage < 1) damage = 1;
                defenseText = $"Você reduziu o dano pela metade, mas ainda sofreu {damage} de dano.";
            }
        }
        else
        {
            damage = CalculateDamage(_enemy.Attack, _hero.Defense);
        }

        narration?.SayEnemy(_enemy, _enemyRevealed, EnemyNarrationEvent.Attack, EnemyNarrationName);

        _hero.CurrentHp = Mathf.Max(0, _hero.CurrentHp - damage);

        string msg = previousMessage + "\n\n";

        if (playerDefending)
        {
            msg += $"{EnemySubject} atacou!\n{defenseText}\n";
        }
        else
        {
            msg += $"{EnemySubject} atacou você e causou {damage} de dano.\n";
        }

        msg += $"Seu HP: {_hero.CurrentHp}/{_hero.MaxHp}";

        if (_hero.CurrentHp <= 0)
        {
            msg += "\n\nVocê foi derrotado.";
            result.BattleEnded = true;
            result.PlayerDied = true;
            result.EnemyDied = false;
        }
        else
        {
            result.BattleEnded = false;
            result.PlayerDied = false;
            result.EnemyDied = false;
        }

        return msg;
    }

    // ====== FUNÇÕES DE SUPORTE ======

    private int CalculateDamage(int attack, int defense)
    {
        float defenseFactor = 1f - (defense * 0.02f);
        defenseFactor = Mathf.Clamp(defenseFactor, 0.5f, 1.0f);

        float raw = attack * defenseFactor;
        float variation = Random.Range(0.8f, 1.2f);
        int finalDamage = Mathf.RoundToInt(raw * variation);

        if (finalDamage < 1)
            finalDamage = 1;

        return finalDamage;
    }

    private bool EnemyDodgedAttack()
    {
        float roll = Random.value;
        return roll < _enemyDodgeChance;
    }

    private bool EnemyCounterAttackOnDodge()
    {
        float roll = Random.value;
        return roll < _enemyCounterChanceOnDodge;
    }

    private string EnemySubject => _enemyRevealed ? $"O {_enemy.Name}" : UnknownEnemySubject;
    private string EnemyObject => _enemyRevealed ? $"o {_enemy.Name}" : UnknownEnemyObject;
    private string EnemyNarrationName => _enemyRevealed ? _enemy.Name : "o inimigo desconhecido";
}
