using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Maui.eCommerce.ViewModels
{
    public class ConfigurationViewModel : INotifyPropertyChanged
    {
        private string _taxRateDisplay;
        private double _taxRate;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public ConfigurationViewModel()
        {
            // Load settings from preferences
            LoadSettings();
            
            // Initialize commands
            SaveCommand = new Command(SaveSettingsExecute);
        }
        
        public ICommand SaveCommand { get; private set; }
        
        public string TaxRateDisplay
        {
            get => _taxRateDisplay;
            set
            {
                if (_taxRateDisplay != value)
                {
                    _taxRateDisplay = value;
                    NotifyPropertyChanged();
                    
                    // Try to parse the value
                    if (double.TryParse(_taxRateDisplay, out double newTaxRate))
                    {
                        // Convert from percentage to decimal
                        _taxRate = newTaxRate / 100.0;
                    }
                }
            }
        }
        
        private void LoadSettings()
        {
            if (Preferences.ContainsKey("TaxRate"))
            {
                _taxRate = Preferences.Get("TaxRate", 0.07);
            }
            else
            {
                _taxRate = 0.07; // Default 7%
            }
            
            // Convert from decimal to percentage for display
            _taxRateDisplay = (_taxRate * 100).ToString();
        }
        
        public bool SaveSettings()
        {
            if (_taxRate >= 0)
            {
                Preferences.Set("TaxRate", _taxRate);
                return true;
            }
            
            return false;
        }
        
        private void SaveSettingsExecute()
        {
            SaveSettings();
        }
        
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}