using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI.Dense
{
    class DenseLayer : ComputationModel
    {
        public override int[] OutputShape => new int[] { neurons };

        public override int GenomeLengthRequirement => neurons * (inputShape[0] + 1);

        private int neurons;
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
                layerOutput[i] = RunNeuron((GeneticSequence[])geneticSequences.Skip(i * (inputShape[0] + 1)).Take(inputShape[0] + 1), layerInput); 
            }

            return layerOutput;
        }

        private double RunNeuron(GeneticSequence[] geneticSequences, double[] layerInput)
        {
            double weightedSum = 0;
            for (int i = 0; i < layerInput.Length; i++)
            {
                //Weights
                double weightValue = (geneticSequences[i].PortionValue * 2) - 1;
                weightedSum += layerInput[i] * weightValue;
            }
            //Bias
            weightedSum += (geneticSequences[geneticSequences.Length - 1].PortionValue * 2) - 1;

            return activationFunction(weightedSum);
        }

        protected override void HandleDeploy(int[] inputShape)
        {
            if (inputShape.Length != 1) throw new Exception("The input into a dense layer must be of a one-dimensional shape.");
        }

        protected override void HandleIncomingGenome(GeneticSequence[] geneticSequences)
        {
        }

        public override Phenotype Clone(Phenotype otherParent)
        {
            return new DenseLayer(neurons, activationFunction);
        }

        public override bool Equals(Phenotype phenotype)
        {
            return phenotype is DenseLayer otherDense && otherDense.neurons == neurons && otherDense.activationFunction.Equals(activationFunction);
        }
    }
}
