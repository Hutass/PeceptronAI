using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Point = System.Drawing.Point;

namespace Peceptron
{
    public class Perceptron
    {
        private double _trainingWeightScale;
        private double _tetaThreshold;
        private int _dimension;
        private Random _random;

        private double[,] Receptor;
        private double[,] Acceptor;

        private InterlayerConnection Connection;

        private string[] _dataCodes;
        private string[] _dataNames;
        private int _codeWidth;
        public Perceptron(int dimension, double tetaThreshold, double trainingWeightScale)
        {
            _dimension = dimension;
            _tetaThreshold = tetaThreshold;
            _trainingWeightScale = trainingWeightScale;
            _random = new Random();
            Connection = new InterlayerConnection(dimension);
            Receptor = new double[dimension, dimension];
            Acceptor = new double[dimension, dimension];
        }

        public void Init(int numberOfConnections, int connectionsLowerBound = 1, bool isNulled = true)
        {
            GenerateConnections(numberOfConnections, connectionsLowerBound);
            GenerateWeights(isNulled);          
        }
        private void GenerateConnections(int numberOfConnections, int lowerBound = 1)
        {

            for (int X = 0; X < _dimension; X++)
            {
                for (int Y = 0; Y < _dimension; Y++)
                {
                    Connection.Bound[X, Y] = new List<Point>();
                    for (int i = 0; i < _random.Next(lowerBound, numberOfConnections); i++)
                    {
                        var point = new Point(_random.Next(_dimension), _random.Next(_dimension));
                        while (Connection.Bound[X, Y].Contains(point))
                        {
                            point = new System.Drawing.Point(_random.Next(_dimension), _random.Next(_dimension));
                        }
                        Connection.Bound[X, Y].Add(point);
                    }
                }
            }
        }
        private void GenerateWeights(bool isNuled = true)
        {
            for (int X = 0; X < _dimension; X++)
            {
                for (int Y = 0; Y < _dimension; Y++)
                {
                    Connection.Weight[X, Y] = new List<double>();
                    for (int code = 0; code < _codeWidth; code++)
                    {
                        var weight = isNuled ? 0 : _random.NextDouble() - 0.5;
                        Connection.Weight[X, Y].Add(weight);
                    }
                }
            }
        }

        private void Percept(Bitmap bitmap)
        {
            for(int X = 0; X < _dimension;X++)
            {
                for(int  Y = 0; Y < _dimension;Y++)
                {
                    Receptor[X,Y] = 1 - Math.Pow(bitmap.GetPixel(X,Y).GetBrightness(), 3);
                }
            }
        }

        private void Effect()
        {
            Acceptor = new double[_dimension, _dimension];
            for (int X = 0; X < _dimension; X++)
            {
                for (int Y = 0; Y < _dimension; Y++)
                {
                    if (Receptor[X, Y] == 0)
                        continue;
                    for(int i=0; i < Connection.Bound[X, Y].Count; i++)
                    {
                        Acceptor[Connection.Bound[X, Y][i].X, Connection.Bound[X, Y][i].Y] += Receptor[X, Y];
                    }
                }
            }
            for (int X = 0; X < _dimension; X++)
            {
                for (int Y = 0; Y < _dimension; Y++)
                {
                    Acceptor[X, Y] = Acceptor[X, Y] > _tetaThreshold? 1 : 0;
                }
            }
        }

        private string Recognize()
        {
            double[] E = new double[_codeWidth];
            Array.Fill<double>(E, 0);
            for (int i = 0; i < _codeWidth; i++)
            {
                for (int X = 0; X < _dimension; X++)
                {
                    for (int Y = 0; Y < _dimension; Y++)
                    {
                        E[i] += Acceptor[X, Y] * Connection.Weight[X, Y][i];
                    }
                }
            }
            var newString = new StringBuilder("".PadLeft(_codeWidth, '0'));          
            for (int i = 0; i < _codeWidth; i++)
            {
                newString[i] = E[i] > 0 ? '1' : '0';
            }
            return newString.ToString().PadLeft(_codeWidth, '0');
        }

        private void Correct(string actualCode, string expectedCode)
        {
            for (int i = 0; i < _codeWidth; i++)
            {
                if (actualCode[i] != expectedCode[i])
                {
                    for (int X = 0; X < _dimension; X++)
                    {
                        for (int Y = 0; Y < _dimension; Y++)
                        {
                            if (Acceptor[X, Y] > 0)
                            {
                                Connection.Weight[X, Y][i] = expectedCode[i] == '0' ? Connection.Weight[X, Y][i] - _trainingWeightScale : Connection.Weight[X, Y][i] + _trainingWeightScale;

                            }
                        }
                    }
                }
            }
        }

