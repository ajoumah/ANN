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
        public static int NumberOFhidden=2;
        public static int speed = 75;
        public static int NumberOfIteration = 1000;
        public Form1()
        {
            InitializeComponent();
            NumberOFhidden = Convert.ToInt32(Math.Round(numericUpDown1.Value, 0));
            speed = trackBar1.Value;
            NumberOfIteration = Convert.ToInt32(Math.Round(numericUpDown2.Value, 0));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.ForeColor = System.Drawing.Color.Black;
            listView1.View = View.Details;
            NumberOFhidden = Convert.ToInt32(Math.Round(numericUpDown1.Value, 0));
            train();
        }
        private  void train()
        {
            // the input values
            double[,] inputs =
            {
                { 0, 0},
                { 0, 1},
                { 1, 0},
                { 1, 1}
            };

            // desired results
            double[] results = { 0, 1, 1, 0 };

            // creating number of  neurons specified by the problem
            NeuronLayer[] hiddenLayer = new NeuronLayer[NumberOFhidden];
            for (int i = 0; i < NumberOFhidden; i++)
            {
                hiddenLayer[i] = new NeuronLayer();
            }
            //NeuronLayer hiddenNeuron1 = new NeuronLayer();
            //NeuronLayer hiddenNeuron2 = new NeuronLayer();
            OutPutNeuronLayer outputNeuron = new OutPutNeuronLayer();
            label1.Text = "randomized weights";
            // randomize weights
            for (int i = 0; i < NumberOFhidden; i++)
            {
                hiddenLayer[i].randomizeWeights();
            }
            //hiddenNeuron1.randomizeWeights();
            //hiddenNeuron2.randomizeWeights();
            outputNeuron.randomizeWeights();
            double[] finalWeights = new double[NumberOFhidden*3];
            double[] finalbias = new double[NumberOFhidden+1];
            double[] finaloutput = new double[NumberOFhidden+1];
            double[] finalinput = new double[2];
            //finalWeights[0] = hiddenNeuron1.weights[0];
            //finalWeights[1] = hiddenNeuron1.weights[1];
            //finalWeights[2] = hiddenNeuron2.weights[0];
            //finalWeights[3] = hiddenNeuron2.weights[1];
            //finalWeights[4] = outputNeuron.weights[0];
            //finalWeights[5] = outputNeuron.weights[1];
            for (int i = 0; i < NumberOFhidden  ; i++)
            {
                finalWeights[i*2] = hiddenLayer[i].weights[0];
                finalWeights[(i*2)+1] = hiddenLayer[i].weights[1];
            }
            for (int i = NumberOFhidden * 2; i < NumberOFhidden * 3; i ++)
            {
                finalWeights[i] = outputNeuron.weights[i- (NumberOFhidden * 2)];
                
            }
            //finalWeights[NumberOFhidden*2] = outputNeuron.weights[0];
            //finalWeights[NumberOFhidden*2 + 1] = outputNeuron.weights[1];
            for (int i = 0; i < NumberOFhidden ; i++)
            {
                finalbias[i] = hiddenLayer[i].biasWeight;
                
            }
            finalbias[NumberOFhidden] = outputNeuron.biasWeight;
            //finalbias[0] = hiddenNeuron1.biasWeight;
            //finalbias[1] = hiddenNeuron2.biasWeight;
            //finalbias[2] = outputNeuron.biasWeight;
            for (int i = 0; i < NumberOFhidden; i++)
            {
                finaloutput[i] = hiddenLayer[i].output;

            }
            finaloutput[NumberOFhidden] = outputNeuron.output;
            //finaloutput[0] = hiddenNeuron1.output;
            //finaloutput[1] = hiddenNeuron2.output;
            //finaloutput[2] = outputNeuron.output;
            finalinput[0] = inputs[3, 0];
            finalinput[1] = inputs[3, 1];
            panel1.Refresh();
            
            drawWeights(finalWeights, finalbias, finaloutput, finalinput);
            label1.Text = "randomized weights";
            int epoch = 0;

        Retry:
            epoch++;
            for (int i = 0; i < 4; i++)  // train network for whole examples
            {
                //  Calculation Output
                finalinput[0] = inputs[i, 0];
                finalinput[1] = inputs[i, 1];
                //hiddenNeuron1.inputs = new double[] { inputs[i, 0], inputs[i, 1] };
                //hiddenNeuron2.inputs = new double[] { inputs[i, 0], inputs[i, 1] };
                for (int j = 0; j < NumberOFhidden; j++)
                {
                    hiddenLayer[j].inputs = new double[] { inputs[i, 0], inputs[i, 1] };
                }
                //outputNeuron.inputs = new double[] { hiddenNeuron1.output, hiddenNeuron2.output };
                outputNeuron.inputs = new double[NumberOFhidden] ;
                for (int j = 0; j < NumberOFhidden; j++)
                {
                    outputNeuron.inputs[j] = hiddenLayer[j].output;
                }
                string s = Convert.ToString(inputs[i, 0]) + " XOR  " + Convert.ToString(inputs[i, 1]) + " =  " + Convert.ToString(outputNeuron.output);
                //textBox1.Text= conver("{0} xor {1} = {2}", inputs[i, 0], inputs[i, 1], outputNeuron.output);
                string[] row = { s };
                var listViewItem = new ListViewItem(row);
                listView1.Items.Add(listViewItem);
                
                for (int j = 0; j < NumberOFhidden; j++)
                {
                    finalWeights[j * 2] = hiddenLayer[j].weights[0];
                    finalWeights[(j * 2) + 1] = hiddenLayer[j].weights[1];
                }
                //finalWeights[NumberOFhidden * 2] = outputNeuron.weights[0];
                //finalWeights[NumberOFhidden * 2 + 1] = outputNeuron.weights[1];
                for (int j = NumberOFhidden * 2; j < NumberOFhidden * 3; j++)
                {
                    finalWeights[j] = outputNeuron.weights[j - (NumberOFhidden * 2)];

                }
                for (int j = 0; j < NumberOFhidden; j++)
                {
                    finalbias[j] = hiddenLayer[j].biasWeight;

                }
                finalbias[NumberOFhidden] = outputNeuron.biasWeight;
                for (int j = 0; j < NumberOFhidden; j++)
                {
                    finaloutput[j] = hiddenLayer[j].output;

                }
                finaloutput[NumberOFhidden] = outputNeuron.output;
                label1.Text = "final weights";
                panel1.Refresh();

                drawWeights(finalWeights, finalbias, finaloutput, finalinput);
                Thread.Sleep(speed);
                //  backpropagation 

                // calculate error
                outputNeuron.error = sigmoid.derivative(outputNeuron.output) * (results[i] - outputNeuron.output);
                outputNeuron.adjustWeights();

                // Adjust weights
                //hiddenNeuron1.error = sigmoid.derivative(hiddenNeuron1.output) * outputNeuron.error * outputNeuron.weights[0];
                //hiddenNeuron2.error = sigmoid.derivative(hiddenNeuron2.output) * outputNeuron.error * outputNeuron.weights[1];
                for (int j = 0; j < NumberOFhidden; j++)
                {
                    outputNeuron.inputs[j] = hiddenLayer[j].output;
                    hiddenLayer[j].error = sigmoid.derivative(hiddenLayer[j].output) * outputNeuron.error * outputNeuron.weights[j];
                }
                for (int j = 0; j < NumberOFhidden; j++)
                {
                    
                    hiddenLayer[j].adjustWeights();
                }
                //hiddenNeuron1.adjustWeights();
                //hiddenNeuron2.adjustWeights();
                //

            }

            if (epoch < NumberOfIteration)
                goto Retry;
           
            //finalWeights[0] = hiddenNeuron1.weights[0];
            //finalWeights[1] = hiddenNeuron1.weights[1];
            //finalWeights[2] = hiddenNeuron2.weights[0];
            //finalWeights[3] = hiddenNeuron2.weights[1];
            //finalWeights[4] = outputNeuron.weights[0];
            //finalWeights[5] = outputNeuron.weights[1];
            //finalbias[0] = hiddenNeuron1.biasWeight;
            //finalbias[1] = hiddenNeuron2.biasWeight;
            //finalbias[2] =outputNeuron.biasWeight;
            //finaloutput[0] = hiddenNeuron1.output;
            //finaloutput[1] = hiddenNeuron2.output;
            //finaloutput[2] = outputNeuron.output;
            //finalinput[0] = inputs[3, 0];
            //finalinput[1] = inputs[3, 1];
            //panel1.Refresh();
            //label1.Text = "final weights";
            //drawWeights(finalWeights, finalbias, finaloutput, finalinput);
            //textBox1.AppendText("\r\n" );
            //Console.ReadLine();
        }

        public void drawWeights(double[] weights, double[] bais, double[] output, double[] input)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            panel1.Height = NumberOFhidden * 100;
            Graphics g = panel1.CreateGraphics();
            // Create pen.
            Pen blackPen = new Pen(Color.Red, 3);
            Rectangle rectInput1 = new Rectangle(0, NumberOFhidden*12, 50, 50);
            Rectangle rectInput2 = new Rectangle(0, (NumberOFhidden * 12)+100, 50, 50);
            Rectangle rectOutput = new Rectangle(300, (NumberOFhidden * 12) + 12, 50, 50);
            g.DrawString("Input1=" + Convert.ToString(input[0]), new Font("Tahoma", 8), Brushes.Blue, rectInput1, stringFormat);
            
            g.DrawString("Input2=" + Convert.ToString(input[1]), new Font("Tahoma", 8), Brushes.Blue, rectInput2, stringFormat);
            g.DrawString("Output=" + Convert.ToString(Math.Round((Double)output[NumberOFhidden], 4)), new Font("Tahoma", 8), Brushes.Blue, rectOutput, stringFormat);

            g.DrawRectangle(blackPen, rectInput1);
            g.DrawRectangle(blackPen, rectInput2);
            g.DrawRectangle(blackPen, rectOutput);
            g.DrawLine(blackPen, 325, (NumberOFhidden*12) + 62, 325, (NumberOFhidden * 12) + 72);
            g.DrawString("Bais=" + Convert.ToString(Math.Round((Double)bais[NumberOFhidden], 2)), new Font("Tahoma", 8), Brushes.Black, 325, (NumberOFhidden * 12) + 72);

            Rectangle[] rect = new Rectangle[NumberOFhidden];
            for (int i=0;i< NumberOFhidden; i++)
            {
                rect[i]= new Rectangle(150, i*50, 25, 25);
                g.DrawString("n"+ i.ToString()+"="+ Convert.ToString(Math.Round((Double)output[i], 2)), new Font("Tahoma", 8), Brushes.Blue, rect[i], stringFormat);
                g.DrawRectangle(blackPen, rect[i]);
                g.DrawLine(blackPen, 50, (NumberOFhidden*12)+25, 150, (i*50)+12);
                g.DrawLine(blackPen, 50, (NumberOFhidden*12) + 125, 150, (i*50)+12);
                double c = ((NumberOFhidden * 12) + 25)+ ((i * 50) + 12);
                c = c / 2;
                float cc =(float) Math.Round(c, 2);
                double c1 = ((NumberOFhidden * 12) + 125) + ((i * 50) + 12);
                c1 = c1 / 2;
                float cc1 = (float)Math.Round(c1, 2);
                g.DrawString("w" + (i * 2).ToString() + "="  + Convert.ToString(Math.Round((Double)weights[i * 2], 2)), new Font("Tahoma", 8), Brushes.Black, 100,cc );
                g.DrawString("w" + ((i * 2) +1).ToString() + "=" + Convert.ToString(Math.Round((Double)weights[(i * 2)+1], 2)), new Font("Tahoma", 8), Brushes.Black, 100, cc1+15);
                double c3 = ((i * 50) + 12) + ((NumberOFhidden * 12) + 37);
                c3 = c3 / 2;
                float cc3 = (float)Math.Round(c3, 2);
                g.DrawString("wOut" + (i ).ToString() + "=" + Convert.ToString(Math.Round((Double)weights[i+(NumberOFhidden*2)], 2)), new Font("Tahoma", 8), Brushes.Black, 220, cc3);
                g.DrawLine(blackPen, 175, (i * 50) + 12, 300, (NumberOFhidden * 12) + 37);
                g.DrawLine(blackPen, 162, (i*50)+25, 162, (i*50) + 35);
                g.DrawString("Bais" + i.ToString() + "=" + Convert.ToString(Math.Round((Double)bais[i], 2)), new Font("Tahoma", 8), Brushes.Black, 162, (i * 50) + 30);

            }

            blackPen.EndCap = LineCap.ArrowAnchor;
            blackPen.StartCap = LineCap.RoundAnchor;
            //
            
            // Draw rectangle to screen.
            
            

            

        }
        public void drawWeights1(double[] weights,double[] bais,double[] output, double[] input)
        {
            
            Graphics g = panel1.CreateGraphics();
            // Create pen.
            Pen blackPen = new Pen(Color.Red, 3);

            // Create rectangle.
            Rectangle rect1 = new Rectangle(0, 0, 50, 50);
            Rectangle rect2 = new Rectangle(0, 100, 50, 50);
            Rectangle rect3 = new Rectangle(150, 0, 50, 50);
            Rectangle rect4 = new Rectangle(150, 100, 50, 50);
            Rectangle rect5 = new Rectangle(250, 50, 50, 50);
            blackPen.EndCap = LineCap.ArrowAnchor;
            blackPen.StartCap = LineCap.RoundAnchor;
            //
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            // Draw rectangle to screen.
            g.DrawString("Input1="+ Convert.ToString(input[0]), new Font("Tahoma", 8), Brushes.Blue, rect1, stringFormat);
            g.DrawRectangle(blackPen, rect1);
            g.DrawString("Input2=" + Convert.ToString(input[1]), new Font("Tahoma", 8), Brushes.Blue, rect2, stringFormat);
            g.DrawRectangle(blackPen, rect2);
            g.DrawString("n1= "+ Convert.ToString(Math.Round((Double)output[0], 2)), new Font("Tahoma", 8), Brushes.Blue, rect3, stringFormat);
            g.DrawRectangle(blackPen, rect3);
            g.DrawString("n2=" + Convert.ToString(Math.Round((Double)output[1], 2)), new Font("Tahoma", 8), Brushes.Blue, rect4, stringFormat);
            g.DrawRectangle(blackPen, rect4);
            g.DrawString("Output=" + Convert.ToString(Math.Round((Double)output[2], 4)), new Font("Tahoma", 8), Brushes.Blue, rect5, stringFormat);
            g.DrawRectangle(blackPen, rect5);
            
            g.DrawString("w1="+Convert.ToString(Math.Round((Double)weights[0], 2)), new Font("Tahoma", 8), Brushes.Black, 70, 25);
            g.DrawLine(blackPen, 50, 25, 145, 25);
            g.DrawString("w3="+ Convert.ToString(Math.Round((Double)weights[2], 2)), new Font("Tahoma", 8), Brushes.Black, 110, 75);
            g.DrawLine(blackPen, 50, 25, 141, 120);
            g.DrawString("w4="+ Convert.ToString(Math.Round((Double)weights[3], 2)), new Font("Tahoma", 8), Brushes.Black, 90, 130);
            g.DrawLine(blackPen, 50, 125, 145, 125);
            g.DrawString("w2="+ Convert.ToString(Math.Round((Double)weights[1], 2)), new Font("Tahoma", 8), Brushes.Black, 40, 70);
            g.DrawLine(blackPen, 50, 125, 141, 28);
            g.DrawString("w5="+ Convert.ToString(Math.Round((Double)weights[4], 2)), new Font("Tahoma", 8), Brushes.Black, 225, 40);
            g.DrawLine(blackPen, 200, 25, 250, 75);
            g.DrawString("w6="+ Convert.ToString(Math.Round((Double)weights[5], 2)), new Font("Tahoma", 8), Brushes.Black, 220, 100);
            g.DrawLine(blackPen, 200, 125, 248, 78);
            g.DrawString("Bais1="+ Convert.ToString(Math.Round((Double)bais[0], 2)), new Font("Tahoma", 8), Brushes.Black, 175, 60);
            g.DrawLine(blackPen, 175, 50, 175, 70);
            g.DrawString("Bais2=" + Convert.ToString(Math.Round((Double)bais[1], 2)), new Font("Tahoma", 8), Brushes.Black, 175, 160);
            g.DrawLine(blackPen, 175, 150, 175, 170);
            g.DrawString("Bais=" + Convert.ToString(Math.Round((Double)bais[2], 2)), new Font("Tahoma", 8), Brushes.Black, 275, 110);
            g.DrawLine(blackPen, 275, 100, 275, 125);

        }
        private void button2_Click(object sender, EventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            // Create pen.
            Pen blackPen = new Pen(Color.Red, 3);

            // Create rectangle.
            Rectangle rect1 = new Rectangle(0, 0, 50, 50);
            Rectangle rect2 = new Rectangle(0, 100, 50, 50);
            Rectangle rect3 = new Rectangle(150, 0, 50, 50);
            Rectangle rect4 = new Rectangle(150, 100, 50, 50);
            Rectangle rect5 = new Rectangle(250, 50, 50, 50);
            blackPen.EndCap = LineCap.ArrowAnchor;
            blackPen.StartCap = LineCap.RoundAnchor;
            //
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            // Draw rectangle to screen.
            g.DrawString("Input1", new Font("Tahoma", 8), Brushes.Blue, rect1, stringFormat);
            g.DrawRectangle(blackPen, rect1);
            g.DrawString("Input2", new Font("Tahoma", 8), Brushes.Blue, rect2, stringFormat);
            g.DrawRectangle(blackPen, rect2);
            g.DrawString("n1", new Font("Tahoma", 8), Brushes.Blue, rect3, stringFormat);
            g.DrawRectangle(blackPen, rect3);
            g.DrawString("n2", new Font("Tahoma", 8), Brushes.Blue, rect4, stringFormat);
            g.DrawRectangle(blackPen, rect4);
            g.DrawString("Output", new Font("Tahoma", 8), Brushes.Blue, rect5, stringFormat);
            g.DrawRectangle(blackPen, rect5);
            g.DrawString("w1", new Font("Tahoma", 8), Brushes.Black, 70, 25);
            g.DrawLine(blackPen, 50, 25, 145,25 );
            g.DrawString("w3", new Font("Tahoma", 8), Brushes.Black, 110, 75);
            g.DrawLine(blackPen, 50, 25, 141,120 );
            g.DrawString("w4", new Font("Tahoma", 8), Brushes.Black, 110, 130);
            g.DrawLine(blackPen, 50, 125, 145, 125);
            g.DrawString("w2", new Font("Tahoma", 8), Brushes.Black, 70, 70);
            g.DrawLine(blackPen, 50, 125, 141, 28);
            g.DrawString("w5", new Font("Tahoma", 8), Brushes.Black, 225, 40);
            g.DrawLine(blackPen, 200, 25, 250, 75);
            g.DrawString("w6", new Font("Tahoma", 8), Brushes.Black, 220, 100);
            g.DrawLine(blackPen, 200, 125, 248, 78);
            g.DrawString("Bais1", new Font("Tahoma", 8), Brushes.Black, 175, 60);
            g.DrawLine(blackPen, 175, 50, 175, 70);
            g.DrawString("Bais2", new Font("Tahoma", 8), Brushes.Black, 175, 160);
            g.DrawLine(blackPen, 175, 150, 175, 170);
            g.DrawString("Bais", new Font("Tahoma", 8), Brushes.Black, 275, 110);
            g.DrawLine(blackPen, 275, 100, 275, 125);

        }
        public void DrawRectangleRectangle(PaintEventArgs e)
        {

            
        }
        class OutPutNeuronLayer
        {
            public double[] inputs = new double[NumberOFhidden];
            public double[] weights = new double[NumberOFhidden];
            public double error;

            public double biasWeight;

            private Random randomArray = new Random();

            public double output
            {

                get { return sigmoid.output(equ()); }
            }
            public double equ()
            {
                double counter = 0;
                for (int i = 0; i < NumberOFhidden; i++)
                {
                    counter += weights[i] * inputs[i];
                }
                counter += biasWeight;
                return counter;
            }
            public void randomizeWeights()
            {
                //weights[0] = randomArray.NextDouble();
                //weights[1] = randomArray.NextDouble();
                biasWeight = randomArray.NextDouble();
                for (int i = 0; i < NumberOFhidden; i++)
                {
                    weights[i] = randomArray.NextDouble();
                }
            }

            public void adjustWeights()
            {
                //weights[0] += error * inputs[0];
                //weights[1] += error * inputs[1];
                biasWeight += error;
                //biasWeight = randomArray.NextDouble();
                for (int i = 0; i < NumberOFhidden; i++)
                {
                    weights[i] += error * inputs[i];
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
