using System;
using System.Drawing;
using System.Windows.Forms;

namespace stellaris_mod_manager
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            textBox1.Text = Properties.Settings.Default.Folder;
            textBox2.Text = Properties.Settings.Default.Key;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Properties.Settings.Default.Folder = textBox1.Text;
                Properties.Settings.Default.Key = textBox2.Text;
                Properties.Settings.Default.Save();
                this.Close();
            }
            else
            {
                label1.ForeColor = Color.Red;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            Properties.Settings.Default.Save();
            textBox1.Text = Properties.Settings.Default.Folder;
            textBox2.Text = Properties.Settings.Default.Key;
        }
    }
}
