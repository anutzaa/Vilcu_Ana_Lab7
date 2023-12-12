using Vilcu_Ana_Lab7.Models;
namespace Vilcu_Ana_Lab7;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
	}

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        Shop selectedShop = (ShopPicker.SelectedItem as Shop);
        slist.ShopID = selectedShop.ID;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }

    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList) this.BindingContext)
        {
            BindingContext = new Product()
        });

    }

    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        var selectedProduct = (Product)listView.SelectedItem;

        if (selectedProduct != null)
        {
            var confirm = await DisplayAlert("Confirmation", $"Are you sure you want to delete {selectedProduct.Description}?", "Yes", "No");

            if (confirm)
            {
                // Delete the selected product from the list and update the ListView
                var shopList = (ShopList)BindingContext;
                var listProduct = await App.Database.GetListProductAsync(shopList.ID, selectedProduct.ID);

                if (listProduct != null)
                {
                    await App.Database.DeleteListProductAsync(listProduct.ID);
                    listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);
                }
            }
        }
        else
        {
            await DisplayAlert("Alert", "Please select an item to delete.", "OK");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var items = await App.Database.GetShopsAsync();
        ShopPicker.ItemsSource = (System.Collections.IList)items;
        ShopPicker.ItemDisplayBinding = new Binding("ShopDetails");

        var shopl = (ShopList)BindingContext;
        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }
}