namespace SecurITMemory.Forms;

partial class OptionsForm
{
    private System.ComponentModel.IContainer components = null;
    private Label lblTitre;
    private RadioButton rbPetit;
    private RadioButton rbMoyen;
    private RadioButton rbGrand;
    private Label lblTheme;
    private ComboBox cbTheme;
    private CheckBox cbHardcore;
    private Button btnValider;
    private Button btnAnnuler;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblTitre = new Label();
        rbPetit = new RadioButton();
        rbMoyen = new RadioButton();
        rbGrand = new RadioButton();
        lblTheme = new Label();
        cbTheme = new ComboBox();
        cbHardcore = new CheckBox();
        btnValider = new Button();
        btnAnnuler = new Button();
        SuspendLayout();

        lblTitre.Text = "Options de partie";
        lblTitre.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitre.ForeColor = Color.Gainsboro;
        lblTitre.AutoSize = true;
        lblTitre.Location = new Point(30, 25);

        StyliserRadio(rbPetit, "Facile - 4 x 4 (8 paires)", 80);
        StyliserRadio(rbMoyen, "Moyen - 6 x 4 (12 paires)", 130);
        StyliserRadio(rbGrand, "Difficile - 6 x 6 (18 paires)", 180);

        lblTheme.Text = "Theme de cartes :";
        lblTheme.Font = new Font("Segoe UI", 11F);
        lblTheme.ForeColor = Color.Gainsboro;
        lblTheme.AutoSize = true;
        lblTheme.Location = new Point(50, 240);

        cbTheme.Location = new Point(50, 270);
        cbTheme.Size = new Size(330, 28);
        cbTheme.DropDownStyle = ComboBoxStyle.DropDownList;
        cbTheme.Font = new Font("Segoe UI", 10F);

        cbHardcore.Text = "Mode Hardcore (melange toutes les 30s)";
        cbHardcore.AutoSize = true;
        cbHardcore.Location = new Point(50, 320);
        cbHardcore.Font = new Font("Segoe UI", 11F);
        cbHardcore.ForeColor = Color.FromArgb(220, 100, 120);

        btnValider.Text = "Valider";
        btnValider.Size = new Size(120, 38);
        btnValider.Location = new Point(80, 370);
        btnValider.BackColor = Color.FromArgb(95, 75, 180);
        btnValider.ForeColor = Color.White;
        btnValider.FlatStyle = FlatStyle.Flat;
        btnValider.FlatAppearance.BorderSize = 0;
        btnValider.Click += btnValider_Click;

        btnAnnuler.Text = "Annuler";
        btnAnnuler.Size = new Size(120, 38);
        btnAnnuler.Location = new Point(220, 370);
        btnAnnuler.BackColor = Color.FromArgb(60, 55, 90);
        btnAnnuler.ForeColor = Color.White;
        btnAnnuler.FlatStyle = FlatStyle.Flat;
        btnAnnuler.FlatAppearance.BorderSize = 0;
        btnAnnuler.Click += btnAnnuler_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(420, 440);
        Controls.Add(lblTitre);
        Controls.Add(rbPetit);
        Controls.Add(rbMoyen);
        Controls.Add(rbGrand);
        Controls.Add(lblTheme);
        Controls.Add(cbTheme);
        Controls.Add(cbHardcore);
        Controls.Add(btnValider);
        Controls.Add(btnAnnuler);
        BackColor = Color.FromArgb(20, 18, 38);
        Text = "Options";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ResumeLayout(false);
        PerformLayout();
    }

    private void StyliserRadio(RadioButton radio, string texte, int y)
    {
        radio.Text = texte;
        radio.AutoSize = true;
        radio.Location = new Point(50, y);
        radio.Font = new Font("Segoe UI", 11F);
        radio.ForeColor = Color.Gainsboro;
    }
}
