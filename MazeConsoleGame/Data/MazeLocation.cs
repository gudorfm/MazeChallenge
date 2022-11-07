using System;
using System.Collections.Generic;
using System.Text;

namespace MazeConsoleGame
{
    class MazeLocation
    {
        public string East { get; set; }
        public string South { get; set; }
        public string North { get; set; }
        public string West { get; set; }
        public string On { get; set; }
        

        public MazeLocation()
        {

        }

        public override string ToString()
        {
            return "North: " + this.North + "\nEast: " + this.East + "\nSouth: " + this.South + "\nWest: " + this.West + "\nOn: " + this.On;
        }
    }
}
