using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI.Feedforward
{
    public struct NetworkPosition
    {
        public readonly int LayerIndex;
        public readonly int NeuronIndex;

        public NetworkPosition(int LayerIndex, int NeuronIndex)
        {
            this.LayerIndex = LayerIndex;
            this.NeuronIndex = NeuronIndex;
        }
    }
}
