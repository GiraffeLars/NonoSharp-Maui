using CommunityToolkit.Maui.Views;

namespace NonoSharp.Maui;

public partial class DimensionsPopup : Popup<Int32>
{
	public DimensionsPopup()
	{
		InitializeComponent();
	}
    
    public async void OnSize5ButtonClicked(object? sender, EventArgs e)
	{
		await CloseAsync(5);
	}
    public async void OnSize10ButtonClicked(object? sender, EventArgs e)
    {
        await CloseAsync(10);
    }
    public async void OnSize15ButtonClicked(object? sender, EventArgs e)
    {
        await CloseAsync(15);
    }
}