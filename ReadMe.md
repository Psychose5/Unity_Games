# Games Projekt Titel 
Game:                   Arkanoid - Infinity Edition
Mitglieder:             Marcel Sandhöfner (215984)
Unity Version:          6000.0.69f1

## Verwendete Assets

- Nutzung grundeinstellungen aus dem Unterricht

## Steuerung

| Taste | Funktion |
| :---: | :---: |
| **A** | Nach links bewegen |
| **D** | Nach rechts bewegen |

## Beschreibung des Projektes
Bei dem Projekt handelt es sich um das spiel Arknoid, in dem es darum geht Blöcke mit hilfe eines Balls zu zerstören. Um dies zu erreichen bekommt der Player die Kontrolle über ein Paddle(Block) mit dem er durch zurück schlagen des Balls die Blöcke zerstören soll und den Ball dabei nicht aus der Arena fliegen lassen soll.
Es können auch Powerups spawnen die der Player nutzen kann. 

## Features
- bewegliches Paddle (Player)
    - reflektiert Ball
- Ball macht schaden an Blöcken
- Blöcke spawnen mit random lebenszahl
    - farbe stehen für bestimmte anzahl an leben (max 3 leben)
        - 1 Leben = grün
        - 2 Leben = gelb
        - 3 Leben = rot
- Leben (in Ballform) werden bei Arenaaus weniger
    - und life up mehr 
- Score steigt/sinkt durch Blocktreffer, Ballaus und Abilities einsammeln
- Abilities können spawnen 
- Mögliche Abilities - spawnchance für alle gleich
    - BlockPaddle (Paddlefarbener Block) = Ball prallt von Block ab wie von Paddle
    - Sizeup (Grüner Block) = Paddle wird größer 
    - Sizedown (gelber Block) = Paddle wird kleiner
    - Lifeup (grüne Kapsel) = + 1 Leben
    - Speedup (grüner Ball) = Ball speed up
    - Speeddown (gelber Ball) = Ball speed down
    - Nuke (schwarzroter Zylinder) = -1 hit von jedem Block
- Blöcke werden nach vorne geschoben und Blockreihe spawned nach
    - bei min. 10 zerstörten Blöcken (durch nächsten Paddle hit)
    - bei Arenaout und Reset des Balls
- 8 Reihen werden die Blöcke der ersten Reihe zerstört


## Score
- block -1 leben = 50 punkte
- block zerstört = 100 Punkte (keine 50 extra)
- Leben -1 = -100 Punkte
- zerstörung vorderste blockreihe = -(100 + ((Blockleben - 1) * 50)) per Block
    - wenn spiel vorderste blockreihe zerstören muss
- Abilities:
    - BlockPaddle = 100 Punkte
    - Sizedown = 100 Punkte
    - Sizeup = -100 Punkte
    - Lifeup = -100 Punkte
    - Speedup = 100 Punkte
    - Speeddown = -100 Punkte
    - Nuke = Anzahl Block zerstört + Anzahl Block -1 leben
## (Optional) Besondere Herausforderungen / Lessions Learned
- richtige Reaktion des Balls wenn er den Block vorne/hinten oder seitlich trifft


## Video
Abgabe.mkv = Mit in zip