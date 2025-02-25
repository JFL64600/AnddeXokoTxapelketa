using AnddeXokoTxapelketa.Models;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace AnddeXokoTxapelketa.Classes
{
    public class Tools
    {
        #region Methods
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
                    results.Add(System.Text.Json.JsonSerializer.Deserialize<Tournament>(sr.ReadToEnd()));
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
            using StreamWriter sw = new(Path.Combine(root, tournament.Name, "tournament.json"), false, Encoding.UTF8);
            sw.Write(System.Text.Json.JsonSerializer.Serialize(tournament, GetJsonSerializerOptions()));
            GenerateRotations(root, tournament);
        }
        public static Tournament CloneTournament(Tournament tournament)
        {
            var serialized = JsonConvert.SerializeObject(tournament);
            return JsonConvert.DeserializeObject<Tournament>(serialized);
        }
        private static void GenerateRotations(string root, Tournament tournament)
        {
            List<Rotation> rotations = GetRotations(root, tournament.Name);
            StringBuilder sb = new();
            foreach (Group group in tournament.Girls)
            {
                sb.Clear().AppendLine("N1");
                foreach (Rotation rotation in rotations)
                {
                    sb.AppendLine(rotation.Name);
                    foreach (int[] match in rotation.Matches)
                    {
                        sb.AppendLine($"{group.Players[match[0] - 1].Name} / {group.Players[match[1] - 1].Name}");
                    }
                }
                MessageBox.Show(sb.ToString());
            }
        }
        private static List<Rotation> GetRotations(string root, string tournamentName)
        {
            using StreamReader sr = new(Path.Combine(root, tournamentName, "rotations.json"), Encoding.UTF8);
            return System.Text.Json.JsonSerializer.Deserialize<List<Rotation>>(sr.ReadToEnd());
        }
        public static Leagues GetLeagues(string root, string tournamentName)
        {
            using StreamReader sr = new(Path.Combine(root, tournamentName, "leagues.json"), Encoding.UTF8);
            return System.Text.Json.JsonSerializer.Deserialize<Leagues>(sr.ReadToEnd());
        }
        public static GeneralRanking GetGeneralRanking(string root, string tournamentName)
        {
            using StreamReader sr = new(Path.Combine(root, tournamentName, "generalRanking.json"), Encoding.UTF8);
            return System.Text.Json.JsonSerializer.Deserialize<GeneralRanking>(sr.ReadToEnd());
        }
        public static void SaveGeneralRanking(string root, string tournamentName, GeneralRanking generalRanking)
        {
            generalRanking.Girls.Sort();
            generalRanking.Boys.Sort();
            using (StreamWriter sw = new(Path.Combine(root, tournamentName, "generalRanking.json"), false, Encoding.UTF8))
            {
                sw.Write(System.Text.Json.JsonSerializer.Serialize(generalRanking, GetJsonSerializerOptions()));
            }
            SaveGeneralRanking(root, tournamentName, generalRanking.Girls, false);
            SaveGeneralRanking(root, tournamentName, generalRanking.Boys, true);
        }
        private static void SaveGeneralRanking(string root, string tournamentName, List<PlayerGeneral> playerGenerals, bool forBoys)
        {
            string suffix = (forBoys) ? "M" : "N";
            using StreamWriter sw = new(Path.Combine(root, tournamentName, $"generalRanking{suffix}.txt"), false, Encoding.UTF8);
            int rankGeneral = 1;
            foreach (PlayerGeneral player in playerGenerals)
            {
                if (string.IsNullOrWhiteSpace(player.Name))
                {
                    continue;
                }
                sw.WriteLine($"{rankGeneral}. {player.Name} ({player.Group})");
                rankGeneral++;
            }
        }
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
        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions() { WriteIndented = true };
        }
        #endregion
    }
}
