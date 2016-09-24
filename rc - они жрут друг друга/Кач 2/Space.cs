using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceLib;
using System.Drawing;

namespace Кач_2
{
    public class Space : SpaceLib.Space
    {
        public double gapSize = 200;
        public double totalCost = 0;
        public double ticksPerSecond = 64;

        public int deathCount = 0;

        public override void paint(System.Drawing.Graphics g)
        {
            base.paint(g);
            com.print(g, Color.White, 20, 20, String.Format("Время: {0}", time));
            com.print(g, Color.White, 20, 40, String.Format("Счёт: {0}", (int)totalCost));
            com.print(g, Color.White, 20, 60, String.Format("Смертей: {0}", (int)deathCount));
        }
    }
}
