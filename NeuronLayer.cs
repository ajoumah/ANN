using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{
    class NeuronLayer
    {
        public double[] inputs = new double[2];
        public double[] weights = new double[2];
        public double error;

        public double biasWeight;

        private Random randomArray = new Random();

        public double output
        {
            get { return sigmoid.output(weights[0] * inputs[0] + weights[1] * inputs[1] + biasWeight); }
        }

        public void randomizeWeights()
        {
            weights[0] = randomArray.NextDouble();
            weights[1] = randomArray.NextDouble();
            biasWeight = randomArray.NextDouble();
        }

        public void adjustWeights()
        {
            weights[0] += error * inputs[0];
            weights[1] += error * inputs[1];
            biasWeight += error;
        }
    }
    
}
