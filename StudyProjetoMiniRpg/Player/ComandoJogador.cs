public class ComandoJogador
{
    public AcaoJogador AcaoBase { get; set; }
    public bool AtaqueForte { get; set; } = false;
    public bool FocarDefesa { get; set; } = false;
    public string TextoOriginal { get; set; } = "";
}
