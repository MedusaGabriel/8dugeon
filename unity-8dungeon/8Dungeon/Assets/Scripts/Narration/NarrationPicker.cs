using System;
using System.Collections.Generic;

public class NarrationPicker
{
    private readonly System.Random _rng = new System.Random();
    private readonly Dictionary<string, int> _last = new Dictionary<string, int>();

    public string Pick(string setKey, NarrationEvent evt, List<string> lines)
    {
        if (lines == null || lines.Count == 0) return null;
        if (lines.Count == 1) return lines[0];

        string k = $"{setKey}|{evt}";
        _last.TryGetValue(k, out int last);

        int idx = _rng.Next(lines.Count);
        if (idx == last)
            idx = (idx + 1 + _rng.Next(lines.Count - 1)) % lines.Count;

        _last[k] = idx;
        return lines[idx];
    }
}
