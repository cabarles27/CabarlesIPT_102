using CabarlesWPF.ViewModels;

namespace CabarlesWPF.Commands;

public class AddStudentCommand : BaseCommand
{
    private readonly AddStudentViewModel _viewModel;

    public AddStudentCommand(AddStudentViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override async void Execute(object? parameter)
    {
        await _viewModel.AddStudentAsync();
    }
}
