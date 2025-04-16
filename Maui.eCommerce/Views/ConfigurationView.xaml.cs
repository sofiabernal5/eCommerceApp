using Maui.eCommerce.ViewModels;
using System;

namespace Maui.eCommerce.Views
{
    public partial class ConfigurationView : ContentPage
    {
        private ConfigurationViewModel ViewModel => BindingContext as ConfigurationViewModel;
        
        public ConfigurationView()
        {
            InitializeComponent();
            BindingContext = new ConfigurationViewModel();
        }
        
        private void TaxRateTextChanged(object sender, TextChangedEventArgs e)
        {
            // Validation can be handled here if needed
        }
        
        private async void SaveClicked(object sender, EventArgs e)
        {
            bool success = ViewModel.SaveSettings();
            if (success)
            {
                await DisplayAlert("Success", "Settings saved successfully", "OK");
                await GoBack();
            }
            else
            {
                await DisplayAlert("Error", "Please enter a valid tax rate percentage", "OK");
            }
        }
        
        private async void CancelClicked(object sender, EventArgs e)
        {
            await GoBack();
        }
        
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}