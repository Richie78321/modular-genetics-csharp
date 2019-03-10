using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics
{
    public abstract class Phenotype : IEquatable<Phenotype>
    {
        /// <summary>
        /// The number of genetic sequences required for this phenotype to function.
        /// </summary>
        public abstract int GenomeLengthRequirement
        {
            get;
        }

        private GeneticSequence[] geneticSequences;
        protected GeneticSequence[] GeneticSequences => geneticSequences;
        /// <summary>
        /// Set the genome of this phenotype. It is not recommended to use this manually.
        /// </summary>
        /// <param name="geneticSequences">The genetic sequences to set.</param>
        public void SetGenome(GeneticSequence[] geneticSequences)
        {
            this.geneticSequences = geneticSequences;
            HandleIncomingGenome(geneticSequences);
        }

        /// <summary>
        /// Callled when a new genome is set for the phenotype.
        /// </summary>
        /// <param name="geneticSequences">The new genome.</param>
        protected abstract void HandleIncomingGenome(GeneticSequence[] geneticSequences);

        /// <summary>
        /// Creates a new instance of the phenotype based on itself and an equivalent phenotype from another parent.
        /// </summary>
        /// <param name="otherParent">The corresponding phenotype from the other parent.</param>
        /// <returns>Returns the new instance of the phenotype.</returns>
        public abstract Phenotype Clone(Phenotype otherParent);

        /// <summary>
        /// Checks for equality between two phenotypes.
        /// </summary>
        /// <param name="phenotype">The other phenotype.</param>
        /// <returns>Returns if the phenotypes are equal.</returns>
        public abstract bool Equals(Phenotype phenotype);
    }
}
