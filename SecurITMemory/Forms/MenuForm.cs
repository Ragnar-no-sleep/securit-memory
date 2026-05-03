namespace SecurITMemory.Forms;

public partial class MenuForm : Form
{
    private int _taillePlateau = 16;

    public MenuForm()
    {
        InitializeComponent();
    }

    private void btnJouer_Click(object sender, EventArgs e)
    {
        using var jeu = new JeuForm(_taillePlateau);
        Hide();
        jeu.ShowDialog(this);
        Show();
    }

    private void btnOptions_Click(object sender, EventArgs e)
    {
        using var options = new OptionsForm(_taillePlateau);
        if (options.ShowDialog(this) == DialogResult.OK)
            _taillePlateau = options.TailleChoisie;
    }

    private void btnScores_Click(object sender, EventArgs e)
    {
        using var scores = new ScoresForm();
        scores.ShowDialog(this);
    }

    private void btnQuitter_Click(object sender, EventArgs e) => Close();
}
