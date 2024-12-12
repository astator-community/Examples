using System;
using System.IO;
using System.Threading.Tasks;
using Astator.Graphics;
using Astator.Script;
using Astator.ThirdParty.RapidOCR;

namespace ocr文字识别
{
    public class Program
    {
        [EntryMethod]
        public static async Task MainAsync(string workDir)
        {
            try
            {
                var detPath = Path.Combine(workDir, "assets", "ch_PP-OCRv4_det_infer.onnx");
                var clsPath = Path.Combine(workDir, "assets", "ch_ppocr_mobile_v2.0_cls_train.onnx");
                var recPath = Path.Combine(workDir, "assets", "ch_PP-OCRv4_rec_infer.onnx");
                var keysPath = Path.Combine(workDir, "assets", "keys.txt");
                var testPngPath = Path.Combine(workDir, "assets", "test.png");

                var ocr = new RapidOCRHelper(detPath, clsPath, recPath, keysPath);

                var testImg = WrapImage.CreateFromFile(testPngPath);
                var result = ocr.Detect(testImg);

                Console.WriteLine(result.ToString());

               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

}
