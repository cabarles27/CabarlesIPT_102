using Domain.Commands;
using Domain.Models;
using Domain.Queries;
using CabarlesWPF.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CabarlesWPF.ViewModels;

public class AddStudentViewModel : BaseViewModel
{
    private readonly ICreateStudent _createStudent;
    private readonly IUpdateStudent _updateStudent;
    private readonly IDeleteStudent _deleteStudent;
    private readonly IGetAllStudents _getAllStudents;

    private int _studentId;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private int _age;
    private string _course = string.Empty;
    private bool _isEditMode;
    private string _searchText = string.Empty;
    private ObservableCollection<StudentModel> _students = new();
    private ObservableCollection<StudentModel> _filteredStudents = new();
    private StudentModel? _selectedStudent;

    public int StudentId
    {
        get => _studentId;
        set => SetProperty(ref _studentId, value);
    }

    public string FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }

    public string LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }

    public int Age
    {
        get => _age;
        set => SetProperty(ref _age, value);
    }

    public string Course
    {
        get => _course;
        set => SetProperty(ref _course, value);
    }

    public bool IsEditMode
    {
        get => _isEditMode;
        set => SetProperty(ref _isEditMode, value);
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            FilterStudents();
        }
    }

    public ObservableCollection<StudentModel> Students
    {
        get => _students;
        set => SetProperty(ref _students, value);
    }

    public ObservableCollection<StudentModel> FilteredStudents
    {
        get => _filteredStudents;
        set => SetProperty(ref _filteredStudents, value);
    }

    public StudentModel? SelectedStudent
    {
        get => _selectedStudent;
        set => SetProperty(ref _selectedStudent, value);
    }

    public ICommand AddStudentCommand { get; }
    public ICommand UpdateStudentCommand { get; }
    public ICommand DeleteStudentCommand { get; }
    public ICommand EditStudentCommand { get; }

    public AddStudentViewModel(
        ICreateStudent createStudent,
        IUpdateStudent updateStudent,
        IDeleteStudent deleteStudent,
        IGetAllStudents getAllStudents)
    {
        _createStudent = createStudent;
        _updateStudent = updateStudent;
        _deleteStudent = deleteStudent;
        _getAllStudents = getAllStudents;

        AddStudentCommand = new AddStudentCommand(this);
        UpdateStudentCommand = new UpdateStudentCommand(this);
        DeleteStudentCommand = new DeleteStudentCommand(this);
        EditStudentCommand = new EditStudentCommand(this);

        // Load students asynchronously without blocking
        Task.Run(async () =>
        {
            try
            {
                await LoadStudentsAsync();
            }
            catch (Exception ex)
            {
                // Log error but don't crash the app
                System.Diagnostics.Debug.WriteLine($"Error loading students: {ex.Message}");
            }
        });
    }

    public async Task LoadStudentsAsync()
    {
        try
        {
            var students = await _getAllStudents.ExecuteAsync();
            
            // Update UI on the UI thread
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                Students = new ObservableCollection<StudentModel>(students);
                FilteredStudents = new ObservableCollection<StudentModel>(students);
            });
        }
        catch (Exception ex)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"Error loading students: {ex.Message}\n\nPlease make sure the database is set up correctly.", 
                    "Database Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            });
        }
    }

    public async Task AddStudentAsync()
    {
        try
        {
            var student = new StudentModel
            {
                FirstName = FirstName,
                LastName = LastName,
                Age = Age,
                Course = Course
            };

            var success = await _createStudent.ExecuteAsync(student);
            if (success)
            {
                MessageBox.Show("Student added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                await LoadStudentsAsync();
            }
            else
            {
                MessageBox.Show("Failed to add student.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public async Task UpdateStudentAsync()
    {
        try
        {
            var student = new StudentModel
            {
                StudentId = StudentId,
                FirstName = FirstName,
                LastName = LastName,
                Age = Age,
                Course = Course
            };

            var success = await _updateStudent.ExecuteAsync(student);
            if (success)
            {
                MessageBox.Show("Student updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                IsEditMode = false;
                await LoadStudentsAsync();
            }
            else
            {
                MessageBox.Show("Failed to update student.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public async Task DeleteStudentAsync(int studentId)
    {
        try
        {
            var success = await _deleteStudent.ExecuteAsync(studentId);
            if (success)
            {
                MessageBox.Show("Student deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadStudentsAsync();
            }
            else
            {
                MessageBox.Show("Failed to delete student.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void EditStudent(StudentModel student)
    {
        StudentId = student.StudentId;
        FirstName = student.FirstName;
        LastName = student.LastName;
        Age = student.Age;
        Course = student.Course;
        IsEditMode = true;
    }

    private void ClearForm()
    {
        StudentId = 0;
        FirstName = string.Empty;
        LastName = string.Empty;
        Age = 0;
        Course = string.Empty;
    }

    private void FilterStudents()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            FilteredStudents = new ObservableCollection<StudentModel>(Students);
        }
        else
        {
            var filtered = Students.Where(s =>
                s.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                s.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                s.Course.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            FilteredStudents = new ObservableCollection<StudentModel>(filtered);
        }
    }
}
