using System.ComponentModel;

namespace AnddeXokoTxapelketa.Models.New
{
    public class SortablePlayer: Player, IComparable
    {
        #region Properties
        public int Victories { get; set; } = 0;
        public int Points { get; set; } = 0;
        public int ConcededPoints{ get; set; } = 0;
        #endregion
        #region Methods
        public int CompareTo(object? obj)
        {

            //int totalMy = Results.Sum(x => x);
            //int victoriesMy = (from int result in Results
            //                   where result == 3
            //                   select result).Count();
            //int pointsMy = (from int result in Results
            //                where result != 3
            //                select result).Sum();
            //Player player = (Player)obj;
            //int totalAgainst = player.Results.Sum(x => x);
            //int victoriesAgainst = (from int result in player.Results
            //                        where result == 3
            //                        select result).Count();
            //int pointsAgainst = (from int result in player.Results
            //                     where result != 3
            //                     select result).Sum();
            SortablePlayer opponent = (SortablePlayer)obj;
            if (Victories > opponent.Victories)
            {
                return -1;
            }
            if (Victories < opponent.Victories)
            {
                return 1;
            }
            if (Points > opponent.Points)
            {
                return -1;
            }
            if (Points < opponent.Points)
            {
                return 1;
            }
            if (ConcededPoints > opponent.ConcededPoints)
            {
                return 1;
            }
            if (ConcededPoints < opponent.ConcededPoints)
            {
                return -1;
            }
            return string.Compare(Name, opponent.Name);
        }
        #endregion
    }
}
