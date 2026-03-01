using CabarlesWPF.ViewModels;

namespace CabarlesWPF.Commands;

public class UpdateStudentCommand : BaseCommand
{
    private readonly AddStudentViewModel _viewModel;

    public UpdateStudentCommand(AddStudentViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override async void Execute(object? parameter)
    {
        await _viewModel.UpdateStudentAsync();
    }
}
