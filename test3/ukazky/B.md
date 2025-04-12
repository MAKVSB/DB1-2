# Databázové systémy II, 2022/2023, Test PS/SQL, Varianta: B

Doba trvání: 75min
Maximální počet bodů/minimální počet bodů **15/8**

Pouze kompletní a korektní vypracování úkolu je hodnoceno nenulovým počtem bodů. Za jeden vypracovaný úkol dostanete 8b za dva 15b.

# Úkoly

1. Vytvořte tabulku _officials_ s atributy _official_id, official_name a official_type_ (stejného datového typu jako atribut _official_type_ v tabulce _gaame_officials_). Následně vytvořte bez-parametrickou funkci, která každého hráče z tabulky _game_officials_ zkopíruje do tabulky _officials_, přičemž _official_type_ mu nastavte podle toho, jakého rozhodčího dělal častěji. V připadě, že zorhodčí byl stajný počet krát v roli čárového rozhodčího (_linesman_) jako hlavního rozhodčího (_referee_), nastaví se _official_type_ na hodnotu _undefined_. Nezapomeňte, že jednotliví rozhodčí se v tabulce _game_officials_ nacházení více než jednou. Funkce bude vracet počet zpracovaných rozhodčích a bude řešena jako transakce.

2. Vytvořte trigger _TDeleteGameEvent_, který bude spuštěn před smazáním záznamu z tabulky _Game_plays_. Trigger ověří o jaký typ události šlo. Pokud šlo o gód (event = 'Goal'), odečte tento gól z atributu _home_goals_/_away_goals_ (podle toho, který tým dal mazaný gól) a také tento gód odpočte danému týmu z tabulky _game_teams_stats_ (atribut _goals_). Pokud šlo pouze o střelu (event = 'Shot'), odečte tuto střelu danému týmu z tabulky _game_teams_stats_ (atribut _shots_).
