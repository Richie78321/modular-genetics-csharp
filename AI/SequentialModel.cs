using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI
{
    class SequentialModel : Phenotype
    {
        public override int GenomeLengthRequirement => throw new NotImplementedException();

        public override Phenotype Clone(Phenotype otherParent)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(Phenotype phenotype)
        {
            throw new NotImplementedException();
        }

        protected override void HandleIncomingGenome(GeneticSequence[] geneticSequences)
        {
            throw new NotImplementedException();
        }
    }
}
