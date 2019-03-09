using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModularGenetics.AI.Feedforward;

namespace ModularGenetics.AI.Convolutional
{
    public class NormalizationLayer : IStackAction
    {
        private NetworkArchitecture.NumericalFunction normalizationFunction;

        public NormalizationLayer(NetworkArchitecture.NumericalFunction normalizationFunction)
        {
            this.normalizationFunction = normalizationFunction;
        }

        public double[][,] StackAction(double[][,] inputStack)
        {
            double[][,] outputStack = new double[inputStack.Length][,];
            for (int i = 0; i < outputStack.Length; i++)
            {
                outputStack[i] = ApplyNormalization(inputStack[i]);
            }

            return outputStack;
        }

        public double[,] ApplyNormalization(double[,] inputData)
        {
            double[,] outputData = new double[inputData.GetLength(0), inputData.GetLength(1)];
            for (int i = 0; i < outputData.GetLength(0); i++) for (int j = 0; j < outputData.GetLength(1); j++) outputData[i, j] = normalizationFunction(inputData[i, j]);

            return outputData;
        }
    }
}
