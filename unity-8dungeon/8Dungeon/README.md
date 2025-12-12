# 8Dungeon - Arquitetura do Projeto

## 📋 Visão Geral

**8Dungeon** é um jogo RPG de batalha baseado em texto desenvolvido em Unity. O projeto implementa um sistema de combate por turnos onde o jogador cria um herói, enfrenta inimigos e toma decisões estratégicas durante a batalha.

## 🏗️ Arquitetura Geral

O projeto segue uma arquitetura modular baseada em **separação de responsabilidades**, dividida em camadas:

1. **Camada de Controle (Controllers)** - Gerenciam o fluxo do jogo e a interface
2. **Camada de Sistema (Systems)** - Implementam a lógica de negócio
3. **Camada de Dados (Data/Models)** - Definem estruturas e persistência
4. **Camada de Serviços (Services)** - Fornecem funcionalidades auxiliares
5. **Camada de Factory (Factories)** - Criam e inicializam entidades

---

## 📁 Estrutura de Pastas

```
Assets/
├── Scripts/
│   ├── GameController.cs           # Controlador principal do jogo
│   ├── Battle/                     # Sistema de batalha
│   ├── Commands/                   # Parsing de comandos do jogador
│   ├── Enemies/                    # Lógica e dados de inimigos
│   ├── Heroes/                     # Lógica e dados de heróis
│   ├── Narration/                  # Sistema de narrativas
│   └── Debug/                      # Ferramentas de debug
├── Resources/
│   ├── Data/                       # Arquivos JSON de configuração
│   ├── Enemies/                    # ScriptableObjects de inimigos
│   ├── HeroClasses/                # ScriptableObjects de classes
│   └── Narrations/                 # Dados de narração
├── Scenes/
│   └── BattleScene.unity           # Cena principal do jogo
├── Prefabs/                        # (Vazio no momento)
└── UI/                             # Elementos de interface
```

---

## 🎮 Camada de Controle

### **GameController.cs**
**Localização:** `Assets/Scripts/GameController.cs`

**Responsabilidade:** 
- Controlador principal que gerencia todo o fluxo do jogo
- Gerencia máquina de estados (State Machine)
- Conecta UI com a lógica de negócio
- Coordena criação de herói, introdução de inimigos e batalha

**Estados do jogo:**
```csharp
public enum GameState
{
    HeroName,      // Digitando nome do herói
    HeroClass,     // Selecionando classe
    EnemyIntro,    // Apresentação do inimigo
    PlayerTurn,    // Turno do jogador
    BattleEnd      // Fim da batalha
}
```

**Dependências:**
- `BattleController` - Para executar batalhas
- `HeroFactory` - Para criar heróis
- `EnemyFactory` - Para criar inimigos
- `CommandParser` - Para interpretar comandos

**Problemas atuais:**
- ⚠️ **Classe muito grande** (278 linhas) com múltiplas responsabilidades
- ⚠️ Gerencia UI diretamente (deveria delegar)
- ⚠️ Contém lógica de criação de entidades

---

### **BattleController.cs**
**Localização:** `Assets/Scripts/Battle/BattleController.cs`

**Responsabilidade:**
- Coordena as ações de batalha
- Serve como intermediário entre GameController e BattleSystem
- Gerencia referências a Herói e Inimigo

**Métodos principais:**
```csharp
public BattleRoundResult ExecutePlayerAction(AcaoJogador acao)
```

**Dependências:**
- `BattleSystem` - Para executar mecânicas de batalha
- `HeroStats` e `EnemyStats` - Entidades da batalha

**Observação:**
- ✅ Classe bem focada e enxuta (62 linhas)
- ✅ Segue Single Responsibility Principle

---

## ⚔️ Camada de Sistema

### **BattleSystem.cs**
**Localização:** `Assets/Scripts/Battle/BattleSystem.cs`

**Responsabilidade:**
- Implementa toda a mecânica de combate
- Calcula dano, esquivas, contra-ataques e defesas
- Gerencia probabilidades de eventos de batalha
- Produz resultados de cada rodada

**Métodos principais:**
```csharp
public BattleRoundResult PlayerAttack()
public BattleRoundResult PlayerDefend()
public BattleRoundResult PlayerFlee()
private void EnemyTurn(BattleRoundResult result)
```

**Configurações de batalha:**
- `enemyDodgeChance` - Chance de inimigo esquivar
- `enemyCounterChanceOnDodge` - Chance de contra-ataque após esquiva
- `playerPerfectBlockChance` - Chance de bloquear perfeitamente

