using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionPlanner.Swarm.KalmanFilter
{
    class LinearAlgebra
    {
        static public void Print(double[,] A)
        {
            for (int row = 0; row < A.GetLength(0); row++)
            {
                for (int col = 0; col < A.GetLength(1); col++)
                {
                    Console.Write(A[row, col] + " ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
        static public void Print(double[] A) //Method overload
        {
            for (int i = 0; i < A.GetLength(0); i++)
            {
                Console.Write(A[i] + " ");
            }
            Console.WriteLine("");
        }

        static public double[,] Transpose(double[,] A)
        {

            int nRow = A.GetLength(0);
            int nCol = A.GetLength(1);

            double[,] B = new double[nCol, nRow];
            for (int row = 0; row < nRow; row++)
            {
                for (int col = 0; col < nCol; col++)
                {
                    B[col, row] = A[row, col];
                }
            }
            return B;

            /*
            double[,] B = new double[3, 2] { {0.2785,0.9649}, {0.5469,0.1576 },{0.9575,0.9706 } };

            double[,] C = matrixMath.tranpose(B);

            matrixMath.print(C);
            */
        }

        static public double[,] Add(double[,] A, double[,] B)
        {
            //A = a x b. B = c x d
            int a = A.GetLength(0);
            int b = A.GetLength(1); //Number of columns in A
            int c = B.GetLength(0); //Number of rows in B
            int d = B.GetLength(1);



            if (a != c | b != d)
            {
                Console.WriteLine("Error: Matrix dimensions not equal.");
                return null;
            }
            else
            {
                double[,] C = new double[a, b];
                for (int row = 0; row < a; row++)
                {
                    for (int col = 0; col < b; col++)
                    {
                        C[row, col] = A[row, col] + B[row, col];
                    }
                }
                return C;
            }

            /*
            double[,] A = new double[2, 3] { { 0.8147, 0.1270, 0.6324 }, { 0.9058, 0.9134, 0.0975 } };
            double[,] B = new double[3, 2] { { 0.2785, 0.9649 }, { 0.5469, 0.1576 }, { 0.9575, 0.9706 } };


            double[,] C = matrixMath.add(A, B);

            double[,] A = new double[2, 2] { { 1, 2 }, { 3, 4 } };
            double[,] B = new double[2, 2] { { 5, 6 }, { 7, 8 } };


            double[,] C = matrixMath.add(A, B);
             matrixMath.print(C);
            */

        }
        static public double[] Add(double[] A, double[] B) //Method overload
        {
            int a = A.GetLength(0);
            int c = B.GetLength(0);



            if (a != c)
            {
                Console.WriteLine("Error: Vector dimensions must be equal for addition.");
                return null;
            }
            else
            {
                double[] C = new double[a];
                for (int i = 0; i < a; i++)
                {
                    C[i] = A[i] + B[i];
                }
                return C;
            }

        }

        static public double[,] Subtract(double[,] A, double[,] B)
        {
            //A-B

            //A = a x b. B = c x d
            int a = A.GetLength(0);
            int b = A.GetLength(1); //Number of columns in A
            int c = B.GetLength(0); //Number of rows in B
            int d = B.GetLength(1);

            double[,] C = new double[a, b];

            if (a != c | b != d)
            {
                Console.WriteLine("Error: Matrix dimensions not equal. Cannot subtract");
                return null;
            }
            else
            {
                for (int row = 0; row < a; row++)
                {
                    for (int col = 0; col < b; col++)
                    {
                        C[row, col] = A[row, col] - B[row, col];
                    }
                }
                return C;
            }

        }
        static public double[] Subtract(double[] A, double[] B) //Method overload
        {
            //A-B
            int a = A.GetLength(0);
            int b = B.GetLength(0);

            if (a != b)
            {
                Console.WriteLine("Error: Vectors are not the same length. Cannot subtract");
                return null;
            }
            else
            {
                double[] C = new double[a];
                for (int i = 0; i < a; i++)
                {
                    C[i] = A[i] - B[i];
                }
                return C;
            }
        }

        static public double[,] Multiply(double[,] A, double[,] B)
        {
            //A = a x b. B = c x d
            int a = A.GetLength(0);
            int b = A.GetLength(1); //Number of columns in A
            int c = B.GetLength(0); //Number of rows in B
            int d = B.GetLength(1);

            double[,] C = new double[a, d];
            double sum;

            if (b != c)
            {
                Console.WriteLine("Error: Matrix inner dimensions not equal.");
                return null;
            }
            else
            {

                for (int row = 0; row < a; row++)
                {

                    for (int col = 0; col < d; col++)
                    {
                        sum = 0;
                        for (int k = 0; k < c; k++)
                        {
                            sum = sum + A[row, k] * B[k, col];
                        }
                        C[row, col] = sum;
                    }

                }


                return C;
            }

            /*
    double[,] A = new double[2, 3] { {0.8147,0.1270,0.6324 }, {0.9058,0.9134,0.0975} };
    double[,] B = new double[3, 2] { {0.2785,0.9649}, {0.5469,0.1576 },{0.9575,0.9706 } };

    double[,] C = matrixMath.multiply(A,B);

    matrixMath.print(C);
    */
        } //Static means you can use this method without declaring an instance of the class LinearAlgebra.

        static private double EuclideanNorm(double[] A)
        {
            double norm;
            double sum = 0;

            for (int i = 0; i < A.GetLength(0); i++)
            {
                sum = sum + A[i] * A[i];
            }

            norm = Math.Sqrt(sum);
            return norm;

            /*double[] A = new double[] { 1, 2, 3, 4 };
            double n = matrixMath.euclideanNorm(A);
            Console.Write("Euclidean norm = ");
            Console.WriteLine(n);
            */
        }

        static private double[] VectorProjection(double[] A, double[] B)
        {
            int nItems = A.GetLength(0);
            int nItems2 = B.GetLength(0);

            if (nItems != nItems2)
            {
                Console.WriteLine("Error: Vectors must be same length to perform vector projection.");
                return null;
            }
            else
            {
                double frac = VectorDot(A, B) / VectorDot(A, A);


                double[] vecProjection = new double[nItems];

                for (int i = 0; i < nItems; i++)
                {
                    vecProjection[i] = frac * A[i];
                }

                return vecProjection;
            }

            /*double[] A = new double[] { 1, 2, 3, 4 };
            double[] B = new double[] { 5, 6, 7, 8 };
            double[] n = matrixMath.vectorProjection(A, B);
            Console.Write("Vector projection = ");
            matrixMath.print(n);*/
        }

        static private double VectorDot(double[] A, double[] B)
        {
            int nItemsA = A.GetLength(0);
            int nItemsB = B.GetLength(0);

            if (nItemsA != nItemsB)
            {
                Console.WriteLine("Error: Vectors must be same length to perform the dot producdt");
                return double.NaN;
            }
            else
            {
                double sum = 0;
                for (int i = 0; i < nItemsA; i++)
                {
                    sum = sum + A[i] * B[i];
                }
                return sum;
            }

            /*double[] A = new double[] { 1, 2, 3, 4 };
            double[] B = new double[] { 5, 6, 7, 8 };
            double n = matrixMath.vectorDot(A, B);
            Console.Write("Vector dot = ");
            Console.WriteLine(n);*/
        }

        static public double[,] IdentitySquare(int n)
        {
            double[,] identityMatrix = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                identityMatrix[i, i] = 1;
            }

            return identityMatrix;
        }

        public class Inverse
        {
            class EigResult
            {
                public double[,] eigenvalues;
                public double[,] eigenvectors;
            }

            class QrDecompositionResult
            {
                public double[,] Q;
                public double[,] R;
            }

            static private QrDecompositionResult QrDecompositionGramSchmidt(double[,] A)
            {
                QrDecompositionResult result = new QrDecompositionResult();
                int nRow = A.GetLength(0);
                int nCol = A.GetLength(1);

                double[,] u = new double[nRow, nCol];
                double[,] e = new double[nRow, nCol];
                double[] uTemp = new double[nRow];
                double[] vTemp = new double[nRow];

                double[] ATemp = new double[nRow];

                for (int i = 0; i < nRow; i++)
                {
                    u[i, 0] = A[i, 0];
                    uTemp[i] = u[i, 0];
                }

                double n = EuclideanNorm(uTemp); //norm(u(:,1));

                for (int i = 0; i < nRow; i++)
                {
                    e[i, 0] = u[i, 0] / n; //e(:,1) = u(:,1)/norm(u(:,1));
                }


                for (int col = 1; col < nCol; col++)
                {
                    double[] sum = new double[nRow];
                    for (int i = 0; i < (col); i++)
                    {
                        for (int j = 0; j < nRow; j++)
                        {
                            uTemp[j] = u[j, i]; //uTemp = u(:,i)
                            vTemp[j] = A[j, col]; //vTemp - A(:,col)
                        }

                        sum = Add(sum, VectorProjection(uTemp, vTemp)); //sum = sum + vectorProjection(u(:,i),A(:,col))
                    }



                    for (int j = 0; j < nRow; j++)
                    {
                        u[j, col] = A[j, col] - sum[j]; //u(:,col) = A(:,col) - sum;
                    }


                    for (int i = 0; i < nRow; i++)
                    {
                        uTemp[i] = u[i, col]; //uTemp = u(:,col)
                    }

                    n = EuclideanNorm(uTemp); //norm(u(:,col));

                    for (int j = 0; j < nRow; j++)
                    {
                        e[j, col] = u[j, col] / n; //e(:,col) = u(:,col)/norm(u(:,col))
                    }
                }

                result.Q = e;

                double[,] R = new double[nRow, nCol];
                double[] eTemp = new double[nRow];

                for (int row = 0; row < nRow; row++)
                {
                    for (int col = row; col < nCol; col++)
                    {

                        for (int i = 0; i < nRow; i++)
                        {
                            ATemp[i] = A[i, col]; //ATemp = A(:,col)
                            eTemp[i] = e[i, row]; //eTemp = e(:,row)
                        }

                        R[row, col] = VectorDot(ATemp, eTemp); //dot(A(:,col),e(:,row))
                    }
                }

                result.R = R;

                return result;

                /*

                double[,] A = new double[3, 3] { { 1, 1, 0 }, { 1, 0, 1 }, { 0, 1, 1 } };
                //double[,] A = new double[3, 3] { { 12, -51, 4 }, { 6, 167, -68 }, { -4, 24, -41 } };
                //double[,] A = new double[3, 3] { { 2, 1, 0 }, { 1, 3, -1 }, { 0, -1, 6 } };
                qrDecompositionResult result = matrixMath.qrDecompositionGramSchmidt(A);
                Console.WriteLine("QR Decomposition: Q = ");
                matrixMath.print(result.Q);
                Console.WriteLine("QR Decomposition: R = ");
                matrixMath.print(result.R);

                Console.WriteLine("Q*R should be equal to A. Q*R = ");
                matrixMath.print(matrixMath.multiply(result.Q, result.R));
                */
            }

            static private EigResult EigQRbasic(double[,] A, int iterations)
            {
                int nRow = A.GetLength(0);
                int nCol = A.GetLength(1);
                EigResult result = new EigResult();

                if (nRow != nCol)
                {
                    Console.WriteLine("Error: Matrix must be square");
                    result.eigenvalues = null;
                    result.eigenvectors = null;

                    return result;
                }
                else
                {
                    //Uses the basic QR algorithm to determine eigenvalues and eigenvectors of matrix A.

                    QrDecompositionResult qrResult;


                    double[,] A_k = A;

                    qrResult = QrDecompositionGramSchmidt(A_k);

                    double[,] S_k = qrResult.Q;
                    A_k = Multiply(qrResult.R, qrResult.Q);

                    //while (flag)
                    for (int j = 1; j < iterations; j++)
                    {
                        qrResult = QrDecompositionGramSchmidt(A_k);
                        S_k = Multiply(S_k, qrResult.Q);
                        A_k = Multiply(qrResult.R, qrResult.Q);

                        /*
                        List<double> lowerDiagElements = new List<double>(); //Lists have variable sizes. Arrays have fixed size.
                        for (int r = 1; r < nRow; r++)
                        {
                            for (int c = 0; c < r; c++)
                            {
                                lowerDiagElements.Add(A_k[r, c]); //Add the lower diagonal element to the list.
                            }
                        }

                        for (int i = 0; i < lowerDiagElements.Count; i++) //Repeat algorithm until the lower diagonal elements are below the threshold.
                        {
                            if (lowerDiagElements[i] >= tolerance)
                            {
                                flag = flag | true; //If the element is above tolerance, continue QR algorithm until all diagonal elements are below the tolerance level.
                            }
                            else
                            {
                                flag = flag | false;
                            }

                        }*/
                    }


                    result.eigenvalues = new double[nRow, nCol];

                    for (int i = 0; i < nRow; i++)
                    {
                        result.eigenvalues[i, i] = A_k[i, i]; //Eigenvalues are the diagonal elements of A_k.
                    }

                    result.eigenvectors = S_k;

                    return result;
                }

                /*
                double[,] A = new double[3, 3] { { 2, 1, 0 }, { 1, 3, -1 }, { 0, -1, 6 } };

                int iterations = 50;

                eigResult result = matrixMath.eigQRbasic(A, iterations);
                Console.WriteLine("Eigenvalues = ");
                matrixMath.print(result.eigenvalues);
                Console.WriteLine("Eigenvectors = ");
                matrixMath.print(result.eigenvectors);
                */
            }

            static public double[,] InvRG(double[,] A)
            {
                //Calculates the matrix inverse of A


                int nRow = A.GetLength(0);
                int nCol = A.GetLength(1);

                if (nRow != nCol)
                {
                    Console.WriteLine("Error: Matrix must be square to calculate the inverse.");
                    return null;
                }

                else
                {

                    EigResult eigResult;
                    eigResult = EigQRbasic(A, 50);

                    double[,] invEigenvalues = new double[nRow, nCol];

                    for (int i = 0; i < nRow; i++)
                    {
                        invEigenvalues[i, i] = 1 / eigResult.eigenvalues[i, i]; //The eigenvalue matrix is diagonal so the inverse is the same as inverting each element of the diagonal.
                    }

                    double[,] invA = new double[nRow, nCol];
                    invA = LinearAlgebra.Multiply(eigResult.eigenvectors, LinearAlgebra.Multiply(invEigenvalues, LinearAlgebra.Transpose(eigResult.eigenvectors))); // inverse(A) = eigenvectors * inverse(eigenvalues) * inverse(eigenvectors). However, since the eigenvectors are orthogonal, inverse(eigenvectors) = transpose(eigenvectors)



                    return invA;


                }
            }
        }
    }
}
