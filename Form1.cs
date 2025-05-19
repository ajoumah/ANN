// Required namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;

namespace ANN
{
    public partial class Form1 : Form
    {
        // Parameters for the ANN
        public static int NumberOFhidden = 2;    // Number of hidden layer neurons
        public static int speed = 75;            // Training speed (used as delay in ms)
        public static int NumberOfIteration = 1000; // Number of training epochs

        public Form1()
        {
            InitializeComponent();

            // Initialize values from form controls
            NumberOFhidden = Convert.ToInt32(Math.Round(numericUpDown1.Value, 0));
            speed = trackBar1.Value;
            NumberOfIteration = Convert.ToInt32(Math.Round(numericUpDown2.Value, 0));
        }

        // Triggered when the Train button is clicked
        private void button1_Click(object sender, EventArgs e)
        {
            listView1.ForeColor = Color.Black;
            listView1.View = View.Details;

            NumberOFhidden = Convert.ToInt32(Math.Round(numericUpDown1.Value, 0));
            train();
        }

        // Trains the ANN using backpropagation on XOR data
        private void train()
        {
            // XOR input data
            double[,] inputs = {
                { 0, 0 },
                { 0, 1 },
                { 1, 0 },
                { 1, 1 }
            };

            // Expected XOR outputs
            double[] results = { 0, 1, 1, 0 };

            // Create hidden layer neurons
            NeuronLayer[] hiddenLayer = new NeuronLayer[NumberOFhidden];
            for (int i = 0; i < NumberOFhidden; i++)
            {
                hiddenLayer[i] = new NeuronLayer();
                hiddenLayer[i].randomizeWeights();
            }

            // Create and randomize output neuron
            OutPutNeuronLayer outputNeuron = new OutPutNeuronLayer();
            outputNeuron.randomizeWeights();

            // Arrays to hold network state (weights, biases, outputs, etc.)
            double[] finalWeights = new double[NumberOFhidden * 3];
            double[] finalbias = new double[NumberOFhidden + 1];
            double[] finaloutput = new double[NumberOFhidden + 1];
            double[] finalinput = new double[2];

            // Visualize randomized weights
            finalinput[0] = inputs[3, 0];
            finalinput[1] = inputs[3, 1];
            panel1.Refresh();
            drawWeights(finalWeights, finalbias, finaloutput, finalinput);
            label1.Text = "Randomized weights";

            int epoch = 0;

        Retry:
            epoch++;

            for (int i = 0; i < 4; i++) // Loop through XOR examples
            {
                // Set current input
                finalinput[0] = inputs[i, 0];
                finalinput[1] = inputs[i, 1];

                // Feed input to hidden layer
                for (int j = 0; j < NumberOFhidden; j++)
                    hiddenLayer[j].inputs = new double[] { inputs[i, 0], inputs[i, 1] };

                // Hidden layer outputs to output neuron
                outputNeuron.inputs = new double[NumberOFhidden];
                for (int j = 0; j < NumberOFhidden; j++)
                    outputNeuron.inputs[j] = hiddenLayer[j].output;

                // Log output
                string s = $"{inputs[i, 0]} XOR {inputs[i, 1]} = {Math.Round(outputNeuron.output, 4)}";
                listView1.Items.Add(new ListViewItem(new string[] { s }));

                // Update weights and bias arrays for visualization
                for (int j = 0; j < NumberOFhidden; j++)
                {
                    finalWeights[j * 2] = hiddenLayer[j].weights[0];
                    finalWeights[(j * 2) + 1] = hiddenLayer[j].weights[1];
                    finalbias[j] = hiddenLayer[j].biasWeight;
                    finaloutput[j] = hiddenLayer[j].output;
                }
                for (int j = 0; j < NumberOFhidden; j++)
                    finalWeights[NumberOFhidden * 2 + j] = outputNeuron.weights[j];

                finalbias[NumberOFhidden] = outputNeuron.biasWeight;
                finaloutput[NumberOFhidden] = outputNeuron.output;

                // Refresh visualization
                panel1.Refresh();
                drawWeights(finalWeights, finalbias, finaloutput, finalinput);
                Thread.Sleep(speed);

                // --- Backpropagation ---
                // Calculate output error
                outputNeuron.error = sigmoid.derivative(outputNeuron.output) * (results[i] - outputNeuron.output);
                outputNeuron.adjustWeights();

                // Calculate and backpropagate error to hidden layer
                for (int j = 0; j < NumberOFhidden; j++)
                {
                    hiddenLayer[j].error = sigmoid.derivative(hiddenLayer[j].output) * outputNeuron.error * outputNeuron.weights[j];
                    hiddenLayer[j].adjustWeights();
                }
            }

            // Repeat until max iteration
            if (epoch < NumberOfIteration)
                goto Retry;
        }

        // Draws network visualization on panel1
        public void drawWeights(double[] weights, double[] bais, double[] output, double[] input)
        {
            StringFormat format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            panel1.Height = NumberOFhidden * 100;

            Graphics g = panel1.CreateGraphics();
            Pen pen = new Pen(Color.Red, 3);

            Rectangle input1 = new Rectangle(0, NumberOFhidden * 12, 50, 50);
            Rectangle input2 = new Rectangle(0, NumberOFhidden * 12 + 100, 50, 50);
            Rectangle outputNode = new Rectangle(300, NumberOFhidden * 12 + 12, 50, 50);

            g.DrawString("Input1=" + input[0], new Font("Tahoma", 8), Brushes.Blue, input1, format);
            g.DrawString("Input2=" + input[1], new Font("Tahoma", 8), Brushes.Blue, input2, format);
            g.DrawString("Output=" + Math.Round(output[NumberOFhidden], 4), new Font("Tahoma", 8), Brushes.Blue, outputNode, format);

            g.DrawRectangle(pen, input1);
            g.DrawRectangle(pen, input2);
            g.DrawRectangle(pen, outputNode);

            g.DrawLine(pen, 325, NumberOFhidden * 12 + 62, 325, NumberOFhidden * 12 + 72);
            g.DrawString("Bias=" + Math.Round(bais[NumberOFhidden], 2), new Font("Tahoma", 8), Brushes.Black, 325, NumberOFhidden * 12 + 72);
        }
    }
}
