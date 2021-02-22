using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearRegression
{
    class LinearSolver
    {
        public List<MyPoint> pointsCollection { get; set; }
        private List<double> xCollection = new List<double>();
        private List<double> yCollection = new List<double>();

        public LinearSolver(IEnumerable<MyPoint> collection)
        {
            pointsCollection = new List<MyPoint>();
            foreach (MyPoint point in collection) pointsCollection.Add(point);
            InitializeCollection();
        }

        public void InitializeCollection()
        {
            foreach(MyPoint singlePoint in pointsCollection)
            {
                xCollection.Add(singlePoint.X);
                yCollection.Add(singlePoint.Y);
            }
        }

        public double AverageX()
        {
            return xCollection.Average();
        }

        public double AverageY()
        {
            return yCollection.Average();
        }

        public double SolveForA()
        {
            double paramOne = 0;
            double paramTwo = 0;
            foreach(MyPoint point in pointsCollection)
            {
                paramOne += (point.X - AverageX()) * (point.Y - AverageY());
                paramTwo += Math.Pow((point.X - AverageX()), 2);
            }
            return paramOne / paramTwo;
        }

        public double SolveForB()
        {
            return AverageY() - SolveForA() * AverageX();
        }

        public string functionExample()
        {
            return $"Y = {Math.Round(SolveForA(),4)}X + {Math.Round(SolveForB(), 4)}";
        }

        public List<DataPoint> startValues()
        {
            return new List<DataPoint> { new DataPoint(xCollection.Min(), SolveForA() * xCollection.Min() + SolveForB()), new DataPoint(xCollection.Max(), SolveForA() * xCollection.Max() + SolveForB()) };
        }
    }
}
