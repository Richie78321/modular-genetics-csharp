using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics
{
    public class ModularMember
    {
        /// <summary>
        /// Breeds two ModularMembers and produces two children.
        /// </summary>
        /// <param name="parent1">The first parent.</param>
        /// <param name="parent2">The second parent.</param>
        /// <param name="mutationRate">The mutation rate during breeding.</param>
        /// <param name="random">The random to be used while breeding.</param>
        /// <returns>Returns the two children.</returns>
        public static ModularMember[] BreedMembers(ModularMember parent1, ModularMember parent2, double mutationRate, Random random)
        {
            //Ensure breedable
            if (parent1.CanBreedWith(parent2))
            {
                GeneticSequence[] child1Genome = new GeneticSequence[parent1.genome.Length];
                GeneticSequence[] child2Genome = new GeneticSequence[parent1.genome.Length];

                for (int i = 0; i < parent1.genome.Length; i++)
                {
                    GeneticSequence[] crossoverSequences = GeneticSequence.Crossover(parent1.genome[i], parent2.genome[i], random);
                    child1Genome[i] = crossoverSequences[0];
                    child2Genome[i] = crossoverSequences[1];
                }

                //Mutate each genome
                for (int i = 0; i < child1Genome.Length; i++) child1Genome[i] = child1Genome[i].BitwiseMutate(mutationRate, random);
                for (int i = 0; i < child2Genome.Length; i++) child2Genome[i] = child2Genome[i].BitwiseMutate(mutationRate, random);

                ModularMember[] parents = new ModularMember[] { parent1, parent2 };
                GeneticSequence[][] childSequences = new GeneticSequence[][] { child1Genome, child2Genome };
                ModularMember[] children = new ModularMember[2];
                for (int i = 0; i < children.Length; i++)
                {
                    Phenotype[] phenotypes = new Phenotype[parents[i].phenotypes.Length];
                    for (int j = 0; j < phenotypes.Length; j++) phenotypes[j] = parents[i].phenotypes[j].Clone(parents[(i + 1) % parents.Length].phenotypes[j]);

                    children[i] = new ModularMember(phenotypes);
                    children[i].AssignGenome(childSequences[i]);
                }

                return children;
            }
            else throw new Exception("Provided parents are not breedable.");
        }

        /// <summary>
        /// Creates a random genome for a set of phenotypes.
        /// </summary>
        /// <param name="phenotypes">The set of phenotypes.</param>
        /// <param name="random">The random to be used for the random generation of genetic sequences.</param>
        /// <param name="sequenceBitLength">The bit length of the random genetic sequences.</param>
        /// <returns>Returns the randomly-generated genome.</returns>
        private static GeneticSequence[] GetRandomGenome(Phenotype[] phenotypes, Random random, int sequenceBitLength)
        {
            int sequenceLength = 0;
            for (int i = 0; i < phenotypes.Length; i++) sequenceLength += phenotypes[i].GenomeLengthRequirement;

            GeneticSequence[] genome = new GeneticSequence[sequenceLength];
            for (int i = 0; i < genome.Length; i++) genome[i] = new GeneticSequence(sequenceBitLength, random);

            return genome;
        }

        //Object
        private Phenotype[] phenotypes;
        public Phenotype[] Phenotypes => phenotypes;

        /// <summary>
        /// Creates a new ModularMember with a randomly-initialized genome.
        /// </summary>
        /// <param name="phenotypes">The phenotypes of the ModularMember.</param>
        /// <param name="random">The random to be used for the random generation of the genome.</param>
        /// <param name="sequenceBitLength">The length of the randomly-generated genetic sequences.</param>
        public ModularMember(Phenotype[] phenotypes, Random random, int sequenceBitLength = 8)
        {
            this.phenotypes = phenotypes;

            //Assign random genome
            AssignGenome(GetRandomGenome(phenotypes, random, sequenceBitLength));
        }

        /// <summary>
        /// Creates a new ModularMember but does not initialize the genome.
        /// </summary>
        /// <param name="phenotypes">The phenotypes of the ModularMember.</param>
        public ModularMember(Phenotype[] phenotypes)
        {
            this.phenotypes = phenotypes;
        }

        private bool genomeAssigned = false;
        public bool GenomeAssigned => genomeAssigned;
        private GeneticSequence[] genome;
        /// <summary>
        /// Assigns a genome to the ModularMember.
        /// </summary>
        /// <param name="genome">The genome to be assigned.</param>
        /// <returns>Returns whether the assignment was successful.</returns>
        public bool AssignGenome(GeneticSequence[] genome)
        {
            int sequenceLength = 0;
            for (int i = 0; i < phenotypes.Length; i++) sequenceLength += phenotypes[i].GenomeLengthRequirement;

            //Ensure correct length and not already assigned
            if (genomeAssigned || genome.Length != sequenceLength) return false;
            else
            {
                int sequenceIndex = 0;

                //Assign to phenotypes
                for (int i = 0; i < phenotypes.Length; i++)
                {
                    GeneticSequence[] individualSequences = new GeneticSequence[phenotypes[i].GenomeLengthRequirement];
                    for (int j = 0; j < individualSequences.Length; j++) individualSequences[j] = genome[sequenceIndex + j];

                    phenotypes[i].SetGenome(individualSequences);
                    sequenceIndex += phenotypes[i].GenomeLengthRequirement;
                }

                this.genome = genome;
                genomeAssigned = true;

                return true;
            }
        }

        /// <summary>
        /// Determines if two ModularMembers can breed.
        /// </summary>
        /// <param name="member">The other ModularMember.</param>
        /// <returns>Returns whether the ModularMembers are breedable.</returns>
        public bool CanBreedWith(ModularMember member)
        {
            //Compare lengths
            if (phenotypes.Length != member.phenotypes.Length || genome.Length != member.genome.Length || genome[0].BinarySequence.Length != member.genome[0].BinarySequence.Length) return false;
            else
            {
                //Compare phenotypes
                for (int i = 0; i < phenotypes.Length; i++) if (!phenotypes[i].Equals(member.phenotypes[i])) return false;
                return true;
            }
        }
    }
}
