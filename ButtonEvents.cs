using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mastermind
{
    /// <summary>
    /// Interaction logic for MastermindWindow.xaml
    /// </summary>
    public partial class MastermindWindow : Window
    {

        /// <summary>
        /// Sets the event for removing a clicked button by double clicking
        /// </summary>
        private void SetEvents()
        {
            foreach (StackPanel s in new StackPanel[] { s9, s8, s7, s6, s5, s4, s3, s2, s1 })
            {
                foreach (Button btn in s.Children)
                {
                    btn.Click += UndoClick;
                }
            }
        }

        /// <summary>
        /// Evenhandler for undo a clicked button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UndoClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Background != Brushes.White)
            {
                btn.Background = Brushes.White;
                for (int i = 0; i < active.Children.Count; i++)
                {
                    Button check = (Button)active.Children[i];
                    if (check.Background == Brushes.White)
                    {
                        btnTurn = i;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Event handler for button row of buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sBaseRed_Click(object sender, RoutedEventArgs e)
        {
            SetColor(Brushes.Red);
        }

        private void sBaseGreen_Click(object sender, RoutedEventArgs e)
        {
            SetColor(Brushes.Green);
        }

        private void sBaseYellow_Click(object sender, RoutedEventArgs e)
        {
            SetColor(Brushes.Yellow);
        }

        private void sBaseBlue_Click(object sender, RoutedEventArgs e)
        {
            SetColor(Brushes.Blue);
        }

        private void sBasePink_Click(object sender, RoutedEventArgs e)
        {
            SetColor(Brushes.Pink);
        }

        private void sBasePurple_Click(object sender, RoutedEventArgs e)
        {
            SetColor(Brushes.Purple);
        }

        private void sBaseOrange_Click(object sender, RoutedEventArgs e)
        {
            SetColor(Brushes.Orange);
        }
    }
}