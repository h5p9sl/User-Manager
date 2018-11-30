using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserManager
{
    public partial class Form1 : Form
    {
        private readonly string constTitle = "User Manager";

        private Loader loader = null;
        public UserManager userManager = null;
        private bool fileOpened = false;
        private string filePath = null;
        private bool unsavedChanges = false;

        public Form1()
        {
            this.loader = new Loader();
            this.userManager = new UserManager();
            InitializeComponent();
        }

        private void updateUserList(string filter = null)
        {
            this.listBox1.Items.Clear();
            if (filter != null)
            {
                filter = filter.ToLower();
                foreach (var user in this.userManager)
                {
                    if (user.username.ToLower().Contains(filter) == true)
                    {
                        this.listBox1.Items.Add(user.username);
                    }
                }
            }
            else
            {
                foreach (var user in this.userManager)
                {
                    this.listBox1.Items.Add(user.username);
                }
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!this.fileOpened)
            {
                var result = this.saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.fileOpened = true;
                    this.filePath = saveFileDialog1.FileName;
                    this.Text = this.constTitle + " - " + System.IO.Path.GetFileName(this.filePath);
                    this.loader.SaveDatabase(ref this.userManager, filePath);
                }
            }
            else
            {
                this.loader.SaveDatabase(ref this.userManager, filePath);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.fileOpened)
            {
                this.closeToolStripMenuItem_Click(sender, e);
            }
            var result = this.openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.fileOpened = true;
                this.filePath = this.openFileDialog1.FileName;
                this.Text = this.constTitle + " - " + System.IO.Path.GetFileName(this.filePath);
                this.loader.LoadDatabase(ref this.userManager, filePath);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.fileOpened)
            {
                if (this.unsavedChanges)
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("You have unsaved changes. Save?", "Unsaved Changes", buttons);
                    if (result == DialogResult.Yes)
                    {
                        this.saveToolStripMenuItem1_Click(sender, e);
                    }
                }
                this.listBox1.Items.Clear();
                this.filePath = null;
                this.Text = this.constTitle;
                this.fileOpened = false;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem1_Click(sender, e);
        }

        private void listBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void newUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserInformation user = new UserInformation(ref this.userManager);
            this.userManager.Add(user);
            this.listBox1.Items.Add(user.username);
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem != null)
            {
                EditUser userEditer = new EditUser(ref this.userManager, this.listBox1.SelectedIndex);
                userEditer.ShowDialog();
                userEditer.Dispose();
                this.updateUserList();
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (this.listBox1.SelectedItem == null)
            {
                this.contextMenuStrip1.Items[1].Enabled = false;
            }
            else
            {
                this.contextMenuStrip1.Items[1].Enabled = true;
            }
        }

        private void editUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1_DoubleClick(sender, e);
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F && e.Control == true)
            {
                this.filterTextBox1.Select();
            }
        }

        private void filterTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.updateUserList(this.filterTextBox1.Text);
        }

        private void filterClearButton1_Click(object sender, EventArgs e)
        {
            this.filterTextBox1.Clear();
        }
    }
}
