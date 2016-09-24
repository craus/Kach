using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Кач_2
{
    [Serializable]
    public class Unit : SpaceLib.Unit
    {
        public new Space space { get { return base.space as Space; } }

        public Unit(double x = 0, double y = 0, double z = 1)
            : base(x, y, z)
        { }
    }
}
