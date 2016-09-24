using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceLib;
using System.Drawing;
using System.Windows.Forms;

namespace Кач_2
{
    public class LevelupButton : SpaceLib.Button
    {
        public new Space space { get { return base.space as Space; } }

        public const double timeBase = 1.616;
        public const double timeMultiplyer = 100;
        public const double defaultSize = 10;
        public const double sizePartPerLevel = 0.25;
        public const int sacrifitionLevelDelta = 3;
        public const double timeTextRadius = 1;
        public const double minDistanceToOthers = 100;

        public double timeToLevelup = timeMultiplyer;
        public double maxPenaltyTime = timeMultiplyer;
        public int level = 0;
        public double currentTime = 0;
        public double penaltyTime = 0;
        public Pie timePie;
        public Pie penaltyTimePie;
        public Text levelText;
        public Circle circle;
        public Circle underCursorCircle;
        public double sizeBase;
        public Text timeText;

        public LevelupButton(double x, double y, double z = defaultSize)
            : base(x,y,z, details: new Circle())
        {
            sizeBase = z;
            z = sizeBase * (level + 1);
            (circle = Circle.last).color(com.dart(Color.Yellow, Color.White, 0.5));
            underCursor(underCursorCircle = new Circle().color(Color.Green).to<Circle>());
            addTotal(timePie = new Pie().color(com.dart(Color.Blue, Color.White, 0.5)).layer(1).to<Pie>());
            addTotal(penaltyTimePie = new Pie().color(com.dart(Color.Red, Color.White, 0.5)).layer(1).to<Pie>());
            addTotal(levelText = new Text("0").layer(2).to<Text>());
            underCursor(timeText = new Text("0").layer(2).to<Text>());
            timeText.size = 0.5;
            timeText.c = Color.Transparent;
        }

        private void moveAwayFromOthers()
        {
            while (true)
            {
                double minDistanceToOthers = double.PositiveInfinity;
                foreach (LevelupButton b in space.units.OfType<LevelupButton>())
                    if (b != this)
                        minDistanceToOthers = Math.Min(minDistanceToOthers, com.dist(x, y, b.x, b.y));
                if (minDistanceToOthers < LevelupButton.minDistanceToOthers)
                {
                    x = com.rnd(space.gapSize, space.size.Width - space.gapSize);
                    y = com.rnd(space.gapSize, space.size.Height - space.gapSize);
                }
                else
                {
                    break;
                }
            }
        }

        void addTotal(Detail d)
        {
            add(d);
            underCursorDetails.Add(d);
        }

        bool firstTick = true;
        public override void tick()
        {
            if (firstTick)
            {
                moveAwayFromOthers();
                firstTick = false;
            }
            base.tick();
            currentTime += space.tickTime;
            currentTime = Math.Min(currentTime, timeToLevelup);
            int time;
            if (currentTime == timeToLevelup)
            {
                penaltyTime += space.tickTime;
                time = ((int)((maxPenaltyTime - penaltyTime) / space.ticksPerSecond) + 1);
                if (penaltyTime > maxPenaltyTime)
                {
                    die();
                    space.deathCount++;
                }
            }
            else
            {
                time = ((int)((timeToLevelup - currentTime) / space.ticksPerSecond) + 1);
            }
            double angle = Math.PI / 2;
            timeText.x = timeTextRadius * Math.Cos(angle);
            timeText.y = timeTextRadius * Math.Sin(angle);
            timePie.sweep = Math.PI * 2 * currentTime / timeToLevelup;
            penaltyTimePie.sweep = Math.PI * 2 * penaltyTime / maxPenaltyTime;
            levelText.c = canLevelup() ? Color.Black : Color.Red;
            timeText.text = time.ToString();
            //timeText.c = time < 5 ? Color.Red : Color.White;
        }

        bool canLevelup()
        {
            return level < sacrifitionLevelDelta || space.units.OfType<LevelupButton>().Where(b => (b.level == this.level - sacrifitionLevelDelta)).FirstOrDefault() != null;
        }

        private void levelup()
        {
            if (level >= sacrifitionLevelDelta)
            {
                LevelupButton sacrifition = space.units.OfType<LevelupButton>().Where(b => (b.level == this.level - sacrifitionLevelDelta)).FirstOrDefault();
                if (sacrifition == null) return;
                sacrifition.die();
            }
            space.totalCost += timeToLevelup;
            currentTime = 0;
            level++;
            penaltyTime = 0; 
            levelText.text = level.ToString();
            maxPenaltyTime = timeToLevelup = Math.Pow(timeBase, level) * timeMultiplyer;
            z = sizeBase * (level * sizePartPerLevel + 1);
        }

        public override void mousedown(MouseButtons button)
        {
            base.mousedown(button);
            if (currentTime >= timeToLevelup)
            {
                levelup();
            }
        }
    }
}
