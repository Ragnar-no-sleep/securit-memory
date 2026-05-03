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
        var scores = Leaderboard.Charger()
            .OrderBy(s => s.Essais)
            .ThenBy(s => s.TempsSecondes)
            .Take(20);

        foreach (var s in scores)
        {
            var item = new ListViewItem(s.Joueur);
            item.SubItems.Add($"{s.TaillePlateau} cartes");
            item.SubItems.Add(s.Essais.ToString());
            item.SubItems.Add($"{s.TempsSecondes / 60:00}:{s.TempsSecondes % 60:00}");
            item.SubItems.Add(s.Date.ToString("dd/MM/yyyy HH:mm"));
            listView.Items.Add(item);
        }
    }

    private void btnFermer_Click(object sender, EventArgs e) => Close();
}
