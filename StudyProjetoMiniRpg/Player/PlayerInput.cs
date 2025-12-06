using System;

public static class PlayerInput
{
    public static AcaoJogador LerAcaoJogador()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("O que você quer fazer? (Atacar, Defender ou Fugir... talvez Analisar?)");
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine($"O seu personagem ,não entendeu sua ação, escreva algo que ele entenda");
                continue;
            }

            string texto = input.ToLower();

            if (ContemQualquer(texto, new[] { "atac", "bater", "golpe" }))
            {
                return AcaoJogador.Atacar;
            }

            // Defender: 'defend', 'defesa', 'proteg'
            if (ContemQualquer(texto, new[] { "defend", "defesa", "proteg" }))
            {
                return AcaoJogador.Defender;
            }

            // Fugir: 'fug', 'correr', 'sair', 'escapar'
            if (ContemQualquer(texto, new[] { "fug", "correr", "sair", "escapar" }))
            {
                return AcaoJogador.Fugir;
            }
            if (ContemQualquer(texto, new[] { "analis", "examinar", "observar", "info", "informação" }))
            {
                return AcaoJogador.Analisar;
            }


            Console.WriteLine("Não entendi sua intenção. Tente algo como: 'quero atacar', 'vou defender', 'quero fugir'.");
        }
    }

    private static bool ContemQualquer(string texto, string[] chaves)
    {
        foreach (string chave in chaves)
        {
            if (texto.Contains(chave))
            {
                return true;
            }
        }
        return false;
    }
}