**Dependências:**
- `NarrationController` - Para adicionar narrativas aos eventos
- `HeroStats` e `EnemyStats` - Para modificar status

**Problemas atuais:**
- ⚠️ **Classe grande** (247 linhas)
- ⚠️ Mistura cálculo de dano com mensagens de batalha
- ⚠️ Lógica de turno do inimigo embutida

---

### **BattleTurn.cs**
**Localização:** `Assets/Scripts/Battle/BattleTurn.cs`

**Responsabilidade:**
- *Arquivo vazio ou não implementado*
- Provavelmente destinado a gerenciar ordem de turnos

---

## 👥 Camada de Entidades

### **HeroStats.cs**
**Localização:** `Assets/Scripts/Heroes/HeroStats.cs`

**Responsabilidade:**
- Define estrutura de dados do herói
- Armazena atributos (HP, Ataque, Defesa)

```csharp
public class HeroStats
{
    public string Name;
    public HeroClass Class;
    public string ClassKey;  // Para buscar narrações
    public int MaxHp;
    public int CurrentHp;
    public int Attack;
    public int Defense;
}
```

**Observação:**
- ✅ Estrutura simples e clara
- ⚠️ Poderia ter métodos de comportamento (TakeDamage, Heal, etc.)

---

### **HeroClassData.cs**
**Localização:** `Assets/Scripts/Heroes/HeroClassData.cs`

**Responsabilidade:**
- ScriptableObject que armazena dados de classes de herói
- Permite configuração via Unity Editor

---

### **EnemyStats.cs**
**Localização:** `Assets/Scripts/Enemies/EnemyStats.cs`

**Responsabilidade:**
- Define estrutura de dados do inimigo
- Inclui atributos extras (DodgeChance, CounterChance)

```csharp
public class EnemyStats
{
    public EnemyArchetype Archetype;
    public string Name;
    public int MaxHp, CurrentHp;
    public int Attack, Defense;
    public float DodgeChance;
    public float CounterChance;
}
```

---

### **EnemyTypeData.cs**
**Localização:** `Assets/Scripts/Enemies/EnemyTypeData.cs`

**Responsabilidade:**
- ScriptableObject para configuração de tipos de inimigos

---

## 🏭 Camada de Factory

### **HeroFactory.cs**
**Localização:** `Assets/Scripts/Heroes/HeroFactory.cs`

**Responsabilidade:**
- Cria instâncias de heróis baseado em classe escolhida
- Carrega dados de ScriptableObjects
- Aplica valores padrão se dados não forem encontrados

```csharp
public static HeroStats CreateHero(string name, HeroClass heroClass)
```

**Observação:**
- ✅ Separa lógica de criação do uso
- ✅ Implementa cache de dados

---

### **EnemyFactory.cs**
**Localização:** `Assets/Scripts/Enemies/EnemyFactory.cs`

**Responsabilidade:**
- Cria inimigos aleatórios
- Filtra arquétipos "Default" do sorteio
- Carrega dados de ScriptableObjects

```csharp
public static EnemyStats CreateRandomEnemy()
```

**Observação:**
- ✅ Boa separação de responsabilidades
- ✅ Sistema de fallback para dados faltantes

---

## 💬 Camada de Narrativa

### **NarrationController.cs** (Singleton)
**Localização:** `Assets/Scripts/Narration/NarrationController.cs`

**Responsabilidade:**
- Singleton que fornece acesso ao sistema de narração
- Coordena busca e exibição de textos narrativos

```csharp
public void Say(string heroClassKey, NarrationEvent evt)
```

**Dependências:**
- `HeroNarrationRepository` - Para buscar textos
- `HeroNarrationsLoader` - Para carregar configurações

---

### **HeroNarrationRepository.cs**
**Localização:** `Assets/Scripts/Narration/HeroNarrationRepository.cs`

**Responsabilidade:**
- Repositório que busca textos de narração por classe e evento
- Implementa fallback para default e mensagens genéricas

```csharp
public interface IHeroNarrationRepository
{
    string GetLine(string key, NarrationEvent evt);
}
```

**Observação:**
- ✅ Usa interface para permitir testes e substituição
- ✅ Sistema de fallback bem implementado

---

### **NarrationPicker.cs**
**Localização:** `Assets/Scripts/Narration/NarrationPicker.cs`

**Responsabilidade:**
- Seleciona aleatoriamente uma linha entre várias opções
- Evita repetição imediata de narrativas

---

### **NarrationService.cs**
**Localização:** `Assets/Scripts/Narration/NarrationService.cs`

