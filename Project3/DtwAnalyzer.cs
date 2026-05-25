using System;
using System.Collections.Generic;

namespace Project3
{
    public enum DtwMetric
    {
        Euclidean,
        Cosine
    }

    public class DtwAnalyzer
    {
        public List<double[]> FramesX { get; private set; }
        public List<double[]> FramesY { get; private set; }

        public double[,]? LocalDistanceMatrix { get; private set; }
        public double[,]? GlobalCostMatrix { get; private set; }

        public double[]? OptimalPathX { get; private set; }
        public double[]? OptimalPathY { get; private set; }

        public double TotalDistance { get; private set; }
        public double NormalizedDistance { get; private set; }

        private DtwMetric _metric;

        public DtwAnalyzer(List<double[]> framesX, List<double[]> framesY, DtwMetric metric = DtwMetric.Euclidean)
        {
            FramesX = framesX ?? throw new ArgumentNullException(nameof(framesX));
            FramesY = framesY ?? throw new ArgumentNullException(nameof(framesY));
            _metric = metric;
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
                    LocalDistanceMatrix[y, x] = _metric == DtwMetric.Euclidean
                        ? CalculateEuclideanDistance(FramesX[x], FramesY[y])
                        : CalculateCosineDistance(FramesX[x], FramesY[y]);
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
                    double costDiag = GlobalCostMatrix[y - 1, x - 1] + 2.0 * LocalDistanceMatrix[y, x];
                    double costLeft = GlobalCostMatrix[y, x - 1] + LocalDistanceMatrix[y, x];
                    double costDown = GlobalCostMatrix[y - 1, x] + LocalDistanceMatrix[y, x];

                    GlobalCostMatrix[y, x] = Math.Min(costDiag, Math.Min(costLeft, costDown));
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

            NormalizedDistance = TotalDistance / (nx + ny);
        }

        private double CalculateEuclideanDistance(double[] vecA, double[] vecB)
        {
            double sum = 0;
            int length = Math.Min(vecA.Length, vecB.Length);
            // Zaczynamy od i = 1, pomijając indeks 0 (C0 dla MFCC, stała (DC) dla FFT)
            for (int i = 1; i < length; i++)
            {
                double diff = vecA[i] - vecB[i];
                sum += diff * diff;
            }
            return Math.Sqrt(sum);
        }

        private double CalculateCosineDistance(double[] vecA, double[] vecB)
        {
            double dotProduct = 0;
            double normA = 0;
            double normB = 0;
            int length = Math.Min(vecA.Length, vecB.Length);

            for (int i = 1; i < length; i++)
            {
                dotProduct += vecA[i] * vecB[i];
                normA += vecA[i] * vecA[i];
                normB += vecB[i] * vecB[i];
            }

            if (normA == 0 || normB == 0) return 1.0;

            double cosineSimilarity = dotProduct / (Math.Sqrt(normA) * Math.Sqrt(normB));
            return 1.0 - cosineSimilarity;
        }
    }
}