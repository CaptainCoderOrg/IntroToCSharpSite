namespace CaptainCoder;
public readonly record struct Portrait(string Name, string Src);
public static class Portraits {
    public static readonly Portrait CaptainCoder = new ("Captain Coder", "captain-coder.png");
}