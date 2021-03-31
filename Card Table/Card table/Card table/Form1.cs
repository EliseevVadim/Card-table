using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;

namespace Card_table
{
    public struct CardInfo
    {
        public PointF point;
        public Image card;
        public int cardangle;
    }
    public partial class Form1 : Form
    {
        List<CardInfo> infos = new List<CardInfo>();
        PointF point;
        bool Pressed = false;
        Graphics g;
        List<Image> images = new List<Image>();
        Bitmap bit;
        bool b = false;
        public Form1()
        {
            InitializeComponent();
            g = this.CreateGraphics();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Graphics dc = CreateGraphics();
            bit = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            Graphics bufDC = Graphics.FromImage(bit);
            bufDC.FillRectangle(new LinearGradientBrush(new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height), Color.DarkGreen, Color.LightGreen, LinearGradientMode.Vertical), ClientRectangle);
            foreach (string s in Directory.EnumerateFiles(Environment.CurrentDirectory + @"\картинки"))
            {
                Image image = Image.FromFile(s);
                images.Add(image);
            }
            bufDC.DrawImage(images[0], 10, 10, 100, 140);
            bufDC.Dispose();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!b)
            {
                Graphics bufDC = Graphics.FromImage(bit);
                bufDC.FillRectangle(new LinearGradientBrush(new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height), Color.DarkGreen, Color.LightGreen, LinearGradientMode.Vertical), ClientRectangle);
                Image image = Image.FromFile(Environment.CurrentDirectory + @"\for.png");
                for (int i = 0; i < 36; i += 3)
                    bufDC.DrawImage(image, 600f + i, 30f + i, 135, 189);

                bufDC.Dispose();
                g.DrawImage(bit, 0, 0);
                b = true;
            }
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

            try
            {
                if ((e.X > 600f + 36 && e.X < 600f + 171) && (e.Y > 30f + 36 && e.Y < 30f + 225))
                {
                    point = Cursor.Position;
                    Random random = new Random();
                    Random angle = new Random();
                    int index = random.Next(0, images.Count);
                    Graphics bufDC = Graphics.FromImage(bit);
                    bufDC.DrawImage(images[index], e.X, e.Y, 135, 189);
                    bufDC.Dispose();
                    g.DrawImage(bit, 0, 0);
                    randcard = images[index];
                    images.Remove(images[index]);
                    Pressed = true;
                }
            }
            catch
            {
                MessageBox.Show("Карты закончились");
            }
        }
        Image randcard;
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Graphics bufDC = Graphics.FromImage(bit);
                point.X = e.X;
                point.Y = e.Y;
                try
                {
                    bufDC.Clear(Color.Beige);
                    bufDC.FillRectangle(new LinearGradientBrush(new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height), Color.DarkGreen, Color.LightGreen, LinearGradientMode.Vertical), ClientRectangle);
                    for (int i = 0; i < infos.Count; i++)
                    {
                        bufDC.TranslateTransform(infos[i].point.X, infos[i].point.Y);
                        bufDC.RotateTransform(infos[i].cardangle);
                        bufDC.TranslateTransform(-infos[i].point.X, -infos[i].point.Y);
                        bufDC.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        bufDC.DrawImage(infos[i].card, infos[i].point.X, infos[i].point.Y, 135, 189);
                        bufDC.ResetTransform();
                    }
                    bufDC.ResetTransform();
                    bufDC.DrawImage(randcard, point.X, point.Y, 135, 189);
                    Image image = Image.FromFile(Environment.CurrentDirectory + @"\for.png");
                    for (int i = 0; i < images.Count; i++)
                    {
                        bufDC.DrawImage(image, 600f + i, 30f + i, 135, 189);
                    }
                    bufDC.Dispose();
                    g.DrawImage(bit, 0, 0);
                }
                catch
                {

                }
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (Pressed)
                {
                    Pressed = false;
                    Random angle = new Random();
                    int ra = angle.Next(0, 360);
                    Graphics bufDC = Graphics.FromImage(bit);
                    bufDC.Clear(Color.White);
                    bufDC.FillRectangle(new LinearGradientBrush(new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height), Color.DarkGreen, Color.LightGreen, LinearGradientMode.Vertical), ClientRectangle);
                    Image image = Image.FromFile(Environment.CurrentDirectory + @"\for.png");
                    for (int i = 0; i < images.Count; i++)
                    {
                        bufDC.DrawImage(image, 600f + i, 30f + i, 135, 189);
                    }
                    for(int i=0; i<infos.Count; i++)
                    {
                        bufDC.TranslateTransform(infos[i].point.X, infos[i].point.Y);
                        bufDC.RotateTransform(infos[i].cardangle);
                        bufDC.TranslateTransform(-infos[i].point.X, -infos[i].point.Y);
                        bufDC.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        bufDC.DrawImage(infos[i].card, infos[i].point.X, infos[i].point.Y, 135, 189);
                        bufDC.ResetTransform();
                    }
                    bufDC.ResetTransform();
                    PointF point = new PointF(e.X, e.Y);
                    bufDC.TranslateTransform(point.X, point.Y);
                    bufDC.RotateTransform(ra);
                    bufDC.TranslateTransform(-point.X, -point.Y);
                    bufDC.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    bufDC.DrawImage(randcard, point.X, point.Y, 135, 189);
                    bufDC.Dispose();
                    g.DrawImage(bit, 0, 0);
                    infos.Add(new CardInfo { card = randcard, cardangle = ra, point = point });
                    randcard = null;
                }
            }
            catch
            {

            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                Graphics bufDC = Graphics.FromImage(bit);
                infos.Clear();
                images.Clear();
                foreach (string s in Directory.EnumerateFiles(Environment.CurrentDirectory + @"\картинки"))
                {
                    Image im = Image.FromFile(s);
                    images.Add(im);
                }
                bufDC.Clear(Color.Red);
                bufDC.FillRectangle(new LinearGradientBrush(new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height), Color.DarkGreen, Color.LightGreen, LinearGradientMode.Vertical), ClientRectangle);
                Image image = Image.FromFile(Environment.CurrentDirectory + @"\for.png");
                for (int i = 0; i < images.Count; i++)
                {
                    bufDC.DrawImage(image, 600f + i, 30f + i, 135, 189);
                }
                g.DrawImage(bit, 0, 0);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            bit = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
        }
    }
}