**Responsabilidade:**
- Serviço alternativo de narração (parece estar duplicado com NarrationController)
- Carrega e processa templates de narração

**Problema:**
- ⚠️ **Duplicação de responsabilidade** com NarrationController
- ⚠️ Dois sistemas de narração diferentes coexistindo

---

### **Enums de Narração**

**NarrationEvent.cs** - Eventos de narração do herói:
```csharp
public enum NarrationEvent
{
    EncounterStart, PlayerAttack, PlayerDefend, 
    PlayerAnalyze, PlayerHit, EnemyHit, 
    Victory, Defeat
}
```

**NarrationKey.cs** - Chaves genéricas de narração:
```csharp
public enum NarrationKey
{
    PlayerTurnStart, EnemyTurnStart,
    PlayerAttackHit, EnemyAttackHit,
    EnemyDefeated, PlayerDefeated
}
```

**Problema:**
- ⚠️ Dois sistemas de chaves diferentes (NarrationEvent vs NarrationKey)

---

## 🎯 Camada de Comandos

### **CommandParser.cs**
**Localização:** `Assets/Scripts/Commands/CommandParser.cs`

**Responsabilidade:**
- Interpreta texto digitado pelo jogador
- Converte strings em ações do enum `AcaoJogador`

```csharp
public static AcaoJogador ParsePlayerAction(string input)
{
    // "atac" -> AcaoJogador.Atacar
    // "defen" -> AcaoJogador.Defender
    // "fug"/"sair" -> AcaoJogador.Fugir
}
```

**Observação:**
- ✅ Simples e eficaz
- ⚠️ Poderia ser expandido para sinônimos mais ricos

---

### **PlayerAction.cs**
**Localização:** `Assets/Scripts/Commands/PlayerAction.cs`

**Responsabilidade:**
- Define enum de ações do jogador

```csharp
public enum AcaoJogador
{
    Atacar, Defender, Fugir, Invalida
}
```

---

## 📊 Camada de Dados

### **hero_narrations.json**
**Localização:** `Assets/Resources/Data/hero_narrations.json`

**Responsabilidade:**
- Armazena todas as narrativas específicas de cada classe de herói
- Define textos para eventos como ataques, defesas, vitórias

**Estrutura:**
```json
{
  "defaultKey": "default",
  "sets": [
    {
      "key": "mago",
      "encounterStart": ["Minhas runas brilham..."],
      "playerAttack": ["Lanço um feitiço!"]
    }
  ]
}
```

---

### **HeroNarrationsLoader.cs & HeroNarrationsModels.cs**
**Localização:** `Assets/Resources/Data/`

**Responsabilidade:**
- Carrega e deserializa JSON de narrações
- Define modelos de dados para configuração

---

## 🔄 Fluxo de Execução

### 1. **Inicialização**
```
GameController.Start()
  └─> EnterHeroNameState()
      └─> Espera input do jogador
```

### 2. **Criação do Herói**
```
OnConfirmButtonPressed()
  └─> ParseHeroNameInput()
      └─> EnterHeroClassState()
          └─> ParseHeroClassInput()
              └─> HeroFactory.CreateHero(name, class)
                  └─> EnterEnemyIntroState()
```

### 3. **Início da Batalha**
```
EnterEnemyIntroState()
  └─> EnemyFactory.CreateRandomEnemy()
  └─> new BattleController(hero, enemy, configs)
      └─> new BattleSystem(...)
          └─> NarrationController.Say(EncounterStart)
```

### 4. **Loop de Batalha**
```
OnConfirmButtonPressed() [no estado PlayerTurn]
  └─> CommandParser.ParsePlayerAction(input)
      └─> BattleController.ExecutePlayerAction(action)
          └─> BattleSystem.PlayerAttack/Defend/Flee()
              ├─> Calcula resultados
              ├─> NarrationController.Say(eventos)
              ├─> EnemyTurn() se ainda vivo
              └─> return BattleRoundResult
                  └─> GameController atualiza UI
```

---

## 🔴 Problemas e Code Smells Identificados

### 1. **GameController muito grande**
- **Problema:** 278 linhas, múltiplas responsabilidades
- **Impacto:** Difícil manutenção e testes
- **Solução:** Separar em UIController + GameStateManager

### 2. **Duplicação de Sistema de Narração**
- **Problema:** `NarrationController` vs `NarrationService`
- **Impacto:** Confusão, código duplicado
- **Solução:** Escolher um sistema e remover o outro

### 3. **Enums de Narração Conflitantes**
- **Problema:** `NarrationEvent` vs `NarrationKey`
- **Impacto:** Inconsistência no código
- **Solução:** Unificar em um único enum ou esclarecer uso

