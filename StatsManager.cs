using System;
using System.Linq;

namespace Mastermind
{
    /// <summary>
    /// Class that communicates with db
    /// </summary>
    public class StatsManager
    {
        /// <summary>
        /// Instance of db enitity
        /// </summary>
        StatsEntities db;
        public StatsManager()
        {
            db = new StatsEntities();
        }

        /// <summary>
        /// Eventhandler that adds game to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnFinish(object sender, Finish e)
        {
            var query = from s in db.Stats
                     select s;
            int id = query.Count();
            Stat newStat = new Stat()
            {
                Id = id,
                Win = e.Win,
                Date = e.Date
            };
            db.Stats.Add(newStat);
            db.SaveChanges();
        }

        /// <summary>
        /// Gets stats on wins
        /// </summary>
        /// <returns></returns>
        public IQueryable<bool> GetWins()
        {
            var stats = from s in db.Stats
                        select s.Win;

            return stats;
                
        }

        /// <summary>
        /// Gets the last game played
        /// </summary>
        /// <returns></returns>
        public string GetLastPlayed()
        {
            var stats = from s in db.Stats
                        select s.Date;
            var last = stats.OrderByDescending(x => x).FirstOrDefault().ToShortDateString();
            return last;

        }

    }
}
