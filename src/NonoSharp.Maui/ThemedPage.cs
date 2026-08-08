using Picross.Maui.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Picross.Maui
{
    /// <summary>
    /// A <c>ContentPage</c> that automatically changes themes when the system's theme changes.
    /// Supports add views to <c>this.views</c> to be automatically invalidated when the theme changes as well
    /// </summary>
    public partial class ThemedPage : ContentPage
    {
        protected Dictionary<string, GraphicsView> views;

        public ThemedPage() : base() 
        {
            views = new();

            // Set Background color (in case of system mismatch) and add event handling when system theme changes
            BackgroundColor = Theme.BackgroundColor;
            Application.Current!.RequestedThemeChanged += (s, a) =>
            {
                UpdateTheme();
            };
        }

        protected virtual void UpdateTheme()
        {
            BackgroundColor = Theme.BackgroundColor;

#if !ANDROID
            // Change the bar color as well to avoid a weird mismatch
            // Not on android since the nav bar is set to purple by default (colors.xml in Android folder)
            if (this.Parent is NavigationPage navPage)
            {
                navPage.BarBackgroundColor = Theme.BackgroundColor;
            }
#endif
            InvalidateViews();
        }

        internal void InvalidateViews()
        {
            foreach(GraphicsView view in views.Values)
            {
                view.Invalidate();
            }
        }
    }
}
