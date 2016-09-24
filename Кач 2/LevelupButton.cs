using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceLib;
using System.Drawing;
using System.Windows.Forms;

namespace Кач_2
{
    [Serializable]
    public class LevelupButton : SpaceLib.Button
    {
        public new Space space { get { return base.space as Space; } }

        public static bool safetyNet = false;

        public const double startTimeToLevelup = 10;
        public const double startPenaltyTime = 100;
        public const double defaultSize = 10;
        public const double sizePartPerLevel = 0.25;
        public const int sacrifitionLevelDelta = 3;
        public const double timeTextRadius = 1;
        public const double minDistanceToOthers = 1000;
        public const double randomInTimings = 1;

        //public const double timeBase = 1.4656+0.15;
        //public const double penaltyTimeBase = timeBase * 0.8;

        public double timeToLevelup;
        public double maxPenaltyTime;
        public int level = 0;
        public double currentTime = 0;
        public double penaltyTime = 0;
        public Pie timePie;
        public Pie penaltyTimePie;
        public Text levelText;
        public Circle circle;
        public Circle underCursorCircle;
        public double sizeBase;

        public int race;

        public Color[] raceColors = new Color[] { Color.Blue };

        public LevelupButton(double x, double y, double z = defaultSize)
            :base(x,y,z)
        {
            onClick = () =>
            {
                if (currentTime >= timeToLevelup)
                {
                    levelup();
                }
            };

            sizeBase = z;
            z = sizeBase * (level + 1);
            basic.add(circle = new Circle().color(com.dart(Color.Yellow, Color.White, 0.5)) as Circle);
            underCursor.add(underCursorCircle = new Circle().color(Color.Green) as Circle);

            race = com.rand.Next(raceColors.Length);
            add(timePie = new Pie().color(com.dart(raceColors[race], Color.White, 0.5)).layer(1).to<Pie>());
            add(penaltyTimePie = new Pie().color(com.dart(Color.Red, Color.White, 0.5)).layer(1).to<Pie>());
            add(levelText = new Text("0").layer(2).to<Text>());
            timeToLevelup = startTimeToLevelup;
            maxPenaltyTime = startPenaltyTime;
        }

        public override void paint(Graphics g, int layer)
        {
            z = findZ();

            base.paint(g, layer);
        }

        private void moveAwayFromOthers()
        {
            double preferredDistance =  LevelupButton.minDistanceToOthers;
            while (true)
            {
                double minDistanceToOthers = double.PositiveInfinity;
                foreach (LevelupButton b in space.units.OfType<LevelupButton>())
                    if (b != this)
                        minDistanceToOthers = Math.Min(minDistanceToOthers, com.dist(x, y, b.x, b.y));
                if (minDistanceToOthers < preferredDistance)
                {
                    x = com.rnd(space.gapSize, space.size.Width - space.gapSize);
                    y = com.rnd(space.gapSize, space.size.Height - space.gapSize);
                    preferredDistance *= 0.9;
                }
                else
                {
                    break;
                }
            }
        }

        public override void firstTick()
        {
            moveAwayFromOthers();
        }

        public override void tick()
        {
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
                    if (safetyNet) levelup();
                    if (penaltyTime > maxPenaltyTime)
                    {
                        explode(100 * (1 + level), 1 + level / 3, 1 + level / 3);
                        space.deadMass += mass(level);
                        space.deathCount++;
                    }
                }
                penaltyTimePie.sweep = Math.PI * 2 * penaltyTime / maxPenaltyTime;
            }
            else
            {
                time = ((int)((timeToLevelup - currentTime) / space.ticksPerSecond) + 1);
                penaltyTimePie.sweep = 0;
            }
            timePie.sweep = Math.PI * 2 * currentTime / timeToLevelup;
            
            levelText.c = canLevelup() ? Color.Black : Color.Red;
            if (space.bot && currentTime == timeToLevelup) mousedown(MouseButtons.Left);
        }

        int mass(int level)
        {
            if (level <= sacrifitionLevelDelta) return 1;
            return mass(level - 1) + mass(level - 1 - sacrifitionLevelDelta);
        }

        LevelupButton sacrifition()
        {
            return space.units.OfType<LevelupButton>().Where(b => (b.level == this.level - sacrifitionLevelDelta && !b.dead)).FirstOrDefault();
        }

        bool canLevelup()
        {
            return level < sacrifitionLevelDelta || sacrifition() != null;
        }

        private void levelup()
        {
            if (level >= sacrifitionLevelDelta)
            {
                var sac = sacrifition();
                if (sac == null) return;
                if (sac == space.oldest) space.oldestBonus();
                sac.die();
            }
            space.totalCost += timeToLevelup;
            currentTime = 0;
            int levelupCount = 1;

            for (int i = 0; i < levelupCount; i++)
            {
                level++;
                maxPenaltyTime = timeToLevelup = timeToLevelup + penaltyTime;
            }

            penaltyTime = 0;
            levelText.text = level.ToString();
            z = findZ();
        }

        private double findZ()
        {
            return sizeBase * (level * sizePartPerLevel + 1) * (space.oldest == this ? 1.6 : 1);
        }
    }
}
