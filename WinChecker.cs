using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Mastermind
{
    /// <summary>
    /// Holds the winning colors and checks if there is a win
    /// </summary>
    class WinChecker
    {
        List<SolidColorBrush> winningColors;

        public List<SolidColorBrush> WinningColors { get => winningColors; }

        /// <summary>
        /// Starts a new game with random winning colors
        /// </summary>
        public void StartGame()
        {
            var ran = new Random();
            winningColors = new List<SolidColorBrush>();
            List<SolidColorBrush> allColors = new List<SolidColorBrush> { Brushes.Red, Brushes.Green, Brushes.Yellow, Brushes.Blue, Brushes.Orange, Brushes.Pink, Brushes.Purple };
            for (int i = 0; i < 5; i++)
            {
                int index = ran.Next(allColors.Count);
                if (winningColors.Contains(allColors[index]))
                    i--;
                else winningColors.Add(allColors[index]);
            }
        }

        /// <summary>
        /// Checks the current row against winning row
        /// Creates two lists with black and white
        /// Black if right color and right place
        /// White is only right color
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public List<SolidColorBrush> CheckRow(List<Brush> row)
        {
            row = row.Distinct().ToList();
            List<SolidColorBrush> black = new List<SolidColorBrush>();
            List<SolidColorBrush> white = new List<SolidColorBrush>();
            for (int i = 0; i < row.Count; i++)
            {
                if (row[i] == winningColors[i])
                {
                    black.Add(Brushes.Black);
                }
                if (winningColors.Contains((SolidColorBrush)row[i]) && (row[i] != winningColors[i]))
                {
                    white.Add(Brushes.White);
                }
            }
            return SortColors(black, white);
        }   

        /// <summary>
        /// Checks if there is a win
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool CheckWin(List<Brush> row)
        {
            for (int i = 0; i < row.Count; i++)
            {
                if (!(row[i] == winningColors[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns a list where blacks are first then the whites
        /// </summary>
        /// <param name="black"></param>
        /// <param name="white"></param>
        /// <returns></returns>
        private List<SolidColorBrush> SortColors(List<SolidColorBrush> black, List<SolidColorBrush> white)
        {
            List<SolidColorBrush> finalList = new List<SolidColorBrush>();
            for (int i = 0; i < black.Count; i++)
            {
                finalList.Add(black[i]);
            }
            for (int i = 0; i < white.Count; i++)
            {
                finalList.Add(white[i]);
            }
            return finalList;
        }
    }
}
