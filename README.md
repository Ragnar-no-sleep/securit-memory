# SecurIT Memory

Mini-jeu de cartes Memory aux couleurs de **SecurIT**, start-up de cybersécurité, conçu pour le stand du Salon de l'Innovation Tech.

C# / WinForms — Visual Studio 2022 — .NET 8.

## Équipe

| Membre | Périmètre |
|---|---|
| **Titouan Corneloup** | Logique métier — `Models/EtatCarte.cs`, `Models/Carte.cs`, `Models/JeuMemory.cs` |
| **Robin Goncalves** | Interface WinForms — `Forms/MenuForm`, `Forms/OptionsForm`, `Forms/JeuForm` |
| **Stanislaz Faure** | Bonus & assets — `Models/Score.cs`, `Models/Leaderboard.cs`, `Models/Sons.cs`, `Forms/ScoresForm`, `Assets/` |

## Lancer le projet

1. Ouvrir `SecurITMemory.sln` dans **Visual Studio 2022**.
2. Charge de travail requise : « Développement .NET pour bureau ».
3. F5 (NuGet restore automatique pour `Microsoft.Data.Sqlite`).

Tous les assets sont fournis : 4 banques de 19 images PNG (Cyber, Matériel, Logiciel, Cryptographie) et 4 fichiers WAV (clic, succès, échec, victoire).

## Architecture (3 couches)

```
SecurITMemory/
├── Program.cs                  Point d'entrée → MenuForm
├── Models/                     Logique & Données
│   ├── EtatCarte.cs            enum Cachee / Revelee / Trouvee
│   ├── Carte.cs                Classe Carte encapsulée
│   ├── JeuMemory.cs            Liste, mélange, vérification, MelangerCachees pour Hardcore
│   ├── BibliothequeImages.cs   enum Theme + catalogue par thème
│   ├── Sons.cs                 Helper SoundPlayer
│   ├── Score.cs                Entrée du leaderboard
│   └── Leaderboard.cs          Persistance SQLite
├── Forms/                      Interface
│   ├── MenuForm.cs             Menu : Jouer / Options / Scores / Quitter
│   ├── OptionsForm.cs          Taille, thème, mode Hardcore
│   ├── JeuForm.cs              Grille avec bordures et hover, 3 timers (délai, chronomètre, Hardcore), cache d'images
│   └── ScoresForm.cs           Tableau des scores avec lignes alternées
└── Assets/
    ├── Cards/Cyber|Materiel|Logiciel|Crypto/   76 PNG (4 thèmes)
    └── Sounds/                 4 WAV
```

## Logique de jeu

1. **Mélange** des cartes via Fisher-Yates au démarrage.
2. **Révélation** au clic. `JeuMemory.Selectionner` ignore les clics quand deux cartes sont déjà révélées (anti-bug du 3ᵉ clic).
3. **Vérification** des IDs à la 2ᵉ carte : paire trouvée → `EtatCarte.Trouvee` ; sinon `Timer` de 1,2 s puis retournement automatique.
4. **Chronomètre** mis à jour chaque seconde par un second `Timer`.
5. **Victoire** détectée quand toutes les paires sont trouvées → saisie du nom et enregistrement dans le leaderboard.

## Choix techniques

- **Encapsulation stricte** sur `Carte` : champs privés, `Etat` modifiable uniquement par `Reveler` / `Cacher` / `MarquerTrouvee`.
- **`enum EtatCarte`** plutôt que des booléens combinés : empêche les états incohérents.
- **`enum ResultatSelection`** renvoyé par `Selectionner` pour ne pas exposer l'état interne du jeu à la couche UI.
- **Fisher-Yates + `Random.Shared`** pour un mélange uniforme en O(n) sans seed dupliqué.
- **`System.Windows.Forms.Timer`** (et non `System.Timers.Timer`) : tick sur le thread UI, pas d'`Invoke` requis.
- **Cache d'images** statique dans `JeuForm.ChargerImage` pour éviter de relire le disque à chaque rafraîchissement.
- **SQLite via `Microsoft.Data.Sqlite`** pour la persistance des scores : index sur `(taille_plateau, essais, temps_secondes)` pour des `Top` rapides.

## Bonus implémentés

### 1. Leaderboard SQLite

Table `scores` avec un index dédié au tri du Top. Les colonnes `theme` et `hardcore` permettent de filtrer / segmenter les classements selon le mode de partie.

### 2. Quatre thèmes de cartes

Sélecteur dans Options :

- **Cybersécurité** (par défaut) — cadenas, pare-feu, virus, 2FA, hash…
- **Matériel** — RAM, CPU, GPU, SSD, ventilo…
- **Logiciel** — Linux, Windows, Git, Docker, IDE…
- **Cryptographie** — clés publique/privée, AES, RSA, JWT, OAuth…

Chaque thème a ses 18 paires + son dos de carte.

### 3. Effets sonores

`System.Media.SoundPlayer` joue 4 WAV générés en synthèse PCM :

- `click.wav` — pulse 1500 Hz à la révélation
- `success.wav` — arpège ascendant C-E-G sur paire trouvée
- `fail.wav` — glissando descendant 600→200 Hz sur paire ratée
- `victory.wav` — arpège C-E-G-C' en fin de partie

`Sons.Active = false` permet de couper le son.

### 4. Mode Hardcore

Activable dans Options. Toutes les **30 secondes**, les cartes encore cachées sont permutées aléatoirement (Fisher-Yates sur les indices Cachée), pendant que les cartes en cours d'essai et les paires déjà trouvées restent en place. Le mode est tracé dans le score pour distinguer les performances Hardcore au leaderboard.
