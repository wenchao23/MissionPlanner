using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionPlanner.Swarm.KalmanFilter
{
    class KalmanFilterMath
    {

        public class KalmanFilterMatrices
        {
            public double[,] F;
            public double[,] H;
            public double[,] Q;
            public double[,] R;
            public double[,] covariancePredict;
            public double[,] covarianceUpdate;
            public double[,] statePredict;
            public double[,] stateUpdate;
            public double[,] innovation;
            public double[,] S;
            public double[,] K; //Kalman filter gain
            public int nUAV = 1;
            public double sigmaX = 0.000002;
            public double sigmaXdot = 0.000002;
            public double sigmaGPS = 0.000002;

        }

        static public void RunKalmanFilter(KalmanFilterMatrices kalmanFilterMatrices, double deltaT, double[,] zGPS)
        {
            kalmanFilterMatrices.F = UpdateF(kalmanFilterMatrices.F, deltaT);

            kalmanFilterMatrices.statePredict = LinearAlgebra.Multiply(kalmanFilterMatrices.F, kalmanFilterMatrices.stateUpdate);

            kalmanFilterMatrices.covariancePredict = LinearAlgebra.Add(LinearAlgebra.Multiply(LinearAlgebra.Multiply(kalmanFilterMatrices.F, kalmanFilterMatrices.covarianceUpdate), LinearAlgebra.Transpose(kalmanFilterMatrices.F)), kalmanFilterMatrices.Q);

            kalmanFilterMatrices.innovation = LinearAlgebra.Subtract(zGPS, LinearAlgebra.Multiply(kalmanFilterMatrices.H, kalmanFilterMatrices.statePredict));

            kalmanFilterMatrices.S = LinearAlgebra.Add(LinearAlgebra.Multiply(LinearAlgebra.Multiply(kalmanFilterMatrices.H, kalmanFilterMatrices.covariancePredict), LinearAlgebra.Transpose(kalmanFilterMatrices.H)), kalmanFilterMatrices.R);


            double[,] invS = LinearAlgebra.Inverse.InvRG(kalmanFilterMatrices.S);
            //Console.WriteLine("S*invS = Identity Matrix? = ");
           // LinearAlgebra.Print(LinearAlgebra.Multiply(kalmanFilterMatrices.S, invS));

            kalmanFilterMatrices.K = LinearAlgebra.Multiply(LinearAlgebra.Multiply(kalmanFilterMatrices.covariancePredict, LinearAlgebra.Transpose(kalmanFilterMatrices.H)), invS);

            kalmanFilterMatrices.stateUpdate = LinearAlgebra.Add(kalmanFilterMatrices.statePredict, LinearAlgebra.Multiply(kalmanFilterMatrices.K, kalmanFilterMatrices.innovation));

            kalmanFilterMatrices.covarianceUpdate = LinearAlgebra.Multiply(LinearAlgebra.Subtract(LinearAlgebra.IdentitySquare(6 * kalmanFilterMatrices.nUAV), LinearAlgebra.Multiply(kalmanFilterMatrices.K, kalmanFilterMatrices.H)), kalmanFilterMatrices.covariancePredict);
        }

        static private double[,] UpdateF(double[,] F, double deltaT)
        {
            int nRow = F.GetLength(0);

            int c = 1;
            for (int r = 0; r < nRow; r = r + 2)
            {
                F[r, c] = deltaT;
                c = c + 2;
            }
            return F;
        }
    }
}
