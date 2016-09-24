using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpaceLib;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Drawing.Drawing2D;

namespace Кач_2
{
    public partial class Form1 : Form
    {
        public const string saveFileName = "save.dat";

        public List<Space> games = new List<Space>();

        public Space space;

        public Form1()
        {
            InitializeComponent();
            com.form = this;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Application.Exit();
            if (e.KeyCode == Keys.P || e.KeyCode == Keys.Space || e.KeyCode == Keys.Pause) space.playPause();
            if (e.KeyCode == Keys.N || e.KeyCode == Keys.F2) newGame();
            if (e.KeyCode == Keys.S) save();
            if (e.KeyCode == Keys.Delete) deleteGame();
            if (e.KeyCode == Keys.Right) moveGame(1);
            if (e.KeyCode == Keys.Left) moveGame(-1);
            if (e.KeyCode == Keys.B) space.bot = !space.bot;
        }

        private void deleteGame()
        {
            int x = games.IndexOf(space);
            games.Remove(space);

            if (games.Count == 0)
            {
                newGame();
            }
            else
            {
                space = games[(x + games.Count) % games.Count];
            }
        }

        private void moveGame(int p)
        {
            space = games[(games.IndexOf(space) + p + games.Count) % games.Count];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            load();
            if (games.Count == 0) newGame();
        }

        private void newGame()
        {
            space = new Space();
            space.size = this.ClientSize;
            space.add(new LevelupButtonGenerator());
            games.Add(space);
        }

        private void load()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = null;
            try
            {
                fs = new FileStream(saveFileName, FileMode.Open);
                games = bf.Deserialize(fs) as List<Space>;
                foreach (Space s in games) s.adapt();
                int cur = (int)bf.Deserialize(fs);
                space = games[cur];
            }
            catch (Exception e)
            {
                com.message(e);
                games = new List<Space>();
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (space != null)
            {
                space.paint(e.Graphics);
            }
            //com.print(e.Graphics, Color.White, 0, 0, com.messages, 10);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (space != null)
            {
                space.tick();
                Refresh();
            }
        }

        private void save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = null;
            try
            {
                fs = new FileStream(saveFileName, FileMode.OpenOrCreate);
                bf.Serialize(fs, games);
                bf.Serialize(fs, games.IndexOf(space));
            }
            catch (Exception e)
            {
                com.message(e);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            space.mousedown(e.Button);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            save();
        }
    }
}
