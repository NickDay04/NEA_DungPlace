using System.Collections;
using System.Collections.Generic;

namespace BackendDungeonGen
{
    class Room
    {
        public double sigLevel;
        //public int noofDoors;
        public string tag;

        public Room(double sigLevel, string tag)
        {
            this.sigLevel = sigLevel;
            this.tag = tag;
        }
    }
}