using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CafeSystem
{
    public partial class Form1 : Form
    {
        // Menyu və yeməklərin qiymətləri
        private Dictionary<string, double> menuPrices = new Dictionary<string, double>()
        {
            { "Tort", 5.0 },
            { "Kola", 2.0 },
            { "Meyvə Şirəsi", 3.0 },
            { "Burger", 6.5 },
            { "Sendviç", 4.0 },
            { "Pitsa", 8.0 },
            { "Keks", 2.5 },
            { "Xot-doq", 3.5 },
            { "Peçenye", 1.5 }
        };

        public Form1()
        {
            InitializeComponent();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            // Şəkillərə kliklədikdə səbətə əlavə olunması
            if (picTort != null) picTort.Click += (s, e) => AddToCart("Tort");
            if (picKola != null) picKola.Click += (s, e) => AddToCart("Kola");
            if (picŞirə != null) picŞirə.Click += (s, e) => AddToCart("Meyvə Şirəsi");
            if (picBurger != null) picBurger.Click += (s, e) => AddToCart("Burger");
            if (picSendviç != null) picSendviç.Click += (s, e) => AddToCart("Sendviç");
            if (picPitsa != null) picPitsa.Click += (s, e) => AddToCart("Pitsa");
            if (picKeks != null) picKeks.Click += (s, e) => AddToCart("Keks");
            if (picXotdoq != null) picXotdoq.Click += (s, e) => AddToCart("Xot-doq");
            if (picPeçenye != null) picPeçenye.Click += (s, e) => AddToCart("Peçenye");

            // Düymələrin hadisələri (Events)
            if (btnSəbətdənSil != null) btnSəbətdənSil.Click += BtnSəbətdənSil_Click;
            if (btnYenilə != null) btnYenilə.Click += BtnYenilə_Click;
            if (btnYekunHesab != null) btnYekunHesab.Click += BtnYekunHesab_Click;
            if (btnHesabla != null) btnHesabla.Click += BtnHesabla_Click;
            if (btnTəmizlə != null) btnTəmizlə.Click += BtnTəmizlə_Click;
        }

        // Səbətə yemək əlavə etmək üçün köməkçi metod
        private void AddToCart(string itemName)
        {
            lstSəbət.Items.Add(itemName);
        }

        // 1. SƏBƏTDƏN SİL DÜYMƏSİ
        private void BtnSəbətdənSil_Click(object sender, EventArgs e)
        {
            if (lstSəbət.SelectedItem != null)
            {
                string silinenYemek = lstSəbət.SelectedItem.ToString();
                lstSəbət.Items.Remove(lstSəbət.SelectedItem);

                // Tələb olunan bildiriş: "Yeməyin adı və səbətdən silindi"
                MessageBox.Show($"{silinenYemek} səbətdən silindi", "Bildiriş", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Zəhmət olmasa, səbətdən silmək üçün bir yemək seçin!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 2. YENİLƏ DÜYMƏSİ
        private void BtnYenilə_Click(object sender, EventArgs e)
        {
            // Tələb olunan bildiriş: "Xanalar sıfırlansınmı?"
            DialogResult result = MessageBox.Show("Xanalar sıfırlansınmı?", "Təsdiq", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // "Hə" seçilərsə bütun xanaları və səbəti sıfırlayır
                lstSəbət.Items.Clear();
                txtHesab.Text = "";
                txtMəbləğ.Text = "";
                txtQalıq.Text = "";
            }
        }

        // 3. YEKUN HESAB DÜYMƏSİ
        private void BtnYekunHesab_Click(object sender, EventArgs e)
        {
            // Səbətdə yemək yoxdursa
            if (lstSəbət.Items.Count == 0)
            {
                MessageBox.Show("Səbətdə yemək yoxdur!", "Bildiriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHesab.Text = "";
            }
            else
            {
                // Yemək olarsa ödənilməsi tələb olunan məbləği hesablayır
                double umumiMəbləğ = 0;
                foreach (var item in lstSəbət.Items)
                {
                    string yemekAdı = item.ToString();
                    if (menuPrices.ContainsKey(yemekAdı))
                    {
                        umumiMəbləğ += menuPrices[yemekAdı];
                    }
                }

                // "Hesab" xanasında göstərir
                txtHesab.Text = umumiMəbləğ.ToString("0.00");
            }
        }

        // 4. HESABLA DÜYMƏSİ
        private void BtnHesabla_Click(object sender, EventArgs e)
        {
            // Əvvəlcə hesab xanasının doluluğunu yoxlayırıq
            if (string.IsNullOrWhiteSpace(txtHesab.Text) || !double.TryParse(txtHesab.Text, out double hesab))
            {
                MessageBox.Show("Əvvəlcə 'Yekun hesab' düyməsinə basın!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Məbləğ xanasına yazılan pulu yoxlayırıq
            if (double.TryParse(txtMəbləğ.Text, out double daxilEdilənMəbləğ))
            {
                if (daxilEdilənMəbləğ < hesab)
                {
                    // Pul hesabdan azdırsa xəta bildirişi
                    MessageBox.Show("Daxil edilən məbləğ hesabdan azdır", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQalıq.Text = "";
                }
                else
                {
                    // Şərt ödənilərsə pulun üzərini "Qalıq" xanasında göstərir
                    double qalıq = daxilEdilənMəbləğ - hesab;
                    txtQalıq.Text = qalıq.ToString("0.00");
                }
            }
            else
            {
                MessageBox.Show("Zəhmət olmasa 'Məbləğ' xanasına düzgün pul ədədi yazın!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 5. TƏMİZLƏ DÜYMƏSİ
        private void BtnTəmizlə_Click(object sender, EventArgs e)
        {
            // Yalnız sol küncdə olan Məbləğ və Qalıq xanalarını təmizləyir
            txtMəbləğ.Text = "";
            txtQalıq.Text = "";
        }
    }
}