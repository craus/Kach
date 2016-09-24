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
        public int maxCd = 500;
        int cd = 0;

        public Pie timePie;

        public LevelupButtonGenerator(double x, double y, double z)
            :base(x,y,z)
        {
            //add(timePie = new Pie().color(com.dart(Color.Gray, Color.Black, 0.5)).layer(0).to<Pie>());
            //timePie.start = Math.PI * 3 / 2;
        }

        public override void tick()
        {
            cd--;
            //timePie.sweep = 2 * Math.PI * (maxCd-cd) / maxCd;
            if (cd > 0) return;
            cd = maxCd;
            space.add(new LevelupButton(com.rnd(space.gapSize, space.size.Width - space.gapSize), com.rnd(space.gapSize, space.size.Height - space.gapSize)));
        }

    }
}
