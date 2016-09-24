using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceLib;
using System.Drawing;

namespace Кач_2
{
    [Serializable]
    public class FloatLabel : Label
    {
        public double lifetime;

        public FloatLabel(string text, double x, double y, double r, double lifetime)
            : base(text, x, y, r)
        {
            this.lifetime = lifetime;
            sf.Alignment = sf.LineAlignment = StringAlignment.Center;
        }

        public override void tick()
        {
            base.tick();
            if (age > lifetime) die();
        }

        public override void paintInternal(Graphics g)
        {
            if (textFunction != null) text = textFunction.Invoke(this);
            g.DrawString(text, new Font("Times New Roman", 1f), new SolidBrush(com.transparent(c, age/lifetime)), 0, 0, sf);
        }
    }
}
