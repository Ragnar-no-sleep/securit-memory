using SecurITMemory.Models;

namespace SecurITMemory.Forms;

public partial class ScoresForm : Form
{
    public ScoresForm()
    {
        InitializeComponent();
        Charger();
    }

    private void Charger()
    {
        listView.Items.Clear();
        
        // On récupère TOUS les scores, on les trie proprement par performance
        var tousLesScores = Leaderboard.Charger();
        
        if (tousLesScores.Count == 0)
        {
            listView.Items.Add(new ListViewItem("Aucun score enregistré") { ForeColor = Color.Gray });
            return;
        }

        var top20 = tousLesScores
            .OrderBy(s => s.Essais)
            .ThenBy(s => s.TempsSecondes)
            .Take(20);

        foreach (var s in top20)
        {
            var item = new ListViewItem(s.Joueur);
            item.SubItems.Add($"{s.TaillePlateau}");
            item.SubItems.Add(s.Essais.ToString());
            item.SubItems.Add($"{s.TempsSecondes / 60:00}:{s.TempsSecondes % 60:00}");
            item.SubItems.Add(s.Date.ToString("dd/MM/yyyy HH:mm"));
            
            // Alternance de couleur pour la lisibilité "Pro"
            if (listView.Items.Count % 2 == 0) item.BackColor = Color.FromArgb(30, 28, 48);
            
            listView.Items.Add(item);
        }
    }

    private void btnFermer_Click(object sender, EventArgs e) => Close();
}