        public void Train(string directoryPath, int maxSteps, bool isDoubleCoded, int numberOfConnections, int connectionsLowerBound = 1, bool isNulled = true, double minAccuracy = 1, int debug = 0)
        {
            if (!Directory.Exists(directoryPath))
                throw new FileNotFoundException(nameof(directoryPath), "Путь к директории указан неверно!!");

            var dataTypeCount = Directory.GetDirectories(directoryPath).Length;
            _codeWidth = isDoubleCoded ? (int)Math.Ceiling(Math.Log2(dataTypeCount)) : dataTypeCount;
            var directoryNames = Directory.GetDirectories(directoryPath);
            var directoryPointer = new int[dataTypeCount];
            var directorySize = new int[dataTypeCount];
            for (int i = 0; i < dataTypeCount; i++)
            {
                directorySize[i] = Directory.GetFiles(directoryNames[i]).Length;
            }
            var dataTypeRepeat = new int[dataTypeCount];
            for (int i = 0; i < dataTypeCount; i++)
            {
                dataTypeRepeat[i] = Convert.ToInt32(directoryNames[i].Split('_',StringSplitOptions.None).Last());
            }
            string[] dataCodes = new string[dataTypeCount];
            for (int i = 0; i < dataTypeCount; i++)
            {
                if (isDoubleCoded)
                {
                    dataCodes[i] = Convert.ToString(i, 2).PadLeft(_codeWidth, '0');
                }
                else
                {
                    var newString = new StringBuilder("".PadLeft(_codeWidth, '0'));
                    newString[i] = '1';
                    dataCodes[i] = newString.ToString().PadLeft(_codeWidth, '0');
                }
            }
            var dataNames = new string[dataTypeCount];
            for (int i = 0; i < dataTypeCount; i++)
            {
                var splitedPath = directoryNames[i].Split('_', StringSplitOptions.None);
                dataNames[i] = splitedPath[splitedPath.Length - 3];
            }
            _dataCodes = dataCodes;
            _dataNames = dataNames;

            if (Connection.Bound[0,0] == null)
                Init(numberOfConnections, connectionsLowerBound, isNulled);

            bool isTrained = false;
            int step = 0;
            int currentDataType = 0;
            while (!isTrained)
            {
                if (directoryPointer[currentDataType] == 0)
                    directoryPointer[currentDataType] = 1;
                for (int i = 0; i < dataTypeRepeat[currentDataType]; i++)
                {
                    Percept(new Bitmap(directoryNames[currentDataType] + "\\" + directoryPointer[currentDataType].ToString().PadLeft(5, '0') + ".png"));
                    Effect();
                    Correct(Recognize(), dataCodes[currentDataType]);
                    step++;

                    if(step % debug == 1 && step > 1)
                    {
                        double totalAccuracy = 0;
                        Debug.WriteLine("");
                        foreach(var file in directoryNames)
                        {
                            var fileAcuracy = CheckAccuracy(file);
                            Debug.WriteLine(fileAcuracy.ToString() + "   " + step + "   " + file);
                            totalAccuracy += fileAcuracy;
                        }
                        Debug.WriteLine(totalAccuracy / dataTypeCount + "   Total");
                        Debug.WriteLine("");
                    }
                }
                directoryPointer[currentDataType] = (directoryPointer[currentDataType] + 1) % directorySize[currentDataType];

                currentDataType = (currentDataType + 1) % dataTypeCount;
                isTrained = step >= maxSteps? true : false;
            }

            
        }

        public string RecognizeImage(string imagePath)
        {
            Percept(new Bitmap(imagePath));
            Effect();
            var code = Recognize();
            if (_dataCodes.Contains(code))
                return _dataNames[Array.IndexOf(_dataCodes, code)];
            else
                return "";
        }

        public double CheckAccuracy(string directoryPath)
        {
            var fileNames = Directory.GetFiles(directoryPath);           
            var splitedPath = directoryPath.Split('_', StringSplitOptions.None);
            var directoryName = directoryPath.Contains("_") ? splitedPath[splitedPath.Length - 3] : splitedPath.Last();

            var correctGuess = 0;
            foreach (var file in fileNames)
            {
                if(RecognizeImage(file).Split('\\').Last() == directoryName.Split('\\').Last())
                    correctGuess++;
            }

            return (double)correctGuess / fileNames.Length;
        }
    }



    public class InterlayerConnection
    {
        public List<Point>[,] Bound {  get; set; }
        public List<double>[,] Weight { get; set; }

        public InterlayerConnection (int dimention)
        {
            Bound = new List<System.Drawing.Point>[dimention,dimention];
            Weight = new List<double>[dimention,dimention];
        }
    }
}
