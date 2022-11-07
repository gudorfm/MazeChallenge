using System;
using System.Collections.Generic;
using System.Text;

namespace MazeConsoleGame
{
    class MazeData
    {
        public int id { get; set; }
        public int size { get; set; }
        public int coins { get; set; }
        public int bumps { get; set; }
        public int secondsLeft { get; set; }

        public MazeData()
        {

        }

        public override string ToString()
        {
            return ("Id: " + this.id + "\nsize:" + this.size + "\nCoins gathered: " + this.coins + "\nWall bumps: " + this.bumps + "\nTime left: " + secondsLeft + "\n");
        }
    }

    
}
