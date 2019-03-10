using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI
{
    abstract class ComputationModel
    {
        private bool isDeployed = false;
        public bool IsDeployed => isDeployed;
        public void Deploy(int[] inputShape)
        {
            if (!isDeployed)
            {
                isDeployed = true;
                HandleDeploy(inputShape);
            }
        }

        public abstract int[] OutputShape { get; }

        public abstract int ParameterRequirement { get; }

        protected abstract void HandleDeploy(int[] inputShape);
        public abstract object Transform(object input);
    }
}
