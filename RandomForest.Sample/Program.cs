using System;
using System.Xml;
using System.IO;
using CsvHelper;
using System.Linq;

namespace RandomForest.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            RegressionRandomForest regressionRF;
            using (XmlReader reader = XmlReader.Create("regression_rf.pmml"))
            {
                regressionRF = new RegressionRandomForest(reader);
            }

            // we have the random forest! now let's build a dictionary to compare the predictions in R with the ones here
            using (TextReader reader = File.OpenText("regression_rf_prediction.csv"))
            {
                // the whole point of this is to build a dictionary with the keys and the values from the csv
                var csv = new CsvParser(reader);
                string[] headers = csv.Read();
                int lineNumber = 1;
                while (true)
                {
                    var row = csv.Read();
                    if(row == null)
                    {
                        break;
                    }

                    var item = headers.Zip(row, (key, value) => new { key, value }).ToDictionary(e => e.key, e => e.value);

                    var rPrediction = double.Parse(item["PredictedG3"]);
                    var cSharpPrediction = regressionRF.Predict(item);

                    Console.WriteLine($"Processing line {lineNumber++} - R prediction: {rPrediction} - C#: {cSharpPrediction}");

                    var differencePercentage = Math.Abs((rPrediction - cSharpPrediction) / rPrediction);
                    if(differencePercentage >= 0.00001)
                    {
                        Console.WriteLine("   OOOOOPSSSSS");
                    }
                }
            }

            ClassificationRandomForest classificationRF;
            using (XmlReader reader = XmlReader.Create("classification_rf.pmml"))
            {
                classificationRF = new ClassificationRandomForest(reader);
            }

            using (TextReader reader = File.OpenText("classification_rf_prediction.csv"))
            {
                var csv = new CsvParser(reader);
                string[] headers = csv.Read();
                int lineNumber = 1;
                while (true)
                {
                    var row = csv.Read();
                    if (row == null)
                    {
                        break;
                    }

                    var item = headers.Zip(row, (key, value) => new { key, value }).ToDictionary(e => e.key, e => e.value);

                    var rPrediction = item["PredictedOccupancy"];
                    var cSharpPrediction = classificationRF.Predict(item);

                    Console.WriteLine($"Processing line {lineNumber++} - R prediction: {rPrediction} - C#: {cSharpPrediction}");

                    if (rPrediction != cSharpPrediction)
                    {
                        Console.WriteLine("   OOOOOPSSSSS");
                    }
                }
            }

            using (TextReader reader = File.OpenText("classification_rf_probabilities.csv"))
            {
                var csv = new CsvParser(reader);
                string[] headers = csv.Read();
                int lineNumber = 1;
                while (true)
                {
                    var row = csv.Read();
                    if (row == null)
                    {
                        break;
                    }

                    var item = headers.Zip(row, (key, value) => new { key, value }).ToDictionary(e => e.key, e => e.value);

                    var rYes = double.Parse(item["yes"]);
                    var rNo = double.Parse(item["no"]);
                    var cSharpProbabilities = classificationRF.GetProbabilities(item);

                    Console.WriteLine($"Processing line {lineNumber++} - p(yes) R: {rYes} - C#: {cSharpProbabilities["yes"]} - p(no) R: {rNo} - C#: {cSharpProbabilities["no"]}");

                    if (cSharpProbabilities["yes"] != rYes || cSharpProbabilities["no"] != rNo)
                    {
                        Console.WriteLine("        OOOOOOPS");
                    }

                    Console.WriteLine($"Processing line {lineNumber++} - C#: {string.Join(", ", cSharpProbabilities.Select(e => $"{e.Key}: {e.Value}"))}");
                }
            }
        }
    }
}
