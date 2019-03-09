using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI.Feedforward
{
    internal class Dendrite
    {
        public static int GetSequenceIndex(NetworkArchitecture networkArchitecture, NetworkPosition senderNeuron, NetworkPosition receiverNeuron)
        {
            int index = 0;

            //Sum prev layers
            for (int i = 0; i < receiverNeuron.LayerIndex; i++)
            {
                if (i == 0) index += networkArchitecture.LayerSizes[0] * 2;
                else
                {
                    index += networkArchitecture.LayerSizes[i] * networkArchitecture.LayerSizes[i - 1];
                    index += networkArchitecture.LayerSizes[i];
                }
            }

            //Sum prev neurons
            if (receiverNeuron.LayerIndex == 0)
            {
                index += receiverNeuron.NeuronIndex * 2;
            }
            else
            {
                index += receiverNeuron.NeuronIndex * networkArchitecture.LayerSizes[receiverNeuron.LayerIndex - 1];
                index += receiverNeuron.NeuronIndex;
            }

            //Sum prev connections
            index += senderNeuron.NeuronIndex;

            return index;
        }

        //Object
        private GeneticSequence weightSequence;
        public double WeightValue => (weightSequence.PortionValue * 2) - 1;

        public Dendrite(GeneticSequence[] networkSequences, NetworkArchitecture networkArchitecture, NetworkPosition senderNeuron, NetworkPosition receiverNeuron)
        {
            int sequenceIndex = GetSequenceIndex(networkArchitecture, senderNeuron, receiverNeuron);
            weightSequence = networkSequences[sequenceIndex];
        }
    }
}
