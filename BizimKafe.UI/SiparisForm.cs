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
    public partial class SiparisForm : Form
    {
        public event MasaTasindiEventHandler MasaTasindi;

        private readonly KafeVeri _db;
        private readonly Siparis _siparis;
        public SiparisForm(KafeVeri db,Siparis siparis)
        {
            _db = db;
            _siparis = siparis;
            InitializeComponent();
            BilgileriGuncelle();
            UrunleriGuncelle();
        }
        private void UrunleriGuncelle()
        {
            cboUrun.DataSource = _db.Urunler;
        }
        private void BilgileriGuncelle()
        {
            Text = $"Masa {_siparis.MasaNo}"; //Formun girişi
            lblMasaNo.Text = _siparis.MasaNo.ToString("00");
            lblOdemeTutari.Text = _siparis.ToplamTutarTL;
            dgvSiparisDetaylar.DataSource= _siparis.SiparisDetaylar.ToList(); // To list metodu bir lsiteyi kopyalayıp yenisini olusturur.
            MasanoLariYukle();
        }

        private void MasanoLariYukle()
        {
            cmbMasaNo.Items.Clear();
            for (int i = 1; i <= _db.MasaAdet; i++)
            {
                if (!_db.AktifSiparisler.Any(x => x.MasaNo == i))
                {
                    cmbMasaNo.Items.Add(i);
                }

            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            Urun urun = (Urun)cboUrun.SelectedItem;
            if (urun == null)
                return;
            SiparisDetay sd= _siparis.SiparisDetaylar.FirstOrDefault(x=>x.UrunAd==urun.UrunAd);
            if (sd != null)
            {
                sd.Adet += (int)nudAdet.Value;
            }
            else
            {
                sd = new SiparisDetay()
                {
                    UrunAd = urun.UrunAd,
                    BirimFiyat = urun.BirimFiyat,
                    Adet = (int)nudAdet.Value,
                };
                _siparis.SiparisDetaylar.Add(sd);
            }
            BilgileriGuncelle();
        }

        private void btnAnaSayfayaDon_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOde_Click(object sender, EventArgs e)
        {
            SiparisiKapat(_siparis.ToplamTutar(), SiparisDurum.Odendi);
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            SiparisiKapat(0, SiparisDurum.Iptal);
        }
        private void SiparisiKapat(decimal odenenTutar, SiparisDurum yeniDurum)
        {
            _siparis.KapanisZamani = DateTime.Now;
            _siparis.OdenenTutar = odenenTutar;
            _siparis.Durum = yeniDurum;
            _db.AktifSiparisler.Remove(_siparis);
            _db.GecmisSiparisler.Add(_siparis);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbMasaNo.SelectedIndex < 0) return;

            int eskiMasaNo= _siparis.MasaNo;
            int hedefNo = (int)cmbMasaNo.SelectedItem;
            _siparis.MasaNo = hedefNo;

            BilgileriGuncelle();
            if (MasaTasindi != null)
                MasaTasindi(eskiMasaNo, hedefNo);
        }

        private void dgvSiparisDetaylar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        
    }
}
