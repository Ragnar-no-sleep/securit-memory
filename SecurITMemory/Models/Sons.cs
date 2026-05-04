using System.Media;

namespace SecurITMemory.Models;

// Joue les effets sonores du jeu via System.Media.SoundPlayer.
// Les WAV sont dans Assets/Sounds/ et copies en sortie via le csproj.
// Si un fichier manque ou le hardware audio echoue, on n'interrompt pas la partie.
public static class Sons
{
    public const string Clic = "click.wav";
    public const string Succes = "success.wav";
    public const string Echec = "fail.wav";
    public const string Victoire = "victory.wav";

    public static bool Active { get; set; } = true;

    public static void Jouer(string nomFichier)
    {
        if (!Active) return;

        try
        {
            string chemin = Path.Combine(AppContext.BaseDirectory, "Assets", "Sounds", nomFichier);
            if (!File.Exists(chemin)) return;

            using var player = new SoundPlayer(chemin);
            player.Play();
        }
        catch (Exception)
        {
            // Pas de son, mais on ne crashe pas l'application.
        }
    }
}
