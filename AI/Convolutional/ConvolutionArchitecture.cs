using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI.Convolutional
{
    public class ConvolutionArchitecture : IEquatable<ConvolutionArchitecture>
    {
        public readonly int ConvolutionStep;
        public readonly int FilterSize;
        public readonly int NumFilters;

        public ConvolutionArchitecture(int convolutionStep, int filterSize, int numFilters)
        {
            ConvolutionStep = convolutionStep;
            FilterSize = filterSize;
            NumFilters = numFilters;
        }

        public bool Equals(ConvolutionArchitecture other)
        {
            return ConvolutionStep == other.ConvolutionStep && FilterSize == other.FilterSize && NumFilters == other.NumFilters;
        }
    }
}
