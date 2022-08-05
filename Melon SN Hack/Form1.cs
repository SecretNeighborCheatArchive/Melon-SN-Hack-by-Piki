using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnityEngine;

namespace Melon_SN_Hack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr one, int two, int three, int four);
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            game_Page1.Hide();
            lobby_Page1.Hide();
        }

        private void ChangeMode(Mode md)
        {
            if (md == currentMode) return;
            switch (md)
            {
                case Mode.Lobby:
                    currentMode = Mode.Lobby;
                    game_Page1.Hide();
                    lobby_Page1.Show();
                    empty_Lobby_Page1.Hide();
                    lobby_Page1.UpdateControl();
                    break;
                case Mode.Game:
                    currentMode = Mode.Game;
                    game_Page1.Show();
                    lobby_Page1.Hide();
                    empty_Lobby_Page1.Hide();
                    game_Page1.UpdateControl();
                    break;
                case Mode.Menu:
                    currentMode = Mode.Menu;
                    game_Page1.Hide();
                    lobby_Page1.Hide();
                    empty_Lobby_Page1.Show();
                    break;
                case Mode.Empty:
                    currentMode = Mode.Empty;
                    game_Page1.Hide();
                    lobby_Page1.Hide();
                    empty_Lobby_Page1.Hide();
                    break;
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MyMod.menu)
            {
                if (!hadFocus)
                {
                    SetActiveWindow(this.Handle);
                    hadFocus = true;
                }
                WindowState = FormWindowState.Normal;
            }
            else
            {
                hadFocus = false;
                WindowState = FormWindowState.Minimized;
                Location = new Point(-300, 0);
            }
            label2.Text = "Status: " + MyMod.status;
            Mode mode = (Mode)MyMod.mode;
            ChangeMode(mode);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab) MyMod.menu = false;
        }

        private enum Mode
        {
            Menu,
            Lobby,
            Game,
            Empty
        }

        private Mode currentMode = Mode.Menu;

        private void Form1_Leave(object sender, EventArgs e)
        {
            if (currentMode == Mode.Lobby) lobby_Page1.UpdateControl();
        }

        bool hadFocus = false;
    }
}
