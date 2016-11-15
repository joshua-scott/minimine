using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace minimine
{
    /// <summary>
    /// Interaction logic for MineBoard.xaml
    /// </summary>
    public partial class MineBoard : UserControl
    {
        private MiniMine game;

        public MineBoard()
        {
            InitializeComponent();
            game = new MiniMine(this);                  // Create new instance of MiniMine called 'game'
            game.Init();                                // Set-up the game!
        }

        public void Init()
        {
            
        }

        private void buttonClick(object sender, RoutedEventArgs e)                          // Event handler for standard click
        {
            game.Play(sender);
        }

        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)     // Event handler for right-click
        {
            game.TagButton(sender);
        }
    }
}
