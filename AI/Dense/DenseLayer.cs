using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI.Dense
{
    class DenseLayer : ComputationModel
    {
        public override int[] OutputShape => throw new NotImplementedException();

        public override int ParameterRequirement => throw new NotImplementedException();

        private int neurons;
        public DenseLayer(int neurons)
        {
            this.neurons = neurons;
        }

        public override object Transform(object input)
        {
            throw new NotImplementedException();
        }

        protected override void HandleDeploy(int[] inputShape)
        {
            throw new NotImplementedException();
        }
    }
}
