using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace RP_to_Eur
{
    public partial class MainWindow : Window
    {

// Definieren Sie die Größen der RP-Bundles und ihre entsprechenden Kosten in Euro
private readonly Dictionary<int, double> rpBundles = new Dictionary<int, double>()
        {
            {450, 4.99},
            {1380, 10.99},
            {2800, 21.99},
            {4500, 34.99},
            {6500, 49.99},
            {13500, 99.99}
        };
        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            AmountTextBox.Text = "1";
        }
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox_TextChanged(itemPrice, null);
                // Berechnen Sie den Preis in Euro und die Anzahl der Bundles für jede RP-Bundle-Größe
                var bundleResults = new List<BundleResult>();
                foreach (KeyValuePair<int, double> rpBundle in rpBundles)
                {
                    double euroCost = rpBundle.Value * currentItemPrice / rpBundle.Key;
                    double priceBundles = rpBundle.Value;
                    int numBundles = (int)Math.Ceiling((double)currentItemPrice / rpBundle.Key);
                    double gesGeld = rpBundle.Value * numBundles;
                    bundleResults.Add(new BundleResult(rpBundle.Key, priceBundles, euroCost,  numBundles, gesGeld));
                }

                // Aktualisieren Sie die Listview mit den Bundle-Ergebnissen
                ResultsListView.ItemsSource = bundleResults;
                
                // Formatieren Sie die Anzeige der Zahlen auf zwei Nachkommastellen
                foreach (BundleResult bundleResult in bundleResults)
                {
                    bundleResult.BundleSize = int.Parse(bundleResult.BundleSize.ToString());
                    bundleResult.EuroCost = double.Parse(bundleResult.EuroCost.ToString("N2", new CultureInfo("de-DE")));
                    bundleResult.PriceBundles = double.Parse(bundleResult.PriceBundles.ToString("N2", new CultureInfo("de-DE")));
                    bundleResult.GesGeld = double.Parse(bundleResult.GesGeld.ToString("N2", new CultureInfo("de-DE")));

                }
                ItemPriceTextBox.Text = itemPrice.ToString("N0");   
        }

        private double itemPrice = 0;
        private double amount = 0;
        public double currentItemPrice;

        private void UpdateTextBoxValues()
        {
            // Try to parse the text in the text boxes as decimals
            double.TryParse(ItemPriceTextBox.Text, out itemPrice);
            double.TryParse(AmountTextBox.Text, out amount);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTextBoxValues();
            // Calculate the total and update the itemPriceTextBox
            currentItemPrice = itemPrice * amount;
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateTextBoxValues();
            // Increment the amount and update the AmountTextBox
            amount++;
            AmountTextBox.Text = amount.ToString();
            CalculateButton_Click(null, null);
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateTextBoxValues();
            if (amount > 1)
            {
                // Decrement the amount and update the AmountTextBox
                amount--;
                AmountTextBox.Text = amount.ToString();
            }
            CalculateButton_Click(null, null);
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void Click_Minimieren(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Click_Maximieren(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                MaximierenButton.ToolTip = "Maximieren";
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                MaximierenButton.ToolTip = "Fenstermodus";
            }
        }
        private void Click_Schliessen(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    // Definieren Sie eine Klasse, um die Ergebnisse für jedes Bundle zu speichern
    public class BundleResult
    {
        public int BundleSize { get; set; }
        public double PriceBundles { get; set; }
        public double EuroCost { get; set; }
        public int NumBundles { get; set; }
        public double GesGeld { get; set; }

        public BundleResult(int bundleSize, double priceBundles, double euroCost, int numBundles, double gesGeld)
        {
            BundleSize = bundleSize;
            PriceBundles = priceBundles;
            EuroCost = euroCost;
            NumBundles = numBundles;
            GesGeld = gesGeld;
        }
    }

}

