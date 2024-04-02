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
                throw new FileNotFoundException(nameof(directoryPath), "Путь к директории указан неверно!!");
            _directoryPath = directoryPath;
        }
        /// <summary>
        /// Создает набор квадратных изображений со случайно размещенным текстом на них
        /// </summary>
        /// <param name="drawnSymbol">Рисуемый символ</param>
        /// <param name="datasetSize">Число сгенерированных изображений</param>
        /// <param name="imageSize">Размер изображения</param>
        /// <param name="minSizeInPixels">Минимальный размер символа в пикселях</param>
        /// <param name="rotationAngle">Угол поворота текста</param>
        /// <param name="xCorrection">Смещение по x (отрицательное подвинет знак вверх), может понадобиться для символов в строчном наборе</param>
        /// <param name="yCorrection">Смещение по x (отрицательное подвинет знак вверх), может понадобиться для символов в строчном наборе</param>
        /// <param name="fontSizeCorrection">Множитель размера шрифта</param>
        /// <param name="borderOffset">Отступ от границы изображения в процентах</param>
        /// <param name="fontFamilies">Используемые для генерации семейства шрифтов</param>
        public void CreateSymbol(string drawnSymbol, int datasetSize, int imageSize, int minSizeInPixels, int rotationAngle = 0, int repeats = 1, double borderOffset = 0.05, double xCorrection = 0.0, double yCorrection = 0.0, double fontSizeCorrection = 1, string[] fontFamilies = null)
        {
            try
            {
                fontFamilies = fontFamilies ?? FontFamilies;
                string symbolPath = _directoryPath + @"\" + drawnSymbol + "_" + ((byte)drawnSymbol[0]) + "_" + repeats;
                if(!Directory.Exists(symbolPath))
                    Directory.CreateDirectory(symbolPath);
                int lastFile = 0;
                if (Directory.GetFiles(symbolPath).Length > 0)
                    lastFile = Convert.ToInt32(Path.GetFileNameWithoutExtension(Directory.GetFiles(symbolPath)?.Last()));
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 1L);

                for (int i = 0; i < datasetSize; i++)
                {
                    Bitmap image = new(imageSize, imageSize);

                    Graphics g = Graphics.FromImage(image);
                    g.FillRectangle(System.Drawing.Brushes.White, 0, 0, imageSize, imageSize);
                    var fontFamily = fontFamilies[_random.Next(fontFamilies.Length)];
                    var fontSize = (float)(_random.Next((int)(imageSize * (1 - 2 * borderOffset) - minSizeInPixels)) + minSizeInPixels);
                    var fontRotate = _random.Next(2 * rotationAngle + 1) - rotationAngle;
                    var xOffset = ((1 - 2 * borderOffset) * imageSize - fontSize - fontSize * xCorrection) < 0 ? 0 : _random.Next((int)((1 - 2 * borderOffset) * imageSize - fontSize - fontSize * xCorrection));
                    var yOffset = ((1 - 2 * borderOffset) * imageSize - fontSize - fontSize * yCorrection) < 0 ? 0 : _random.Next((int)((1 - 2 * borderOffset) * imageSize - fontSize - fontSize * yCorrection));
                    g.RotateTransform(fontRotate);
                    g.DrawString(
                        drawnSymbol,
                        new Font(fontFamily, (int)(fontSize * fontSizeCorrection), GraphicsUnit.Pixel),
                        System.Drawing.Brushes.Black,
                        (float)(imageSize * borderOffset + fontSize * xCorrection + xOffset),
                        (float)(imageSize * borderOffset + fontSize * yCorrection + yOffset)
                        );

                    image.Save(symbolPath + @"\" + $"{(lastFile + i + 1):D5}.png", GetEncoder(ImageFormat.Png), encoderParameters);
                }

            }
            catch { Exception ex; }
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
