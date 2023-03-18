using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RP_to_Eur
{
    public partial class MainWindow : Window
    {
        // Definieren Sie die Größen der RP-Bundles und ihre entsprechenden Kosten in Euro
        private readonly Dictionary<int, double> rpBundles = new Dictionary<int, double>()
        {
            {450, 5.00},
            {1350, 10.00},
            {2800, 20.00},
            {5000, 35.00},
            {10000, 70.00}
        };
        public MainWindow()
        {
            InitializeComponent();
        }
        public int itemCount = 1;

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            // Überprüfen Sie, ob der Benutzer eine gültige Ganzzahl eingegeben hat
            if (int.TryParse(ItemPriceTextBox.Text, out int itemPrice))
            {
                // Berechnen Sie den Preis in Euro und die Anzahl der Bundles für jede RP-Bundle-Größe
                var bundleResults = new List<BundleResult>();
                foreach (KeyValuePair<int, double> rpBundle in rpBundles)
                {
                    double euroCost = rpBundle.Value * itemPrice * itemCount / rpBundle.Key;
                    int numBundles = (int)Math.Ceiling((double)itemPrice * itemCount / rpBundle.Key);
                    bundleResults.Add(new BundleResult(rpBundle.Key, euroCost, numBundles));
                }

                // Überprüfen Sie, ob der Artikelpreis höher ist als die größte RP-Bundle-Größe
                int largestBundleSize = rpBundles.Keys.Max();
                if (itemPrice > largestBundleSize)
                {
                    double euroCost = rpBundles[largestBundleSize] * itemPrice * itemCount / largestBundleSize;
                    bundleResults.Add(new BundleResult(itemPrice, euroCost, itemCount));
                }

                // Aktualisieren Sie die Listview mit den Bundle-Ergebnissen
                ResultsListView.ItemsSource = bundleResults;

                // Formatieren Sie die Anzeige der Zahlen auf zwei Nachkommastellen
                ItemPriceTextBox.Text = itemPrice.ToString("N2");
                ItemQuantityTextBox.Text = itemCount.ToString();
                foreach (BundleResult bundleResult in bundleResults)
                {
                    bundleResult.BundleSize = int.Parse(bundleResult.BundleSize.ToString());
                    bundleResult.EuroCost = double.Parse(bundleResult.EuroCost.ToString("N2"));
                }
            }
        }

        private void ItemQuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(ItemQuantityTextBox.Text, out int itemCount))
            {
                this.itemCount = itemCount;
                CalculateButton_Click(sender, e); // recalculate the results
            }
        }

        private void IncrementButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ItemQuantityTextBox.Text, out itemCount))
            {
                itemCount++;
                ItemQuantityTextBox.Text = itemCount.ToString();
                CalculateButton_Click(sender, e); // recalculate the results
            }
        }
        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            if (itemCount > 0)
            {
                itemCount--;
                ItemQuantityTextBox.Text = itemCount.ToString();
                CalculateButton_Click(sender, e);
            }
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
        public double EuroCost { get; set; }
        public int NumBundles { get; set; }

        public BundleResult(int bundleSize, double euroCost, int numBundles)
        {
            BundleSize = bundleSize;
            EuroCost = euroCost;
            NumBundles = numBundles;
        }
    }

}

