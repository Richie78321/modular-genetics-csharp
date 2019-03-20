using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI.Dense
{
    public class DenseLayer : ComputationModel
    {
        public override int[] OutputShape => new int[] { neurons };

        public override int GenomeLengthRequirement => neurons * (inputShape[0] + 1);

        private int neurons;
        public int Neurons => neurons;
        private ActivationFunction activationFunction;
        /// <summary>
        /// Creates a new dense neural network layer with a specified number of neurons.
        /// </summary>
        /// <param name="neurons">The neurons in the dense layer.</param>
        /// <param name="activationFunction">The activation function to be used.</param>
        public DenseLayer(int neurons, ActivationFunction activationFunction)
        {
            this.neurons = neurons;
            this.activationFunction = activationFunction;
        }

        public override object Transform(object input)
        {
            //Assume object to be single-dimension
            double[] layerInput = (double[])input;
            double[] layerOutput = new double[neurons];

            GeneticSequence[] geneticSequences = GeneticSequences;
            for (int i = 0; i < layerOutput.Length; i++)
            {
                layerOutput[i] = RunNeuron(i * (inputShape[0] + 1), layerInput); 
            }

            return layerOutput;
        }

        private double RunNeuron(int startIndex, double[] layerInput)
        {
            double weightedSum = 0;
            for (int i = 0; i < layerInput.Length; i++)
            {
                //Weights
                weightedSum += layerInput[i] * bakedParameterValues[startIndex + i];
            }
            //Bias
            weightedSum += bakedParameterValues[startIndex + layerInput.Length];

            return activationFunction(weightedSum);
        }

        protected override void HandleDeploy(int[] inputShape)
        {
            if (inputShape.Length != 1) throw new Exception("The input into a dense layer must be of a one-dimensional shape.");
        }

        private double[] bakedParameterValues = null;
        public double[] BakedParameterValues => bakedParameterValues;
        protected override void HandleIncomingGenome(GeneticSequence[] geneticSequences)
        {
            bakedParameterValues = new double[geneticSequences.Length];
            for (int i = 0; i < geneticSequences.Length; i++)
            {
                bakedParameterValues[i] = (geneticSequences[i].PortionValue * 2) - 1;
            }
        }

        public override Phenotype Clone(Phenotype otherParent)
        {
            DenseLayer newLayer = new DenseLayer(neurons, activationFunction);
            newLayer.Deploy(inputShape);
            return newLayer;
        }

        public override bool Equals(Phenotype phenotype)
        {
            return phenotype is DenseLayer otherDense && otherDense.neurons == neurons && otherDense.activationFunction.Equals(activationFunction);
        }
    }
}
