using MudBlazor;

namespace CaptainCoder;

public class Themes
{

    private static readonly DarkPalette DarkPalette = new () { 
        //#b2d9ff
    };

    public static readonly MudTheme s_CSharpTheme = new ()
    {
        // Palette = new Palette()
        // {
        //     Primary = Colors.Blue.Default,
        //     Secondary = Colors.Green.Accent4,
        //     AppbarBackground = Colors.Red.Default,
        // },
        // PaletteDark = new Palette()
        // {
        //     Primary = Colors.Blue.Lighten1
        // },
        PaletteDark = DarkPalette,

        // LayoutProperties = new LayoutProperties()
        // {
        //     DrawerWidthLeft = "260px",
        //     DrawerWidthRight = "300px"
        // }

        Typography = new Typography()
        {
            Default = new Default() { FontSize = "1.0rem", LineHeight = 1.5 }
        }
    };
}