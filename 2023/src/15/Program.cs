Console.WriteLine(File.ReadAllText("input.txt").Split(',').Select(x => x.Aggregate(0, (acc, src) => (acc + src) * 17 % 256)).Sum());
