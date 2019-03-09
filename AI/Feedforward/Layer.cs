using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI.Feedforward
{
    internal class Layer
    {
        private Neuron[] neurons;
        private bool isInputLayer;

        public Layer(GeneticSequence[] networkSequences, NetworkArchitecture networkArchitecture, int layerIndex)
        {
            isInputLayer = layerIndex == 0;

            neurons = new Neuron[networkArchitecture.LayerSizes[layerIndex]];
            for (int i = 0; i < neurons.Length; i++) neurons[i] = new Neuron(networkSequences, networkArchitecture, new NetworkPosition(layerIndex, i));
        }

        public double[] RunLayer(double[] prevLayerInput)
        {
            double[] layerOutput = new double[neurons.Length];
            if (isInputLayer)
            {
                for (int i = 0; i < neurons.Length; i++) layerOutput[i] = neurons[i].RunNeuron(new double[] { prevLayerInput[i] });
            }
            else
            {
                for (int i = 0; i < neurons.Length; i++) layerOutput[i] = neurons[i].RunNeuron(prevLayerInput);
            }

            return layerOutput;
        }
    }
}
