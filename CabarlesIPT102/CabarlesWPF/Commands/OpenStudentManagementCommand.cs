using CabarlesWPF.Services;
using CabarlesWPF.ViewModels;

namespace CabarlesWPF.Commands;

public class OpenStudentManagementCommand : BaseCommand
{
    private readonly INavigationService _navigationService;

    public OpenStudentManagementCommand(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public override void Execute(object? parameter)
    {
        _navigationService.NavigateTo<AddStudentViewModel>();
    }
}
