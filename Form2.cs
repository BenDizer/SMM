using System;
using System.Drawing;
using System.Windows.Forms;

namespace stellaris_mod_manager
{
    public partial class Form2 : Form
    {
        string File, File_Doc;
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
                Properties.Settings.Default.Folder = File;
                Properties.Settings.Default.Folder_Doc = File_Doc;
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

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
                File_Doc = folderBrowserDialog1.SelectedPath + @"\Stellaris";
                File = folderBrowserDialog1.SelectedPath + @"\Stellaris\mod";
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }
    }
}
