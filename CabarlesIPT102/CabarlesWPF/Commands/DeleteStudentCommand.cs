using CabarlesWPF.ViewModels;

namespace CabarlesWPF.Commands;

public class DeleteStudentCommand : BaseCommand
{
    private readonly AddStudentViewModel _viewModel;

    public DeleteStudentCommand(AddStudentViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override async void Execute(object? parameter)
    {
        if (parameter is int studentId)
        {
            await _viewModel.DeleteStudentAsync(studentId);
        }
    }
}
