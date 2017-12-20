using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApplication2
{

    public partial class Form1 : Form
    {
        private int x=-1, y=-1;
        private Image b;
        public Form1()
        {
            InitializeComponent();
        }

        private Bitmap bmpScreenshot;
        private Graphics gfxScreenshot;
        private Bitmap Btmp;
        private void Form1_Load(object sender, EventArgs e)
        {
            bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                Screen.PrimaryScreen.Bounds.Height,
                PixelFormat.Format24bppRgb);
            gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                Screen.PrimaryScreen.Bounds.Y,
                0,
                0,
                Screen.PrimaryScreen.Bounds.Size,
                CopyPixelOperation.SourceCopy);
            Left = 0;
            Top = 0;
            Size = Screen.PrimaryScreen.Bounds.Size;
            pictureBox1.Size = Size;
            b = (Image) bmpScreenshot.Clone();
            pictureBox1.Image = b;
            pictureBox1.Controls.Add(pictureBox2);
            pictureBox1.Controls.Add(pictureBox3);
            pictureBox1.Controls.Add(pictureBox4);
            pictureBox1.Controls.Add(pictureBox5);
            var bmpRedW = new Bitmap(Screen.PrimaryScreen.Bounds.Width, 1, PixelFormat.Format32bppArgb);
            for (int i = 0; i < Screen.PrimaryScreen.Bounds.Width; i++)
            {
                bmpRedW.SetPixel(i, 0, Color.Red);
            }
            var bmpRedH = new Bitmap(1, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            for (int j = 0; j < Screen.PrimaryScreen.Bounds.Height; j++)
            {
                bmpRedH.SetPixel(0, j, Color.Red);
            }
            pictureBox2.Image = bmpRedH;
            pictureBox3.Image = bmpRedH;
            pictureBox4.Image = bmpRedW;
            pictureBox5.Image = bmpRedW;
        }

        public static Bitmap cropAtRect(Bitmap b, Rectangle r)
        {
            Bitmap nb = new Bitmap(r.Width, r.Height);
            Graphics g = Graphics.FromImage(nb);
            g.DrawImage(b, -r.X, -r.Y);
            return nb;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            int x1 = x < e.X ? x : e.X, x2 = x > e.X ? x : e.X, y1 = y < e.Y ? y : e.Y, y2 = y > e.Y ? y : e.Y;
            int w = x2 - x1;
            int h = y2 - y1;
            if ((w == 0) || (h == 0))
            {
                Close();
                return;
            }
            Size temp = new Size(w, h);
            var bmpScreenshot1 = cropAtRect(bmpScreenshot, new System.Drawing.Rectangle(x1, y1, w, h));
            //var bmpScreenshot1 = bmpScreenshot.Clone(new System.Drawing.Rectangle(x, y, w, h), PixelFormat.Format32bppArgb);
            var MyPicturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            var ScreenShotPath = MyPicturesPath + "\\screenshot.png";
            bmpScreenshot1.Save(ScreenShotPath, ImageFormat.Png);
            var ClipboardFile = new StringCollection();
            ClipboardFile.Add(ScreenShotPath);
            var ClipboardData = new DataObject();
            ClipboardData.SetImage(bmpScreenshot1);
            ClipboardData.SetText(ScreenShotPath);
            ClipboardData.SetFileDropList(ClipboardFile);
            Clipboard.SetDataObject(ClipboardData, true);
            Close();
        }

        Pen pr = new Pen(Color.Red);
        Brush pt = new SolidBrush(Color.Transparent);
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (x != -1)
            {
                int x1 = x < e.X ? x : e.X, x2 = x > e.X ? x : e.X, y1 = y < e.Y ? y : e.Y, y2 = y > e.Y ? y : e.Y;
                int w = x2 - x1;
                int h = y2 - y1;
                Rectangle r = new Rectangle(x1, y1, w, h);
                pictureBox2.Size = new Size(1, h);
                pictureBox3.Size = new Size(1, h);
                pictureBox2.Location = new Point(x1, y1);
                pictureBox3.Location = new Point(x1 + w, y1);
                pictureBox4.Size = new Size(w, 1);
                pictureBox5.Size = new Size(w, 1);
                pictureBox4.Location = new Point(x1, y1);
                pictureBox5.Location = new Point(x1, y1 + h);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
        }
    }
}
