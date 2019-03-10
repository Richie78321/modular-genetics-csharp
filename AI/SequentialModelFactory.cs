using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI
{
    class SequentialModelFactory
    {
        private int[] inputShape;
        public SequentialModelFactory(int[] inputShape)
        {
            this.inputShape = inputShape;
        }

        private List<ComputationModel> computationModels = new List<ComputationModel>();
        public void AddModel(ComputationModel model)
        {
            int[] nextInputShape;
            if (computationModels.Count != 0) nextInputShape = computationModels[computationModels.Count - 1].OutputShape;
            else nextInputShape = this.inputShape;

            model.Deploy(nextInputShape);
            computationModels.Add(model);
        }

        public SequentialModel DeployModel()
        {
            return new SequentialModel(computationModels.ToArray());
        }
    }
}
