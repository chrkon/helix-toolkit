using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelixToolkit
{
    public interface IHumanInterfaceDevice
    {
        /// <summary>
        /// The name of the Human Interface Device
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Camera Track = moving camera horizontal left/right (perpendicular to ViewDirection)
        /// </summary>
        event Action<double> CameraTrack; 
        
        /// <summary>
        /// Camera Crane = moving camera vertical up/down (perpendicular to ViewDirection)
        /// </summary>
        event Action<double> CameraCrane; 
        
        /// <summary>
        /// Camera Dolly = moving camera horizontal forward/backward (along ViewDirection)
        /// </summary>
        event Action<double> CameraDolly;

        /// <summary>
        /// Camera Panning = turning camera left/right around Updirection (Aeroplane : Yaw)
        /// only ViewDirection will be changed
        /// </summary>
        event Action<double> CameraPan;

        /// <summary>
        /// Camera Tilt = turning camera up/down around Vector perpendicular to UpDirection and ViewDirection (Aeroplane : Pitch)
        /// ViewDirection and UpDirection will be changed
        /// </summary>
        event Action<double> CameraTilt;

        /// <summary>
        /// Camera Rolling = rotate camera left/right around ViewDirection (Aeroplane : Roll)
        /// only UpDirection will be changed
        /// </summary>
        event Action<double> CameraRoll;

        /// <summary>
        /// Camera Zoom = increase/decrease Field of View angle
        /// </summary>
        event Action<double> CameraZoom;


    }
}
