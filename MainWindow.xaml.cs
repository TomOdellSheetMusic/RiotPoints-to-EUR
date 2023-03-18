using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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
            AmountTextBox.Text = "1";
            // Attach event handlers to the buttons
            PlusButton.Click += PlusButton_Click;
            MinusButton.Click += MinusButton_Click;

            // Attach event handlers to the text boxes
            ItemPriceTextBox.TextChanged += TextBox_TextChanged;
            AmountTextBox.TextChanged += TextBox_TextChanged;
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox_TextChanged(null, null);
            double itemCountPrice = total;
            // Überprüfen Sie, ob der Benutzer eine gültige Ganzzahl eingegeben hat
            if (int.TryParse(ItemPriceTextBox.Text, out int itemPrice))
            {
                // Berechnen Sie den Preis in Euro und die Anzahl der Bundles für jede RP-Bundle-Größe
                var bundleResults = new List<BundleResult>();
                foreach (KeyValuePair<int, double> rpBundle in rpBundles)
                {
                    double euroCost = rpBundle.Value * itemCountPrice / rpBundle.Key;
                    double priceBundles = rpBundle.Value;
                    int numBundles = (int)Math.Ceiling((double)itemCountPrice / rpBundle.Key);
                    double gesGeld = rpBundle.Value * numBundles;
                    bundleResults.Add(new BundleResult(rpBundle.Key, priceBundles, euroCost,  numBundles, gesGeld));
                }

                // Aktualisieren Sie die Listview mit den Bundle-Ergebnissen
                ResultsListView.ItemsSource = bundleResults;
                
                // Formatieren Sie die Anzeige der Zahlen auf zwei Nachkommastellen
                ItemPriceTextBox.Text = itemPrice.ToString("N2");
                foreach (BundleResult bundleResult in bundleResults)
                {
                    bundleResult.BundleSize = int.Parse(bundleResult.BundleSize.ToString());
                    bundleResult.EuroCost = double.Parse(bundleResult.EuroCost.ToString("N2"));
                    bundleResult.PriceBundles = double.Parse(bundleResult.PriceBundles.ToString("N2"));
                }
                ItemPriceTextBox.Text = itemPrice.ToString("0");
                
            }
        }
        private double itemPrice = 0;
        private double amount = 0;
        public double total = 0;
        public double currentItemPrice;

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Try to parse the text in the text boxes as decimals
            double.TryParse(ItemPriceTextBox.Text, out itemPrice);
            double.TryParse(AmountTextBox.Text, out amount);
        // Calculate the total and update the TotalTextBox
            total = itemPrice * amount;
            currentItemPrice = itemPrice;
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            // Try to parse the text in the text boxes as decimals
            double.TryParse(ItemPriceTextBox.Text, out itemPrice);
            double.TryParse(AmountTextBox.Text, out amount);
            // Increment the amount and update the AmountTextBox
            amount++;
            AmountTextBox.Text = amount.ToString();
            CalculateButton_Click(null, null);
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            // Try to parse the text in the text boxes as decimals
            double.TryParse(ItemPriceTextBox.Text, out itemPrice);
            double.TryParse(AmountTextBox.Text, out amount);
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

