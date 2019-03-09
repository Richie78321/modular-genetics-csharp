using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI.Convolutional
{
    public class PoolingLayer : IStackAction
    {
        public readonly int PoolingSize;

        public PoolingLayer(int poolingSize)
        {
            PoolingSize = poolingSize;
        }

        public double[][,] StackAction(double[][,] inputStack)
        {
            double[][,] outputStack = new double[inputStack.Length][,];
            for (int i = 0; i < outputStack.Length; i++)
            {
                outputStack[i] = ApplyPooling(inputStack[i]);
            }

            return outputStack;
        }

        private double[,] ApplyPooling(double[,] inputData)
        {
            double[,] outputData = new double[(int)Math.Ceiling((double)inputData.GetLength(0) / PoolingSize), (int)Math.Ceiling((double)inputData.GetLength(1) / PoolingSize)];
            for (int i = 0; i < inputData.GetLength(0); i += PoolingSize)
            {
                for (int j = 0; j < inputData.GetLength(1); j += PoolingSize)
                {
                    double maxValue = inputData[i, j];
                    for (int k = 0; k < PoolingSize && k + i < inputData.GetLength(0); k++)
                    {
                        for (int l = 0; l < PoolingSize && l + j < inputData.GetLength(1); l++)
                        {
                            maxValue = Math.Max(maxValue, inputData[k + i, l + j]);
                        }
                    }
                    outputData[i / PoolingSize, j / PoolingSize] = maxValue;
                }
            }

            return outputData;
        }
    }
}
