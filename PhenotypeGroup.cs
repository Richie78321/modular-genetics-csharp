using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics
{
    public class PhenotypeGroup : Phenotype
    {
        public override int GenomeLengthRequirement => phenotypes.Sum(phenotype => phenotype.GenomeLengthRequirement);

        private Phenotype[] phenotypes;
        public Phenotype[] Phenotypes => phenotypes;
        public PhenotypeGroup(Phenotype[] phenotypes)
        {
            this.phenotypes = phenotypes;
        }

        public override Phenotype Clone(Phenotype otherParent)
        {
            //Assume to be another phenotype group
            PhenotypeGroup groupParent = (PhenotypeGroup)otherParent;

            Phenotype[] phenotypeClones = new Phenotype[phenotypes.Length];
            for (int i = 0; i < phenotypeClones.Length; i++)
            {
                phenotypeClones[i] = phenotypes[i].Clone(groupParent.Phenotypes[i]);
            }

            return new PhenotypeGroup(phenotypeClones);
        }

        public override bool Equals(Phenotype phenotype)
        {
            if (phenotype is PhenotypeGroup otherGroup && otherGroup.Phenotypes.Length == phenotypes.Length)
            {
                for (int i = 0; i < phenotypes.Length; i++) if (!phenotypes[i].Equals(otherGroup.Phenotypes[i])) return false;
                return true;
            }
            else return false;
        }

        protected override void HandleIncomingGenome(GeneticSequence[] geneticSequences)
        {
            //Split among contained
            int nextIndex = 0;
            for (int i = 0; i < phenotypes.Length; i++)
            {
                int lengthRequirement = phenotypes[i].GenomeLengthRequirement;
                phenotypes[i].SetGenome(geneticSequences.Skip(nextIndex).Take(lengthRequirement).ToArray());
                nextIndex += lengthRequirement;
            }
        }
    }
}
