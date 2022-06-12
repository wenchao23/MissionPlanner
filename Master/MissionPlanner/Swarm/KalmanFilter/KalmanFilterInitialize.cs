using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionPlanner.Swarm.KalmanFilter
{
    class KalmanFilterInitialize
    {
        
        static public KalmanFilterMath.KalmanFilterMatrices InitializeKalmanFilter(double[,] zGPS)
        {
             int nUAV = 1;
         double sigmaX = 0.000002;
         double sigmaXdot = 0.000002;
         double sigmaGPS = 0.000002;

        KalmanFilterMath.KalmanFilterMatrices kalmanFilterMatrices = new KalmanFilterMath.KalmanFilterMatrices(); //Create a (local) instance of the class KalmanFilterMatrices.

            kalmanFilterMatrices.F = InitializeF(nUAV);
            kalmanFilterMatrices.H = InitializeH(nUAV);
            kalmanFilterMatrices.Q = InitializeQ(nUAV, sigmaX, sigmaXdot);
            kalmanFilterMatrices.R = InitializeR(nUAV, sigmaGPS);

            kalmanFilterMatrices.stateUpdate = InitializeState(nUAV, zGPS);
            kalmanFilterMatrices.covarianceUpdate = InitializeCovariance(nUAV,sigmaGPS);

            return kalmanFilterMatrices;
        }

        static private double[,] InitializeF(int nUAV)
        {
            double[,] F = new double[6 * nUAV, 6 * nUAV];

            for (int i = 0; i < 6 * nUAV; i++)
            {
                F[i, i] = 1;
            }

            return F;
        }

        static private double[,] InitializeH(int nUAV)
        {
            double[,] H = new double[3 * nUAV, 6 * nUAV];

            int c = 0;
            for (int r = 0; r < 3 * nUAV; r++)
            {
                H[r, c] = 1;
                c = c + 2;
            }

            return H;
        }

        static private double[,] InitializeQ(int nUAV, double sigmaX, double sigmaXdot)
        {
            double[,] Q = new double[6 * nUAV, 6 * nUAV];

            for (int i = 0; i < 6 * nUAV; i = i + 2)
            {
                Q[i, i] = sigmaX * sigmaX;
            }

            for (int i = 1; i < 6 * nUAV; i = i + 2)
            {
                Q[i, i] = sigmaXdot * sigmaXdot;
            }
            return Q;
        }

        static private double[,] InitializeR(int nUAV, double sigmaGPS)
        {
            double[,] R = new double[3 * nUAV, 3 * nUAV];

            for (int i = 0; i < 3 * nUAV; i++)
            {
                R[i, i] = sigmaGPS * sigmaGPS;
            }

            return R;
        }

        static private double[,] InitializeState(int nUAV, double[,] zGPS)
        {
            double[,] state = new double[6 * nUAV, 1];
            int j = 0;

            for (int r = 0; r < 6 * nUAV; r = r + 2)
            {
                state[r, 0] = zGPS[j, 0];
                j = j + 1;
            }

            return state;
        }

        static private double[,] InitializeCovariance(int nUAV, double sigmaGPS)
        {
            double[,] covariance = new double[6 * nUAV, 6 * nUAV];

            for (int i = 0; i < 6 * nUAV; i++)
            {
                covariance[i, i] = sigmaGPS * sigmaGPS;
            }

            return covariance;
        }
    }
}
