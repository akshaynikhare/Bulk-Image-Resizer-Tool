using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace BulkImageCroping
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void AddFileButton_Click(object sender, EventArgs e)
        {
            if (addFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in addFileDialog.FileNames)
                {
                    if (File.Exists(file))
                    {
                        inputListBox.Items.Add(file);
                    }
                    if (inputListBox.Items.Count > 1)
                    {
                        cropBtn.Enabled = true;
                    }
                }

            }
            updateUI();
        }

        private void InputListBox_SelectedIndexChanged(object sender, EventArgs e)
        {


            priview.Image = new Bitmap(inputListBox.SelectedItem.ToString());
            updateUI();
        }





        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            inputListBox.Items.Remove(inputListBox.SelectedItem);
            updateUI();

        }

        private void updateUI()
        {
            if (inputListBox.SelectedIndex > -1)
            {
                RemoveButton.Enabled = true;
            }
            else
            {
                RemoveButton.Enabled = false;

            }

            if (inputListBox.Items.Count > 0)
            {
                RemoveAllButton.Enabled = true;
                cropBtn.Enabled = true;
            }
            else
            {
                RemoveAllButton.Enabled = false;
                cropBtn.Enabled = true;
            }
        }

        private void RemoveAllButton_Click(object sender, EventArgs e)
        {
            inputListBox.Items.Clear();
            updateUI();
        }

        private void CropBtn_Click(object sender, EventArgs e)
        {




            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {

                progressBar.Visible = true;
                this.Enabled = false;

                for (int i = 0; i < inputListBox.Items.Count; i++)
                {
                    if (File.Exists(folderBrowserDialog1.SelectedPath + "\\" + Path.GetFileName((string)inputListBox.Items[i])))
                    {
                        MessageBox.Show("The file exist. Overwritting. \n " + folderBrowserDialog1.SelectedPath + "\\" + Path.GetFileName((string)inputListBox.Items[i]));
                    }





                    using (Bitmap nl = ResizeImage(new Bitmap((string)inputListBox.Items[i]), Int32.Parse(W.Text), Int32.Parse(H.Text)))
                    {


                        nl.Save(folderBrowserDialog1.SelectedPath + "\\" + Path.GetFileName((string)inputListBox.Items[i]), ImageFormat.Jpeg);
                        // priview.Navigate(folderBrowserDialog1.SelectedPath + "\\" + Path.GetFileName((string)inputListBox.Items[i]));
                        //close using
                    }

                    progressBar.Value = (int)(((i + 1) / (double)inputListBox.Items.Count) * 100);
                }

                this.Enabled = true;
                progressBar.Visible = false;
                System.Diagnostics.Process.Start(folderBrowserDialog1.SelectedPath);
                inputListBox.Items.Clear();
            }



            updateUI();

        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/akshaynikhare");
        }
    }
}
