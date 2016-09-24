using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceLib;
using System.Drawing;

namespace Кач_2
{
    [Serializable]
    public class Space : SpaceLib.Space
    {
        public double gapSize = 100;
        public double totalCost = 0;

        public int deathCount = 0;

        public LevelupButton oldest = null;

        public int bornMass = 0;
        public int deadMass = 0;
        public const int bornReputation = 1;
        public const int deadReputation = 10;
        public int reputation { get { return bornMass * bornReputation - deadMass * deadReputation; } }

        public bool bot = false;

        public Space()
        {
            speed = 10;
            ticksPerSecond = 100;
        }

        public override void paint(System.Drawing.Graphics g)
        {
            base.paint(g);
            com.print(g, com.transparent(Color.White,0.95), 20, 20, String.Format("Время: {0}", (int)time));
            com.print(g, com.transparent(Color.White, 0.95), 20, 40, String.Format("Счёт: {0}", (int)totalCost));
            com.print(g, com.transparent(Color.White, 0.95), 20, 60, String.Format("Смертей: {0}", (int)deathCount));
            //com.print(g, com.transparent(Color.White, 0.95), 20, 80, String.Format("Репутация: {0}", (int)reputation));
            com.print(g, com.transparent(Color.White, 0.95), 20, 100, String.Format("Родилось: {0}", (int)bornMass));
            com.print(g, com.transparent(Color.White, 0.95), 20, 120, String.Format("Умерло: {0}", (int)deadMass));
            if (!play) com.print(g, com.transparent(Color.Silver), size.Width / 2, size.Height / 2, "Пауза", 400, StringAlignment.Center, StringAlignment.Center); 
        }

        internal void adapt()
        {
            speed = 1;
        }

        public override void tick()
        {
            base.tick();
            if (oldest == null || oldest.dead)
            {
                //oldest = units.OfType<LevelupButton>().OrderBy(l => l.level).LastOrDefault(); 
            }
        }

        internal void oldestBonus()
        {
            //reputation += 30;
        }
    }
}
