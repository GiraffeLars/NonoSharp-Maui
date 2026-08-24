using Microsoft.Maui.Controls;

namespace NonoSharp.Maui;

public class App : Application
{
    public App()
    {
        Resources.MergedDictionaries.Add(new Resources.Styles.Colors());
        MainPage = new NavigationPage(new MainPage());
    }
}