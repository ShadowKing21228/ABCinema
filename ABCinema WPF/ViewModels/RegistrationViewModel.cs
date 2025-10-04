using System.ComponentModel;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ABCinema_WPF.Data;
using ABCinema_WPF.Models;
using ABCinema_WPF.Services;
using ABCinema_WPF.Utils;
using ABCinema_WPF.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ABCinema_WPF.ViewModels;

public class RegistrationViewModel()
{
    public string Email { get; set; } = string.Empty;
    
    public string UserName { get; set; } = string.Empty;
    
    public ICommand RegisterCommand { get; }
    
    private readonly IDialogCoordinator dialogCoordinator;
    
    private readonly IWindowService _windowService;
    
    public RegistrationViewModel(IDialogCoordinator dialogCoordinator, IWindowService windowService) : this()
    {
        _windowService = windowService;
        RegisterCommand = new RelayCommand(ExecuteLogin);
        this.dialogCoordinator = dialogCoordinator;
    }
    
    private async Task ExecuteLogin(object? parameter)
    {
        if (parameter is not PasswordBox passwordBox) return;
        
        var password = passwordBox.Password;

        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(Email) ||  string.IsNullOrEmpty(UserName))
            await dialogCoordinator.ShowMessageAsync(this, "All field is need be filled", "Some fields is not filled, but all is required.");
        
        else
        {
            
            if (!MailAddress.TryCreate(Email, out var mailAddress))
                await dialogCoordinator.ShowMessageAsync(this, "Your Email is not match",
                    "Email Text is not email for real. Type real email, please.");
            
            else
            {
                if (await AuthModel.Register(UserName, Email, password))
                {
                    await UserSession.RecordUserData(Email);
                    _windowService.CloseActive();
                    _windowService.Show<MainWindow>();
                }

                else
                    await dialogCoordinator.ShowMessageAsync(this, "Registration Failed", 
                        $"Пользователь с почтой {Email} уже существует!");
            }
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
}