RandomForest.Sample
===
Ever wondered how to use [RandomForest](https://github.com/g3rv4/RandomForest) but were afraid to ask? this is for you! ([the code you want is here](https://github.com/g3rv4/RandomForest.Sample/blob/master/RandomForest.Sample/Program.cs))

I generated a random forest for regressions using [caret](http://caret.r-forge.r-project.org/) in [R](https://www.r-project.org/). You can see [the regression R code to generate and export the model here](https://github.com/g3rv4/RandomForests/blob/master/RegressionRandomForest.Rmd). It's predicting the final grade of math students given a bunch of variables (the data was grabbed from the [UCI Machine Learning Repository](http://archive.ics.uci.edu/ml/datasets/Student+Performance)). You can see [the classification code here](https://github.com/g3rv4/RandomForests/blob/master/ClassificationRandomForest.Rmd)

I then generated and exported the PMML file. I also used the model in R to predict the values for all the lines.

Lastly, I ran the predictions in C# and compared them with the ones generated in R. This project is doing exactly that.

If there's a difference bigger than 0.001%, it outputs "OOOOOPSSSSS". You're welcome to run this and verify it's *100% OOOOOPSSSSS free*.