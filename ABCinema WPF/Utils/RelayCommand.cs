using System.Windows.Input;

namespace ABCinema_WPF.Utils;

public class RelayCommand(Func<object?, Task> execute, Func<object?, bool>? canExecute = null) : ICommand
{
    public bool CanExecute(object? parameter) => canExecute?.Invoke(parameter) ?? true;
    public async void Execute(object? parameter) => await execute(parameter);
    public event EventHandler? CanExecuteChanged;
}
