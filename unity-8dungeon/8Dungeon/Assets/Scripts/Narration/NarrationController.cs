using System;
using UnityEngine;

public class NarrationController : MonoBehaviour
{
    public static NarrationController Instance { get; private set; }

    private IHeroNarrationRepository _heroRepo;
    private IEnemyNarrationRepository _enemyRepo;
    private event Action<string> _onNarration;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        var heroCfg = HeroNarrationsLoader.Load();
        _heroRepo = new HeroNarrationRepository(heroCfg, new NarrationPicker());

        var enemyCfg = EnemyNarrationsLoader.Load();
        if (enemyCfg != null)
        {
            _enemyRepo = new EnemyNarrationRepository(enemyCfg);
        }
    }

    public void Say(string heroClassKey, NarrationEvent evt)
    {
        if (_heroRepo == null) return;
        string line = _heroRepo.GetLine(heroClassKey, evt);

        if (string.IsNullOrEmpty(line))
        {
            return;
        }

        _onNarration?.Invoke(line);
    }

    public void SayEnemy(EnemyStats enemy, bool revealed, EnemyNarrationEvent evt, string displayName)
    {
        if (_enemyRepo == null || enemy == null) return;

        string line = _enemyRepo.GetLine(enemy.Archetype, revealed, evt);
        if (string.IsNullOrWhiteSpace(line))
        {
            return;
        }

        string name = string.IsNullOrWhiteSpace(displayName)
            ? enemy.Name
            : displayName;

        line = line.Replace("{enemyName}", name);
        _onNarration?.Invoke(line);
    }

    public void RegisterListener(Action<string> listener)
    {
        if (listener == null) return;
        _onNarration += listener;
    }

    public void UnregisterListener(Action<string> listener)
    {
        if (listener == null) return;
        _onNarration -= listener;
    }
}
