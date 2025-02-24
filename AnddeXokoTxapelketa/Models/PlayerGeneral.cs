namespace AnddeXokoTxapelketa.Models
{
    public class PlayerGeneral : Player, IComparable
    {
        public int Position { get; set; } = 0;
        public string Group { get; set; } = string.Empty;
        public int CompareTo(object? obj)
        {
            int totalMy = Results.Sum(x => x);
            int victoriesMy = (from int result in Results
                               where result == 3
                               select result).Count();
            int pointsMy = (from int result in Results
                            where result != 3
                            select result).Sum();
            PlayerGeneral player = (PlayerGeneral)obj;
            int totalAgainst = player.Results.Sum(x => x);
            int victoriesAgainst = (from int result in player.Results
                                    where result == 3
                                    select result).Count();
            int pointsAgainst = (from int result in player.Results
                                 where result != 3
                                 select result).Sum();
            if (Position < player.Position)
            {
                return -1;
            }
            if (Position > player.Position)
            {
                return 1;
            }
            if (totalMy > totalAgainst)
            {
                return -1;
            }
            if (totalMy < totalAgainst)
            {
                return 1;
            }
            if (victoriesMy > victoriesAgainst)
            {
                return -1;
            }
            if (victoriesMy < victoriesAgainst)
            {
                return 1;
            }
            if (pointsMy > pointsAgainst)
            {
                return -1;
            }
            if (pointsMy < pointsAgainst)
            {
                return 1;
            }
            return string.Compare(Name, player.Name);
        }
    }
}