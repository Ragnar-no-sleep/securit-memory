using System.Text.Json;

namespace SecurITMemory.Models;

// Persistance simple des scores dans un fichier JSON local.
// Choix volontaire : pas de SQL pour rester dans le périmètre du TP de base
// (la connexion SQL est listée comme bonus optionnel dans la consigne).
public static class Leaderboard
{
    private static readonly string Fichier = Path.Combine(
        AppContext.BaseDirectory, "scores.json");

    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    public static List<Score> Charger()
    {
        if (!File.Exists(Fichier)) return new List<Score>();
        try
        {
            string json = File.ReadAllText(Fichier);
            return JsonSerializer.Deserialize<List<Score>>(json) ?? new List<Score>();
        }
        catch (Exception)
        {
            // Fichier corrompu : on repart d'une liste vide plutôt que de crasher l'appli.
            return new List<Score>();
        }
    }

    public static void Ajouter(Score score)
    {
        var scores = Charger();
        scores.Add(score);
        File.WriteAllText(Fichier, JsonSerializer.Serialize(scores, Options));
    }

    // Top N pour une taille donnée, classé par essais puis par temps.
    public static List<Score> Top(int taillePlateau, int n = 10)
    {
        return Charger()
            .Where(s => s.TaillePlateau == taillePlateau)
            .OrderBy(s => s.Essais)
            .ThenBy(s => s.TempsSecondes)
            .Take(n)
            .ToList();
    }
}
