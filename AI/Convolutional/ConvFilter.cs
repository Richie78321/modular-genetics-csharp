using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI.Convolutional
{
    internal class ConvFilter
    {
        private GeneticSequence[,] filterSequences;
        private ConvolutionArchitecture convolutionArchitecture;

        public ConvFilter(ConvolutionArchitecture convolutionArchitecture, GeneticSequence[] geneticSequences, int stackIndex)
        {
            this.convolutionArchitecture = convolutionArchitecture;

            int sequenceCount = (int)Math.Pow(convolutionArchitecture.FilterSize, 2);
            int sequenceStartIndex = sequenceCount * stackIndex;

            filterSequences = new GeneticSequence[convolutionArchitecture.FilterSize, convolutionArchitecture.FilterSize];
            for (int i = sequenceStartIndex; i < sequenceStartIndex + sequenceCount; i++) filterSequences[(i - sequenceStartIndex) % convolutionArchitecture.FilterSize, (i - sequenceStartIndex) / convolutionArchitecture.FilterSize] = geneticSequences[i];
        }

        public double[,] Convolve(double[,] inputMap)
        {
            double[,] outputMap = new double[(int)Math.Ceiling((double)inputMap.GetLength(0) / convolutionArchitecture.ConvolutionStep), (int)Math.Ceiling((double)inputMap.GetLength(1) / convolutionArchitecture.ConvolutionStep)];
            for (int j = 0; j < inputMap.GetLength(1); j += convolutionArchitecture.ConvolutionStep)
            {
                for (int i = 0; i < inputMap.GetLength(0); i += convolutionArchitecture.ConvolutionStep)
                {
                    //Get input portion 
                    double[,] inputPortion = new double[Math.Min(convolutionArchitecture.FilterSize, inputMap.GetLength(0) - i), Math.Min(convolutionArchitecture.FilterSize, inputMap.GetLength(1) - j)];
                    for (int k = 0; k < inputPortion.GetLength(0); k++) for (int l = 0; l < inputPortion.GetLength(1); l++) inputPortion[k, l] = inputMap[i + k, j + l];

                    double filterApplication = ApplyFilter(inputPortion);
                    outputMap[i / convolutionArchitecture.ConvolutionStep, j / convolutionArchitecture.ConvolutionStep] = filterApplication;
                }
            }

            return outputMap;
        }

        private double ApplyFilter(double[,] inputPortion)
        {
            double outputSum = 0;
            for (int i = 0; i < filterSequences.GetLength(0) && i < inputPortion.GetLength(0); i++)
            {
                for (int j = 0; j < filterSequences.GetLength(1) && j < inputPortion.GetLength(1); j++)
                {
                    outputSum += ((filterSequences[i, j].PortionValue * 2) - 1) * inputPortion[i, j];
                }
            }

            //Average
            return outputSum /= (inputPortion.GetLength(0) * inputPortion.GetLength(1));
        }
    }
}
