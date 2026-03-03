using CabarlesWPF.ViewModels;

namespace CabarlesWPF.Services;

public interface INavigationService
{
    void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;
}
