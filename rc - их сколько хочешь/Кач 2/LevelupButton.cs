using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceLib;
using System.Drawing;

namespace Кач_2
{
    public class LevelupButton : SpaceLib.Button
    {
        public const double timeBase = 1.616;
        public const double timeMultiplyer = 100;
        public const double defaultSize = 10;
        public const double sizePartPerLevel = 0.25;

        public double timeToLevelup = timeMultiplyer;
        public double maxPenaltyTime = timeMultiplyer;
        public int level = 0;
        public double currentTime = 0;
        public double penaltyTime = 0;
        public Pie timePie;
        public Pie penaltyTimePie;
        public Text levelText;
        public double sizeBase;

        public LevelupButton(double x, double y, double z = defaultSize, params Detail[] details)
            : base(x,y,z, details)
        {
            sizeBase = z;
            z = sizeBase * (level + 1);
            Circle.last.color(com.dart(Color.Yellow, Color.White, 0.5));
            addTotal(timePie = new Pie().color(com.dart(Color.Blue, Color.White, 0.5)).layer(1).to<Pie>());
            addTotal(penaltyTimePie = new Pie().color(com.dart(Color.Red, Color.White, 0.5)).layer(1).to<Pie>());
            addTotal(levelText = new Text("0").layer(2).to<Text>()); 
        }

        void addTotal(Detail d)
        {
            add(d);
            underCursorDetails.Add(d);
        }

        public override void tick()
        {
            base.tick();
            currentTime += space.tickTime;
            if (currentTime > timeToLevelup)
            {
                penaltyTime += space.tickTime;
                currentTime = timeToLevelup;
                if (penaltyTime > maxPenaltyTime)
                {
                    die();
                }
            }
            timePie.sweep = Math.PI * 2 * currentTime / timeToLevelup;
            penaltyTimePie.sweep = Math.PI * 2 * penaltyTime / maxPenaltyTime;
        }

        private void levelup()
        {
            currentTime = 0;
            level++;
            levelText.text = level.ToString();
            maxPenaltyTime = timeToLevelup = Math.Pow(timeBase, level) * timeMultiplyer;
            z = sizeBase * (level * sizePartPerLevel + 1);
        }

        public override void mousedown()
        {
            base.mousedown();
            if (currentTime >= timeToLevelup)
            {
                levelup();
                penaltyTime = 0;
            }
        }
    }
}
