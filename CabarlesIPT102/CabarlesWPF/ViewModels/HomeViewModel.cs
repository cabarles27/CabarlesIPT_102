using CabarlesWPF.Commands;
using CabarlesWPF.Services;
using System.Windows.Input;

namespace CabarlesWPF.ViewModels;

public class HomeViewModel : BaseViewModel
{
    public ICommand NavigateToStudentManagementCommand { get; }

    public HomeViewModel(INavigationService navigationService)
    {
        NavigateToStudentManagementCommand = new OpenStudentManagementCommand(navigationService);
    }
}
