using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI.Convolutional
{
    public class ConvolutionalNetwork : Phenotype, IStackAction
    {
        private static int GetGenomeRequirement(ConvolutionArchitecture convolutionArchitecture)
        {
            return (int)Math.Pow(convolutionArchitecture.FilterSize, 2) * convolutionArchitecture.NumFilters;
        }

        //Object
        private ConvolutionArchitecture convolutionArchitecture;

        public ConvolutionalNetwork(ConvolutionArchitecture convolutionArchitecture)
        {
            this.convolutionArchitecture = convolutionArchitecture;
            genomeRequirement = GetGenomeRequirement(convolutionArchitecture);
        }

        public override int GenomeLengthRequirement => genomeRequirement;
        private int genomeRequirement;

        public override Phenotype Clone(Phenotype otherParent)
        {
            return new ConvolutionalNetwork(convolutionArchitecture);
        }

        public override bool Equals(Phenotype phenotype)
        {
            return phenotype is ConvolutionalNetwork network && network.convolutionArchitecture.Equals(convolutionArchitecture);
        }

        private ConvFilter[] convFilters;
        protected override void HandleIncomingGenome(GeneticSequence[] geneticSequences)
        {
            convFilters = new ConvFilter[convolutionArchitecture.NumFilters];
            for (int i = 0; i < convFilters.Length; i++) convFilters[i] = new ConvFilter(convolutionArchitecture, geneticSequences, i);
        }

        public double[][,] StackAction(double[][,] inputStack)
        {
            List<double[,]> outputStack = new List<double[,]>(inputStack.Length * convolutionArchitecture.NumFilters);
            for (int i = 0; i < inputStack.Length; i++)
            {
                for (int j = 0; j < convFilters.Length; j++)
                {
                    outputStack.Add(convFilters[j].Convolve(inputStack[i]));
                }
            }

            return outputStack.ToArray();
        }
    }
}
