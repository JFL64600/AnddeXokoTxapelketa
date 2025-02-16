using AnddeXokoTxapelketa.Models;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Text.Json;

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
        }
        public static Tournament CloneTournament(Tournament tournament)
        {
            var serialized = JsonConvert.SerializeObject(tournament);
            return JsonConvert.DeserializeObject<Tournament>(serialized);

        }
        public static List<PlayerGeneral> GetGeneralRanking(string root, string tournamentName)
        {
            using StreamReader sr = new(Path.Combine(root, tournamentName, "generalRankingM.json"), Encoding.UTF8);
            return System.Text.Json.JsonSerializer.Deserialize<List<PlayerGeneral>>(sr.ReadToEnd());
        }

        public static void SaveGeneralRanking(string root, string tournamentName, List<PlayerGeneral> players)
        {
            using (StreamWriter sw = new(Path.Combine(root, tournamentName, "generalRankingM.json"), false, Encoding.UTF8))
            {
                sw.Write(System.Text.Json.JsonSerializer.Serialize(players, GetJsonSerializerOptions()));
            }
            using (StreamWriter sw = new(Path.Combine(root, tournamentName, "generalRankingM.txt"), false, Encoding.UTF8))
            {
                int rankGeneral = 1;
                foreach (PlayerGeneral player in players)
                {
                    if (string.IsNullOrWhiteSpace(player.Name))
                    {
                        continue;
                    }
                    sw.WriteLine($"{rankGeneral}. {player.Name} ({player.Group})");
                    rankGeneral++;
                }
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
