using System.Text.RegularExpressions;

namespace CB.NCLF22.Parsers;

public class CEFParser
{
    static List<string> Data = new List<string>();

    // removed for brevity, but could be useful!
    //static Dictionary<string, int> InvalidLogins { get; set; } = new Dictionary<string, int>();

    static Dictionary<string, int> validLogins { get; set; } = new Dictionary<string, int>();

    static int UniqueValidAttempts { get; set; } = 0;
    static Dictionary<string, int> IPs { get; set; } = new Dictionary<string, int>();

    public static void Main(string[] args)
    {
        using (var fileStream = new StreamReader(File.Open("auth.log", FileMode.Open)))
        {

            var ipRegex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            var userRegex = new Regex(@"(for|by|authenticating) user (\b[a-zA-Z0-9]{1,}\b)");
            var line = fileStream.ReadLine();
            while (line != default)
            {
                MatchCollection userMatches = userRegex.Matches(line);
                foreach (Match match in userMatches)
                {

                    var username = match.Value.Split(' ').Last();

                    if (validLogins.ContainsKey(username))
                    {
                        validLogins[username] += 1;
                    }
                    else
                    {
                        validLogins.Add(username, 1);
                        UniqueValidAttempts++;
                    }
                }

                MatchCollection matches = ipRegex.Matches(line);

                foreach (Match match in matches)
                {
                    if (IPs.ContainsKey(match.Value))
                        IPs[match.Value] += 1;
                    else
                        IPs.Add(match.Value, 1);
                }

                Data.Add(line);
                line = fileStream.ReadLine();
            }
            var sorted = validLogins.ToList().OrderByDescending(x => x.Value);

            Console.WriteLine($"Total Unique IPs: {IPs.Count()}");
            Console.WriteLine($"Total valid Users: {validLogins.Count()}");
            Console.WriteLine($"Third most attempted user: {sorted.ElementAt(2).Value}");
        }
    }
}
