namespace SecurITMemory.Forms;

partial class ScoresForm
{
    private System.ComponentModel.IContainer components = null;
    private Label lblTitre;
    private ListView listView;
    private Button btnFermer;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblTitre = new Label();
        listView = new ListView();
        btnFermer = new Button();
        SuspendLayout();

        lblTitre.Text = "Tableau des scores";
        lblTitre.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitre.ForeColor = Color.FromArgb(180, 140, 255);
        lblTitre.AutoSize = true;
        lblTitre.Location = new Point(20, 20);

        listView.View = View.Details;
        listView.FullRowSelect = true;
        listView.GridLines = false;
        listView.Location = new Point(20, 65);
        listView.Size = new Size(560, 380);
        listView.Font = new Font("Segoe UI", 10F);
        listView.BackColor = Color.FromArgb(20, 18, 38);
        listView.ForeColor = Color.Gainsboro;
        listView.BorderStyle = BorderStyle.None;
        listView.Columns.Add("Joueur", 130);
        listView.Columns.Add("Plateau", 90);
        listView.Columns.Add("Essais", 70);
        listView.Columns.Add("Temps", 80);
        listView.Columns.Add("Date", 160);

        btnFermer.Text = "Fermer";
        btnFermer.Size = new Size(120, 38);
        btnFermer.Location = new Point(460, 460);
        btnFermer.BackColor = Color.FromArgb(95, 75, 180);
        btnFermer.ForeColor = Color.White;
        btnFermer.FlatStyle = FlatStyle.Flat;
        btnFermer.FlatAppearance.BorderSize = 0;
        btnFermer.Click += btnFermer_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(600, 510);
        Controls.Add(lblTitre);
        Controls.Add(listView);
        Controls.Add(btnFermer);
        BackColor = Color.FromArgb(20, 18, 38);
        Text = "Scores - SecurIT Memory";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ResumeLayout(false);
        PerformLayout();
    }
}
