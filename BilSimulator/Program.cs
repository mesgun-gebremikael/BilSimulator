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
        }
    }

} 
              




          
