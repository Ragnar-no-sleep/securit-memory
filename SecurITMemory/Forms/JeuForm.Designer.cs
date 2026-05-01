namespace SecurITMemory.Forms;

partial class JeuForm
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel panelGrille;
    private Panel panelHaut;
    private Label lblChrono;
    private Label lblEssais;
    private Label lblPaires;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        panelHaut = new Panel();
        lblChrono = new Label();
        lblEssais = new Label();
        lblPaires = new Label();
        panelGrille = new TableLayoutPanel();
        SuspendLayout();

        panelHaut.Dock = DockStyle.Top;
        panelHaut.Height = 60;
        panelHaut.BackColor = Color.FromArgb(30, 25, 55);
        panelHaut.Controls.Add(lblChrono);
        panelHaut.Controls.Add(lblEssais);
        panelHaut.Controls.Add(lblPaires);

        lblChrono.Text = "⏱ 00:00";
        lblChrono.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblChrono.ForeColor = Color.Gainsboro;
        lblChrono.AutoSize = true;
        lblChrono.Location = new Point(20, 18);

        lblEssais.Text = "Essais : 0";
        lblEssais.Font = new Font("Segoe UI", 12F);
        lblEssais.ForeColor = Color.Gainsboro;
        lblEssais.AutoSize = true;
        lblEssais.Location = new Point(220, 20);

        lblPaires.Text = "Paires : 0 / 0";
        lblPaires.Font = new Font("Segoe UI", 12F);
        lblPaires.ForeColor = Color.Gainsboro;
        lblPaires.AutoSize = true;
        lblPaires.Location = new Point(420, 20);

        panelGrille.Dock = DockStyle.Fill;
        panelGrille.Padding = new Padding(10);
        panelGrille.BackColor = Color.FromArgb(20, 18, 38);

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 720);
        Controls.Add(panelGrille);
        Controls.Add(panelHaut);
        BackColor = Color.FromArgb(20, 18, 38);
        Text = "SecurIT Memory — Partie en cours";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(700, 600);
        ResumeLayout(false);
    }
}
