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
    }

    // ====== AÇÕES DO JOGADOR ======

    public BattleRoundResult PlayerAttack()
    {
        var result = new BattleRoundResult();

        // 1) Tenta desviar
        bool dodged = EnemyDodgedAttack();

        if (dodged)
        {
            string msg =
                $"Você atacou o {_enemy.Name}, mas ele DESVIOU do seu ataque!\n" +
                $"HP do inimigo: {_enemy.CurrentHp}/{_enemy.MaxHp}";

            // 2) Pode contra-atacar
            bool counter = EnemyCounterAttackOnDodge();
            if (counter)
            {
                msg += $"\n\nO {_enemy.Name} aproveita a abertura e contra-ataca!";
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

            string msg =
                $"Você atacou o {_enemy.Name} e causou {damage} de dano.\n" +
                $"HP do inimigo: {_enemy.CurrentHp}/{_enemy.MaxHp}";

            if (_enemy.CurrentHp <= 0)
            {
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
        var result = new BattleRoundResult();

        string msg =
            $"Você se prepara para defender o ataque do {_enemy.Name}...";

        string afterDefense = EnemyAttackInternal(msg, playerDefending: true, result: result);
        result.Message = afterDefense;
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

        _hero.CurrentHp = Mathf.Max(0, _hero.CurrentHp - damage);

        string msg = previousMessage + "\n\n";

        if (playerDefending)
        {
            msg += $"{_enemy.Name} atacou!\n{defenseText}\n";
        }
        else
        {
            msg += $"O {_enemy.Name} atacou você e causou {damage} de dano.\n";
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
}
