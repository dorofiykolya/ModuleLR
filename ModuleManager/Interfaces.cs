using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class Point
    {
        public int X;
        public int Y;

        public static bool operator ==(Point p1, Point p2)
        {
            if (ReferenceEquals(p1, p2)) return true;
            if (ReferenceEquals(p1, null)) return false;
            if (ReferenceEquals(p2, null)) return false;
            return p1.Equals(p2);
        }

        public static bool operator !=(Point p1, Point p2)
        {
            if (ReferenceEquals(p1, p2)) return false;
            if (ReferenceEquals(p1, null)) return true;
            if (ReferenceEquals(p2, null)) return true;
            return !p1.Equals(p2);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Point;
            if (other != null)
            {
                return other.X == X && other.Y == Y;
            }
            return base.Equals(obj);
        }

        protected bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }

    public class TeleportRecord
    {
        public bool IsTwoWay;
        public Point From;
        public Point To;
    }

    public enum AiType
    {

    }

    public class GuardRecord
    {
        public int Id;
        public Point Spawn;
        public AiType AiType;
    }

    public enum CellType
    {
        Empty,
        Block,
        Solid,
        Ladder,
        RopeBar,
        Trap,
        HLadr,
        Teleport
    }

    public enum CharacterAction
    {
        Unknown = -1,
        Stop = 0,
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4,
        Fall = 5,
        FallBar = 6,
        DigLeft = 7,
        DigRight = 8,
        Digging = 9,
        InHole = 10,
        ClimpOut = 11,
        Reborn = 12
    }

    public class CellRecord
    {
        public Point Point;
        public CellType CellType;
    }

    public class GameLevelRecord
    {
        public int Time; // Длительность уровня 
        public int GuardRespawnTime;
        public int CellRespawnTime;
        public Point Size;
        public CellRecord[] Cells; // Непустые клеточки
        public GuardRecord[] Guards; // Описание охранников
        public TeleportRecord[] Teleports;
        public Point[] GoldGhests;
        public Point[] GuardRespawn;
        public Point RunnerSpawn;
    }


    /*

        Gold = 0x07,
        Guard = 0x08,
        Runner = 0x09,
        Reborn = 0x10
        */

}
