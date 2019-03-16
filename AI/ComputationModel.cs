using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI
{
    public abstract class ComputationModel : Phenotype
    {
        public delegate double ActivationFunction(double input);
        public static double ReLUActivation(double input)
        {
            if (input > 0) return input;
            else return 0;
        }
        public static double LinearActivation(double input)
        {
            return input;
        }
        public static double[] SoftmaxFunction(double[] input, double baseVal = Math.E)
        {
            double[] exponentialValues = new double[input.Length];
            for (int i = 0; i < exponentialValues.Length; i++) exponentialValues[i] = Math.Pow(baseVal, input[i]);
            double valueSum = exponentialValues.Sum();
            for (int i = 0; i < exponentialValues.Length; i++) exponentialValues[i] /= valueSum;
            return exponentialValues;
        }

        //Object
        private bool isDeployed = false;
        public bool IsDeployed => isDeployed;
        protected int[] inputShape;
        public int[] InputShape => inputShape;
        public void Deploy(int[] inputShape)
        {
            if (!isDeployed)
            {
                isDeployed = true;
                this.inputShape = inputShape;
                HandleDeploy(inputShape);
            }
        }

        public abstract int[] OutputShape { get; }

        protected abstract void HandleDeploy(int[] inputShape);
        public abstract object Transform(object input);
    }
}
