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
    public partial class Empty_Lobby_Page : UserControl
    {
        public Empty_Lobby_Page()
        {
            InitializeComponent();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            MyMod.lastCommand = "join";
        }
    }
}
