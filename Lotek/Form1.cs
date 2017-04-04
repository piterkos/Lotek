using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lotek
{
    public partial class Form1 : Form
    {
        GraLiczbowa gra;
        List<string> wybrane;

        public Form1()
        {
            InitializeComponent();
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
            gra = new GraLiczbowa();

            for (int i = 0; i < gra.PobierzWylosowane().Count; i++)
            {
                foreach (Control label in tabPage3.Controls)
                {
                    if (label.Name == "lotto" + (i + 1) + "_lbl")
                        label.Text = gra.PobierzWylosowane()[i];
                }
            }

        }
    }
}
