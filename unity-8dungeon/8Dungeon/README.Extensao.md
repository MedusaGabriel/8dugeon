# 8Dungeon - Guia de Extensão

Este guia resume passos práticos para expandir o protótipo sem alterar a estrutura existente.

## Adicionando uma Nova Classe de Herói

1. **Criar ScriptableObject**
   - Menu: *Assets > Create > 8Dungeon > Hero Class Data*.
   - Preencha nome, pontos de vida, ataque, defesa e descrição.
   - Salve em `Assets/Resources/HeroClasses` para que o `HeroFactory` carregue automaticamente.
2. **Registrar Narrações**
   - Abra `Assets/Resources/Data/hero_narrations.json`.
   - Crie um novo conjunto com a mesma `key` definida no ScriptableObject.
   - Forneça textos para eventos relevantes (ataque, defesa, análise, vitória, derrota).
3. **Atualizar UI (opcional)**
   - Ajuste o prompt do `GameController` se quiser sugerir a nova classe ao jogador.

## Adicionando um Novo Inimigo

1. **Configurar Arquétipo**
   - Menu: *Assets > Create > 8Dungeon > Enemy Type Data*.
   - Defina atributos, alcance, chances de esquiva/contra-ataque e tags.
   - Salve em `Assets/Resources/Enemies`.
2. **Narrativas do Inimigo**
   - Edite `Assets/Resources/Data/enemy_narrations.json` (ou arquivo equivalente) adicionando entradas para o identificador do arquétipo.
   - Inclua textos para aparição, ataque, dano recebido e morte.
3. **Balanceamento**
   - Ajuste as probabilidades de encontro no `GameController` ou crie lógica adicional no `EnemyFactory` para controlar frequência.

## Expandindo Comandos de Batalha

1. **Enum de Ação**
   - Acrescente a nova opção ao enum `AcaoJogador` em `Assets/Scripts/Commands/PlayerAction.cs`.
2. **Parser**
   - Ajuste `CommandParser.ParsePlayerAction` para reconhecer palavras-chave que representem a ação.
3. **Lógica de Combate**
   - Crie método específico em `BattleSystem` com o comportamento desejado.
   - Atualize `GameController.HandlePlayerTurnInput` para chamar o novo método e tratar o `BattleRoundResult`.
4. **Narrativas**
   - Inclua falas correspondentes nos arquivos de herói/inimigo para cobrir o novo evento.

## Novas Ações de Exploração

1. **Parser**
   - Estenda `ExplorationCommandParser` para interpretar o novo verbo ou formato de entrada.
2. **ExplorationManager**
   - Implemente métodos auxiliares para mutar o estado (por exemplo, abrir portas, interagir com objetos).
   - Atualize `ExplorationMoveResult` se precisar devolver informações extra.
3. **UI**
   - Use o `GridViewController` ou componentes adicionais para apresentar feedback visual.
4. **Narrativa Ambiental**
   - Opcionalmente, crie um novo repositório de textos ou reaproveite o `NarrationController` para narrativas de exploração.

## Integração de Sistemas Adicionais

- **Inventário simples**: mantenha uma lista de itens no `GameController`, serialize dados em ScriptableObjects e exiba na UI com um painel adicional.
- **Habilidades especiais**: modele habilidades como ScriptableObjects, associe-as a classes de herói e invoque efeitos específicos dentro de novos métodos do `BattleSystem`.
- **Eventos aleatórios**: implemente um serviço dedicado (por exemplo, `EventManager`) e invoque-o a partir do `ExplorationManager` para disparar encontros não combativos.

## Boas Práticas ao Estender

- Preserve a separação entre dados (ScriptableObjects/JSON) e lógica (Systems). Isso permite alterar conteúdo sem recompilar código.
- Centralize mudanças no `GameController` apenas quando precisar introduzir novos estados ou fluxos; demais comportamentos devem residir em classes especializadas.
- Prefira adicionar métodos às entidades (`HeroStats`, `EnemyStats`) quando precisar modificar atributos com regras consistentes (por exemplo, `ApplyDamage`, `RestoreHealth`).
- Para recursos carregados em `Resources`, mantenha pastas coesas e utilize prefixos nos nomes dos assets para facilitar buscas (`HeroClass_Mago`, `Enemy_Slime`, etc.).
