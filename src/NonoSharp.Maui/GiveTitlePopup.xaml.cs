using CommunityToolkit.Maui.Views;

namespace NonoSharp.Maui;

public partial class GiveTitlePopup : Popup<String?>
{
	private readonly string? currentTitle;

	public GiveTitlePopup(string? currentTitle)
	{
		this.currentTitle = currentTitle;
		InitializeComponent();
	}

	async void OnTitleButtonClicked(object? sender, EventArgs e)
	{
		var title = TitleEntry.Text?.Trim();

		if (string.IsNullOrEmpty(title))
		{
			await CloseAsync(null);
		}
		else
		{
			await CloseAsync(title);
		}
	}

	async void OnCancelButtonClicked(object? sender, EventArgs e)
	{
		await CloseAsync(currentTitle);
	}
}