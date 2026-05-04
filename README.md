# SecurIT Memory

Mini-jeu de cartes Memory aux couleurs de **SecurIT**, start-up de cybersécurité, conçu pour le stand du Salon de l'Innovation Tech.

C# / WinForms — Visual Studio 2022 — .NET 8.

## Équipe

| Membre | Périmètre |
|---|---|
| **Titouan Corneloup** | Logique métier — `Models/EtatCarte.cs`, `Models/Carte.cs`, `Models/JeuMemory.cs` |
| **Robin Goncalves** | Interface WinForms — `Forms/MenuForm`, `Forms/OptionsForm`, `Forms/JeuForm` |
| **Stanislaz Faure** | Bonus & assets — `Models/Score.cs`, `Models/Leaderboard.cs`, `Forms/ScoresForm`, `Assets/Cards/` |

## Lancer le projet

1. Ouvrir `SecurITMemory.sln` dans **Visual Studio 2022**.
2. Charge de travail requise : « Développement .NET pour bureau ».
3. F5.

Les 19 images de cartes (1 dos + 18 icônes cyber) sont déjà fournies dans `SecurITMemory/Assets/Cards/`.

## Architecture (3 couches)

```
SecurITMemory/
├── Program.cs                  Point d'entrée → MenuForm
├── Models/                     Logique & Données
│   ├── EtatCarte.cs            enum Cachee / Revelee / Trouvee
│   ├── Carte.cs                Classe Carte encapsulée
│   ├── JeuMemory.cs            Liste des cartes, mélange, vérification de paire
│   ├── BibliothequeImages.cs   Catalogue d'images
│   ├── Score.cs                Entrée du leaderboard
│   └── Leaderboard.cs          Persistance JSON locale
├── Forms/                      Interface
│   ├── MenuForm.cs             Menu : Jouer / Options / Scores / Quitter
│   ├── OptionsForm.cs          Taille du plateau (4×4, 6×4, 6×6)
│   ├── JeuForm.cs              Grille + chronomètre + timer délai
│   └── ScoresForm.cs           Tableau des scores
└── Assets/Cards/               19 PNG (1 dos + 18 icônes cyber)
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

## Bonus — Leaderboard local

À chaque victoire, le score (joueur, plateau, essais, temps, date) est sauvegardé dans `scores.json` à côté de l'exécutable. Le menu principal donne accès au top 20, classé par essais puis par temps.
