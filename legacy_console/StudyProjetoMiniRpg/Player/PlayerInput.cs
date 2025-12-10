using System;

public static class PlayerInput
{
    private static readonly HeroInputSynonymsRepository _synonymsRepo =
        new HeroInputSynonymsRepository();

    public static ComandoJogador LerComandoJogador(Hero heroi)
    {
        while (true)
        {
            Console.Write("O que você faz? ");
            string? texto = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(texto))
            {
                Console.WriteLine("Não entendi sua ação. Tente descrever de outra forma.");
                continue;
            }

            texto = texto.ToLower().Trim();

            var comando = new ComandoJogador
            {
                TextoOriginal = texto
            };

            string classe = heroi.classe?.ToLower() ?? "";

            HeroInputSynonymsDefinition defs = _synonymsRepo.GetMergedForClass(classe);

            if (ContemQualquer(texto, defs.Fugir))
            {
                comando.AcaoBase = AcaoJogador.Fugir;
                return comando;
            }

            if (ContemQualquer(texto, defs.Analisar))
            {
                comando.AcaoBase = AcaoJogador.Analisar;
                return comando;
            }

            if (ContemQualquer(texto, defs.Defender))
            {
                comando.AcaoBase = AcaoJogador.Defender;
                return comando;
            }

            if (ContemQualquer(texto, defs.AtaqueForte))
            {
                comando.AcaoBase = AcaoJogador.Atacar;
                comando.AtaqueForte = true;
                return comando;
            }

            if (ContemQualquer(texto, defs.Atacar))
            {
                comando.AcaoBase = AcaoJogador.Atacar;
                return comando;
            }

            Console.WriteLine("Não entendi bem sua intenção. Você queria atacar, defender, analisar ou fugir?");
            string? resposta = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(resposta))
                continue;

            resposta = resposta.ToLower().Trim();

            if (resposta.StartsWith("atac"))
            {
                comando.AcaoBase = AcaoJogador.Atacar;
                return comando;
            }
            if (resposta.StartsWith("defend") || resposta.Contains("escudo"))
            {
                comando.AcaoBase = AcaoJogador.Defender;
                return comando;
            }
            if (resposta.StartsWith("analis") || resposta.Contains("examinar"))
            {
                comando.AcaoBase = AcaoJogador.Analisar;
                return comando;
            }
            if (resposta.StartsWith("fug") || resposta.Contains("correr"))
            {
                comando.AcaoBase = AcaoJogador.Fugir;
                return comando;
            }

            Console.WriteLine("Ainda não consegui entender sua ação. Tente descrever de outra forma.");
        }
    }

    private static bool ContemQualquer(string texto, System.Collections.Generic.List<string>? termos)
    {
        if (termos == null || termos.Count == 0)
            return false;

        foreach (var termo in termos)
        {
            if (string.IsNullOrWhiteSpace(termo)) 
                continue;

            if (texto.Contains(termo.ToLower()))
                return true;
        }

        return false;
    }
}
