using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BilSimulator
{
    internal class Program
    {
        // Mina egna värden till ThingSpeak
        private const string ChannelId = "3134394";
        private const string WriteApiKey = "4OYTWST0953SEX3F";

        static async Task Main(string[] args)
        {
            // Startposition för bilen (ungefär Stockholm)
            double latitude = 59.3293;
            double longitude = 18.0686;

            Console.WriteLine("BilSimulator startar... (Tryck Ctrl + C för att avsluta)\n");

            var slump = new Random();
            int rpm = slump.Next(800, 1200);
            int hastighet = 0;
            int bransle = 100;
            double temp = 85;

            while (true)
            {
                // Slumpa värden (simulerar körning)
                int steg = slump.Next(-5, 10);
                hastighet = Math.Clamp(hastighet + steg, 0, 120);
                rpm = Math.Clamp((hastighet * 50) + slump.Next(-200, 200), 800, 6000);

                if (hastighet > 0)
                    bransle = Math.Max(0, bransle - 1);

                temp = Math.Clamp(80 + (rpm / 6000.0) * 20, 80, 100);

                // Flytta bilen lite (gps-rörelse)
                latitude += (slump.NextDouble() - 0.5) / 1000;
                longitude += (slump.NextDouble() - 0.5) / 1000;

                // Skapa ibland en felkod
                string felkod = "OK";
                if (slump.Next(0, 20) == 5) // slumpmässigt ibland
                {
                    string[] felkoder = { "P0300", "P0171", "P0420", "P0128" };
                    felkod = felkoder[slump.Next(felkoder.Length)];
                    Console.WriteLine("Felkod upptäckt: " + felkod);
                }

                Console.WriteLine($"Skickar data => RPM: {rpm}, Hastighet: {hastighet} km/h, Bränsle: {bransle}%, Temp: {temp:F1}°C");

                // Skicka till ThingSpeak
                await SkickaTillThingSpeak(rpm, hastighet, bransle, temp, felkod, latitude, longitude);

                // Vänta 15 sekunder (ThingSpeak kräver paus)
                await Task.Delay(TimeSpan.FromSeconds(15));
            }
        }
        // Den här metoden skickar datan till ThingSpeak
        private static async Task SkickaTillThingSpeak(int rpm, int hastighet, int bransle, double temp, string felkod, double latitude, double longitude)
        {
            string url = $"https://api.thingspeak.com/update?api_key={WriteApiKey}" +
                         $"&field1={rpm}" +
                         $"&field2={hastighet}" +
                         $"&field3={bransle}" +
                         $"&field4={temp.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                         $"&field5={felkod}" +
                         $"&field6={latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                         $"&field7={longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Data skickad till ThingSpeak.\n");
                }
                else
                {
                    Console.WriteLine("Kunde inte skicka data till ThingSpeak.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fel vid anslutning: " + ex.Message);
            }
        }
    }
}


              




          
