namespace CaptainCoder;
public readonly record struct Portrait(string Name, string Src);
public static class Portraits {
    public static readonly Portrait CaptainCoder = new ("Captain Coder", "captain-coder.png");
    public static readonly Portrait Bbeg = new ("Bbeg", "bbeg.jpg");
    public static readonly Portrait Nefaria = new ("Nefaria", "nefaria.png");
}