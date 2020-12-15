using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        FLOWERSEntities2 context = new FLOWERSEntities2();

        public Form1()
        {
            InitializeComponent();
            LoadFlowers();
            SearchFlowers();
            ApplyDesign();
        }

        private List<Flowers> _flowers = new List<Flowers>();
        private void LoadFlowers()
        {
            _flowers.Clear();
            using(StreamReader sr = new StreamReader("flowers.csv", Encoding.Default))
            {
                sr.ReadLine(); //Remove headers

                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(',');

                    Flowers f = new Flowers();
                    f.FlowerType = line[0];
                    f.Area = line[1];
                    _flowers.Add(f);

                    FillChart(line[0], line[1]);
                }
                FillDataGridView(_flowers);
            }
        }

        public void FillChart(string x, string y)
        {
            chart1.Series["Flowers"].Points.AddXY(x, y);
        }

        public void FillDataGridView(object flowers)
        {

            dataGridView1.DataSource = flowers;
        }

        public void SearchFlowers()
        {
            string searchValue = textBox1.Text.ToLower();

            var flowers = (from x in context.FLOWERS
                           where x.FlowerName.Contains(searchValue)
                           select x.FlowerName).ToList();

            listBox1.DataSource = flowers.ToList();
        }

        public void ApplyDesign()
        {
            chart1.Titles.Add("Area used for the production of flower bulbs in the Netherlands in 2019, by flower type");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SearchFlowers();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen greenPen = new Pen(Color.ForestGreen);
            Pen pinkPen = new Pen(Color.DeepPink);
            Brush myBrush = new SolidBrush(Color.Pink);

            // Stem
            g.DrawLine(greenPen, 200, 500, 200, 600);

            // Leaves
            Rectangle bottomLeaf = new Rectangle(150, 500, 100, 100);
            g.DrawArc(greenPen, bottomLeaf, 0, 90);

            Rectangle topLeaf = new Rectangle(200, 550, 100, 100);
            g.DrawArc(greenPen, topLeaf, 180, 90);

            // Flower
            Rectangle bottomOutline = new Rectangle(175, 400, 50, 100);
            g.DrawArc(pinkPen, bottomOutline, 0, 180);

            Rectangle rightPetal = new Rectangle(200, 450, 50, 100);
            g.DrawArc(pinkPen, rightPetal, 180, 90);

            Rectangle leftPetal = new Rectangle(150, 450, 50, 100);
            g.DrawArc(pinkPen, leftPetal, 270, 90);

            Rectangle bottomPetal = new Rectangle(190, 450, 20, 50);
            g.DrawArc(pinkPen, bottomPetal, 270, 180);
            g.DrawArc(pinkPen, bottomPetal, 90, 180);
        }
    }
}
