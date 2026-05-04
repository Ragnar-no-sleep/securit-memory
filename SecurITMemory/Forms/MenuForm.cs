using SecurITMemory.Models;

namespace SecurITMemory.Forms;

public partial class MenuForm : Form
{
    private int _taillePlateau = 16;
    private Theme _theme = Theme.Cybersecurite;

    public MenuForm()
    {
        InitializeComponent();
    }

    private void btnJouer_Click(object sender, EventArgs e)
    {
        using var jeu = new JeuForm(_taillePlateau, _theme);
        Hide();
        jeu.ShowDialog(this);
        Show();
    }

    private void btnOptions_Click(object sender, EventArgs e)
    {
        using var options = new OptionsForm(_taillePlateau, _theme);
        if (options.ShowDialog(this) == DialogResult.OK)
        {
            _taillePlateau = options.TailleChoisie;
            _theme = options.ThemeChoisi;
        }
    }

    private void btnScores_Click(object sender, EventArgs e)
    {
        using var scores = new ScoresForm();
        scores.ShowDialog(this);
    }

    private void btnQuitter_Click(object sender, EventArgs e) => Close();
}
