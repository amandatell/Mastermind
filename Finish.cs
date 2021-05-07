using System;

namespace Mastermind
{
    /// <summary>
    /// EventArgs class Finish
    /// </summary>
    public class Finish : EventArgs
    {
        private bool win;
        private DateTime date;

        public Finish(bool win)
        {
            this.win = win;
            date = DateTime.Now;
        }

        public bool Win { get => win; set => win = value; }
        public DateTime Date { get => date; set => date = value; }
    }
}
