using MudBlazor;

namespace CaptainCoder;

public class Themes
{

    private static readonly DarkPalette DarkPalette = new () { 
        //#b2d9ff
        
        BackgroundGrey = new ("#f5f5f5"),
        GrayDefault = new ("#282828"),

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
        Palette = new PaletteLight() {
            GrayDefault = new ("#282828"),
        },

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