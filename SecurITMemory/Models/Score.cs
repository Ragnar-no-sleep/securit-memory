namespace SecurITMemory.Models;

// Représente une entrée du leaderboard.
public class Score
{
    public string Joueur { get; set; } = "Anonyme";
    public int TaillePlateau { get; set; }
    public int Essais { get; set; }
    public int TempsSecondes { get; set; }
    public string Theme { get; set; } = "Cybersecurite";
    public bool Hardcore { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}
