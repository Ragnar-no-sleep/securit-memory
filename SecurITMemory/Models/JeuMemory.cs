namespace SecurITMemory.Models;

// Gestionnaire de partie : maintient la liste des cartes, applique la logique
// de sélection / vérification de paire et expose les compteurs (essais, paires).
// La couche UI (JeuForm) ne fait qu'appeler Selectionner et lire l'état.
public class JeuMemory
{
    private readonly List<Carte> _cartes = new();
    private Carte? _premiereSelection;
    private Carte? _secondeSelection;

    public IReadOnlyList<Carte> Cartes => _cartes;
    public int TaillePlateau { get; }
    public int NombreEssais { get; private set; }
    public int PairesTrouvees { get; private set; }
    public int PairesTotales => _cartes.Count / 2;
    public bool EstTermine => PairesTrouvees == PairesTotales;

    // Vrai quand deux cartes sont révélées et qu'on attend le retournement
    // déclenché par le timer de délai. Tant que c'est vrai, les nouveaux clics sont ignorés
    // (sécurité contre le piège du 3e clic mentionné dans le cahier des charges).
    public bool AttenteRetournement => _premiereSelection is not null && _secondeSelection is not null;

    public JeuMemory(int taillePlateau, IList<string> imagesDisponibles)
    {
        if (taillePlateau % 2 != 0)
            throw new ArgumentException("Le nombre total de cartes doit être pair.", nameof(taillePlateau));

        int nbPaires = taillePlateau / 2;
        if (imagesDisponibles.Count < nbPaires)
            throw new ArgumentException($"Il faut au moins {nbPaires} images pour cette taille de plateau.");

        TaillePlateau = taillePlateau;
        InitialiserCartes(nbPaires, imagesDisponibles);
        Melanger();
    }

    // Pour chaque image on instancie deux cartes avec le même ID : ce sont les deux moitiés d'une paire.
    private void InitialiserCartes(int nbPaires, IList<string> imagesDisponibles)
    {
        for (int id = 0; id < nbPaires; id++)
        {
            string image = imagesDisponibles[id];
            _cartes.Add(new Carte(id, image));
            _cartes.Add(new Carte(id, image));
        }
    }

    // Mélange Fisher-Yates : O(n), uniforme, in-place.
    // On utilise Random.Shared (thread-safe, depuis .NET 6) plutôt qu'une nouvelle instance
    // pour éviter les répétitions de seed quand on relance plusieurs parties rapidement.
    private void Melanger()
    {
        var rng = Random.Shared;
        for (int i = _cartes.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (_cartes[i], _cartes[j]) = (_cartes[j], _cartes[i]);
        }
    }

    // Point d'entrée appelé par l'UI à chaque clic sur une carte.
    // Le résultat indique à l'UI ce qu'elle doit faire (rafraîchir, démarrer le timer, etc.).
    public ResultatSelection Selectionner(Carte carte)
    {
        // On ignore les clics non valides : carte déjà révélée/trouvée, ou attente de retournement.
        if (carte.Etat != EtatCarte.Cachee || AttenteRetournement)
            return ResultatSelection.Ignoree;

        carte.Reveler();

        // Premier clic d'une paire potentielle : on mémorise et on attend le second.
        if (_premiereSelection is null)
        {
            _premiereSelection = carte;
            return ResultatSelection.PremiereCarte;
        }

        // Second clic : on a un essai complet à comptabiliser.
        _secondeSelection = carte;
        NombreEssais++;

        if (_premiereSelection.EstPaireAvec(_secondeSelection))
        {
            // Paire trouvée : les deux cartes restent visibles définitivement.
            _premiereSelection.MarquerTrouvee();
            _secondeSelection.MarquerTrouvee();
            PairesTrouvees++;
            ReinitialiserSelection();
            return ResultatSelection.PaireTrouvee;
        }

        // Paire ratée : l'UI doit lancer le timer de délai, puis appeler RetournerCartesNonAppariees.
        return ResultatSelection.PaireRatee;
    }

    // Appelé par l'UI au tick du timer de délai pour recacher les deux cartes ratées.
    public void RetournerCartesNonAppariees()
    {
        _premiereSelection?.Cacher();
        _secondeSelection?.Cacher();
        ReinitialiserSelection();
    }

    private void ReinitialiserSelection()
    {
        _premiereSelection = null;
        _secondeSelection = null;
    }
}

// Communique à la couche UI le résultat d'un clic, sans qu'elle ait à inspecter
// l'état interne du jeu — bonne séparation des responsabilités.
public enum ResultatSelection
{
    Ignoree,
    PremiereCarte,
    PaireTrouvee,
    PaireRatee
}
