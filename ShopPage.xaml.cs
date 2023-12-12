using Microsoft.Maui.Devices.Sensors;
using Plugin.LocalNotification;
using Vilcu_Ana_Lab7.Models;

namespace Vilcu_Ana_Lab7;

public partial class ShopPage : ContentPage
{
	public ShopPage()
	{
		InitializeComponent();

	}

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        await App.Database.SaveShopAsync(shop);
        await Navigation.PopAsync();
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;

        if (await DisplayAlert("Delete Shop", "Are you sure you want to delete this shop?", "Yes", "No"))
        {
            await App.Database.DeleteShopAsync(shop);
            await Navigation.PopAsync();
        }
    }


    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        var address = shop.Address;
        var locations = await Geocoding.GetLocationsAsync(address);

        var options = new MapLaunchOptions
        {
            Name = "Magazinul meu preferat" };

        var location = locations?.FirstOrDefault();

        // var myLocation = await Geolocation.GetLocationAsync();
        var myLocation = new Location(46.77568636785231, 23.619091046239856);

        var distance = myLocation.CalculateDistance(location, DistanceUnits.Kilometers);
        if (distance < 4)
        {
            var request = new NotificationRequest
            {
                Title = "Ai de facut cumparaturi in apropiere!",
                Description = address,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(1)
                }
            };
            await LocalNotificationCenter.Current.Show(request);
        }

        await Map.OpenAsync(location, options);
    }
}