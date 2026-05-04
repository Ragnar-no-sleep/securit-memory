using Microsoft.Data.Sqlite;

namespace SecurITMemory.Models;

// Persistance des scores dans une base SQLite locale (scores.db a cote de l'executable).
// Migration depuis l'ancienne implementation JSON : meme interface publique
// (Charger / Ajouter / Top), donc aucune modification n'est necessaire cote UI.
public static class Leaderboard
{
    private static readonly string DbPath = Path.Combine(AppContext.BaseDirectory, "scores.db");
    private static readonly string ConnectionString = $"Data Source={DbPath}";

    static Leaderboard()
    {
        Initialiser();
    }

    private static void Initialiser()
    {
        using var conn = new SqliteConnection(ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS scores (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                joueur TEXT NOT NULL,
                taille_plateau INTEGER NOT NULL,
                essais INTEGER NOT NULL,
                temps_secondes INTEGER NOT NULL,
                theme TEXT NOT NULL DEFAULT 'Cybersecurite',
                hardcore INTEGER NOT NULL DEFAULT 0,
                date TEXT NOT NULL
            );
            CREATE INDEX IF NOT EXISTS idx_scores_taille ON scores(taille_plateau, essais, temps_secondes);";
        cmd.ExecuteNonQuery();
    }

    public static void Ajouter(Score score)
    {
        using var conn = new SqliteConnection(ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO scores (joueur, taille_plateau, essais, temps_secondes, theme, hardcore, date)
            VALUES ($joueur, $taille, $essais, $temps, $theme, $hardcore, $date);";
        cmd.Parameters.AddWithValue("$joueur", score.Joueur);
        cmd.Parameters.AddWithValue("$taille", score.TaillePlateau);
        cmd.Parameters.AddWithValue("$essais", score.Essais);
        cmd.Parameters.AddWithValue("$temps", score.TempsSecondes);
        cmd.Parameters.AddWithValue("$theme", score.Theme);
        cmd.Parameters.AddWithValue("$hardcore", score.Hardcore ? 1 : 0);
        cmd.Parameters.AddWithValue("$date", score.Date.ToString("o"));
        cmd.ExecuteNonQuery();
    }

    public static List<Score> Charger()
    {
        var resultats = new List<Score>();
        using var conn = new SqliteConnection(ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT joueur, taille_plateau, essais, temps_secondes, theme, hardcore, date
            FROM scores
            ORDER BY essais ASC, temps_secondes ASC;";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            resultats.Add(new Score
            {
                Joueur = reader.GetString(0),
                TaillePlateau = reader.GetInt32(1),
                Essais = reader.GetInt32(2),
                TempsSecondes = reader.GetInt32(3),
                Theme = reader.GetString(4),
                Hardcore = reader.GetInt32(5) == 1,
                Date = DateTime.Parse(reader.GetString(6))
            });
        }
        return resultats;
    }

    public static List<Score> Top(int taillePlateau, int n = 10)
    {
        var resultats = new List<Score>();
        using var conn = new SqliteConnection(ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT joueur, taille_plateau, essais, temps_secondes, theme, hardcore, date
            FROM scores
            WHERE taille_plateau = $taille
            ORDER BY essais ASC, temps_secondes ASC
            LIMIT $n;";
        cmd.Parameters.AddWithValue("$taille", taillePlateau);
        cmd.Parameters.AddWithValue("$n", n);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            resultats.Add(new Score
            {
                Joueur = reader.GetString(0),
                TaillePlateau = reader.GetInt32(1),
                Essais = reader.GetInt32(2),
                TempsSecondes = reader.GetInt32(3),
                Theme = reader.GetString(4),
                Hardcore = reader.GetInt32(5) == 1,
                Date = DateTime.Parse(reader.GetString(6))
            });
        }
        return resultats;
    }
}
