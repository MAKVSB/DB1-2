# Databázové systémy II, 2022/2023, Test PS/SQL, Varianta: A

Doba trvání: 75min
Maximální počet bodů/minimální počet bodů **15/8**

Pouze kompletní a korektní vypracování úkolu je hodnoceno nenulovým počtem bodů. Za jeden vypracovaný úkol dostanete 8b za dva 15b.

# Úkoly

1. Napište procedutu, která bude mít na vstupu id hry, jméno a typ rozhodčího. Procedura nejprve zkontroluje, jestli hra s daným id existuje a pokud ne, tak vypíše "Hra neexistuje" a procedura se ukonči. Pot= zkontroluje, zda typ rozhodčího je _referee_ nebo _linesman_. Pokud ani jedno, procedura vypíše "Neexistující typ rozhodčího" a procedura se ukončí. Procedura následně zkontroluje, zda se v dané sezóně už rozhodčí nezůčastnil více než 100 her a pokud ne, přiřadí rozhodčího na danou hru. Vopačném případě procedura vypíše "Rozhodčí příliš zaneprázdněn" a procedura se ukončí. Procedura bude řešena jako transakce.

2. Přidejte do tabulky _team_info_ atribut aggresivity typu int s možnými hodnotami (0,1,2). Vytvořte funkci, která pro každý tým spočítá, kolik útoků na jiné hráče mají na svědomí (útokem se rozumí game*plays.event = 'Penalty' a současně game_plays_players.playertype = 'DrewBy'). Pokud je počet penalt menší než 750, nastaví atribut danému týmu na 0. Je-li v rozmezí 750-1250, hostující nebo domácí (je nutné tedy spočítat penalty pro *team_id_for* a také *team_id_against\*). Funkce bude řešena jako transakce a bude vracet počet zpracovaných týmů.
