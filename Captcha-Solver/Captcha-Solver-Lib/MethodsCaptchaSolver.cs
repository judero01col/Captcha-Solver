using System;
using System.Drawing;
using Tesseract;

namespace CaptchaSolverLib
{
    public class MethodsCaptchaSolver
    {
        public static string OCR(Bitmap b)
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
                //MessageBox.Show($"Erro: {ex.Message}");
                return null;
            }
        }
    }
}
