namespace SecurITMemory.Forms;

public partial class OptionsForm : Form
{
    public int TailleChoisie { get; private set; }

    public OptionsForm(int tailleActuelle)
    {
        InitializeComponent();
        TailleChoisie = tailleActuelle;
        SelectionnerRadio(tailleActuelle);
    }

    private void SelectionnerRadio(int taille)
    {
        rbPetit.Checked = taille == 16;
        rbMoyen.Checked = taille == 24;
        rbGrand.Checked = taille == 36;
    }

    private void btnValider_Click(object sender, EventArgs e)
    {
        if (rbGrand.Checked) TailleChoisie = 36;
        else if (rbMoyen.Checked) TailleChoisie = 24;
        else TailleChoisie = 16;

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnAnnuler_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
