using System.Text.Json;
using System.Runtime.Versioning;

namespace Convert
{
    [SupportedOSPlatform("windows")]
    public partial class Form1 : Form
    {
        
    }
}

namespace Convert
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly Color _primaryColor = Color.FromArgb(41, 128, 185); // Nice blue
        private readonly Color _secondaryColor = Color.FromArgb(46, 204, 113); // Green
        private readonly Color _accentColor = Color.FromArgb(230, 126, 34); // Orange
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        // UI Controls
        private ComboBox cmbFromCurrency = null!;
        private ComboBox cmbToCurrency = null!;
        private TextBox txtAmount = null!;
        private Button btnConvert = null!;
        private Label lblResult = null!;
        private Label lblStatus = null!;
        private Panel headerPanel = null!;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();

            // Get API key from environment variable  
            _apiKey = Environment.GetEnvironmentVariable("POLYGON_API_KEY") ?? string.Empty;

            if (string.IsNullOrEmpty(_apiKey))
            {
                lblStatus.Text = "Warning: API key not found. Set POLYGON_API_KEY environment variable.";
                lblStatus.ForeColor = Color.Red;
                btnConvert.Enabled = false;
            }

            _httpClient = new HttpClient();
        }

        private void InitializeCustomComponents()
        {
            // Form settings
            this.Text = "Currency Converter";
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Header panel
            headerPanel = new Panel
            {
                BackColor = _primaryColor,
                Dock = DockStyle.Top,
                Height = 70
            };

            Label lblHeader = new()
            {
                Text = "Currency Converter",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            headerPanel.Controls.Add(lblHeader);
            this.Controls.Add(headerPanel);

            // From Currency
            Label lblFrom = new()
            {
                Text = "From:",
                Location = new Point(30, 100),
                AutoSize = true
            };
            this.Controls.Add(lblFrom);

            cmbFromCurrency = new ComboBox
            {
                Location = new Point(150, 100),
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFromCurrency.Items.AddRange(["USD", "EUR", "GBP", "JPY", "CAD", "AUD", "CHF", "CNY", "INR", "RUB", "NOK", "SEK", "ISK", "NZD"]);
            cmbFromCurrency.SelectedIndex = 5; // AUD selected by default
            this.Controls.Add(cmbFromCurrency);

            // To Currency
            Label lblTo = new()
            {
                Text = "To:",
                Location = new Point(30, 140),
                AutoSize = true
            };
            this.Controls.Add(lblTo);

            cmbToCurrency = new ComboBox
            {
                Location = new Point(150, 140),
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbToCurrency.Items.AddRange(["USD", "EUR", "GBP", "JPY", "CAD", "AUD", "CHF", "CNY", "INR", "RUB", "NOK", "SEK", "ISK", "NZD"]);
            cmbToCurrency.SelectedIndex = 0; // USD selected by default
            this.Controls.Add(cmbToCurrency);

            // Amount
            Label lblAmount = new()
            {
                Text = "Amount:",
                Location = new Point(30, 180),
                AutoSize = true
            };
            this.Controls.Add(lblAmount);

            txtAmount = new TextBox
            {
                Location = new Point(150, 180),
                Width = 120,
                Text = "100"
            };
            this.Controls.Add(txtAmount);

            // Convert button
            btnConvert = new Button
            {
                Text = "Convert",
                Location = new Point(150, 220),
                Size = new Size(120, 40),
                BackColor = _secondaryColor,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnConvert.FlatAppearance.BorderSize = 0;
            btnConvert.Click += BtnConvert_Click;
            this.Controls.Add(btnConvert);

            // Result label
            lblResult = new Label
            {
                Text = "Result will appear here",
                Location = new Point(30, 280),
                Size = new Size(420, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(240, 240, 240)
            };
            this.Controls.Add(lblResult);

            // Status label
            lblStatus = new Label
            {
                Text = "Ready",
                Location = new Point(30, 320),
                Size = new Size(420, 20),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblStatus);
        }

        private async void BtnConvert_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                lblStatus.Text = "API key not set. Set the POLYGON_API_KEY environment variable.";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                lblStatus.Text = "Please enter a valid amount.";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            if (cmbFromCurrency.SelectedItem == null || cmbToCurrency.SelectedItem == null)
            {
                lblStatus.Text = "Please select both currencies.";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            string fromCurrency = cmbFromCurrency.SelectedItem.ToString() ?? string.Empty;
            string toCurrency = cmbToCurrency.SelectedItem.ToString() ?? string.Empty;

            lblStatus.Text = "Converting...";
            lblStatus.ForeColor = _accentColor;
            btnConvert.Enabled = false;

            try
            {
                var result = await ConvertCurrencyAsync(fromCurrency, toCurrency, amount);
                if (result != null)
                {
                    lblResult.Text = $"{amount} {fromCurrency} = {result.Converted} {toCurrency}";
                    lblStatus.Text = "Conversion successful.";
                    lblStatus.ForeColor = _secondaryColor;
                }
                else
                {
                    lblResult.Text = "Conversion failed.";
                    lblStatus.Text = "Error: Conversion result is null.";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = "Conversion failed.";
                lblStatus.Text = $"Error: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnConvert.Enabled = true;
            }
        }

        private async Task<ConversionResult?> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            string url = $"https://api.polygon.io/v1/conversion/{fromCurrency}/{toCurrency}?amount={amount}&precision=2&apiKey={_apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API error: {response.StatusCode}");
            }

            string responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ConversionResult>(responseContent, _jsonSerializerOptions);
        }

        // Class to deserialize the API response
        private class ConversionResult
        {
            public string? From { get; set; }
            public string? To { get; set; }
            public decimal InitialAmount { get; set; }
            public decimal Converted { get; set; }
        }
    }
}
