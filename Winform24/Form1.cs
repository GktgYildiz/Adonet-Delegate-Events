using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winform24
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UrunBilgisi();
            listBox1.SelectedIndex = -1;
        }
        private void UrunBilgisi()
        {
            this.productsTableAdapter.Fill(this.northwindDataSet1.Products);

        }
        Form2 frm2 = new Form2();

        private void button1_Click(object sender, EventArgs e)
        {
            frm2.UrunEkle += new Form2.UrunEkleEventHandler(UrunBilgisi);
            frm2.Show();
        }

        private void güncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {

                frm2 = new Form2((DataRowView)listBox1.SelectedItem, "Guncelle");
                frm2.UrunGuncelle += new Form2.UrunGuncelleEventHandler(UrunBilgisi);
                frm2.Show();
            }
            else
            {
                MessageBox.Show("sanki bişi seçsen mi acaba? :))))");
            }



        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                frm2 = new Form2((DataRowView)listBox1.SelectedItem, "Sil");
                frm2.UrunSil += new Form2.UrunSilEventHandler(UrunBilgisi);
                frm2.Show();
            }
            else
            {
                MessageBox.Show("sanki bişi seçsen mi acaba? :))))");
            }


        }
    }
}
