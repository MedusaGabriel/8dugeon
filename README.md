# 🎮 8dugeon - RPG por Turno

Um RPG por turno completo desenvolvido com Next.js, TypeScript, Tailwind CSS e Material-UI.

## 🎯 Sobre o Jogo

8dugeon é um RPG de exploração de masmorras onde você controla um personagem em batalhas por turno. Escolha sua classe, explore salas misteriosas, enfrente criaturas perigosas e evolua seu personagem!

### ✨ Características

- **5 Classes Jogáveis**: Guerreiro, Mago, Ladino, Clérigo e Bardo
- **Sistema de Batalha por Turno**: Combates estratégicos com diferentes ataques e habilidades
- **Exploração de Masmorra**: Salas proceduralmente geradas com encontros aleatórios
- **Sistema de Progressão**: Level up, ganhe XP, gold e melhore seus atributos
- **Bestiário Completo**: 9 tipos diferentes de criaturas com níveis variados
- **Interface Moderna**: Design responsivo com Tailwind CSS e componentes MUI

## 🚀 Como Começar

### Pré-requisitos

- Node.js 18+ instalado
- npm ou yarn

### Instalação

```bash
# Clone o repositório
git clone https://github.com/MedusaGabriel/8dugeon.git

# Entre na pasta
cd 8dugeon

# Instale as dependências
npm install
```

### Rodar em Desenvolvimento

```bash
npm run dev
```

Abra [http://localhost:3000](http://localhost:3000) no seu navegador.

### Build para Produção

```bash
npm run build
npm start
```

## 📁 Estrutura do Projeto

```
src/
├── app/                    # Páginas e rotas (Next.js App Router)
│   ├── dungeon/           # Página principal do jogo
│   ├── creatures/         # Bestiário
│   ├── classes/           # Classes jogáveis
│   └── status/            # Status do personagem
├── components/            # Componentes React
│   ├── game/             # Componentes específicos do jogo
│   └── ui/               # Componentes de UI reutilizáveis
├── lib/                   # Lógica do jogo
│   └── game/
│       ├── actions.ts     # Sistema de ações e dano
│       ├── battleEngine.ts # Motor de batalha
│       └── world.ts       # Geração de masmorras
├── data/                  # Dados do jogo
│   ├── creatures.ts       # Todas as criaturas
│   ├── classes.ts         # Todas as classes
│   ├── attacks.ts         # Todos os ataques
│   └── status.ts          # Efeitos de status
├── store/                 # Gerenciamento de estado (Zustand)
│   └── gameStore.ts       # Store principal do jogo
└── types/                 # TypeScript types
    └── game.d.ts          # Interfaces do jogo
```

## 🎮 Como Jogar

1. **Criar Personagem**: Escolha um nome e uma classe
2. **Explorar**: Navegue pela masmorra escolhendo direções
3. **Combater**: Use seus ataques estrategicamente contra inimigos
4. **Evoluir**: Ganhe XP, suba de nível e fique mais forte
5. **Sobreviver**: Gerencie HP e Mana para continuar sua jornada

### Classes Disponíveis

- **⚔️ Guerreiro**: Alto HP e defesa, especialista em combate corpo a corpo
- **🔮 Mago**: Alto dano mágico e mana, fraco em defesa
- **🗡️ Ladino**: Alto crítico e velocidade, ataques furtivos
- **✨ Clérigo**: Equilibrado, pode curar e atacar
- **🎵 Bardo**: Versátil, buffs e debuffs poderosos

## 🛠️ Tecnologias

- **Framework**: Next.js 14 (App Router)
- **Linguagem**: TypeScript
- **Estilização**: Tailwind CSS + Material-UI (MUI)
- **Estado Global**: Zustand
- **Ícones**: Material Icons
- **Deploy**: Vercel

## 📦 Deploy no Vercel

[![Deploy with Vercel](https://vercel.com/button)](https://vercel.com/new/clone?repository-url=https://github.com/MedusaGabriel/8dugeon)

Ou manualmente:

```bash
npm i -g vercel
vercel login
vercel
```

## 🎨 Recursos do Código

- ✅ TypeScript com tipos fortes
- ✅ Arquitetura modular e escalável
- ✅ Componentização React
- ✅ Estado global com Zustand
- ✅ ESLint + Prettier configurados
- ✅ Design responsivo
- ✅ Animações CSS

## 📝 Licença

Este projeto é open source e está disponível sob a licença MIT.

## 🤝 Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues e pull requests.

---

Desenvolvido com ❤️ por [MedusaGabriel](https://github.com/MedusaGabriel)
