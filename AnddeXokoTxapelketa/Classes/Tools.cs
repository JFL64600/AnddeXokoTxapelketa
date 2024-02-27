using AnddeXokoTxapelketa.Models;
using System.IO;
using System.Text;
using System.Text.Json;

namespace AnddeXokoTxapelketa.Classes
{
    public class Tools
    {
        #region Methods
        //public static List<Tournament> GetTournaments(string root)
        //{
        //    List<Tournament> results = [];
        //    DirectoryInfo diRoot = new(root);
        //    foreach (DirectoryInfo di in diRoot.GetDirectories())
        //    {
        //        string fileName = Path.Combine(root, di.Name, "tournament.json");
        //        if (File.Exists(fileName))
        //        {
        //            using StreamReader sr = new(fileName, Encoding.UTF8);
        //            results.Add(JsonSerializer.Deserialize<Tournament>(sr.ReadToEnd()));
        //        }
        //    }
        //    return results;
        //}
        public static List<Tournament> GetTournaments(string root)
        {
            List<Tournament> results = [];
            DirectoryInfo diRoot = new(root);
            foreach (DirectoryInfo di in diRoot.GetDirectories())
            {
                string fileName = Path.Combine(root, di.Name, "tournament.json");
                if (File.Exists(fileName))
                {
                    using StreamReader sr = new(fileName, Encoding.UTF8);
                    results.Add(JsonSerializer.Deserialize<Tournament>(sr.ReadToEnd()));
                }
            }
            return results;
        }
        public static void CreateTournament(string root, string name, int girls, int girlsGroups, int boys, int boysGroups)
        {
            string path = Path.Combine(root, name);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            SaveTournament(
                root,
                new Tournament()
                {
                    Name = name,
                    Girls = GetGroups(girls, girlsGroups),
                    Boys = GetGroups(boys, boysGroups)
                });
        }
        public static void SaveTournament(string root, Tournament tournament)
        {
            string path = Path.Combine(root, tournament.Name);
            using StreamWriter sw = new(Path.Combine(path, "tournament.json"), false, Encoding.UTF8);
            sw.Write(JsonSerializer.Serialize(tournament, new JsonSerializerOptions() { WriteIndented = true }));
        }
        //public static List<Group> CreateGroups(int teams, int groups)
        //{
        //    List<Group> result = [];
        //    int teamsGroup = Math.DivRem(teams, groups, out int teamsPlus);
        //    for (int i = 0; i < groups; i++)
        //    {
        //        result.Add(new Group()
        //        {
        //            Code = ((char)(65 + i)).ToString()
        //        });
        //        int newTeamsGroup = teamsGroup;
        //        if (teamsPlus > 0)
        //        {
        //            newTeamsGroup++;
        //            teamsPlus--;
        //        }
        //        for (int j = 0; j < newTeamsGroup; j++)
        //        {
        //            result.Last().Teams.Add(new Team()
        //            {
        //                Code = $"{result.Last().Code}{j + 1}",
        //                Results = CreateResults(newTeamsGroup - 1)
        //            });
        //        }
        //    }
        //    return result;
        //}
        //public static List<Result> CreateResults(int teams)
        //{
        //    List<Result> result = [];
        //    for (int i = 0; i < teams; i++)
        //    {
        //        result.Add(new Result());
        //    }
        //    return result;
        //}
        //public static bool NumberValidation(string value)
        //{
        //    System.Text.RegularExpressions.Regex regex = new("[^0-9]+");
        //    return regex.IsMatch(value);
        //}
        //public static int GetIntegerText(string value)
        //{
        //    if (string.IsNullOrWhiteSpace(value))
        //    {
        //        return 0;
        //    }
        //    return int.Parse(value);
        //}
        //public static int GetScoreMaxCount(int scoreMax, params int[] scores)
        //{
        //    return (from int s in scores
        //            where s == scoreMax
        //            select s).Count();
        //}
        //public static string GetScore(int scoreMax, int[] scores, int[] againstScores)
        //{
        //    if (scores.Sum() + againstScores.Sum() == 0)
        //    {
        //        return string.Empty;
        //    }
        //    int scoreMaxCount = GetScoreMaxCount(scoreMax, scores);
        //    if (scoreMaxCount == 3)
        //    {
        //        return "V";
        //    }
        //    int points = 0;
        //    for (int i = 0; i < againstScores.Length; i++)
        //    {
        //        if (againstScores[i] == scoreMax)
        //        {
        //            points += scores[i];
        //        }
        //    }
        //    return $"{scoreMaxCount} {points}";
        //}
        //public static int[] GetResult(Result result)
        //{
        //    return [result.M1, result.M2, result.M3, result.M4, result.M5];
        //}
        #endregion
        #region Privates
        private static List<Group> GetGroups(int players, int groups)
        {
            List<Group> result = [];
            int playersGroup = Math.DivRem(players, groups, out int playersPlus);
            for (int i = 0; i < groups; i++)
            {
                result.Add(new Group());
                int newPlayersGroup = playersGroup;
                if (playersPlus > 0)
                {
                    newPlayersGroup++;
                    playersPlus--;
                }
                for (int j = 0; j < newPlayersGroup; j++)
                {
                    result.Last().Players.Add(new Player() { Results = [.. new int[newPlayersGroup - 1]] });
                }
            }
            return result;
        }
        #endregion
    }
}
