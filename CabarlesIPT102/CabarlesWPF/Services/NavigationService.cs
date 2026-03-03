using Microsoft.Extensions.DependencyInjection;
using CabarlesWPF.Stores;
using CabarlesWPF.ViewModels;
using System;

namespace CabarlesWPF.Services;

public class NavigationService : INavigationService
{
    private readonly NavigationStore _navigationStore;
    private readonly IServiceProvider _serviceProvider;

    public NavigationService(NavigationStore navigationStore, IServiceProvider serviceProvider)
    {
        _navigationStore = navigationStore;
        _serviceProvider = serviceProvider;
    }

    public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel
    {
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
        _navigationStore.CurrentViewModel = viewModel;
    }
}
