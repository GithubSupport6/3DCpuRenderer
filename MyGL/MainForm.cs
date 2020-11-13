﻿using MyGL.Service;
using MyGL.Service.Files;
using MyGL.Service.GraphicsProvider;
using MyGL.Service.Math2D;
using MyGL.Service.Math3D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGL
{
    public partial class MainForm : Form
    {
        IGraphicsProvider provider;
        Bitmap image;
        MyGraphics graphics;
        IParser Parser = new WaveObjParser();
        int x = 1;
        int y = 1;
        float z = 0.5f;
        float dz = 0.001f;
        Object3D obj;
        Random random = new Random();
        float c = 5;

        public MainForm()
        {
            InitializeComponent();
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                MainPanel,
                new object[] { true }
            );
            MainPanel.MouseWheel += MainPanel_MouseWheel;
        }

        private void MainPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            //z += e.Delta * dz;
            c += (float)e.Delta / 30;
            if (c < 1)
            {
                c = 1;
            }
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {
            if (obj == null)
            {
                return;
            }

            e.Graphics.Clear(Color.Black);

            image = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height);

            provider = new BitmapProvider(image);
            graphics = new MyGraphics(provider);
            //graphics.Fill(Color.Black);

            int Width = e.ClipRectangle.Width;
            int Height = e.ClipRectangle.Height;

            Vec3f zero = new Vec3f(0,0,0);

            Vec3f ligth = new Vec3f(
                ((float)e.ClipRectangle.Width / 2 - x)  / (Width / 2),
                (y - (float)e.ClipRectangle.Height / 2) / (Height / 2),
                z);

            ligth.Normalize();

            //graphics.DrawLine(new Vec2i(200, 200), new Vec2i(x, y), Color.White);
            graphics.DrawObject(obj,Color.White,ligth, c);
            graphics.DrawLight(ligth, Width / 2);
            e.Graphics.DrawImage(image,0,0);
        }


        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainPanel_MouseMove(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
            MainPanel.Invalidate();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                obj = Parser.Parse(OpenFileDialog.FileName);
            }
        }

        private void Menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '+')
            {
                z += 0.1f;
            }
            else
            {
                z -= 0.1f;
            }
        }
    }
}
