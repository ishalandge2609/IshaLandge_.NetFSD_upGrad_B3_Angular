namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Product product = new Product();
            var products = product.GetProducts();

            // 1.FMCG Products
            Console.WriteLine("\n1. FMCG Products:");
            var q1 = from p in products
                     where p.ProCategory == "FMCG"
                     select p;

            foreach (var item in q1)
                Console.WriteLine($"{item.ProCode}\t{item.ProName}\t{item.ProMrp}");

            // 2. Grain Products
            Console.WriteLine("\n2. Grain Products:");
            var q2 = from p in products
                     where p.ProCategory == "Grain"
                     select p;

            foreach (var item in q2)
                Console.WriteLine($"{item.ProCode}\t{item.ProName}\t{item.ProMrp}");

            // 3. Sort by Product Code
            Console.WriteLine("\n3. Sort by Product Code:");
            var q3 = from p in products
                     orderby p.ProCode ascending
                     select p;

            foreach (var item in q3)
                Console.WriteLine($"{item.ProCode}\t{item.ProName}");

            // 4. Sort by Category
            Console.WriteLine("\n4. Sort by Category:");
            var q4 = from p in products
                     orderby p.ProCategory ascending
                     select p;

            foreach (var item in q4)
                Console.WriteLine($"{item.ProCategory}\t{item.ProName}");

            // 5. Sort by MRP (Ascending)
            Console.WriteLine("\n5. Sort by MRP Asc:");
            var q5 = from p in products
                     orderby p.ProMrp ascending
                     select p;

            foreach (var item in q5)
                Console.WriteLine($"{item.ProName}\t{item.ProMrp}");

            // 6. Sort by MRP (Descending)
            Console.WriteLine("\n6. Sort by MRP Desc:");
            var q6 = from p in products
                     orderby p.ProMrp descending
                     select p;

            foreach (var item in q6)
                Console.WriteLine($"{item.ProName}\t{item.ProMrp}");

            // 7. Group by Category
            Console.WriteLine("\n7. Group by Category:");
            var q7 = from p in products
                     group p by p.ProCategory;

            foreach (var group in q7)
            {
                Console.WriteLine($"\nCategory: {group.Key}");
                foreach (var item in group)
                    Console.WriteLine(item.ProName);
            }

            // 8. Group by MRP
            Console.WriteLine("\n8. Group by MRP:");
            var q8 = from p in products
                     group p by p.ProMrp;

            foreach (var group in q8)
            {
                Console.WriteLine($"\nMRP: {group.Key}");
                foreach (var item in group)
                    Console.WriteLine(item.ProName);
            }

            // 9. Highest Price in FMCG
            Console.WriteLine("\n9. Highest Price in FMCG:");
            var q9 = (from p in products
                      where p.ProCategory == "FMCG"
                      orderby p.ProMrp descending
                      select p).FirstOrDefault();

            Console.WriteLine($"{q9.ProName}\t{q9.ProMrp}");

            // 10. Total Count
            Console.WriteLine("\n10. Total Products:");
            var q10 = (from p in products select p).Count();
            Console.WriteLine(q10);

            // 11. FMCG Count
            Console.WriteLine("\n11. FMCG Count:");
            var q11 = (from p in products
                       where p.ProCategory == "FMCG"
                       select p).Count();
            Console.WriteLine(q11);

            // 12. Max Price
            Console.WriteLine("\n12. Max Price:");
            var q12 = (from p in products select p.ProMrp).Max();
            Console.WriteLine(q12);

            // 13. Min Price
            Console.WriteLine("\n13. Min Price:");
            var q13 = (from p in products select p.ProMrp).Min();
            Console.WriteLine(q13);

            // 14. All below 30?
            Console.WriteLine("\n14. All below 30?");
            var q14 = (from p in products select p).All(p => p.ProMrp < 30);
            Console.WriteLine(q14);

            // 15. Any below 30?
            Console.WriteLine("\n15. Any below 30?");
            var q15 = (from p in products select p).Any(p => p.ProMrp < 30);
            Console.WriteLine(q15);

            Console.ReadLine();

        }
        }
    }


