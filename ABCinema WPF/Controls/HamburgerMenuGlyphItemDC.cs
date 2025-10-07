using System.Windows;
using MahApps.Metro.Controls;

namespace ABCinema_WPF.Controls;

public class HamburgerMenuGlyphItemDc : HamburgerMenuGlyphItem
{
    private object? _viewModel;
    
    public FrameworkElement? View => Tag as FrameworkElement;
    
    public object? ViewModel
    {
        get => _viewModel;
        set
        {
            _viewModel = value;

            if (Tag is FrameworkElement fe) {
                fe.DataContext = value;
            }
        }
    }

    public new object? Tag
    {
        get => base.Tag;
        init
        {
            base.Tag = value;
            TryApplyDataContext();
        }
    }

    private void TryApplyDataContext()
    {
        if (Tag is FrameworkElement fe && _viewModel != null)
            fe.DataContext = _viewModel;
    }
    
    public async Task InitViewModelAsync(Func<FrameworkElement, Task<object>> factory)
    {
        if (View != null)
        {
            var vm = await factory(View);
            View.DataContext = vm;
        }
    }
}