using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Кач_2
{
    public class Unit : SpaceLib.Unit
    {
        public new Space space { get { return base.space as Space; } }

        public Unit(double x, double y, double z)
            : base(x, y, z)
        { }
    }
}
