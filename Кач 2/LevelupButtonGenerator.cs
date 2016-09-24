using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceLib;
using System.Drawing;

namespace Кач_2
{
    [Serializable]
    public class LevelupButtonGenerator : Unit
    {
        public double minCD, maxCD;
        double currentCD = 0;
        
        public static LevelupButtonGenerator last;

        public LevelupButtonGenerator(double CD = 400, double currentCD = 0)
        {
            last = this;
            minCD = maxCD = CD;
            this.currentCD = currentCD;
        }

        public override void tick()
        {
            //currentCD -= space.tickTime;
            //if (currentCD > 0) return;
            //currentCD = com.rnd(minCD, maxCD);
            if (!space.units.OfType<LevelupButton>().Any(l => l.level == 0))
            {
                space.add(new LevelupButton(com.rnd(space.gapSize, space.size.Width - space.gapSize), com.rnd(space.gapSize, space.size.Height - space.gapSize)));
                space.bornMass += 1;
            }
        }

    }
}
