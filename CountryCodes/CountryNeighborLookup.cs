using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class CountryNeighborLookup
{
    private static readonly string CountriesFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "countries-with-neighbors.json");

    public static void Run()
    {
        string countryCode = ReadCountryCodeFromUser();
        List<Country> countries = LoadCountries();
        Country selectedCountry = FindCountryByCode(countries, countryCode);
        CheckIsCountryNull(selectedCountry);
        DisplayAdjacentCountries(selectedCountry);
    }

    private static string ReadCountryCodeFromUser()
    {
        Console.Write("Enter country code: ");
        return Console.ReadLine()?.ToUpper();
    }

    private static List<Country> LoadCountries()
    {
        if (!File.Exists(CountriesFilePath))
        {
            throw new FileNotFoundException("Countries data file not found.");
        }

        string jsonContent = File.ReadAllText(CountriesFilePath);
        return JsonConvert.DeserializeObject<List<Country>>(jsonContent);
    }

    private static Country FindCountryByCode(IEnumerable<Country> countries, string countryCode)
    {
        Country country = countries.FirstOrDefault(
            c => c.CountryCode.Equals(countryCode));

        return country;
    }

    private static void CheckIsCountryNull(Country country)
    {
        if (country == null)
            throw new InvalidDataException("Country Code not found.");
    }

    private static void DisplayAdjacentCountries(Country country)
    {
        if (country.AdjacentCountries.Count == 0)
        {
            Console.WriteLine("No adjacent countries found.");
            return;
        }

        Console.WriteLine("Adjacent Countries:");
        foreach (string neighbor in country.AdjacentCountries)
        {
            Console.WriteLine("- " + neighbor);
        }
    }
}
