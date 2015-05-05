using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MarkdownAddin
{
    public partial class ImgWnd : Form
    {
        public ImgWnd()
        {
            InitializeComponent();
        }

        private void ImgWnd_Load(object sender, EventArgs e)
        {

        }

        private void pbxBrowse_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "图片(*.jpg, *.gif, *.bmp, *.png)|*.jpg;*.gif;*.bmp;*.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                pbxPreview.ImageLocation = dialog.FileName;
            }
        }

        private void txtImg_Leave(object sender, EventArgs e)
        {
            var content = txtImg.Text.Trim();
        }
    }
}
