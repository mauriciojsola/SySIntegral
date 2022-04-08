using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SySIntegral.Web.Areas.Api.Controllers;

namespace SySIntegral.DataGenerator
{
    class Program
    {
     
        static readonly HttpClient client = new HttpClient();
        private static readonly Random rnd = new Random();

        public static int GetRandomNumber(int min, int max)
        {
            lock (rnd) // synchronize
            {
                return rnd.Next(min, max);
            }
        }

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://msola2022-001-site1.dtempurl.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var startDate = new DateTime(2022, 01, 05);
                for (var d = 0; d < 90; d++)
                {
                    for (var h = 0; h < 24; h++)
                    {
                        // Create 5 registries per hour
                        for (var r = 0; r < 5; r++)
                        {
                            var rDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, h, GetRandomNumber(0, 59), GetRandomNumber(0, 59));
                            var data = new EggCounterRegistry
                            {
                                DeviceId = "df73016a-b1bf-11ec-b909-0242ac120002",
                                WhiteEggsCount = GetRandomNumber(100, 1300),
                                ColorEggsCount = GetRandomNumber(100, 1000),
                                ReadTimestamp = rDate.ToString("yyyyMMddHHmmss"),
                                ExportTimestamp = rDate.ToString("yyyyMMddHHmmss"),
                            };

                            var url = await CreateRegistryAsync(data);
                            Console.WriteLine($"Created Registry for date {rDate:u}");
                            Task.Delay(500).Wait();
                        }
                    }
                    startDate = startDate.AddDays(1);
                }
               

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();

            static async Task<Uri> CreateRegistryAsync(EggCounterRegistry data)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(
                    "api/contador-huevos/registro", data);
                response.EnsureSuccessStatusCode();

                // return URI of the created resource.
                return response.Headers.Location;
            }


        }
    }
}
