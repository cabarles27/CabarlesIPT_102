using Domain.Models;
using CabarlesWPF.ViewModels;

namespace CabarlesWPF.Commands;

public class EditStudentCommand : BaseCommand
{
    private readonly AddStudentViewModel _viewModel;

    public EditStudentCommand(AddStudentViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override void Execute(object? parameter)
    {
        if (parameter is StudentModel student)
        {
            _viewModel.EditStudent(student);
        }
    }
}
