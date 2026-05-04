using SecurITMemory.Models;

namespace SecurITMemory.Forms;

public partial class JeuForm : Form
{
    private readonly JeuMemory _jeu;
    private readonly Theme _theme;
    private readonly Dictionary<PictureBox, Carte> _liaisons = new();
    private readonly System.Windows.Forms.Timer _timerDelai;
    private readonly System.Windows.Forms.Timer _chronometre;
    private DateTime _debutPartie;

    public JeuForm(int taillePlateau, Theme theme)
    {
        InitializeComponent();

        _theme = theme;
        _jeu = new JeuMemory(taillePlateau, BibliothequeImages.Images(theme));

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
            var container = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(8),
                BackColor = Color.Transparent
            };

            var pb = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(45, 40, 80),
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None
            };

            // Effet de bordure via le panel parent
            container.Paint += (s, e) => {
                var rect = container.ClientRectangle;
                rect.Inflate(-4, -4);
                ControlPaint.DrawBorder(e.Graphics, rect, Color.FromArgb(100, 90, 150), ButtonBorderStyle.Solid);
            };

            pb.Click += Carte_Click;
            pb.MouseEnter += (s, e) => { if (_liaisons[pb].Etat == EtatCarte.Cachee) pb.BackColor = Color.FromArgb(60, 55, 110); };
            pb.MouseLeave += (s, e) => { if (_liaisons[pb].Etat == EtatCarte.Cachee) pb.BackColor = Color.FromArgb(45, 40, 80); };

            _liaisons[pb] = carte;
            container.Controls.Add(pb);
            panelGrille.Controls.Add(container);
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
            System.Media.SystemSounds.Asterisk.Play(); // Petit son de succès
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
            ? BibliothequeImages.Dos(_theme)
            : carte.CheminImage;

        pb.Image = ChargerImage(chemin);

        switch (carte.Etat)
        {
            case EtatCarte.Trouvee:
                pb.BackColor = Color.FromArgb(40, 110, 70);
                pb.Cursor = Cursors.Default;
                break;
            case EtatCarte.Revelee:
                pb.BackColor = Color.FromArgb(80, 70, 160);
                pb.Cursor = Cursors.Default;
                break;
            case EtatCarte.Cachee:
                pb.BackColor = Color.FromArgb(45, 40, 80);
                pb.Cursor = Cursors.Hand;
                break;
        }
    }

    private static readonly Dictionary<string, Image> _imageCache = new();

    private static Image? ChargerImage(string cheminRelatif)
    {
        if (_imageCache.TryGetValue(cheminRelatif, out var img))
            return img;

        try
        {
            string cheminNettoye = cheminRelatif.Replace('/', Path.DirectorySeparatorChar);
            
            // Cherche d'abord dans le répertoire de l'exécutable
            string chemin = Path.Combine(Application.StartupPath, cheminNettoye);
            if (!File.Exists(chemin))
                chemin = Path.Combine(Environment.CurrentDirectory, cheminNettoye);

            if (File.Exists(chemin))
            {
                var loaded = Image.FromFile(chemin);
                _imageCache[cheminRelatif] = loaded;
                return loaded;
            }

            return null;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur image {cheminRelatif}: {ex.Message}");
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
            TempsSecondes = (int)ecoule.TotalSeconds,
            Theme = _theme.ToString()
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
