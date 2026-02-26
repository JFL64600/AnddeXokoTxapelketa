using AnddeXokoTxapelketa.Interfaces;
using AnddeXokoTxapelketa.Models;
using ClosedXML.Excel;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace AnddeXokoTxapelketa.Classes
{
    public class Tools
    {
        #region Declarations
        public enum GroupType
        {
            Girls,
            Boys
        }
        #endregion
        #region Methods
        public static T GetObjects<T>(string jsonFileName)
        {
            using StreamReader sr = new(jsonFileName, Encoding.UTF8);
            return System.Text.Json.JsonSerializer.Deserialize<T>(sr.ReadToEnd());
        }
        public static void SetObjects(object objects, string jsonFileName)
        {
            using StreamWriter sw = new(jsonFileName, false, Encoding.UTF8);
            sw.Write(System.Text.Json.JsonSerializer.Serialize(objects, GetJsonSerializerOptions()));
        }
        public static List<ITournament> GetTournaments(string root)
        {
            List<ITournament> results = [];
            DirectoryInfo diRoot = new(root);
            foreach (DirectoryInfo di in diRoot.GetDirectories())
            {
                string fileName = Path.Combine(root, di.Name, "tournament.json");
                if (File.Exists(fileName))
                {
                    using StreamReader sr = new(fileName, Encoding.UTF8);
                    results.Add(System.Text.Json.JsonSerializer.Deserialize<Tournament>(sr.ReadToEnd()));
                }
                else if (File.Exists(Path.Combine(root, di.Name, "girls.players.json")) || File.Exists(Path.Combine(root, di.Name, "boys.players.json")))
                {
                    Models.New.Tournament tournament = new() { Name = di.Name };
                    fileName = Path.Combine(root, di.Name, "girls.players.json");
                    if (File.Exists(fileName))
                    {
                        tournament.Girls = GetObjects<List<Models.New.Player>>(fileName);
                    }
                    foreach (FileInfo fi in new DirectoryInfo(Path.Combine(root, di.Name)).GetFiles("girls.group.*.json"))
                    {
                        tournament.Groups.Add(GetObjects<Models.New.Group>(fi.FullName));
                    }
                    results.Add(tournament);
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
        public static void GenerateRotations(string root, Tournament tournament)
        {
            List<Calendar> calendars = GetCalendars(root, tournament.Name);
            string fileName = Path.Combine(root, tournament.Name, "rotations.xlsx");
            if (!File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            using XLWorkbook wb = new();
            int groupIndex = 1;
            foreach (Group group in tournament.Girls)
            {
                if (GenerateGroupRotations(wb, group, groupIndex, GetRotations(calendars, group), false))
                {
                    groupIndex++;
                }
            }
            groupIndex = 1;
            foreach (Group group in tournament.Boys)
            {
                if (GenerateGroupRotations(wb, group, groupIndex, GetRotations(calendars, group), true))
                {
                    groupIndex++;
                }
            }
            wb.SaveAs(fileName);
        }
        private static bool GenerateGroupRotations(XLWorkbook wb, Group group, int groupIndex, List<Rotation> rotations, bool forBoys)
        {
            if (rotations is null)
            {
                return false;
            }
            string prefix = (forBoys) ? "M" : "N";
            string groupName = $"{prefix}{groupIndex}";
            IXLWorksheet ws = wb.Worksheets.Add(groupName);
            ws.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Style.Font.FontSize = 18;
            ws.Style.Font.FontName = "Aptos Narrow";
            ws.Columns("A").Width = 16;
            ws.Columns("B").Width = 5;
            ws.Columns("C").Width = 5;
            ws.Columns("D").Width = 16;
            ws.Rows().Height = 24;
            int rotationIndex = 1;
            int rowIndex = 1;
            foreach (Rotation rotation in rotations)
            {
                ws.Cell($"A{rowIndex}").Value = $"{groupName} - Rotation {rotationIndex}";
                ws.Cell($"A{rowIndex}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell($"A{rowIndex}").Style.Font.Bold = true;
                ws.Range($"A{rowIndex}:D{rowIndex}").Merge();
                rowIndex++;
                foreach (int[] match in rotation.Matches)
                {
                    ws.Cell($"A{rowIndex}").Value = group.Players[match[0] - 1].Name;
                    ws.Cell($"D{rowIndex}").Value = group.Players[match[1] - 1].Name;
                    rowIndex++;
                }
                rotationIndex++;
            }
            return true;
        }
        private static List<Rotation> GetRotations(List<Calendar> calendars, Group group)
        {
            int count = group.Players.Count(c => !string.IsNullOrWhiteSpace(c.Name));
            if ((count % 2) != 0)
            {
                count++;
            }
            int index = calendars.FindIndex(c => c.Teams == count);
            return (index >= 0) ? calendars[index].Rotations : null;
        }
        private static List<Calendar> GetCalendars(string root, string tournamentName)
        {
            using StreamReader sr = new(Path.Combine(root, tournamentName, "rotations.json"), Encoding.UTF8);
            return System.Text.Json.JsonSerializer.Deserialize<List<Calendar>>(sr.ReadToEnd());
        }
        public static Leagues GetLeagues(string root, string tournamentName)
        {
            return GetObjects<Leagues>(Path.Combine(root, tournamentName, "leagues.json"));
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
                result.Add(new Models.Group());
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
