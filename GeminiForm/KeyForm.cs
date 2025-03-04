using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeminiForm
{
    public partial class KeyForm: Form
    {
        public KeyForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string key = richTextBox1.Text;  // Lấy giá trị từ RichTextBox
            Form1 f = new Form1(key);
            f.Show();// Tạo form mới với tham số key
            this.Hide();  // Đóng form cũ
        }

    }
}
