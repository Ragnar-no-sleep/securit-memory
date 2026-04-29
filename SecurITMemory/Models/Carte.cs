namespace SecurITMemory.Models;

// Classe métier représentant une carte du jeu Memory.
// Encapsulation : tous les champs sont privés, accès via propriétés.
// L'état n'est modifiable qu'en passant par les méthodes Reveler / Cacher / MarquerTrouvee
// pour empêcher un appelant externe de mettre la carte dans un état incohérent.
public class Carte
{
    // Identifiant partagé par les deux cartes d'une même paire.
    // Lecture seule depuis l'extérieur : on fixe l'ID au constructeur et il ne change plus.
    public int Id { get; }

    // Chemin de l'image affichée quand la carte est révélée.
    public string CheminImage { get; }

    // État courant : Cachee (départ), Revelee (cliquée), Trouvee (paire validée).
    // private set => seules les méthodes de cette classe peuvent le changer.
    public EtatCarte Etat { get; private set; } = EtatCarte.Cachee;

    public Carte(int id, string cheminImage)
    {
        Id = id;
        CheminImage = cheminImage;
    }

    // Une carte ne peut être révélée que si elle est encore cachée (idempotent).
    public void Reveler()
    {
        if (Etat == EtatCarte.Cachee)
            Etat = EtatCarte.Revelee;
    }

    // Recache une carte uniquement si elle était révélée (on ne "décache" pas une carte trouvée).
    public void Cacher()
    {
        if (Etat == EtatCarte.Revelee)
            Etat = EtatCarte.Cachee;
    }

    // Marque la carte comme appariée définitivement.
    public void MarquerTrouvee() => Etat = EtatCarte.Trouvee;

    // Deux cartes forment une paire si elles partagent le même Id.
    public bool EstPaireAvec(Carte autre) => autre is not null && Id == autre.Id;
}
