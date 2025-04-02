using Maui.eCommerce.ViewModels;


namespace Maui.eCommerce.Views
{
    public partial class CartManagementView : ContentPage
    {
        public CartManagementView()
        {
            InitializeComponent();
        }
        private  void GoBackClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//InventoryManagement");
        }

    }
}