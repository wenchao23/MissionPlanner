using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionPlanner.Swarm.KalmanFilter
{
    class Parameters
    {
        public int nUAV;
        public double sigmaX = 0.2;
        public double sigmaXdot = 0.1;
        public double sigmaGPS = 0.2;
    }
}
