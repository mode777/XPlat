// See https://aka.ms/new-console-template for more information
using AzgaarMapReader;
using Newtonsoft.Json;

Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(File.ReadAllText("samples/Oria.json"));
Console.ReadLine();