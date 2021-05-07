using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mastermind
{
    /// <summary>
    /// Interaction logic for Mastermind.xaml
    /// </summary>
    public partial class MastermindWindow : Window
    {
        // Stackpanels with big buttons 
        Queue<StackPanel> stack;

        // Stackpanel with small buttons
        Queue<StackPanel> stackSmall;

        // The current active stackpanel
        StackPanel active;

        // The current button
        int btnTurn;

        WinChecker checker;
        StatsManager stats;

        // Event to save data in db
        public event EventHandler<Finish> Finish;

        public MastermindWindow()
        {
            stats = new StatsManager();
            InitializeComponent();
            SetEvents();
            foreach (Button btn in sBase.Children)
            {
                btn.IsEnabled = false;
            }
            Finish += stats.OnFinish;

            // Gets the wins and losses from db and last date played
            IQueryable<bool> wins = stats.GetWins();
            string lastGame = stats.GetLastPlayed();
            txtLastGame.Text = lastGame;
            ShowWinAndLosses(wins);
        }

        /// <summary>
        /// Initalizes GUI
        /// </summary>
        private void InitializeGUI()
        {
            string lastGame = stats.GetLastPlayed();
            txtLastGame.Text = lastGame;
            btnTurn = 0;
            active = null;
            stackSmall = new Queue<StackPanel>();
            stack = new Queue<StackPanel>();
            // Create a queue for the stackpanels and disenables them
            foreach (StackPanel s in new StackPanel[] { t9, t8, t7, t6, t5, t4, t3, t2 })
            {
                stackSmall.Enqueue(s);
                foreach (Button btn in s.Children)
                {
                    btn.ClearValue(BackgroundProperty);
                }
            }
            foreach (StackPanel s in new StackPanel[] { s9, s8, s7, s6, s5, s4, s3, s2, s1 })
            {
                stack.Enqueue(s);
                foreach (Button btn in s.Children)
                {
                    btn.ClearValue(BackgroundProperty);
                    btn.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// Shows previous wins and losses
        /// </summary>
        /// <param name="stats"></param>
        public void ShowWinAndLosses(IQueryable<bool> stats)
        {
            int wins = 0;
            int losses = 0;
            foreach (bool item in stats)
            {
                if (item)
                    wins++;
                else
                    losses++;
            }

            txtWins.Text = wins.ToString();
            txtLosses.Text = losses.ToString();
        }

        /// <summary>
        /// Starts a new game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartNew_Click(object sender, RoutedEventArgs e)
        {
            InitializeGUI();
            checker = new WinChecker();
            checker.StartGame();
            DisableButtons();
            foreach (Button btn in sBase.Children)
            {
                btn.IsEnabled = true;
            }
        }

        /// <summary>
        /// Disables all the buttons but the current row
        /// </summary>
        private void DisableButtons()
        {
            if (active != null) stack.Enqueue(active);
            active = stack.Dequeue();
            if (active == s1)
            {
                // If it is the last row
                ShowWinningColors();
                foreach (Button btn in s2.Children)
                {
                    btn.IsEnabled = false;
                }
            }
            else
            {
                foreach (StackPanel s in stack)
                {
                    foreach (Button btn in s.Children)
                    {
                        btn.IsEnabled = false;
                    }
                }
                // Make the new row of buttons white
                foreach (Button btn in active.Children)
                {
                    btn.Background = Brushes.White;
                    btn.IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// Sets the color of the active button
        /// When the whole row is full next row gets enabled
        /// </summary>
        /// <param name="color"></param>
        private void SetColor(SolidColorBrush color)
        {
            Button btn = (Button)active.Children[btnTurn];
            do
            {
                if (btn.Background != Brushes.White)
                {
                    btnTurn++;
                    btn = (Button)active.Children[btnTurn];
                }
            }
            while (btn.Background != Brushes.White);
            btn.Background = color;
            btnTurn++;
            if (btnTurn == active.Children.Count)
            {
                List<Brush> row = GetRowColors();
                List<SolidColorBrush> rightColors = checker.CheckRow(row);
                UpdateSmallButtons(rightColors);
                if (checker.CheckWin(row))
                {
                    bool win = true;
                    ShowWinningColors(win);
                }  
                else
                {
                    DisableButtons();
                    btnTurn = 0;
                }
            }
        }

        /// <summary>
        /// Gets the colors of the row
        /// </summary>
        /// <returns></returns>
        private List<Brush> GetRowColors()
        {
            List<Brush> row = new List<Brush>();
            foreach (Button btn in active.Children)
            {
                row.Add(btn.Background);
            }
            return row;
        }

        /// <summary>
        /// Updates the small buttons with black and white colors
        /// </summary>
        /// <param name="rightColors"></param>
        private void UpdateSmallButtons(List<SolidColorBrush> rightColors)
        {
            StackPanel active = stackSmall.Dequeue();
            for (int i = 0; i < active.Children.Count; i++)
            {
                Button btn = (Button)active.Children[i];
                try
                {
                    btn.Background = rightColors[i];
                }
                catch
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Shows the winning colors
        /// And event OnFinish
        /// </summary>
        /// <param name="win"></param>
        private void ShowWinningColors(bool win = false)
        {
            List<SolidColorBrush> colors = checker.WinningColors;
            for (int i = 0; i < s1.Children.Count; i++)
            {
                Button btn = (Button)s1.Children[i];
                btn.Background = colors[i];

            }
            foreach (Button btn in sBase.Children)
            {
                btn.IsEnabled = false;
            }
            Finish finish = new Finish(win);
            OnFinish(finish);
            IQueryable<bool> wins = stats.GetWins();
            ShowWinAndLosses(wins);
            string lastGame = stats.GetLastPlayed();
            txtLastGame.Text = lastGame;

        }

        /// <summary>
        /// OnFinish event; triggers when a game is finished
        /// </summary>
        /// <param name="e"></param>
        public void OnFinish(Finish e)
        {
            if (Finish != null)
                Finish(this, e);
        }
    }
}