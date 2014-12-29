using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelixToolkit
{
    public interface ICameraControl
    {
        void Move(double dx = 0.0, double dy = 0.0, double dz = 0.0);
        void Pan(double dx = 0.0, double dy = 0.0, double dz = 0.0);
        void Rotate(double dx = 0.0, double dy = 0.0);
        void Zoom(double delta = 0.0);
    }
}
