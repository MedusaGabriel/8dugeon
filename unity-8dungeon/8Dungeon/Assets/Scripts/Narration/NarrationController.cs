using UnityEngine;

public class NarrationController : MonoBehaviour
{
    public static NarrationController Instance { get; private set; }

    private IHeroNarrationRepository _repo;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        var cfg = HeroNarrationsLoader.Load();
        _repo = new HeroNarrationRepository(cfg, new NarrationPicker());
    }

    public void Say(string heroClassKey, NarrationEvent evt)
    {
        if (_repo == null) return;
        Debug.Log(_repo.GetLine(heroClassKey, evt));
    }
}
