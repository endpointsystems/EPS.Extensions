#! "netcoreapp3.1"
#r "nuget: EPS.Extensions.Unique,1.0.0"

using EPS.Extensions.Unique;

Console.WriteLine($"{Unique.Generate(7,0)}");

