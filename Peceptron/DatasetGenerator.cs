using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Peceptron
{
    public class DatasetGenerator
    {
        private string _directoryPath = String.Empty;
        private Random _random = new Random();
        public DatasetGenerator(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new FileNotFoundException(nameof(directoryPath), "Saving path is not exist!!");
            _directoryPath = directoryPath;
        }
        /// <summary>
        /// Создает набор квадратных изображений со случайно размещенным текстом на них
        /// </summary>
        /// <param name="drawnSymbol">Рисуемый символ</param>
        /// <param name="datasetSize">Число сгенерированных изображений</param>
        /// <param name="imageSize">Размер изображения</param>
        /// <param name="rotationAngle">Угол поворота текста</param>
        /// <param name="xCorrection">Смещение по x (отрицательное подвинет знак вверх), может понадобиться для символов в строчном наборе</param>
        /// <param name="yCorrection">Смещение по x (отрицательное подвинет знак вверх), может понадобиться для символов в строчном наборе</param>
        /// <param name="borderOffset">Отступ от границы изображения в процентах</param>
        /// <param name="fontFamilies">Используемые для генерации семейства шрифтов</param>
        public void CreateSymbol(string drawnSymbol, int datasetSize, int imageSize, int rotationAngle = 0, double borderOffset = 0.05, double xCorrection = 0.0, double yCorrection = 0.0, string[] fontFamilies = null)
        {
            fontFamilies = fontFamilies ?? FontFamilies;
            string symbolPath = _directoryPath + @"\" + drawnSymbol;
            Directory.CreateDirectory(symbolPath);
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 1L);
            //var fonts = typeof(Fonts).GetEnumValues();

            for (int i = 0; i < datasetSize; i++)
            {
                Bitmap image = new(imageSize, imageSize);

                Graphics g = Graphics.FromImage(image);
                g.FillRectangle(System.Drawing.Brushes.White, 0, 0, imageSize, imageSize);
                var fontFamily = fontFamilies[_random.Next(fontFamilies.Length)];
                var fontSize = (float)(_random.Next((int)(imageSize * (1 - 2 * borderOffset) - 12)) + 10);
                var fontRotate = _random.Next(2 * rotationAngle + 1) - rotationAngle;
                var xOffset = ((1 - 2 * borderOffset) * imageSize - fontSize - fontSize * xCorrection) < 0? 0 : _random.Next((int)((1 - 2 * borderOffset) * imageSize - fontSize - fontSize * xCorrection));
                var yOffset = ((1 - 2 * borderOffset) * imageSize - fontSize - fontSize * yCorrection) < 0? 0 : _random.Next((int)((1 - 2 * borderOffset) * imageSize - fontSize - fontSize * yCorrection));
                g.RotateTransform(fontRotate);
                g.DrawString(
                    drawnSymbol,
                    new Font(fontFamily, fontSize, GraphicsUnit.Pixel),
                    System.Drawing.Brushes.Black,
                    (float)(imageSize * borderOffset + fontSize * xCorrection + xOffset),
                    (float)(imageSize * borderOffset + fontSize * yCorrection + yOffset)
                    );

                image.Save(symbolPath + @"\" + $"{i:D5}" + ".png", GetEncoder(ImageFormat.Png), encoderParameters);
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private static string[] FontFamilies = new string[]
        {
            "Arial",
            "Calibri",
            "Cambria",
            "Candara",
            "Comic Sans MS",
            "Consolas",
            "Courier New",
            "Impact",
            "Segoe UI",
            "Tahoma",
            "Times New Roman",
            "Verdana"
        };
    }
}
