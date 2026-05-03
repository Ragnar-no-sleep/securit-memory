namespace SecurITMemory.Forms;

partial class MenuForm
{
    private System.ComponentModel.IContainer components = null;
    private Label lblTitre;
    private Label lblSousTitre;
    private Button btnJouer;
    private Button btnOptions;
    private Button btnScores;
    private Button btnQuitter;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblTitre = new Label();
        lblSousTitre = new Label();
        btnJouer = new Button();
        btnOptions = new Button();
        btnScores = new Button();
        btnQuitter = new Button();
        SuspendLayout();

        lblTitre.Text = "SecurIT Memory";
        lblTitre.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
        lblTitre.ForeColor = Color.FromArgb(180, 140, 255);
        lblTitre.AutoSize = true;
        lblTitre.Location = new Point(80, 60);

        lblSousTitre.Text = "Le jeu de memoire des experts en cybersecurite";
        lblSousTitre.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
        lblSousTitre.ForeColor = Color.Gainsboro;
        lblSousTitre.AutoSize = true;
        lblSousTitre.Location = new Point(85, 120);

        StyliserBouton(btnJouer, "Jouer", 180);
        btnJouer.Click += btnJouer_Click;

        StyliserBouton(btnOptions, "Options", 245);
        btnOptions.Click += btnOptions_Click;

        StyliserBouton(btnScores, "Scores", 310);
        btnScores.Click += btnScores_Click;

        StyliserBouton(btnQuitter, "Quitter", 375);
        btnQuitter.Click += btnQuitter_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(560, 480);
        Controls.Add(lblTitre);
        Controls.Add(lblSousTitre);
        Controls.Add(btnJouer);
        Controls.Add(btnOptions);
        Controls.Add(btnScores);
        Controls.Add(btnQuitter);
        BackColor = Color.FromArgb(20, 18, 38);
        Text = "SecurIT Memory";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        ResumeLayout(false);
        PerformLayout();
    }

    private static void StyliserBouton(Button bouton, string texte, int y)
    {
        bouton.Text = texte;
        bouton.Size = new Size(280, 50);
        bouton.Location = new Point(140, y);
        bouton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        bouton.BackColor = Color.FromArgb(95, 75, 180);
        bouton.ForeColor = Color.White;
        bouton.FlatStyle = FlatStyle.Flat;
        bouton.FlatAppearance.BorderSize = 0;
        bouton.Cursor = Cursors.Hand;
    }
}
