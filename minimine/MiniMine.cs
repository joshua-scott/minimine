using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace minimine
{
    public class MiniMine : BoardGame
    {
        private const int row = 6;
        public const int AmountOfButtons = row * row;
        private const int amountofMines = 8;                                    // *********Change # of mines here! 10 is hard! 5 is easy! 7-8 is juuuust riiiight*********
        private const string mine = "*";
        public const string Empty = "";
        protected new Button[] places = new Button[AmountOfButtons];            // Array of buttons
        private string[] content = new string[AmountOfButtons];                 // Array of content (i.e. mine or no)
        private int points;
        private int gamesPlayed, gamesWon;

        private MineBoard board;

        public MiniMine(MineBoard board)
        {
            this.board = (MineBoard)board;                                      // Creates instance of MineBoard called board
            board.grid.Children.CopyTo(places, 0);                              // Copy *reference* of the buttons to array of Buttons called 'places'. 
        }
        
        public override void Init()
        {
            for (int i = 0; i < AmountOfButtons; i++)                           // Clear and reset board
            {
                points = 0;
                content[i] = Empty;
                places[i].Content = Empty;
                places[i].Background = Brushes.Gray;
                places[i].IsEnabled = true;
                places[i].FontSize = 18;
            }
            gamesPlayed++;                
            PlaceMines();
        }

        public override void Play(object sender)
        {
            Button b = (Button)sender;
            int index = board.grid.Children.IndexOf(b);                         // Get index of button clicked
            b.IsEnabled = false;                                                // Disable the selected button 
            if (content[index] == mine)                                         // If it's a mine, show all mines, display score and prompt user to play again
            {
                ShowMine();
                if (MessageBox.Show("You made " + points +
                    " successful moves but then hit a mine!\nDo you want to start a new game?",
                        "You lose! " + gamesWon + "/" + gamesPlayed + " games won", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Init();
                }
                else
                    Application.Current.Shutdown();
            }
            else                                                                // If not a mine, ShowMineInfo for surrounding mines, add point, and check for winner
            {
                bool isLeft = (index % row == 0);                       // Set bools stating if index is top/bottom/left/right of board
                bool isRight = ((index + 1) % row == 0);
                bool isTop = (index >= 0 && index < row);
                bool isBottom = (index >= (AmountOfButtons - row)
                    && index < AmountOfButtons);

                if (!isTop)
                {
                    ShowMineInfo(index - row);
                    if (!isLeft)                                        // above-left
                        ShowMineInfo(index - row - 1);
                    if (!isRight)                                       // above-right
                        ShowMineInfo(index - row + 1);
                }
                if (!isBottom)
                {
                    ShowMineInfo(index + row);
                    if (!isLeft)                                        // below-left
                        ShowMineInfo(index + row - 1);
                    if (!isRight)                                       // below-right
                        ShowMineInfo(index + row + 1);
                }
                if (!isLeft)
                    ShowMineInfo(index - 1);
                if (!isRight)
                    ShowMineInfo(index + 1);

                points++;
                if (points == AmountOfButtons - amountofMines)                  // User won! Display message, prompt to play again
                {
                    gamesWon++;
                    if (MessageBox.Show("Wow, you are such a great minesweeper!\nDo you want to start a new game?",
                        "You win! " + gamesWon + "/" + gamesPlayed + " games won", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Init();
                    }
                    else
                        Application.Current.Shutdown();
                }
            }
        }

        private void PlaceMines()
        {
            Random rnd = new Random();
            for (int i = 0; i < amountofMines; i++)                     
            {
                int mineGoesHere = rnd.Next(AmountOfButtons);               // Generate new number that could be anywhere in content[]
                if (content[mineGoesHere] == mine)                              // If that's already a mine, do this iteration's loop again
                    i--;
                else                                                            // Else make it a mine
                    content[mineGoesHere] = mine;
            }
        }

        private void ShowMine()                                                 // Show all mines  
        {
            for (int i = 0; i < AmountOfButtons; i++)
            {
                if (content[i] == mine)
                {
                    places[i].IsEnabled = true;
                    places[i].Content = mine;
                    places[i].Background = Brushes.Red;
                    places[i].FontSize = 34;
                }
            }
        }

        private int MineInfo(int index)
        {
            int closeMines = 0;
            bool isLeft = (index % row == 0);                       // Set bools stating if index is top/bottom/left/right of board
            bool isRight = ((index + 1) % row == 0);
            bool isTop = (index >= 0 && index < row);
            bool isBottom = (index >= (AmountOfButtons - row) 
                && index < AmountOfButtons);

            if (content[index] == mine)                             // check itself
                closeMines++;
            if (!isTop)
            {
                if (content[index - row] == mine)                   // straight above                                           
                    closeMines++;
                if (!isLeft && content[index - row - 1] == mine)    // above-left
                    closeMines++;
                if (!isRight && content[index - row + 1] == mine)   // above-right
                    closeMines++;
            }
            if (!isBottom)
            {
                if (content[index + row] == mine)                   // straight below                                           
                    closeMines++;
                if (!isLeft && content[index + row - 1] == mine)    // below-left
                    closeMines++;
                if (!isRight && content[index + row + 1] == mine)   // below-right
                    closeMines++;
            }
            if (!isLeft)
            {
                if (content[index - 1] == mine)                     // straight left                                           
                    closeMines++;
            }
            if (!isRight)
            {
                if (content[index + 1] == mine)                     // straight right                                           
                    closeMines++;
            }
            return closeMines;
        }

        private void ShowMineInfo(int index)
        {
            if (places[index].Content.ToString() == Empty)                 // If button content is empty, calculate! If it's not empty, no point in doing it twice.
                places[index].Content = MineInfo(index);                // Show number of mines in surrounding buttons
        }

        public void TagButton(object sender)                        // If button right-click, toggle orange/yellow/grey (to allow user to tag mines)
        {                                                               // Not in class diagram but I implemented it anyway because fuck tha police
            Button b = (Button)sender;
            int index = board.grid.Children.IndexOf(b);
            if (places[index].Background == Brushes.Gray)
                places[index].Background = Brushes.Orange;
            else if (places[index].Background == Brushes.Orange)
                places[index].Background = Brushes.Yellow;
            else
                places[index].Background = Brushes.Gray;
        }
    }
}
