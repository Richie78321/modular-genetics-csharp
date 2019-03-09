using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI.Feedforward
{
    public class FeedforwardNetwork : Phenotype
    {
        private static int GetGenomeRequirement(NetworkArchitecture networkArchitecture)
        {
            int numWeights = networkArchitecture.LayerSizes[0];
            for (int i = 1; i < networkArchitecture.LayerSizes.Length; i++) numWeights += networkArchitecture.LayerSizes[i - 1] * networkArchitecture.LayerSizes[i];

            int numBiases = 0;
            for (int i = 0; i < networkArchitecture.LayerSizes.Length; i++) numBiases += networkArchitecture.LayerSizes[i];

            return numWeights + numBiases;
        }

        //Object
        private NetworkArchitecture networkArchitecture;

        public FeedforwardNetwork(NetworkArchitecture networkArchitecture)
        {
            this.networkArchitecture = networkArchitecture;
            genomeRequirement = GetGenomeRequirement(networkArchitecture);
        }

        private int genomeRequirement;
        public override int GenomeLengthRequirement => genomeRequirement;

        public override Phenotype Clone(Phenotype otherParent)
        {
            return new FeedforwardNetwork(networkArchitecture);
        }

        public override bool Equals(Phenotype phenotype)
        {
            return phenotype is FeedforwardNetwork network && network.networkArchitecture.Equals(networkArchitecture);
        }

        private Layer[] layers;
        protected override void HandleIncomingGenome(GeneticSequence[] geneticSequences)
        {
            layers = new Layer[networkArchitecture.LayerDepth];
            for (int i = 0; i < layers.Length; i++) layers[i] = new Layer(GeneticSequences, networkArchitecture, i);
        }

        public double[] RunNetwork(double[] networkInput)
        {
            //Ensure correct length
            if (networkInput.Length != networkArchitecture.LayerSizes[0]) throw new Exception("Invalid length of network input!");
            else
            {
                double[] layerOutput = networkInput;
                for (int i = 0; i < layers.Length; i++) layerOutput = layers[i].RunLayer(layerOutput);

                return layerOutput;
            }
        }
    }
}
