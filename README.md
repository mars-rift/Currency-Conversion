# Currency Conversion

A Windows Forms application for converting between different currencies using the Polygon.io API.

## Features

- Convert between 14 major world currencies
- Real-time conversion rates via Polygon.io API
- Visual status indicators and error handling
- Modern flat UI design

## Screenshot

![image](https://github.com/user-attachments/assets/d22afeeb-ff5e-4fca-bc90-2ce38f716041)


## Requirements

- Windows OS
- .NET 8.0 or later
- Polygon.io API key

## Setup Instructions

1. **Clone the repository**
   ```
   git clone https://github.com/mars-rift/Currency-Conversion.git
   cd Currency-Conversion
   ```

2. **Set up API key**
   
   The application requires a Polygon.io API key to function. You can get a free API key by signing up at [Polygon.io](https://polygon.io/).
   
   Set up your API key as an environment variable:
   ```
   setx POLYGON_API_KEY "your-api-key-here"
   ```
   
   Note: You'll need to restart any open command prompt windows or Visual Studio for the environment variable to take effect.

3. **Build and run the application**
   
   Open the solution in Visual Studio and press F5 to build and run, or use the .NET CLI:
   ```
   dotnet build
   dotnet run
   ```

## How to Use

1. Select your source currency from the "From" dropdown
2. Select the target currency from the "To" dropdown
3. Enter the amount you want to convert
4. Click the "Convert" button
5. View the conversion result and status

## Supported Currencies

- USD (United States Dollar)
- EUR (Euro)
- GBP (British Pound Sterling)
- JPY (Japanese Yen)
- CAD (Canadian Dollar)
- AUD (Australian Dollar)
- CHF (Swiss Franc)
- CNY (Chinese Yuan)
- INR (Indian Rupee)
- RUB (Russian Ruble)
- NOK (Norwegian Krone)
- SEK (Swedish Krona)
- ISK (Icelandic Króna)
- NZD (New Zealand Dollar)

## Technical Details

The application is built using:
- C# and .NET 8.0
- Windows Forms for the UI
- System.Text.Json for JSON handling
- HttpClient for API communication

## License

MIT License

## Acknowledgements

- Currency conversion powered by [Polygon.io](https://polygon.io/)
- Icons and design inspiration from [Material Design](https://material.io/design)
