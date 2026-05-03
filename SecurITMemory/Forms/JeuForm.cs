using SecurITMemory.Models;

namespace SecurITMemory.Forms;

public partial class JeuForm : Form
{
    private readonly JeuMemory _jeu;
    private readonly Dictionary<PictureBox, Carte> _liaisons = new();
    private readonly System.Windows.Forms.Timer _timerDelai;
    private readonly System.Windows.Forms.Timer _chronometre;
    private DateTime _debutPartie;

    public JeuForm(int taillePlateau)
    {
        InitializeComponent();

        _jeu = new JeuMemory(taillePlateau, BibliothequeImages.ImagesCyber);

        _timerDelai = new System.Windows.Forms.Timer { Interval = 1200 };
        _timerDelai.Tick += TimerDelai_Tick;

        _chronometre = new System.Windows.Forms.Timer { Interval = 1000 };
        _chronometre.Tick += Chronometre_Tick;

        ConstruireGrille();
        DemarrerPartie();
    }

    private void DemarrerPartie()
    {
        _debutPartie = DateTime.Now;
        _chronometre.Start();
        MettreAJourCompteurs();
    }

    private void ConstruireGrille()
    {
        int nbCartes = _jeu.TaillePlateau;
        int colonnes = nbCartes switch { 16 => 4, 24 => 6, 36 => 6, _ => (int)Math.Ceiling(Math.Sqrt(nbCartes)) };
        int lignes = (int)Math.Ceiling(nbCartes / (double)colonnes);

        panelGrille.SuspendLayout();
        panelGrille.Controls.Clear();
        panelGrille.ColumnCount = colonnes;
        panelGrille.RowCount = lignes;
        panelGrille.ColumnStyles.Clear();
        panelGrille.RowStyles.Clear();

        for (int c = 0; c < colonnes; c++)
            panelGrille.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / colonnes));
        for (int r = 0; r < lignes; r++)
            panelGrille.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / lignes));

        foreach (var carte in _jeu.Cartes)
        {
            var pb = new PictureBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(4),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(50, 45, 95),
                Cursor = Cursors.Hand
            };
            pb.Click += Carte_Click;
            _liaisons[pb] = carte;
            panelGrille.Controls.Add(pb);
            AfficherCarte(pb, carte);
        }

        panelGrille.ResumeLayout();
    }

    private void Carte_Click(object? sender, EventArgs e)
    {
        if (sender is not PictureBox pb || !_liaisons.TryGetValue(pb, out var carte))
            return;

        var resultat = _jeu.Selectionner(carte);
        if (resultat == ResultatSelection.Ignoree) return;

        AfficherCarte(pb, carte);
        MettreAJourCompteurs();

        if (resultat == ResultatSelection.PaireTrouvee)
        {
            RafraichirToutesLesCartes();
            if (_jeu.EstTermine) AfficherVictoire();
        }
        else if (resultat == ResultatSelection.PaireRatee)
        {
            _timerDelai.Start();
        }
    }

    private void TimerDelai_Tick(object? sender, EventArgs e)
    {
        _timerDelai.Stop();
        _jeu.RetournerCartesNonAppariees();
        RafraichirToutesLesCartes();
    }

    private void Chronometre_Tick(object? sender, EventArgs e) => MettreAJourCompteurs();

    private void RafraichirToutesLesCartes()
    {
        foreach (var (pb, carte) in _liaisons) AfficherCarte(pb, carte);
    }

    private void AfficherCarte(PictureBox pb, Carte carte)
    {
        string chemin = carte.Etat == EtatCarte.Cachee
            ? BibliothequeImages.DosCarte
            : carte.CheminImage;

        pb.Image = ChargerImage(chemin);

        pb.BackColor = carte.Etat switch
        {
            EtatCarte.Trouvee => Color.FromArgb(60, 130, 90),
            EtatCarte.Revelee => Color.FromArgb(120, 100, 200),
            _ => Color.FromArgb(50, 45, 95)
        };
    }

    private static Image? ChargerImage(string cheminRelatif)
    {
        try
        {
            string chemin = Path.Combine(AppContext.BaseDirectory, cheminRelatif);
            return File.Exists(chemin) ? Image.FromFile(chemin) : null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private void MettreAJourCompteurs()
    {
        var ecoule = DateTime.Now - _debutPartie;
        lblChrono.Text = $"⏱ {ecoule:mm\\:ss}";
        lblEssais.Text = $"Essais : {_jeu.NombreEssais}";
        lblPaires.Text = $"Paires : {_jeu.PairesTrouvees} / {_jeu.PairesTotales}";
    }

    private void AfficherVictoire()
    {
        _chronometre.Stop();
        var ecoule = DateTime.Now - _debutPartie;

        string nom = DemanderNomJoueur();
        Leaderboard.Ajouter(new Score
        {
            Joueur = string.IsNullOrWhiteSpace(nom) ? "Anonyme" : nom.Trim(),
            TaillePlateau = _jeu.TaillePlateau,
            Essais = _jeu.NombreEssais,
            TempsSecondes = (int)ecoule.TotalSeconds
        });

        MessageBox.Show(
            $"Bravo ! Toutes les paires sont trouvées.\n\nTemps : {ecoule:mm\\:ss}\nEssais : {_jeu.NombreEssais}\n\nScore enregistré.",
            "Victoire",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        Close();
    }

    // Mini boîte de dialogue pour saisir le nom du joueur (sans dépendance externe).
    private static string DemanderNomJoueur()
    {
        using var dialog = new Form
        {
            Text = "Votre nom",
            ClientSize = new Size(320, 130),
            FormBorderStyle = FormBorderStyle.FixedDialog,
            StartPosition = FormStartPosition.CenterParent,
            MaximizeBox = false,
            MinimizeBox = false,
            BackColor = Color.FromArgb(20, 18, 38)
        };
        var label = new Label
        {
            Text = "Entrez votre nom :",
            Location = new Point(20, 15),
            AutoSize = true,
            ForeColor = Color.Gainsboro,
            Font = new Font("Segoe UI", 10F)
        };
        var textBox = new TextBox { Location = new Point(20, 45), Size = new Size(280, 25) };
        var ok = new Button
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Location = new Point(220, 80),
            Size = new Size(80, 30),
            BackColor = Color.FromArgb(95, 75, 180),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        ok.FlatAppearance.BorderSize = 0;
        dialog.Controls.AddRange(new Control[] { label, textBox, ok });
        dialog.AcceptButton = ok;
        dialog.ShowDialog();
        return textBox.Text;
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _timerDelai.Stop();
        _chronometre.Stop();
        _timerDelai.Dispose();
        _chronometre.Dispose();
        base.OnFormClosed(e);
    }
}
