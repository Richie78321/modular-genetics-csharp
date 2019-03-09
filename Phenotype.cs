using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics
{
    public abstract class Phenotype : IEquatable<Phenotype>
    {
        public abstract int GenomeLengthRequirement
        {
            get;
        }

        private GeneticSequence[] geneticSequences;
        protected GeneticSequence[] GeneticSequences => geneticSequences;
        public void SetGenome(GeneticSequence[] geneticSequences)
        {
            this.geneticSequences = geneticSequences;
            HandleIncomingGenome(geneticSequences);
        }

        protected abstract void HandleIncomingGenome(GeneticSequence[] geneticSequences);

        public abstract Phenotype Clone(Phenotype otherParent);

        public abstract bool Equals(Phenotype phenotype);
    }
}
