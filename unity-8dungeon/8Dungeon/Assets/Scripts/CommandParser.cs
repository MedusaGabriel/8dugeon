public static class CommandParser
{
    public static AcaoJogador ParsePlayerAction(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return AcaoJogador.Invalida;

        string lower = input.ToLower();

        if (lower.Contains("atac"))
            return AcaoJogador.Atacar;

        if (lower.Contains("defen"))
            return AcaoJogador.Defender;

        if (lower.Contains("fug") || lower.Contains("sair"))
            return AcaoJogador.Fugir;

        return AcaoJogador.Invalida;
    }
}
