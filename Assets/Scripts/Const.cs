using UnityEngine;

namespace Const
{

    public static class CONST
    {
        public const int MAPSIZE = 8;

        public const int MOVEDISTANCE = 1;

    }
    
    public enum directions
    {
        left = -1, right = 1, up = -1, down = 1
    }
    public enum symbolDirections
    {
        left = '<', right = '>', up = '^', down = 'v'
    }

    public enum directionAngle
    {
        left = 90, right = -90, up = 0, down = 180
    }
}
