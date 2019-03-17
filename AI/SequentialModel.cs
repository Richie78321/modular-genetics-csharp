using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI
{
    public class SequentialModel : PhenotypeGroup
    {
        private ComputationModel[] computationModels;
        public ComputationModel[] ComputationModels => computationModels;
        public SequentialModel(ComputationModel[] computationModels) : base(computationModels)
        {
            this.computationModels = computationModels;
        }

        public override Phenotype Clone(Phenotype otherParent)
        {
            //Assume to be another phenotype group
            SequentialModel modelParent = (SequentialModel)otherParent;

            ComputationModel[] modelClones = new ComputationModel[computationModels.Length];
            for (int i = 0; i < modelClones.Length; i++)
            {
                modelClones[i] = (ComputationModel)computationModels[i].Clone(modelParent.ComputationModels[i]);
            }

            return new SequentialModel(modelClones);
        }

        public object Transform(object input)
        {
            object nextInput = input;
            for (int i = 0; i < computationModels.Length; i++) nextInput = computationModels[i].Transform(nextInput);
            return nextInput;
        }
    }
}
