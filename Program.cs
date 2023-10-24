using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


class Program
{
    static double ÖsszPontszám(string nev, List<string[]> rovid, List<string[]> kur)
    {
        var rp = rovid.FirstOrDefault(k => k[0] == nev);
        var dont = kur.Where(k => k[0] == nev).ToList();

        double rovidpont = double.Parse(rp[2]) + double.Parse(rp[3]) - double.Parse(rp[4]);

        if (dont.Count == 0)
        {
            return rovidpont;
        }
        else
        {
            double kurpont = double.Parse(dont[0][2]) + double.Parse(dont[0][3]) - double.Parse(dont[0][4]);
            return rovidpont + kurpont;
        }
    }

    static void Main()
    {
        
        List<string[]> rovid = File.ReadAllLines("rovidprogram.csv").Skip(1).Select(line => line.Split(';')).ToList();
        List<string[]> kur = File.ReadAllLines("donto.csv").Skip(1).Select(line => line.Split(';')).ToList();

        Console.WriteLine("2. feladat\n\tA rövidprogramban " + rovid.Count + " induló volt");

        var orszag = kur.Select(k => k[1]).ToHashSet();
        if (orszag.Contains("HUN"))
        {
            Console.WriteLine("3. feladat: A magyar versenyző bejutott a kűrbe");
        }
        else
        {
            Console.WriteLine("3. feladat: A magyar versenyző nem jutott be a kűrbe");
        }

        Console.WriteLine("5. feladat\n\tKérem a versenyző nevét: ");
        string vers = Console.ReadLine();
        var nev = rovid.Where(k => k[0] == vers).Select(k => k[0]).ToList();

        if (nev.Count == 0)
        {
            Console.WriteLine("\tIlyen nevű induló nem volt");
        }
        else
        {
            Console.WriteLine($"6. feladat\n\tA versenyző összpontszáma: {ÖsszPontszám(nev[0], rovid, kur):.2f}");
        }

        Dictionary<string, int> orsz = new Dictionary<string, int>();
        foreach (var k in kur)
        {
            if (orsz.ContainsKey(k[1]))
            {
                orsz[k[1]]++;
            }
            else
            {
                orsz[k[1]] = 1;
            }
        }

        Console.WriteLine("7. feladat");
        foreach (var k in orsz.Keys)
        {
            if (orsz[k] > 1)
            {
                Console.WriteLine($"\t{k}: {orsz[k]} versenyző");
            }
        }

        using (StreamWriter output = new StreamWriter("vegeredmeny.csv", false, System.Text.Encoding.UTF8))
        {
            var sortedKur = kur.OrderByDescending(k => ÖsszPontszám(k[0], rovid, kur)).ToList();
            for (int i = 0; i < sortedKur.Count; i++)
            {
                output.WriteLine($"{i + 1};{sortedKur[i][0]};{sortedKur[i][1]};{ÖsszPontszám(sortedKur[i][0], rovid, kur):.2f}");
            }
        }
    }
}
