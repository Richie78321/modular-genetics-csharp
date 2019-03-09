using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics
{
    public class GeneticSequence
    {
        public static GeneticSequence[] Crossover(GeneticSequence sequence1, GeneticSequence sequence2, Random random)
        {
            if (sequence1.CanBreedWith(sequence2))
            {
                int crossoverPoint = random.Next(0, sequence1.binarySequence.Length - 1);

                GeneticSequence[] parents = { sequence1, sequence2 };
                bool[][] newSequences = new bool[2][];

                for (int i = 0; i < newSequences.Length; i++)
                {
                    newSequences[i] = new bool[sequence1.binarySequence.Length];
                    for (int j = 0; j <= crossoverPoint; j++) newSequences[i][j] = parents[i].binarySequence[j];
                    for (int j = crossoverPoint + 1; j < sequence1.binarySequence.Length; j++) newSequences[i][j] = parents[(i + 1) % parents.Length].binarySequence[j];
                }

                return new GeneticSequence[] { new GeneticSequence(newSequences[0]), new GeneticSequence(newSequences[1]) };
            }
            else throw new Exception("Supplied sequences are not compatible.");
        }

        //Object
        public readonly int MaxIntValue;

        private bool[] binarySequence;
        public bool[] BinarySequence => (bool[])binarySequence.Clone();

        public GeneticSequence(int sequenceLength, Random random)
        {
            //Ensure valid length
            if (sequenceLength <= 0) throw new Exception("Binary sequence length must be greater than zero.");

            binarySequence = new bool[sequenceLength];

            //Generate random sequence
            for (int i = 0; i < binarySequence.Length; i++)
            {
                if (random.Next(0, 2) == 0) binarySequence[i] = true;
                else binarySequence[i] = false;
            }

            MaxIntValue = (int)Math.Pow(2, sequenceLength) - 1;
            SetValues();
        }

        public GeneticSequence(bool[] binarySequence)
        {
            //Ensure valid length
            if (binarySequence.Length <= 0) throw new Exception("Binary sequence length must be greater than zero.");
            else this.binarySequence = binarySequence;

            MaxIntValue = (int)Math.Pow(2, binarySequence.Length) - 1;
            SetValues();
        }

        private int intValue;
        public int IntValue => intValue;

        private double portionValue;
        public double PortionValue => portionValue;

        private void SetValues()
        {
            intValue = 0;
            for (int i = 0; i < binarySequence.Length; i++) if (binarySequence[i]) intValue += (int)Math.Pow(2, binarySequence.Length - i - 1);

            portionValue = (double)intValue / MaxIntValue;
        }

        public bool CanBreedWith(GeneticSequence partner)
        {
            return (binarySequence.Length == partner.binarySequence.Length);
        }

        public GeneticSequence BitwiseMutate(double mutationRate, Random random)
        {
            bool[] sequenceCopy = new bool[binarySequence.Length];
            for (int i = 0; i < sequenceCopy.Length; i++)
            {
                if (random.NextDouble() <= mutationRate) sequenceCopy[i] = !binarySequence[i];
                else sequenceCopy[i] = binarySequence[i];
            }

            return new GeneticSequence(sequenceCopy);
        }

        public override string ToString()
        {
            string outputString = "{ ";
            for (int i = 0; i < binarySequence.Length; i++)
            {
                if (binarySequence[i]) outputString += "1 ";
                else outputString += "0 ";
            }
            outputString += "}";

            return outputString;
        }
    }
}
