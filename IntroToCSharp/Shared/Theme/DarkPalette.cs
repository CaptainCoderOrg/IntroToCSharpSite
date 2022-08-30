using MudBlazor;

namespace CaptainCoder;

public class DarkPalette : Palette
{
    private static readonly Palette Default = new MudTheme().PaletteDark;
    public DarkPalette() : base() {
        LinesInputs = Default.LinesInputs;
        TableLines = Default.TableLines;
        TableStriped = Default.TableStriped;
        TableHover = Default.TableHover;
        Divider = Default.Divider;
        DividerLight = Default.DividerLight;
        PrimaryDarken = Default.PrimaryDarken;
        PrimaryLighten = Default.PrimaryLighten;
        SecondaryDarken = Default.SecondaryDarken;
        SecondaryLighten = Default.SecondaryLighten;
        TertiaryDarken = Default.TertiaryDarken;
        TertiaryLighten = Default.TertiaryLighten;
        InfoDarken = Default.InfoDarken;
        LinesDefault = Default.LinesDefault;
        InfoLighten = Default.InfoLighten;
        SuccessLighten = Default.SuccessLighten;
        WarningDarken = Default.WarningDarken;
        WarningLighten = Default.WarningLighten;
        ErrorDarken = Default.ErrorDarken;
        ErrorLighten = Default.ErrorLighten;
        DarkDarken = Default.DarkDarken;
        DarkLighten = Default.DarkLighten;
        HoverOpacity = Default.HoverOpacity;
        GrayDefault = Default.GrayDefault;
        GrayLight = Default.GrayLight;
        GrayLighter = Default.GrayLighter;
        GrayDark = Default.GrayDark;
        GrayDarker = Default.GrayDarker;
        SuccessDarken = Default.SuccessDarken;
        AppbarText = Default.AppbarText;
        AppbarBackground = Default.AppbarBackground;
        DrawerIcon = Default.DrawerIcon;
        Black = Default.Black;
        White = Default.White;
        Primary = Default.Primary;
        PrimaryContrastText = Default.PrimaryContrastText;
        Secondary = Default.Secondary;
        SecondaryContrastText = Default.SecondaryContrastText;
        Tertiary = Default.Tertiary;
        TertiaryContrastText = Default.TertiaryContrastText;
        Info = Default.Info;
        InfoContrastText = Default.InfoContrastText;
        Success = Default.Success;
        SuccessContrastText = Default.SuccessContrastText;
        Warning = Default.Warning;
        WarningContrastText = Default.WarningContrastText;
        Error = Default.Error;
        ErrorContrastText = Default.ErrorContrastText;
        Dark = Default.Dark;
        DarkContrastText = Default.DarkContrastText;
        TextPrimary = Default.TextPrimary;
        TextSecondary = Default.TextSecondary;
        TextDisabled = Default.TextDisabled;
        ActionDefault = Default.ActionDefault;
        ActionDisabled = Default.ActionDisabled;
        ActionDisabledBackground = Default.ActionDisabledBackground;
        Background = Default.Background;
        BackgroundGrey = Default.BackgroundGrey;
        Surface = Default.Surface;
        DrawerBackground = Default.DrawerBackground;
        DrawerText = Default.DrawerText;
        OverlayDark = Default.OverlayDark;
        OverlayLight = Default.OverlayLight;
    }
}