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
                    chart1.Series["Flowers"].Points.AddXY(line[0], line[1]);
                }
                dataGridView1.DataSource = _flowers;
                chart1.Titles.Add("Area used for the production of flower bulbs in the Netherlands in 2019, by flower type");
            }
        }

        public void SearchFlowers()
        {
            string searchValue = textBox1.Text.ToLower();

            var flowers = (from x in context.FLOWERS
                           where x.FlowerName.Contains(searchValue)
                           select x.FlowerName).ToList();

            listBox1.DataSource = flowers.ToList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SearchFlowers();
        }
    }
}