### 4. **BattleSystem com muitas responsabilidades**
- **Problema:** Cálculo de dano + IA inimiga + mensagens
- **Impacto:** Difícil testar componentes isoladamente
- **Solução:** Separar em DamageCalculator + EnemyAI + CombatResolver

### 5. **Entidades sem comportamento**
- **Problema:** `HeroStats` e `EnemyStats` são apenas dados
- **Impacto:** Lógica espalhada em outros lugares
- **Solução:** Adicionar métodos como `TakeDamage()`, `IsAlive()`

### 6. **Falta de sistema de Input**
- **Problema:** Input tratado diretamente no GameController
- **Impacto:** Difícil adicionar outros tipos de input (botões, AI)
- **Solução:** Criar InputHandler ou InputManager

### 7. **Acoplamento direto com Unity**
- **Problema:** Lógica de negócio usa Debug.Log e MonoBehaviour
- **Impacto:** Difícil testar fora da Unity
- **Solução:** Injeção de dependências e interfaces

---

## ✅ Pontos Positivos

1. ✅ **Separação em pastas lógicas** - Fácil encontrar código
2. ✅ **Uso de Factories** - Boa separação de criação
3. ✅ **ScriptableObjects** - Configuração via editor
4. ✅ **Sistema de Narração** - Flexível e expansível
5. ✅ **Enums claros** - Facilita leitura do código
6. ✅ **BattleController enxuto** - Boa intermediação

---

## 📋 Resumo de Responsabilidades

| Componente | Responsabilidade | Tamanho | Status |
|------------|-----------------|---------|--------|
| **GameController** | Fluxo do jogo + UI | 278 linhas | 🔴 Muito grande |
| **BattleController** | Coordena batalha | 62 linhas | ✅ Adequado |
| **BattleSystem** | Mecânicas de combate | 247 linhas | 🟡 Grande |
| **HeroFactory** | Cria heróis | 60 linhas | ✅ Adequado |
| **EnemyFactory** | Cria inimigos | 58 linhas | ✅ Adequado |
| **NarrationController** | Singleton de narração | ~25 linhas | ✅ Adequado |
| **HeroNarrationRepository** | Busca textos | ~70 linhas | ✅ Adequado |
| **CommandParser** | Interpreta comandos | ~20 linhas | ✅ Adequado |

---

## 🎯 Próximos Passos Recomendados

### Alta Prioridade
1. **Refatorar GameController** - Separar UI e lógica de estado
2. **Resolver duplicação de narração** - Unificar sistemas
3. **Adicionar comportamento às entidades** - Métodos em Stats

### Média Prioridade
4. **Separar cálculo de dano** - Criar DamageCalculator
5. **Extrair IA do inimigo** - Criar EnemyAI component
6. **Criar InputManager** - Desacoplar input do controller

### Baixa Prioridade
7. **Implementar BattleTurn** - Se necessário para ordem de ação
8. **Adicionar testes unitários** - Começar pelos Factories
9. **Documentar APIs públicas** - Comentários XML

---

## 📚 Arquitetura Recomendada (Futuro)

```
GameManager (Singleton)
  ├─> StateManager
  │     ├─> HeroCreationState
  │     ├─> BattleState
  │     └─> GameOverState
  │
  ├─> BattleOrchestrator
  │     ├─> CombatResolver
  │     ├─> DamageCalculator
  │     └─> TurnManager
  │
  ├─> EntityManager
  │     ├─> HeroController
  │     └─> EnemyController (com EnemyAI)
  │
  ├─> NarrationManager (unificado)
  │     ├─> NarrationRepository
  │     └─> NarrationPicker
  │
  └─> UIController
        ├─> BattleUI
        └─> InputHandler
```

---

## 🛠️ Tecnologias e Padrões Utilizados

- **Unity 2022+** - Engine do jogo
- **C# 10** - Linguagem de programação
- **TextMeshPro** - Renderização de texto
- **ScriptableObjects** - Configuração de dados
- **JSON** - Persistência de narrativas
- **Factory Pattern** - Criação de entidades
- **Singleton Pattern** - NarrationController
- **State Machine** - GameState enum
- **Repository Pattern** - NarrationRepository

---

## 📝 Observações Finais

Este projeto está em um **estágio intermediário de desenvolvimento**. A estrutura base é sólida, mas precisa de refatoração para escalar melhor. O código atual funciona para um protótipo, mas requer melhorias arquiteturais antes de adicionar mais features.

**Data da documentação:** Dezembro 2025
