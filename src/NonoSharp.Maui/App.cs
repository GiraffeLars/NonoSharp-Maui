using Microsoft.Maui.Controls;

namespace Picross.Maui;

public class App : Application
{
    public App()
    {
        MainPage = new NavigationPage(new MainPage());
    }
}