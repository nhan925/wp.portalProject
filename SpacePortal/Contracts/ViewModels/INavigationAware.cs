namespace SpacePortal.Contracts.ViewModels;

public interface INavigationAware
{
    void OnNavigatedTo(object parameter);

    void OnNavigatedFrom();
}
