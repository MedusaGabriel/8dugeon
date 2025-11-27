// Console.WriteLine("Exercicio para praticar conceitos básicos de C#");

// // Atividade 4  e ultima, Cálculo de cura com Limite de vida 
// // basicamente o heroi vai ter um HP MAXIMO, HP ATUAL e HP CURA 
// //eu tenho que pegar o hp atual + hp cura e ser o valor for maior que o hp maximo
// // eu tenho que retorna o hp maximo sem ultrapassa o valor e basicamente o excesso seria uma cura extra descartada

// int hpMax = 250;
// int hpAtual = 132;

// Console.WriteLine("Escreva o valor da cura: ");
// int cura = int.Parse(Console.ReadLine());

// int hpHealed = hpAtual + cura;

// if (hpHealed > hpMax)
// {
//     hpHealed = hpMax;
// }

// Console.WriteLine("HP antes: " + hpAtual);
// Console.WriteLine("Cura recebida: " + cura);
// Console.WriteLine("HP final: " + hpHealed);















// //Ativdade 3 Identificador de classe
// // basicamente eu vou pedir para o usuario escreve uma classe, e vou dizer ser ela e uma classe focada em
// // magia, força e precisção (mago, guerreiro, arquiero)
// // ser for uma classe invalidade eu vou retorna que é uma classe invalidade 
// // em codigo seria algo mais ou menos assim 

// Console.WriteLine("Escolha sua classe: Mago, Guerreiro ou Arqueiro");
// string escolhaDeClasse = Console.ReadLine().ToLower(); // o certo aquis eria usa  string escolhaDeClasse = Console.ReadLine().ToLower();
// // pois sempre vai fica com letra minscula dai não seria necessario fazer a verificação com letra maiuscula e minuscula

// if (escolhaDeClasse == "mago")
// {
//     Console.WriteLine("Classe focada em magia");
// }
// else if (escolhaDeClasse == "guerreiro")
// {
//     Console.WriteLine("Classe focada em força");
// }
// else if (escolhaDeClasse == "arqueiro")
// {
//     Console.WriteLine("Classe focada em precisão");
// }
// else
// {
//     Console.WriteLine("Classe inválida");
// }




















// Console.WriteLine("Exercicio 1 - Sistema simples de nível");

// int nivelJogador = 1;
// int xpLevelUp = 100;

// Console.WriteLine("O jogador está no nível: " + nivelJogador);

// Console.WriteLine("Digite aqui quanto de xp o jogador ganhou:");
// int xpGained = int.Parse(Console.ReadLine());

// void CalcularNivel(int xp)
// {
//     if (xp >= xpLevelUp)
//     {
//         nivelJogador++;
//     }
// }

// CalcularNivel(xpGained);

// if (xpGained == 0)
// {
//     Console.WriteLine("O jogador não ganhou XP e continua no nível " + nivelJogador);
// }
// else if (xpGained < xpLevelUp)
// {
//     Console.WriteLine("O jogador ganhou " + xpGained + " de XP");
//     Console.WriteLine("Faltam " + (xpLevelUp - xpGained) + " para subir de nível.");
// }
// else
// {
//     Console.WriteLine("Parabéns! O jogador ganhou " + xpGained + " de XP e subiu de nível!");
//     Console.WriteLine("Novo nível: " + nivelJogador);
// }
//--------------------------------

// Console.WriteLine("Atividade 2 - Calculadora de dano bruto");

// int defesaInimigo = 50;
// Console.WriteLine("Essa é o valor da defesa do Inimigo " +defesaInimigo);
// Console.WriteLine("Qual o valor do Ataque do jogador?");
// Console.WriteLine("Digite o ataque do jogador:");
// int ataqueJogador = int.Parse(Console.ReadLine());

// // Ser o ataque do jogador for maior que a defesa do inimigo, calcular o dano bruto
// //logo, temos 3 tipos de condições
// // 1- o ataque ser maior que a defesa, exemplo: ataque 70 > defesa 50 
// // logo o dano que o inimigo levou vai ser (70 - 50 ) = 20
// // 2 o ataque ser igual a defesa, exemplo ataque 50 = defesa 50
// // logo o dano que o inimigo levou vai ser 0 
// // e o 3 o ataque ser menor que a defesa, exemplo ataque 30 < defesa 50
// // logo o dano que o inimigo levou tbm vai ser 0 
// // em codigo isso seria algo como 

// if ( ataqueJogador == defesaInimigo)
// {
//     Console.WriteLine("O ataque do jogador é igual a defesa do inimigo, logo o dano bruto é 0");
// }
// else if (ataqueJogador < defesaInimigo)
// {
//     Console.WriteLine("O ataque do jogador é menor que a defesa do inimigo, logo o dano bruto é 0");
// }
// else 
// {
//     int danoBruto = ataqueJogador - defesaInimigo;
//     Console.WriteLine("O ataque do jogador é maior que a defesa do inimigo, logo o dano bruto é: " + danoBruto);
// }

// Console.WriteLine("Atividade 2 - Calculadora de dano bruto- versão IA ");

// int defesaInimigo = 50;
// Console.WriteLine("Defesa do inimigo: " + defesaInimigo);

// Console.WriteLine("Digite o ataque do jogador:");
// int ataqueJogador = int.Parse(Console.ReadLine());

// // Se o ataque for maior que a defesa, há dano. Caso contrário, dano 0.
// int danoBruto;

// if (ataqueJogador > defesaInimigo)
// {
//     danoBruto = ataqueJogador - defesaInimigo;
//     Console.WriteLine("Dano bruto causado: " + danoBruto);
// }
// else
// {
//     danoBruto = 0;
//     Console.WriteLine("O ataque não superou a defesa. Dano bruto: 0");
// }

//--------------------------------


















// Console.WriteLine("Exericio 5 - Mini Combate Numerico");

// int vidaInimigo = 10;
// Console.WriteLine("Diga a def do Inimigo: ");
// string defInimigo = Console.ReadLine();
// int defEnemy = int.Parse(defInimigo);  

// Console.WriteLine("Diga o atk do Jogador: ");
// string atkJogador = Console.ReadLine();
// int atkPlayer = int.Parse(atkJogador);

// void AtacarInimigo(){
//     if (atkPlayer > defEnemy)
//     {
//         vidaInimigo -= (atkPlayer - defEnemy);
//         Console.WriteLine("Dano Causado "+ atkPlayer  +" - "+ defEnemy +" = "+ (atkPlayer - defEnemy));
//         Console.WriteLine("Você atacou o inimigo! Vida restante do inimigo: " + vidaInimigo);
//     }
// }

// Console.WriteLine("Vida do Inimigo: " + vidaInimigo);
// Console.WriteLine("Defesa do Inimigo: " + defEnemy);
// Console.WriteLine("Ataque do jogador: " + atkPlayer);

// AtacarInimigo();
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