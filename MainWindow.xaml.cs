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
                    int numBundles = (int)Math.Ceiling((double)itemPrice/ rpBundle.Key);
                    bundleResults.Add(new BundleResult(rpBundle.Key, euroCost, numBundles));
                }

                // Aktualisieren Sie die Listview mit den Bundle-Ergebnissen
                ResultsListView.ItemsSource = bundleResults;

                // Formatieren Sie die Anzeige der Zahlen auf zwei Nachkommastellen
                ItemPriceTextBox.Text = itemPrice.ToString("N2");
                foreach (BundleResult bundleResult in bundleResults)
                {
                    bundleResult.BundleSize = int.Parse(bundleResult.BundleSize.ToString());
                    bundleResult.EuroCost = double.Parse(bundleResult.EuroCost.ToString("N2"));
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
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            // Try to parse the text in the text boxes as decimals
            double.TryParse(ItemPriceTextBox.Text, out itemPrice);
            double.TryParse(AmountTextBox.Text, out amount);
            // Decrement the amount and update the AmountTextBox
            amount--;
            AmountTextBox.Text = amount.ToString();
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

