using System;
using System.Collections.Generic;
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
            Forest randomForest;
            using (XmlReader reader = XmlReader.Create("regression_rf.pmml"))
            {
                randomForest = new Forest(reader);
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
                    var cSharpPrediction = randomForest.Predict(item);

                    Console.WriteLine($"Processing line {lineNumber++} - R prediction: {rPrediction} - C#: {cSharpPrediction}");

                    var differencePercentage = Math.Abs((rPrediction - cSharpPrediction) / rPrediction);
                    if(differencePercentage >= 0.00001)
                    {
                        Console.WriteLine("   OOOOOPSSSSS");
                    }
                }
            }
        }
    }
}
