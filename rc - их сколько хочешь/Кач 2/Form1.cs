using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpaceLib;

namespace Кач_2
{
    public partial class Form1 : Form
    {
        public Space space = new Space().layers(2);

        public Form1()
        {
            InitializeComponent();
            com.form = this;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            space.size = this.ClientSize;
            space.add(new LevelupButtonGenerator());
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            space.paint(e.Graphics);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            space.tick();
            Refresh();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            space.mousedown();
        }
    }
}
