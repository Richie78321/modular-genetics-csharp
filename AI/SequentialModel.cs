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
        public SequentialModel(ComputationModel[] computationModels) : base(computationModels)
        {
            this.computationModels = computationModels;
        }

        public object Transform(object input)
        {
            object nextInput = input;
            for (int i = 0; i < computationModels.Length; i++) nextInput = computationModels[i].Transform(nextInput);
            return nextInput;
        }
    }
}
