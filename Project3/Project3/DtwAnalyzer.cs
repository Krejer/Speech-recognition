using System;
using System.Collections.Generic;

namespace Project3
{
    public class DtwAnalyzer
    {
        public List<double[]> FramesX { get; private set; }
        public List<double[]> FramesY { get; private set; }

        public double[,]? LocalDistanceMatrix { get; private set; }
        public double[,]? GlobalCostMatrix { get; private set; }

        public double[]? OptimalPathX { get; private set; }
        public double[]? OptimalPathY { get; private set; }

        public double TotalDistance { get; private set; }

        public DtwAnalyzer(List<double[]> framesX, List<double[]> framesY)
        {
            FramesX = framesX ?? throw new ArgumentNullException(nameof(framesX));
            FramesY = framesY ?? throw new ArgumentNullException(nameof(framesY));
        }

        public void CalculateDtw()
        {
            if (LocalDistanceMatrix != null) return;

            int nx = FramesX.Count;
            int ny = FramesY.Count;

            LocalDistanceMatrix = new double[ny, nx];
            GlobalCostMatrix = new double[ny, nx];

            for (int y = 0; y < ny; y++)
            {
                for (int x = 0; x < nx; x++)
                {
                    LocalDistanceMatrix[y, x] = CalculateEuclideanDistance(FramesX[x], FramesY[y]);
                }
            }

            GlobalCostMatrix[0, 0] = LocalDistanceMatrix[0, 0];

            for (int y = 1; y < ny; y++)
                GlobalCostMatrix[y, 0] = GlobalCostMatrix[y - 1, 0] + LocalDistanceMatrix[y, 0];

            for (int x = 1; x < nx; x++)
                GlobalCostMatrix[0, x] = GlobalCostMatrix[0, x - 1] + LocalDistanceMatrix[0, x];

            for (int y = 1; y < ny; y++)
            {
                for (int x = 1; x < nx; x++)
                {
                    double minPrev = Math.Min(GlobalCostMatrix[y - 1, x - 1],
                                     Math.Min(GlobalCostMatrix[y, x - 1],
                                              GlobalCostMatrix[y - 1, x]));

                    GlobalCostMatrix[y, x] = LocalDistanceMatrix[y, x] + minPrev;
                }
            }

            TotalDistance = GlobalCostMatrix[ny - 1, nx - 1];

            List<double> pathX = new List<double>();
            List<double> pathY = new List<double>();

            int currX = nx - 1;
            int currY = ny - 1;

            pathX.Add(currX);
            pathY.Add(currY);

            while (currX > 0 || currY > 0)
            {
                if (currX == 0) currY--;
                else if (currY == 0) currX--;
                else
                {
                    double diag = GlobalCostMatrix[currY - 1, currX - 1];
                    double left = GlobalCostMatrix[currY, currX - 1];
                    double down = GlobalCostMatrix[currY - 1, currX];

                    if (diag <= left && diag <= down)
                    {
                        currX--; currY--;
                    }
                    else if (left < diag && left < down) currX--;
                    else currY--;
                }
                pathX.Add(currX);
                pathY.Add(currY);
            }

            OptimalPathX = pathX.ToArray();
            OptimalPathY = pathY.ToArray();
        }

        private double CalculateEuclideanDistance(double[] vecA, double[] vecB)
        {
            double sum = 0;
            int length = Math.Min(vecA.Length, vecB.Length);
            for (int i = 0; i < length; i++)
            {
                double diff = vecA[i] - vecB[i];
                sum += diff * diff;
            }
            return Math.Sqrt(sum);
        }
    }
}