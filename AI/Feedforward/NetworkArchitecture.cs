using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI.Feedforward
{
    public class NetworkArchitecture : IEquatable<NetworkArchitecture>
    {
        public delegate double NumericalFunction(double input);

        public static double SigmoidFunction(double input)
        {
            return 1.0 / (1 + Math.Pow(Math.E, -input));
        }

        public static double ReLUFunction(double input)
        {
            if (input > 0) return input;
            else return 0;
        }

        //Object
        public readonly NumericalFunction ActivationFunction;
        public readonly int[] LayerSizes;
        public int LayerDepth => LayerSizes.Length;

        public NetworkArchitecture(NumericalFunction ActivationFunction, int inputLayerSize, int outputLayerSize, int hiddenLayerDepth, int hiddenLayerSize)
        {
            this.ActivationFunction = ActivationFunction;
            LayerSizes = new int[hiddenLayerDepth + 2];
            LayerSizes[0] = inputLayerSize;
            LayerSizes[LayerSizes.Length - 1] = outputLayerSize;
            for (int i = 1; i < LayerSizes.Length - 1; i++)
            {
                LayerSizes[i] = hiddenLayerSize;
            }
        }

        public NetworkArchitecture(NumericalFunction ActivationFunction, int[] LayerSizes, float neuronWeightRange = 1F)
        {
            this.LayerSizes = LayerSizes;
        }

        public bool Equals(NetworkArchitecture other)
        {
            if (!ActivationFunction.Equals(other.ActivationFunction)) return false;
            if (LayerDepth != other.LayerDepth) return false;
            else
            {
                for (int i = 0; i < LayerSizes.Length; i++) if (LayerSizes[i] != other.LayerSizes[i]) return false;
                return true;
            }
        }
    }
}
