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
    public partial class FormDJ : Form
    {
        public FormDJ()
        {
            InitializeComponent();
        }

        private void playlists_form_button_Click(object sender, EventArgs e)
        {
            Playlists_form playlists_form = new Playlists_form();
            playlists_form.FormClosed += (s, args) => this.Show();
            playlists_form.Show();
            this.Hide();
        }

        private void Track_form_button_Click(object sender, EventArgs e)
        {
            Track_form track_form = new Track_form();
            track_form.FormClosed += (s, args) => this.Show();
            track_form.Show();
            this.Hide();
        }

        private void genre_form_button_Click(object sender, EventArgs e)
        {
            Genre_form genre_form = new Genre_form();
            genre_form.FormClosed += (s, args) => this.Show();
            genre_form.Show();
            this.Hide();
        }

        private void ads_form_button_Click(object sender, EventArgs e)
        {
            Ad_record_form ad_record_form = new Ad_record_form();
            ad_record_form.FormClosed += (s, args) => this.Show();
            ad_record_form.Show();
            this.Hide();
        }

        private void ads_block_form_button_Click(object sender, EventArgs e)
        {
            Ad_block_form ad_block_form = new Ad_block_form();
            ad_block_form.FormClosed += (s, args) => this.Show();
            ad_block_form.Show();
            this.Hide();
        }

        private void shedule_playlists_form_button_Click(object sender, EventArgs e)
        {
            Shedule_playlists_form shedule_playlists_form = new Shedule_playlists_form();
            shedule_playlists_form.FormClosed += (s, args) => this.Show();
            shedule_playlists_form.Show();
            this.Hide();
        }

        private void shedule_ads_form_button_Click(object sender, EventArgs e)
        {
            Shedule_ads_block_form shedule_ads_block_form = new Shedule_ads_block_form();
            shedule_ads_block_form.FormClosed += (s, args) => this.Show();
            shedule_ads_block_form.Show();
            this.Hide();
        }

        private void online_event_form_button_Click(object sender, EventArgs e)
        {
            Online_event_form online_event_form = new Online_event_form();
            online_event_form.FormClosed += (s, args) => this.Show();
            online_event_form.Show();
            this.Hide();
        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormDJ_Load(object sender, EventArgs e)
        {

        }
    }
}
