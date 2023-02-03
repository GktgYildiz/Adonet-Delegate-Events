using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace Winform24
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            button1.Enabled = false;
        }
        DataRowView _gelen;
        public Form2(DataRowView gelen, string islem)
        {
            InitializeComponent();
            _gelen = gelen;
            if (islem == "Guncelle")
            {
                button1.Enabled = false;
                button2.Enabled = true;

            }
            else if (islem == "Sil")
            {
                button1.Enabled = true;
                button2.Enabled = false;
            }
        }

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Winform24.Properties.Settings.NorthwindConnectionString"].ConnectionString);
        public delegate void UrunEkleEventHandler();
        public event UrunEkleEventHandler UrunEkle;
        //event ile bu tetiklenince kendi üstüne alıyor , event eklemeye gerek kalmıyor

        public delegate void UrunGuncelleEventHandler();
        public event UrunGuncelleEventHandler UrunGuncelle;


        public delegate void UrunSilEventHandler();
        public event UrunSilEventHandler UrunSil;

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'northwindDataSet2.Categories' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.categoriesTableAdapter.Fill(this.northwindDataSet2.Categories);
            if (_gelen != null)
            {
                //guncelle: eğer gelen veri boş değilse güncelleme var
                textBox1.Text = _gelen["ProductName"].ToString();
                textBox2.Text = _gelen["UnitPrice"].ToString();
                textBox3.Text = _gelen["UnitsInStock"].ToString();
                textBox4.Text = _gelen["UnitsOnOrder"].ToString();
                comboBox1.SelectedValue = _gelen["CategoryId"];
            }
            else
            {
                MessageBox.Show("Lütfen yönergelere uygun bir biçimde seçim yapınız.");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_gelen != null)
            {
                //güncelle
                string sorguGuncelle = "Update Products Set ProductName=@name, UnitPrice=@price, UnitsInStock=@stock, UnitsOnOrder=@order, CategoryId=@catId where ProductId = @id";
                using (SqlCommand cmd = new SqlCommand(sorguGuncelle, conn))
                {
                    cmd.Parameters.AddWithValue("@name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@price", textBox2.Text);
                    cmd.Parameters.AddWithValue("@stock", textBox3.Text);
                    cmd.Parameters.AddWithValue("@order", textBox4.Text);
                    cmd.Parameters.AddWithValue("@catId", comboBox1.SelectedValue);

                    cmd.Parameters.AddWithValue("@id", _gelen["ProductId"]);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        UrunGuncelle();
                        this.Close();
                    }
                    else
                    {
                        conn.Close();
                    }
                }

            }
            else
            {
                //ekle
                string sorguInsert = "Insert Products(ProductName,UnitPrice,UnitsInStock,UnitsOnOrder,CategoryId) Values (@name, @price, @stock, @order, @catId)";
                using (SqlCommand cmd = new SqlCommand(sorguInsert, conn))
                {
                    cmd.Parameters.AddWithValue("@name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@price", textBox2.Text);
                    cmd.Parameters.AddWithValue("@stock", textBox3.Text);
                    cmd.Parameters.AddWithValue("@order", textBox4.Text);
                    cmd.Parameters.AddWithValue("@catId", comboBox1.SelectedValue);

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Kayıt Başarıyla Oluşturuldu");
                        UrunEkle();
                        this.Close();
                    }
                    else
                    {
                        conn.Close();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_gelen != null)
            {
                string sorguDelete = "Delete from Products where ProductId=@id";
                using (SqlCommand cmd = new SqlCommand(sorguDelete, conn))
                {
                    cmd.Parameters.AddWithValue("@id", _gelen["ProductId"]);
                    try
                    {
                        DialogResult res = MessageBox.Show("Ürünü silmek istediğinize emin misiniz?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        if (res == DialogResult.OK)
                        {
                            if (conn.State == ConnectionState.Closed)
                            {
                                conn.Open();
                                cmd.ExecuteNonQuery();
                                conn.Close();
                                UrunSil();
                                MessageBox.Show("Ürün başarıyla silindi!");
                                this.Close();
                            }
                            else
                            {
                                conn.Close();
                            }
                        }
                        if (res == DialogResult.Cancel)
                        {
                            this.Close();

                        }


                    }
                    catch (Exception)
                    {

                        MessageBox.Show("beklenmedik bir hata oluştu!");
                    }

                }
            }
            else
            {
                MessageBox.Show("Silinecek ürünün Id'si boş. Lütfen kontrol ediniz");
            }

        }
    }
}
