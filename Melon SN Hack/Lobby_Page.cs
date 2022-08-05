using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Melon_SN_Hack
{
    public partial class Lobby_Page : UserControl
    {
        public Lobby_Page()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "forcestart";
        }

        public void UpdateControl()
        {
            textBox1.Text = MyMod.lobbyName;
            textBox2.Text = MyMod.lobbyPass;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "lobbyname " + textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "lobbypass " + textBox2.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "addplayer";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "rename " + textBox3.Text;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            MyMod.animated = checkBox1.Checked;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "explorer";
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            MyMod.antiKick = checkBox2.Checked;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            MyMod.lastCommand = "neighborlby";
        }
    }
}
