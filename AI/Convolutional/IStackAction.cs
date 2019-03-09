using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics.AI.Convolutional
{
    interface IStackAction
    {
        double[][,] StackAction(double[][,] inputStack);
    }
}
