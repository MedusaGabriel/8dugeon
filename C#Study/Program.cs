Console.WriteLine("Exericio 5 - Mini Combate Numerico");

int vidaInimigo = 10;
Console.WriteLine("Diga a def do Inimigo: ");
string defInimigo = Console.ReadLine();
int defEnemy = int.Parse(defInimigo);  

Console.WriteLine("Diga o atk do Jogador: ");
string atkJogador = Console.ReadLine();
int atkPlayer = int.Parse(atkJogador);

void AtacarInimigo(){
    if (atkPlayer > defEnemy)
    {
        vidaInimigo -= (atkPlayer - defEnemy);
        Console.WriteLine("Dano Causado "+ atkPlayer  +" - "+ defEnemy +" = "+ (atkPlayer - defEnemy));
        Console.WriteLine("Você atacou o inimigo! Vida restante do inimigo: " + vidaInimigo);
    }
}

Console.WriteLine("Vida do Inimigo: " + vidaInimigo);
Console.WriteLine("Defesa do Inimigo: " + defEnemy);
Console.WriteLine("Ataque do jogador: " + atkPlayer);

AtacarInimigo();
//--------------------------------

// Console.WriteLine("Exercicio 4 -  Média");

// Console.WriteLine("Vamos calcular sua média de notas!");

// Console.WriteLine("Esreva sua primeira nota: ");

// string nota1 = Console.ReadLine();
// Console.WriteLine("Esreva sua segunda nota: ");
// string nota2 = Console.ReadLine();
// Console.WriteLine("Esreva sua terceira nota: " );
// string nota3 = Console.ReadLine();

// double nt1 = double.Parse(nota1);
// double nt2 = double.Parse(nota2);
// double nt3 = double.Parse(nota3);

// double media = (nt1 + nt2 + nt3) / 3;

// static void AvaliarMedia(double media)
// {
//     if (media > 7)
//     {
//         Console.WriteLine("Aprovado!");
//     }else if (media < 5 )
//     {
//         Console.WriteLine("Reprovado!");
//     }else if (media >= 5 && media <= 7)
//     {
//         Console.Write("Recuperação!");
//     }
// }

// Console.WriteLine("Sua avaliação é: "+media);
// AvaliarMedia(media);
//--------------------------------

// Console.WriteLine("Exercicio 3 - Verificação de didade");

// Console.WriteLine("Me diga sua idade: ");
// string idade = Console.ReadLine();
// int idadeNum = int.Parse(idade);

// if (idadeNum > 18)
// {
//     Console.WriteLine("Você é maior de idade! pode acessa conteudo do X");
// }
// else if (idadeNum < 18)
// {
//     Console.WriteLine("Você é menor de idade não pode acessar conteudo do X");
// }

//--------------------------------

// Console.WriteLine("Exercicio 2 - Cálculo Simples");

// Console.WriteLine("Esreva o primeiro numero para o calculo");
// string number1 = Console.ReadLine();
// Console.WriteLine("Otimo agora segundo numero");
// string number2 = Console.ReadLine();
// int num1 = int.Parse(number1);
// int num2 = int.Parse(number2);

// Console.WriteLine("Vou lhe dar soma, subtração, multiplicação e deivisão desses dois numeros ok?");

// int soma = num1 + num2;
// int subtracao = num1 - num2;
// int multiplicacao = num1 * num2;
// int divisao = num1 / num2;

// Console.WriteLine("Soma: " + soma + " Subtração: " + subtracao + " Multiplicação: " + multiplicacao + " Divisão: " + divisao);

//--------------------------------

// Console.WriteLine("Exercicio 1 - Saudação ao usuário");

// Console.WriteLine("Esresva seu nome:");
// string nome = Console.ReadLine();
// Console.WriteLine("Otimo agora me diga sua idade e vou lhe fazer uma saudação "+ nome+" !");
// string idade = Console.ReadLine();
// Console.WriteLine("Obrigado por compartilhar seu nome comigo "+nome+" e sua idade é de "+idade+" anos!");