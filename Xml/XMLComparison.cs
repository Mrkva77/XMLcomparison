using System;
using System.Linq;
using System.Xml.Linq;

namespace XMLcomparison
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Unesite putanju 1. XML datoteke:");
            string filePath1 = Console.ReadLine();

            if (string.IsNullOrEmpty(filePath1) || !System.IO.File.Exists(filePath1))
            {
                Console.WriteLine("Datoteka nije pronadjena. Provjerite putanju.");
                return;
            }

            Console.WriteLine("Unesite putanju 2. XML datoteke:");
            string filePath2 = Console.ReadLine();

            if (string.IsNullOrEmpty(filePath2) || !System.IO.File.Exists(filePath2))
            {
                Console.WriteLine("Datoteka nije pronadjena. Provjerite putanju.");
                return;
            }

            try
            {
                XDocument doc1 = XDocument.Load(filePath1);
                XDocument doc2 = XDocument.Load(filePath2);

                var books1 = doc1.Descendants("book").Select(b => new
                {
                    Id = b.Attribute("id")?.Value,
                    Image = b.Attribute("image")?.Value,
                    Name = b.Attribute("name")?.Value
                }).ToList();

                var books2 = doc2.Descendants("book").Select(b => new
                {
                    Id = b.Attribute("id")?.Value,
                    Image = b.Attribute("image")?.Value,
                    Name = b.Attribute("name")?.Value
                }).ToList();

                var differences = books1.Zip(books2, (b1, b2) => new
                {
                    Id1 = b1.Id,
                    Image1 = b1.Image,
                    Name1 = b1.Name,
                    Id2 = b2.Id,
                    Image2 = b2.Image,
                    Name2 = b2.Name
                })
                .Where(diff => diff.Id1 != diff.Id2 || diff.Image1 != diff.Image2 || diff.Name1 != diff.Name2)
                .ToList();

                if (differences.Any())
                {
                    foreach (var diff in differences)
                    {
                        Console.WriteLine($"Razlika: ");
                        Console.WriteLine($"ID1: {diff.Id1} vs ID2: {diff.Id2}");
                        Console.WriteLine($"Image1: {diff.Image1} vs Image2: {diff.Image2}");
                        Console.WriteLine($"Name1: {diff.Name1} vs Name2: {diff.Name2}");
                        Console.WriteLine("--------------------");
                    }
                }
                else
                {
                    Console.WriteLine("Nema razlika između dokumenta.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Došlo je do greške prilikom učitavanja ili usporedbe XML dokumenata: {ex.Message}");
            }

            Console.WriteLine("Pritisnite Enter za izlaz...");
            Console.ReadLine();
        }
    }
}