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
    public partial class Game_Page : UserControl
    {
        public Game_Page()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "spawn " + numericUpDown1.Value.ToString() + " " + textBox1.Text;
            textBox1.Text = string.Empty;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "tpto";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "tptome";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "ghost";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "kill";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "drop";
        }

        private void Game_Page_Load(object sender, EventArgs e)
        {
            comboBox2.Items.Add("Neighbor wins");
            comboBox2.Items.Add("Kids win");
            comboBox2.Items.Add("Time up");
            comboBox2.Items.Add("?");
            comboBox2.SelectedIndex = 0;
            comboBox1.Items.Add("Invincibility");
            comboBox1.Items.Add("Knock");
            comboBox1.Items.Add("Control gates");
            comboBox1.Items.Add("Blind effect");
            comboBox1.Items.Add("Freeze");
            comboBox1.Items.Add("Neighbors room access");
            comboBox1.Items.Add("Tomato yeeted in face?");
            comboBox1.Items.Add("Speed boost");
            comboBox1.Items.Add("Strength");
            comboBox1.Items.Add("Speed and strength");
            comboBox1.SelectedIndex = 0;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "end " + comboBox2.SelectedIndex.ToString();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "windows";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "lights";
        }

        private void button16_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "items";
        }

        private void button17_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "fakedeath";
        }

        private void button18_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "doors";
        }

        public void UpdateControl()
        {
            label5.Text = "Neighbor: " + MyMod.neighborName;
            comboBox3.Items.Clear();
            foreach (string str in MyMod.playerNames) comboBox3.Items.Add(str);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "kick";
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            MyMod.lastCommand = "neighbor";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "buff " + comboBox1.SelectedIndex.ToString();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "rembuff " + comboBox1.SelectedIndex.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "tpall";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "blind";
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyMod.lastCommand = "select " + comboBox3.SelectedItem.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = "Looking at: " + MyMod.lookingAt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "lag";
        }

        private void button21_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "box";
        }
    }
}
