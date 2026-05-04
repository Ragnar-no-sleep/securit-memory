using SecurITMemory.Models;

namespace SecurITMemory.Forms;

public partial class OptionsForm : Form
{
    public int TailleChoisie { get; private set; }
    public Theme ThemeChoisi { get; private set; }

    public OptionsForm(int tailleActuelle, Theme themeActuel)
    {
        InitializeComponent();
        TailleChoisie = tailleActuelle;
        ThemeChoisi = themeActuel;
        SelectionnerRadio(tailleActuelle);
        SelectionnerTheme(themeActuel);
    }

    private void SelectionnerRadio(int taille)
    {
        rbPetit.Checked = taille == 16;
        rbMoyen.Checked = taille == 24;
        rbGrand.Checked = taille == 36;
    }

    private void SelectionnerTheme(Theme theme)
    {
        cbTheme.Items.Clear();
        foreach (Theme t in Enum.GetValues<Theme>())
            cbTheme.Items.Add(BibliothequeImages.LibelleTheme(t));
        cbTheme.SelectedIndex = (int)theme;
    }

    private void btnValider_Click(object sender, EventArgs e)
    {
        if (rbGrand.Checked) TailleChoisie = 36;
        else if (rbMoyen.Checked) TailleChoisie = 24;
        else TailleChoisie = 16;

        ThemeChoisi = (Theme)cbTheme.SelectedIndex;

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnAnnuler_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
