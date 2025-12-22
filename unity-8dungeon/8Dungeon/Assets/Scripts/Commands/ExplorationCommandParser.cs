using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

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

    public static bool TryParseMove(string input, out Vector2Int direction, out int steps, out string feedback)
    {
        direction = Vector2Int.zero;
        steps = 0;
        feedback = null;

        if (string.IsNullOrWhiteSpace(input))
        {
            feedback = "Comando vazio. Diga quantos passos deseja avançar (até 5).";
            return false;
        }

        string lower = RemoveAccents(input.ToLowerInvariant());

        if (!ContainsMovementVerb(lower))
        {
            feedback = "Para explorar, descreva quantos passos você quer avançar.";
            return false;
        }

        direction = ParseDirection(lower);

        Match match = DigitsRegex.Match(lower);
        if (match.Success && int.TryParse(match.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsedDigits))
        {
            steps = ClampSteps(parsedDigits);
            return true;
        }

        foreach (KeyValuePair<string, int> pair in WordNumbers)
        {
            if (lower.Contains(pair.Key))
            {
                steps = ClampSteps(pair.Value);
                return true;
            }
        }

        // Sem número explícito -> movimenta um passo
        steps = 1;
        return true;
    }

    private static bool ContainsMovementVerb(string text)
    {
        if (text.Contains("avanc") || text.Contains("andar") || text.Contains("mover") || text.Contains("moviment") || text.Contains("caminh") ||
            text.Contains("seguir") || text.Contains("prosseguir") || text.Contains("prossiga") || text.Contains("siga") || text.Contains("desloc"))
        {
            return true;
        }

        return text == "ir" || text.StartsWith("ir ") || text.Contains(" ir ") || text.EndsWith(" ir");
    }

    private static Vector2Int ParseDirection(string text)
    {
        if (text.Contains("direita"))
        {
            return Vector2Int.right;
        }

        if (text.Contains("esquerda"))
        {
            return Vector2Int.left;
        }

        if (text.Contains("tras") || text.Contains("trás") || text.Contains("voltar") || text.Contains("retornar"))
        {
            return Vector2Int.down;
        }

        if (text.Contains("cima") || text.Contains("frente") || text.Contains("adiante"))
        {
            return Vector2Int.up;
        }

        return Vector2Int.up;
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
