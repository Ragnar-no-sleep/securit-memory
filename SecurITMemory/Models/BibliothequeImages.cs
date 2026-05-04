namespace SecurITMemory.Models;

public enum Theme
{
    Cybersecurite,
    Materiel,
    Logiciel,
    Cryptographie
}

public static class BibliothequeImages
{
    public static IList<string> Images(Theme theme) => theme switch
    {
        Theme.Materiel => Materiel,
        Theme.Logiciel => Logiciel,
        Theme.Cryptographie => Crypto,
        _ => Cyber
    };

    public static string Dos(Theme theme) => theme switch
    {
        Theme.Materiel => "Assets/Cards/Materiel/dos.png",
        Theme.Logiciel => "Assets/Cards/Logiciel/dos.png",
        Theme.Cryptographie => "Assets/Cards/Crypto/dos.png",
        _ => "Assets/Cards/Cyber/dos.png"
    };

    public static string LibelleTheme(Theme theme) => theme switch
    {
        Theme.Materiel => "Materiel (RAM, CPU, GPU...)",
        Theme.Logiciel => "Logiciel (OS, IDE, Git...)",
        Theme.Cryptographie => "Cryptographie (AES, RSA...)",
        _ => "Cybersecurite (defaut)"
    };

    // Ancienne API conservee pour compatibilite.
    public static IList<string> ImagesCyber => Cyber;
    public const string DosCarte = "Assets/Cards/Cyber/dos.png";

    private static readonly IList<string> Cyber = new List<string>
    {
        "Assets/Cards/Cyber/cadenas.png",
        "Assets/Cards/Cyber/cle.png",
        "Assets/Cards/Cyber/parefeu.png",
        "Assets/Cards/Cyber/virus.png",
        "Assets/Cards/Cyber/bouclier.png",
        "Assets/Cards/Cyber/motdepasse.png",
        "Assets/Cards/Cyber/empreinte.png",
        "Assets/Cards/Cyber/oeil.png",
        "Assets/Cards/Cyber/serveur.png",
        "Assets/Cards/Cyber/cloud.png",
        "Assets/Cards/Cyber/bug.png",
        "Assets/Cards/Cyber/email.png",
        "Assets/Cards/Cyber/wifi.png",
        "Assets/Cards/Cyber/usb.png",
        "Assets/Cards/Cyber/biometrie.png",
        "Assets/Cards/Cyber/vpn.png",
        "Assets/Cards/Cyber/hash.png",
        "Assets/Cards/Cyber/2fa.png"
    };

    private static readonly IList<string> Materiel = new List<string>
    {
        "Assets/Cards/Materiel/ram.png",
        "Assets/Cards/Materiel/cpu.png",
        "Assets/Cards/Materiel/gpu.png",
        "Assets/Cards/Materiel/ssd.png",
        "Assets/Cards/Materiel/hdd.png",
        "Assets/Cards/Materiel/ecran.png",
        "Assets/Cards/Materiel/clavier.png",
        "Assets/Cards/Materiel/souris.png",
        "Assets/Cards/Materiel/cm.png",
        "Assets/Cards/Materiel/alim.png",
        "Assets/Cards/Materiel/ventilo.png",
        "Assets/Cards/Materiel/ethernet.png",
        "Assets/Cards/Materiel/hdmi.png",
        "Assets/Cards/Materiel/batterie.png",
        "Assets/Cards/Materiel/wadapter.png",
        "Assets/Cards/Materiel/scanner.png",
        "Assets/Cards/Materiel/webcam.png",
        "Assets/Cards/Materiel/micro.png"
    };

    private static readonly IList<string> Logiciel = new List<string>
    {
        "Assets/Cards/Logiciel/linux.png",
        "Assets/Cards/Logiciel/windows.png",
        "Assets/Cards/Logiciel/macos.png",
        "Assets/Cards/Logiciel/navigateur.png",
        "Assets/Cards/Logiciel/ide.png",
        "Assets/Cards/Logiciel/terminal.png",
        "Assets/Cards/Logiciel/compilateur.png",
        "Assets/Cards/Logiciel/debugger.png",
        "Assets/Cards/Logiciel/git.png",
        "Assets/Cards/Logiciel/docker.png",
        "Assets/Cards/Logiciel/db.png",
        "Assets/Cards/Logiciel/api.png",
        "Assets/Cards/Logiciel/framework.png",
        "Assets/Cards/Logiciel/lib.png",
        "Assets/Cards/Logiciel/package.png",
        "Assets/Cards/Logiciel/vm.png",
        "Assets/Cards/Logiciel/container.png",
        "Assets/Cards/Logiciel/kernel.png"
    };

    private static readonly IList<string> Crypto = new List<string>
    {
        "Assets/Cards/Crypto/clepub.png",
        "Assets/Cards/Crypto/clepriv.png",
        "Assets/Cards/Crypto/hash.png",
        "Assets/Cards/Crypto/signature.png",
        "Assets/Cards/Crypto/aes.png",
        "Assets/Cards/Crypto/rsa.png",
        "Assets/Cards/Crypto/sha.png",
        "Assets/Cards/Crypto/cert.png",
        "Assets/Cards/Crypto/blockchain.png",
        "Assets/Cards/Crypto/salt.png",
        "Assets/Cards/Crypto/jwt.png",
        "Assets/Cards/Crypto/oauth.png",
        "Assets/Cards/Crypto/tls.png",
        "Assets/Cards/Crypto/pgp.png",
        "Assets/Cards/Crypto/hmac.png",
        "Assets/Cards/Crypto/ecc.png",
        "Assets/Cards/Crypto/kdf.png",
        "Assets/Cards/Crypto/otp.png"
    };
}
