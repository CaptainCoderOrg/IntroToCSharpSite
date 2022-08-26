using Microsoft.AspNetCore.Components;
using MudBlazor;
using IntroToCSharp.Shared.Dialogs;
namespace CaptainCoder;

/// <summary>
/// A PageRef is used to create a page reference on a subclass of the CaptainCoder.Page.
/// Within a subclass of a CaptainCoder.Page, if you add a static PageRef PageRef, it will
/// "automagically" be added to the NavMenu. If you specify the IsAdventure variable to be
/// false, the page will always appear in the menu while in Adventure Mode
/// </summary>
public readonly record struct PageRef(string Name, string Href, int Order, string Parent, bool IsAdventure = true);

public class Page : ComponentBase
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;
    [Inject]
    protected IDialogService DialogService { get; set; } = null!;

    private User? _user = null;
    private UserPages? _userPages = null;
    protected UserPages UserPages { 
        get => _userPages ?? UserPages.Default; 
        private set { _userPages = value; CheckPage(); StateHasChanged(); } 
    }
    protected User User {
        get => _user ?? User.NoUser;
        private set { _user = value; StateHasChanged(); }
    } 


    protected override void OnInitialized()
    {
        base.OnInitialized();
        UserService.Service.OnUserChange += OnUserChange;
    }

    protected virtual void OnUserChange(User? newUser)
    {
        User = newUser ?? User.NoUser;
        if (User.UserPagesRef != null) User.UserPagesRef.DataChangedEvent += OnUserPagesRefChange;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        Utils.ScrollToTop();
    }

    private void OnUserPagesRefChange(UserPages? newPage) => UserPages = newPage ?? UserPages.Default;

    private void CheckPage() {
        if (!User.IsLoggedIn) return;
        string route = $"/{NavigationManager.GetRoute()}";
        PageRef? pageCheck = PageRegistry.FromRoute(route);
        if (pageCheck == null) return;
        PageRef page = (PageRef)pageCheck;
        if (!page.IsAdventure || UserPages.Contains(page)) return;
        AddPageToBookDialog.Show(DialogService, page);
    }

}
