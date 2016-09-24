using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceLib;
using System.Drawing;

namespace Кач_2
{
    public class LevelupButtonGenerator : Unit
    {
        public const double xDelta = 100;
        public double x = 100;

        public int maxCd = 500;
        int cd = 0;
        public override void tick()
        {
            cd--;
            if (cd > 0) return;
            cd = maxCd;
            space.add(new LevelupButton(com.rnd(space.size.Width), com.rnd(space.size.Height), details: new Circle()).underCursor(new Circle().color(Color.Green)));
        }
    }
}
