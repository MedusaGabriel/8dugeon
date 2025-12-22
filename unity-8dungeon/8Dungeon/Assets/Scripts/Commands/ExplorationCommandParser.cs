using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

public static class ExplorationCommandParser
{
    private static readonly Regex DigitsRegex = new Regex(@"(\d+)", RegexOptions.Compiled);
    private static readonly Dictionary<string, int> WordNumbers = new Dictionary<string, int>
    {
        {"um", 1},
        {"uma", 1},
        {"primeiro", 1},
        {"dois", 2},
        {"duas", 2},
        {"segundo", 2},
        {"tres", 3},
        {"três", 3},
        {"terceiro", 3},
        {"quatro", 4},
        {"quinto", 5},
        {"cinco", 5}
    };

    public static bool TryParseSteps(string input, out int steps, out string feedback)
    {
        steps = 0;
        feedback = null;

        if (string.IsNullOrWhiteSpace(input))
        {
            feedback = "Comando vazio. Diga quantos passos deseja avançar (até 5).";
            return false;
        }

        string lower = RemoveAccents(input.ToLowerInvariant());

        if (!lower.Contains("avanc") && !lower.Contains("andar") && !lower.Contains("mover"))
        {
            feedback = "Para explorar, descreva quantos passos você quer avançar.";
            return false;
        }

        Match match = DigitsRegex.Match(lower);
        if (match.Success)
        {
            if (int.TryParse(match.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed))
            {
                steps = ClampSteps(parsed);
                return true;
            }
        }

        foreach (KeyValuePair<string, int> pair in WordNumbers)
        {
            if (lower.Contains(pair.Key))
            {
                steps = ClampSteps(pair.Value);
                return true;
            }
        }

        // Se não encontrar número, assume 1 passo
        steps = 1;
        return true;
    }

    private static int ClampSteps(int value)
    {
        if (value < 1)
        {
            value = 1;
        }
        if (value > 5)
        {
            value = 5;
        }
        return value;
    }

    private static string RemoveAccents(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        string normalized = text.Normalize(System.Text.NormalizationForm.FormD);
        System.Text.StringBuilder builder = new System.Text.StringBuilder(normalized.Length);

        foreach (char c in normalized)
        {
            var cat = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (cat != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                builder.Append(c);
            }
        }

        return builder.ToString().Normalize(System.Text.NormalizationForm.FormC);
    }
}
