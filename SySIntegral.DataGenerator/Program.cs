using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EllipticCurve.Utils;
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
            //client.BaseAddress = new Uri("http://msola2022-001-site1.dtempurl.com/");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var lines = new List<string>();

                var startDate = new DateTime(2022, 11, 01);
                //var line = $"DeviceId,Timestamp,WhiteEggsCount,ColorEggsCount,ReadTimestamp,ExportTimestamp";
                //Debug.WriteLine(line);
                for (var d = 0; d < 8; d++)
                {
                    var whiteCounter = 0;
                    var colorCounter = 0;
                    for (var h = 0; h < 24; h++)
                    {
                        // Create registries per hour
                        for (var r = 0; r < 3; r++)
                        {
                            var rDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, h, GetRandomNumber(r * 12, ((r + 1) * 12) - 1), GetRandomNumber(0, 59));
                            //var data = new EggCounterRegistry
                            //{
                            //    DeviceId = "df73016a-b1bf-11ec-b909-0242ac120002",
                            //    WhiteEggsCount = whiteCounter,
                            //    ColorEggsCount = colorCounter,
                            //    ReadTimestamp = rDate.ToString("yyyyMMddHHmmss"),
                            //    ExportTimestamp = rDate.ToString("yyyyMMddHHmmss"),
                            //};

                            //var url = await CreateRegistryAsync(data);
                            //var line = $"'df73016a-b1bf-11ec-b909-0242ac120002','{DateTime.Now:G}',{whiteCounter},{colorCounter},'{rDate:G}','{rDate:G}'";

                            lines.Add($"insert into EggRegistry (InputDeviceId,[Timestamp],WhiteEggsCount,ColorEggsCount,ReadTimestamp,ExportTimestamp) VALUES (11,'{DateTime.Now:G}',{whiteCounter},{colorCounter},'{rDate:G}','{rDate:G}')");
                            //Debug.WriteLine(line);
                            //Task.Delay(500).Wait();
                            whiteCounter += GetRandomNumber(100, 1300);
                            colorCounter += GetRandomNumber(100, 1000);
                        }
                    }
                    startDate = startDate.AddDays(1);
                }

                await System.IO.File.WriteAllLinesAsync("d:\\TMP\\sysintegraloctdev3.txt", lines);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Presione cualquier tecla para terminar");
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
