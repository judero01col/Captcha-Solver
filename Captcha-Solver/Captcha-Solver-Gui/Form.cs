using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Tesseract;

namespace ControleCaptcha
{
    public partial class Form : System.Windows.Forms.Form
    {
        private List<string> mudancas = new List<string>();
        private Bitmap original = null;
        private Bitmap anterior = null;
        private Bitmap atual = null;
        private bool voltar = false;
        private string fileName = string.Empty;

        public Form()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap($"{Environment.CurrentDirectory}\\img\\informacao.bmp");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                Bitmap image = new Bitmap(openFileDialog1.FileName);
                if (image != null)
                {
                    ZerarMudanca();
                    System.IO.FileInfo fi = new System.IO.FileInfo(openFileDialog1.FileName);
                    fileName = fi.FullName;
                    original = new Bitmap(openFileDialog1.FileName);
                    anterior = CopyBitmap(original);
                    atual = CopyBitmap(original);
                    resultado.Text = string.Empty;
                    gbFiltros.Enabled = true;
                    gbTratamentos.Enabled = true;
                }
                pictureBox1.Image = image;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        public void InserirMudanca(string mudanca)
        {
            try
            {
                if (atual == null)
                    return;
                voltar = true;
                mudancas.Add(mudanca);
                richTextBox1.Text = string.Empty;
                mudancas.ForEach(c => richTextBox1.Text += c + "\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        public void RemoverMudanca()
        {
            try
            {
                if (voltar)
                {
                    mudancas.RemoveAt(mudancas.Count - 1);
                    richTextBox1.Text = string.Empty;
                    if (mudancas.Count > 0) mudancas.ForEach(c => richTextBox1.Text += c + "\n");
                    voltar = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        public void ZerarMudanca()
        {
            try
            {
                if (voltar || mudancas.Count > 0)
                {
                    mudancas.Clear();
                    richTextBox1.Text = string.Empty;
                    voltar = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private string OCR(Bitmap b)
        {
            try
            {
                string res = string.Empty;
                string path = $@"{Environment.CurrentDirectory}\tessdata\";
                using (var engine = new TesseractEngine(path, "eng"))
                {
                    string letters = "abcdefghijklmnopqrstuvwxyz";
                    string numbers = "0123456789";
                    engine.SetVariable("tessedit_char_whitelist", $"{numbers}{letters}{letters.ToUpper()}");
                    engine.SetVariable("tessedit_unrej_any_wd", true);
                    engine.SetVariable("tessedit_adapt_to_char_fragments", true);
                    engine.SetVariable("tessedit_redo_xheight", true);
                    engine.SetVariable("chop_enable", true);
                    Bitmap x = b.Clone(new Rectangle(0, 0, b.Width, b.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    using (var page = engine.Process(x, PageSegMode.SingleLine))
                        res = page.GetText().Replace(" ", "").Trim();
                }
                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
                return null;
            }
        }

        #region Filtros

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Blur");
                var filter = new Blur();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("BrightnessCorrection");
                var filter = new BrightnessCorrection();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("ContrastCorrection");
                var filter = new ContrastCorrection();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("SaturationCorrection");
                var filter = new SaturationCorrection();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Closing");
                var filter = new Closing();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Dilatation");
                var filter = new Dilatation();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Erosion");
                var filter = new Erosion();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("BlobsFiltering");
                var filter = new BlobsFiltering();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("GaussianSharpen");
                var filter = new GaussianSharpen();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Invert");
                var filter = new Invert();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Opening");
                var filter = new Opening();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Sharpen");
                var filter = new Sharpen();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Median");
                var filter = new Median();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        #endregion Filtros

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (atual != null && anterior != null)
                {
                    atual = CopyBitmap(anterior);
                    pictureBox1.Image = atual;
                    RemoverMudanca();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (atual != null && original != null)
                {
                    atual = CopyBitmap(original);
                    pictureBox1.Image = atual;
                    ZerarMudanca();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (atual != null)
                    resultado.Text = OCR(atual);
                else
                    MessageBox.Show("Não existe captcha sendo exibido.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        protected Bitmap CopyBitmap(Bitmap source)
        {
            return new Bitmap(source);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Shrink");
                var filter = new Shrink();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("TirarBorda");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.TirarBorda(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Tirar Pixel Branco Só");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.PintarBrancoEntrePretos(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Colorido para Branco");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.PintarPixelPretoDeBranco(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("EngrossarLinhas");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.engrossarLinhaPreto(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Tirar Pixel Sozinho");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.TirarPixelSozinho(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button33_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Remover Cores");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.RetirarTodasAsCoresEColocarNaImagemFinal(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button35_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("TirarPretoSozinhoHorizontal");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.TirarPixelPretoSozinhoHorizontal(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button34_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("TirarPretoSozinhoVertical");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.TirarPixelPretoSozinhoVertical(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Tirar2pxPretos");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.TirarPixelPretoEntreBranco2Horizontal(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button36_Click_1(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Tirar3pxPretos");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.TirarPixelPretoEntreBranco3Horizontal(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void LimpezaReceita()
        {
            try
            {
                int[,] kernel = {
            { 2,-1, 2 },
            {-1,10,-1 },
            { 2,-1, 2 }
            };
                var convolution = new Convolution(kernel, 2);
                var inverter = new Invert();
                var closing = new Closing();
                atual = closing.Apply(atual);
                atual = convolution.Apply(atual);
                atual = Limpeza.TirarBorda(atual);
                atual = Limpeza.RetirarTodasAsCoresEColocarNaImagemFinal(atual);
                atual = Limpeza.engrossarLinhaPreto(atual);
                atual = Limpeza.FA_TirarPixelPretoSozinhoNaHorizontal(atual);
                atual = Limpeza.FA_TirarPixelPretoSozinhoNaVertical(atual);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private static Bitmap FA_Tirar2PixelsPretosEntreBrancos(Bitmap Imagem)
        {
            try
            {
                bool boolean00 = false;
                bool boolean01 = false;
                bool boolean02 = false;
                bool boolean03 = false;
                bool boolean04 = false;
                bool boolean10 = false;
                bool boolean11 = false;
                bool boolean12 = false;
                bool boolean13 = false;
                bool boolean14 = false;
                bool boolean20 = false;
                bool boolean21 = false;
                bool boolean22 = false;
                bool boolean23 = false;
                bool boolean24 = false;
                bool boolean30 = false;
                bool boolean31 = false;
                bool boolean32 = false;
                bool boolean33 = false;
                bool boolean34 = false;
                bool boolean40 = false;
                bool boolean41 = false;
                bool boolean42 = false;
                bool boolean43 = false;
                bool boolean44 = false;

                Color C00 = default(Color);
                Color C01 = default(Color);
                Color C02 = default(Color);
                Color C03 = default(Color);
                Color C04 = default(Color);
                Color C10 = default(Color);
                Color C11 = default(Color);
                Color C12 = default(Color);
                Color C13 = default(Color);
                Color C14 = default(Color);
                Color C20 = default(Color);
                Color C21 = default(Color);
                Color C22 = default(Color);
                Color C23 = default(Color);
                Color C24 = default(Color);
                Color C30 = default(Color);
                Color C31 = default(Color);
                Color C32 = default(Color);
                Color C33 = default(Color);
                Color C34 = default(Color);
                Color C40 = default(Color);
                Color C41 = default(Color);
                Color C42 = default(Color);
                Color C43 = default(Color);
                Color C44 = default(Color);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        boolean00 = false;
                        boolean01 = false;
                        boolean02 = false;
                        boolean03 = false;
                        boolean04 = false;
                        boolean10 = false;
                        boolean11 = false;
                        boolean12 = false;
                        boolean13 = false;
                        boolean14 = false;
                        boolean20 = false;
                        boolean21 = false;
                        boolean22 = false;
                        boolean23 = false;
                        boolean24 = false;
                        boolean30 = false;
                        boolean31 = false;
                        boolean32 = false;
                        boolean33 = false;
                        boolean34 = false;
                        boolean40 = false;
                        boolean41 = false;
                        boolean42 = false;
                        boolean43 = false;
                        boolean44 = false;

                        //Inicio da validação para verificar se o pixel existe ou não( se a posição é valida na matriz)
                        //Legenda : Se der FALSE é por que o pixel existe, TRUE não existe
                        //@author Abnoan Muniz 19/11/2013
                        //bloco 1
                        if (X + 1 > Imagem.Width - 1)
                        {
                            boolean03 = true;
                            boolean13 = true;
                            boolean23 = true;
                            boolean33 = true;
                            boolean43 = true;
                        }
                        if (X + 2 > Imagem.Width - 1)
                        {
                            boolean04 = true;
                            boolean14 = true;
                            boolean24 = true;
                            boolean34 = true;
                            boolean44 = true;
                        }

                        //bloco 2
                        if (Y + 1 > Imagem.Height - 1)
                        {
                            boolean30 = true;
                            boolean31 = true;
                            boolean32 = true;
                            boolean33 = true;
                            boolean34 = true;
                        }
                        if (Y + 2 > Imagem.Height - 1)
                        {
                            boolean40 = true;
                            boolean41 = true;
                            boolean42 = true;
                            boolean43 = true;
                            boolean44 = true;
                        }

                        //bloco 3
                        if (X - 1 < 0)
                        {
                            boolean01 = true;
                            boolean11 = true;
                            boolean21 = true;
                            boolean31 = true;
                            boolean41 = true;
                        }
                        if (X - 2 < 0)
                        {
                            boolean00 = true;
                            boolean10 = true;
                            boolean20 = true;
                            boolean30 = true;
                            boolean40 = true;
                        }
                        //bloco 4
                        if (Y - 1 < 0)
                        {
                            boolean10 = true;
                            boolean11 = true;
                            boolean12 = true;
                            boolean13 = true;
                            boolean14 = true;
                        }
                        if (Y - 2 < 0)
                        {
                            boolean00 = true;
                            boolean01 = true;
                            boolean02 = true;
                            boolean03 = true;
                            boolean04 = true;
                        }

                        //So pegar o valor do pixel se for FALSE ou seja se o pixel existir na matriz
                        //      --------------------------------->
                        //Ordem 22,23,24,34,44,43,42,41,40,30,31,32,33,20,21,10,11,12,13,14,00,01,02,03,04
                        if (boolean22 == false)
                        {
                            C22 = Imagem.GetPixel(X, Y);
                        }
                        if (boolean23 == false)
                        {
                            C23 = Imagem.GetPixel(X + 1, Y);
                        }
                        if (boolean24 == false)
                        {
                            C24 = Imagem.GetPixel(X + 2, Y);
                        }
                        if (boolean34 == false)
                        {
                            C34 = Imagem.GetPixel(X + 2, Y + 1);
                        }
                        if (boolean44 == false)
                        {
                            C44 = Imagem.GetPixel(X + 2, Y + 2);
                        }
                        if (boolean43 == false)
                        {
                            C43 = Imagem.GetPixel(X + 1, Y + 2);
                        }
                        if (boolean33 == false)
                        {
                            C33 = Imagem.GetPixel(X + 1, Y + 1);
                        }
                        if (boolean32 == false)
                        {
                            C32 = Imagem.GetPixel(X, Y + 1);
                        }
                        if (boolean42 == false)
                        {
                            C42 = Imagem.GetPixel(X, Y + 2);
                        }
                        if (boolean41 == false)
                        {
                            C41 = Imagem.GetPixel(X - 1, Y + 2);
                        }
                        if (boolean31 == false)
                        {
                            C31 = Imagem.GetPixel(X - 1, Y + 1);
                        }
                        if (boolean40 == false)
                        {
                            C40 = Imagem.GetPixel(X - 2, Y + 2);
                        }
                        if (boolean30 == false)
                        {
                            C30 = Imagem.GetPixel(X - 2, Y + 1);
                        }
                        if (boolean21 == false)
                        {
                            C21 = Imagem.GetPixel(X - 1, Y);
                        }
                        if (boolean20 == false)
                        {
                            C20 = Imagem.GetPixel(X - 2, Y);
                        }
                        if (boolean10 == false)
                        {
                            C10 = Imagem.GetPixel(X - 2, Y - 1);
                        }
                        if (boolean11 == false)
                        {
                            C11 = Imagem.GetPixel(X - 1, Y - 1);
                        }
                        if (boolean12 == false)
                        {
                            C12 = Imagem.GetPixel(X, Y - 1);
                        }
                        if (boolean13 == false)
                        {
                            C13 = Imagem.GetPixel(X + 1, Y - 1);
                        }
                        if (boolean14 == false)
                        {
                            C14 = Imagem.GetPixel(X + 2, Y - 1);
                        }
                        if (boolean00 == false)
                        {
                            C00 = Imagem.GetPixel(X - 2, Y - 2);
                        }
                        if (boolean01 == false)
                        {
                            C01 = Imagem.GetPixel(X - 1, Y - 2);
                        }
                        if (boolean02 == false)
                        {
                            C02 = Imagem.GetPixel(X, Y - 2);
                        }
                        if (boolean03 == false)
                        {
                            C03 = Imagem.GetPixel(X + 1, Y - 2);
                        }
                        if (boolean04 == false)
                        {
                            C04 = Imagem.GetPixel(X + 2, Y - 2);
                        }

                        if ((boolean12 == false & boolean13 == false & boolean22 == false & boolean23 == false & boolean32 == false & boolean33 == false & boolean24 == false & boolean21 == false))
                        {
                            if ((((C22.R == 0 & C22.G == 0 & C22.G == 0) & (C23.R == 0 & C23.G == 0 & C23.G == 0) & (C24.R == 255 & C24.G == 255 & C24.B == 255) & (C21.R == 255 & C21.G == 255 & C21.G == 255)) & (((C12.R == 255 & C12.G == 255 & C12.G == 255) & (C13.R == 255 & C13.G == 255 & C13.G == 255)) | ((C32.R == 255 & C32.G == 255 & C32.G == 255) & (C33.R == 255 & C33.G == 255 & C33.G == 255)))))
                            {
                                Imagem.SetPixel(X, Y, Color.FromArgb(C22.A, 255, 255, 255));
                                Imagem.SetPixel(X + 1, Y, Color.FromArgb(C23.A, 255, 255, 255));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
            return Imagem;
        }

        private static Bitmap FA_TirarPixelPretoSozinhoNaHorizontal(Bitmap Imagem)
        {
            try
            {
                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        Color c2 = default(Color);
                        Color c1 = default(Color);
                        Color c3 = default(Color);
                        Color c4 = default(Color);
                        if (X + 1 <= Imagem.Width - 1)
                        {
                            c2 = Imagem.GetPixel(X + 1, Y);
                        }
                        if (X - 1 >= 0)
                        {
                            c1 = Imagem.GetPixel(X - 1, Y);
                        }
                        if (Y + 1 <= Imagem.Height - 1)
                        {
                            c3 = Imagem.GetPixel(X, Y + 1);
                        }
                        if (Y - 1 >= 0)
                        {
                            c4 = Imagem.GetPixel(X, Y - 1);
                        }
                        if ((((c1.R == 255 & c1.G == 255 & c1.B == 255) & (c2.R == 255 & c2.G == 255 & c2.B == 255) & (C.R == 0 & C.G == 0 & C.B == 0)) & ((c3.R == 255 & c3.G == 255 & c3.B == 255) | (c4.R == 255 & c4.G == 255 & c4.B == 255))))
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
            return Imagem;
        }

        private static Bitmap FA_TirarPixelPretoSozinhoNaVertical(Bitmap Imagem)
        {
            try
            {
                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        Color c2 = default(Color);
                        Color c1 = default(Color);
                        Color c3 = default(Color);
                        Color c4 = default(Color);
                        if (Y + 1 <= Imagem.Height - 1)
                        {
                            c2 = Imagem.GetPixel(X, Y + 1);
                        }
                        if (Y - 1 >= 0)
                        {
                            c1 = Imagem.GetPixel(X, Y - 1);
                        }
                        if (X + 1 <= Imagem.Width - 1)
                        {
                            c3 = Imagem.GetPixel(X + 1, Y);
                        }
                        if (X - 1 >= 0)
                        {
                            c4 = Imagem.GetPixel(X - 1, Y);
                        }

                        if ((((c1.R == 255 & c1.G == 255 & c1.B == 255) & (c2.R == 255 & c2.G == 255 & c2.B == 255) & (C.R == 0 & C.G == 0 & C.B == 0)) & ((c3.R == 255 & c3.G == 255 & c3.B == 255) | (c4.R == 255 & c4.G == 255 & c4.B == 255))))
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
            return Imagem;
        }

        private void button44_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("tirar 2");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = FA_Tirar2PixelsPretosEntreBrancos(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button45_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Tirar Pixel Só");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = FA_TirarPixelPretoSozinhoNaHorizontal(atual);
                atual = FA_TirarPixelPretoSozinhoNaVertical(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button46_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("TirarBorda - Baixa");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.TirarBordaBaixa(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button47_Click(object sender, EventArgs e)
        {
            try
            {
                Limpeza.CorrigirTransparencia(atual, fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button49_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Median2");
                var filter = new Median(2);
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button50_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Mean");
                var filter = new Mean();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button51_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("BilateralSmoothing");
                var filter = new BilateralSmoothing();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button52_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Convolution");
                int[,] kernel = {
            { 2,-1, 2 },
            {-1,10,-1 },
            { 2,-1, 2 }
            };
                int[,] kernel2 =
                {
                {0 ,0 ,0 ,0 ,0},
                {0 ,2 ,0 ,2 ,0},
                {0 ,-1, 2,-1,0},
                {0 ,2 ,-1,2 ,0},
                {0 ,0 ,-1,0 ,0}
            };
                var filter = new Convolution(kernel, 4);
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button53_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Retirar Bola");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.RetirarBolinha(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button54_Click(object sender, EventArgs e)
        {
            try
            {
                var bright = new BrightnessCorrection();
                var contrast = new ContrastCorrection();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                for (int j = 0; j < 4; j++)
                {
                    atual = bright.Apply(atual);
                    for (int i = 0; i < 5; i++)
                    {
                        atual = contrast.Apply(atual);
                    }
                }
                atual = contrast.Apply(atual);
                atual = Limpeza.TirarBordaTRF4(atual);
                atual = Limpeza.RetirarBolinha(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button55_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("TRT21");
                var filter = new Opening();
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.TirarBordaTRT21(atual);
                atual = Limpeza.PintarPixelPretoDeBranco(atual);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button58_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("diminuir");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                ResizeNearestNeighbor filter = new ResizeNearestNeighbor(atual.Width / 2, atual.Height / 2);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button57_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("aumentar");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                ResizeNearestNeighbor filter = new ResizeNearestNeighbor(atual.Width * 2, atual.Height * 2);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button61_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Convolution");
                int[,] kernel = {
            { 5,-1, 5 },
            {-1,20,-1 },
            { 5,-1, 5 }
            };
                var filter = new Convolution(kernel, 4);
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = filter.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button70_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("TirarBorda - RS");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.TirarBordaRS(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button74_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("TirarBorda - Cima");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.TirarBordaCima(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button75_Click(object sender, EventArgs e)
        {
            try
            {
                var erosion = new Erosion();
                var median = new Median();
                InserirMudanca("Pegar Azul");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.PegarAzul(atual);
                atual = Limpeza.PintarPixelPretoDeBrancoTRT17(atual);
                atual = Limpeza.PintarBrancoEntrePretos(atual);
                atual = Limpeza.TirarPixelSozinho(atual);
                atual = erosion.Apply(atual);
                atual = median.Apply(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button76_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Limpar 2px");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.Limpar2PX(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                InserirMudanca("Colorido para Branco 2");
                anterior = CopyBitmap(atual);
                atual = atual.Clone(new Rectangle(0, 0, atual.Width, atual.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                atual = Limpeza.PintarPixelPretoDeBranco2(atual);
                pictureBox1.Image = atual;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }
    }
}