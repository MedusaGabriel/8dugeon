NarrationRepository.LoadFromJson("Data/enemy_narrations.json");
Enemy inimigo = EnemyFactory.CriarInimigoAleatorio();
Hero heroi = Hero.CriarHeroi();
BattleSystem.Executar(heroi, inimigo);
