using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI.Feedforward
{
    internal class Neuron
    {
        private Dendrite[] dendrites;
        private GeneticSequence biasSequence;
        private bool isInputNeuron = false;

        private NetworkArchitecture networkArchitecture;

        public Neuron(GeneticSequence[] networkSequences, NetworkArchitecture networkArchitecture, NetworkPosition neuronPosition)
        {
            this.networkArchitecture = networkArchitecture;

            if (neuronPosition.LayerIndex == 0)
            {
                dendrites = new Dendrite[1];
                isInputNeuron = true;
            }
            else dendrites = new Dendrite[networkArchitecture.LayerSizes[neuronPosition.LayerIndex - 1]];

            for (int i = 0; i < dendrites.Length; i++) dendrites[i] = new Dendrite(networkSequences, networkArchitecture, new NetworkPosition(neuronPosition.LayerIndex - 1, i), neuronPosition);

            int extraNeuronIndex = 1;
            if (neuronPosition.LayerIndex != 0) extraNeuronIndex = networkArchitecture.LayerSizes[neuronPosition.LayerIndex - 1];
            int biasIndex = Dendrite.GetSequenceIndex(networkArchitecture, new NetworkPosition(neuronPosition.LayerIndex - 1, extraNeuronIndex), neuronPosition);
            biasSequence = networkSequences[biasIndex];
        }
        
        public double RunNeuron(double[] prevLayerInput)
        {
            //Sum weighted inputs
            double weightedSum = 0;
            for (int i = 0; i < prevLayerInput.Length; i++) weightedSum += dendrites[i].WeightValue * prevLayerInput[i];

            //Add bias if not input neuron
            if (!isInputNeuron) weightedSum += (biasSequence.PortionValue * 2) - 1;

            //Use activation function
            return networkArchitecture.ActivationFunction(weightedSum);
        }
    }
}
