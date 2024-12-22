using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Radiostation
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void dj_button_Click(object sender, EventArgs e)
        {
            FormDJ formDJ = new FormDJ();
            formDJ.FormClosed += (s, args) => this.Show();
            formDJ.Show();
            this.Hide();
        }

        private void manager_button_Click(object sender, EventArgs e)
        {
            FormManager formManager = new FormManager();
            formManager.FormClosed += (s, args) => this.Show();
            formManager.Show();
            this.Hide();
        }

        private void visitor_button_Click(object sender, EventArgs e)
        {
            Guest guest = new Guest();
            guest.FormClosed += (s, args) => this.Show();
            guest.Show();
            this.Hide();
        }
    }
}
