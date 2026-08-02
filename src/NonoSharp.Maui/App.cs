using Microsoft.Maui.Controls;

namespace Picross.Maui;

public class App : Application
{
    public App()
    {
        Resources.MergedDictionaries.Add(new Resources.Styles.Colors());
        MainPage = new NavigationPage(new MainPage());
    }
}