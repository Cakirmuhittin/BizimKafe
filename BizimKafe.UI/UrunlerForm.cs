using BizimKafe.DATA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizimKafe.UI
{
    public partial class UrunlerForm : Form
    {
        private readonly KafeVeri _db;
        public UrunlerForm(KafeVeri db)
        {
            InitializeComponent();
            _db = db;
            UrunleriListele();
        }
        private void UrunleriListele()
        {
            dgvUrunler.DataSource = _db.Urunler.ToList();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string ad = txtUrunAd.Text.Trim();
            if (btnEkle.Text == "EKLE")
            {
                if (string.IsNullOrEmpty(ad))
                {
                    MessageBox.Show("Urun ADı gerekli");
                    return;
                }
                _db.Urunler.Add(new Urun()
                {
                    UrunAd = ad,
                    BirimFiyat = nudBirimFiyat.Value
                });
            }
            else if (btnEkle.Text == "KAYDET")
            {
                DataGridViewRow satir = dgvUrunler.SelectedRows[0];
                Urun urun = (Urun)satir.DataBoundItem;
                urun.BirimFiyat = nudBirimFiyat.Value;
                urun.UrunAd = txtUrunAd.Text;
                UrunleriListele();

                btnEkle.Text = "EKLE";
                btnIptal.Visible = false;
                nudBirimFiyat.Value = 0;
                txtUrunAd.Clear();

            }

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            //Eğer seçili ürün yoksa hata mesajı 
            if (dgvUrunler.SelectedRows.Count == 0) // seçili satır sayısı 0 ise
            {
                MessageBox.Show("Önce ürün seçiniz");
                return; // void metodlarda return direkt döngüden cıkıs saglar
            }

            DialogResult dr = MessageBox.Show("Seçili ürünü silmek istediğinize eminmisiniz.", " Silme Onayı ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.No)
                return;
            DataGridViewRow satir = dgvUrunler.SelectedRows[0];
            Urun urun = (Urun)satir.DataBoundItem;//DataBoundItem datagridwiev lerde istenilen itemi alabilmek icin kullanılır genellikle datasource kullanılınca olr
            _db.Urunler.Remove(urun);
            UrunleriListele();
            nudBirimFiyat.Value = 0;
            txtUrunAd.Clear();

        }

        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            btnEkle.Text = "KAYDET";
            btnIptal.Visible = true;
            DataGridViewRow satir = dgvUrunler.SelectedRows[0];
            Urun urun = (Urun)satir.DataBoundItem;
            txtUrunAd.Text = urun.UrunAd;
            nudBirimFiyat.Value = urun.BirimFiyat;

        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            btnEkle.Text = "EKLE";
            btnIptal.Visible = false;
            nudBirimFiyat.Value = 0;
            txtUrunAd.Clear();

        }
    }
}
