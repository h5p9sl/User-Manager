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
    public partial class EditUser : Form
    {
        private UserManager userManager;
        private UserInformation user;
        private int index;

        void InitializeInfo()
        {
            this.textBox1.Text = this.user.username;
            this.textBox2.Text = this.user.uid.ToString();
            this.richTextBox1.Text = this.user.notes;
            foreach (var _byte in this.user.macAddress)
            {
                this.textBox3.AppendText(_byte.ToString());
            }
            DateTime date = new DateTime((long)user.expiryDate, DateTimeKind.Unspecified);
            this.textBoxExpiryDate.Text = date.ToShortDateString() + " (" + date.ToLongDateString() + ")";
        }

        public EditUser(ref UserManager userManager, int index)
        {
            this.userManager = userManager;
            this.user = userManager.ElementAt(index);
            this.index = index;
            InitializeComponent();
            this.InitializeInfo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.monthCalendar1.Visible = true;
            this.textBoxExpiryDate.Visible = false;
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateTime date;
            if (sender == null || e == null)
            {
                date = DateTime.Now;
            }
            else { date = e.End; }
            this.monthCalendar1.Visible = false;
            this.textBoxExpiryDate.Visible = true;
            this.textBoxExpiryDate.Text = date.ToShortDateString() + " (" + date.ToLongDateString() + ")";
            this.user.expiryDate = (UInt64)date.Ticks;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.user.username = this.textBox1.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.userManager.ElementAt(this.index).SetTo(this.user);
            this.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.user.notes = this.richTextBox1.Text;
        }
    }
}
