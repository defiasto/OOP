using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Picaso
{
    
    public partial class Form1 : Form
    {
        private Bitmap GraphicsImage = new Bitmap(640, 480, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        int size, mode;
        Color fg, bg;
        Point tP1, tP2;
        Point[] bezi = new Point[4];
        bool isDrawing = false;
        int crvcnt = 0;
        List<Point> points = new List<Point>();

        private Rectangle mkRect(Point start, Point end) {
            return new Rectangle(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y), Math.Abs(start.X - end.X), Math.Abs(start.Y - end.Y));
        }

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            Graphics.FromImage(GraphicsImage).Clear(Color.White);
            mode = 2; //rect
            size = 5;
            numericUpDown1.Value = size;
            fg = button1.BackColor;
            bg = button2.BackColor;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Graphics.FromImage(GraphicsImage).Clear(Color.White);
            drawPanel.CreateGraphics().DrawImageUnscaled(GraphicsImage, new Point(0, 0));
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            GraphicsImage = new Bitmap(Image.FromFile(openFileDialog1.FileName), 640, 480);
            //Graphics.FromImage(GraphicsImage).Dispose();
            drawPanel.CreateGraphics().DrawImageUnscaled(GraphicsImage, new Point(0, 0));
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(GraphicsImage, new Point(0, 0));
        }

        private void drawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (mode == 2 || mode == 3 || mode == 0)
            {
                tP1 = e.Location;
            }
            else if (mode == 4)
            {
                points = new List<Point>();
                points.Add(e.Location);
                isDrawing = true;
            }
        }

        private void drawPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
            if (mode != 1) tP2 = e.Location;
            Graphics drgr = Graphics.FromImage(GraphicsImage);
            if (mode == 2)
            {
                if (!transpBox.Checked)
                    drgr.FillRectangle(new SolidBrush(bg), mkRect(tP1, tP2));
                drgr.DrawRectangle(new Pen(fg, size), mkRect(tP1, tP2));
                
            }
            else if (mode == 3)
            {
                if (!transpBox.Checked)
                    drgr.FillEllipse(new SolidBrush(bg), mkRect(tP1, tP2));
                drgr.DrawEllipse(new Pen(fg, size), mkRect(tP1, tP2));
            }
            else if (mode == 0)
            {
                drgr.DrawLine(new Pen(fg, size), tP1, tP2);
            }
            
            
            drawPanel.CreateGraphics().DrawImageUnscaled(GraphicsImage, new Point(0, 0));
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                button1.BackColor = colorDialog1.Color;
                fg = colorDialog1.Color;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                button2.BackColor = colorDialog1.Color;
                bg = colorDialog1.Color;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            size = (int)numericUpDown1.Value;
        }

        private void drawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                points.Add(e.Location);
                Graphics drgr = Graphics.FromImage(GraphicsImage);
                if (size > 1)
                    drgr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                for (int i = 0; i < points.Count - 1; i++)
                    drgr.DrawLine(new Pen(fg, size), points[i], points[i + 1]);
                drawPanel.CreateGraphics().DrawImageUnscaled(GraphicsImage, new Point(0, 0));
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rectBtn_Click(object sender, EventArgs e)
        {
            mode = 2; //rectangle
            transpBox.Enabled = true;
        }

        private void elBtn_Click(object sender, EventArgs e)
        {
            mode = 3; //elipse
            transpBox.Enabled = true;
        }

        private void lnBtn_Click(object sender, EventArgs e)
        {
            mode = 0; //line
            transpBox.Enabled = false;
        }

        private void crvBtn_Click(object sender, EventArgs e)
        {
            mode = 1; //curve
            transpBox.Enabled = false;
        }

        private void penBtn_Click(object sender, EventArgs e)
        {
            mode = 4; //pencil
            transpBox.Enabled = false;
        }

        private void drawPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (mode == 1)
            {
                if (crvcnt < 4)
                {
                    bezi[crvcnt] = e.Location;
                    crvcnt++;
                }
                if (crvcnt == 4)
                {
                    Graphics drgr = Graphics.FromImage(GraphicsImage);
                    drgr.DrawBezier(new Pen(fg, size), bezi[0], bezi[1], bezi[2], bezi[3]);
                    drawPanel.CreateGraphics().DrawImageUnscaled(GraphicsImage, new Point(0, 0));
                    crvcnt = 0;
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            GraphicsImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraphicsImage = new Bitmap(Image.FromFile("about.bmp"), 640, 480);
            drawPanel.CreateGraphics().DrawImageUnscaled(GraphicsImage, new Point(0, 0));
        }

        
    }
}
