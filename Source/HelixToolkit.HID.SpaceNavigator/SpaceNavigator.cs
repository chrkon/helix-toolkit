// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpaceNavigatorDecorator.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// <summary>
//   Space navigator type enumeration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixToolkit.HID
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using TDx.TDxInput;

    /// <summary>
    /// Space navigator type enumeration.
    /// </summary>
    public enum SpaceNavigatorType
    {
        /// <summary>
        /// Is a unknown device.
        /// </summary>
        UnknownDevice = 0,

        /// <summary>
        /// Is a SpaceNavigator.
        /// </summary>
        SpaceNavigator = 6,

        /// <summary>
        /// Is a SpaceExplorer.
        /// </summary>
        SpaceExplorer = 4,

        /// <summary>
        /// Is a SpaceTraveler.
        /// </summary>
        SpaceTraveler = 25,

        /// <summary>
        /// Is a SpacePilot.
        /// </summary>
        SpacePilot = 29
    }

    /// <summary>
    /// Zoom mode.
    /// </summary>
    public enum SpaceNavigatorZoomMode
    {
        /// <summary>
        /// In and out.
        /// </summary>
        InOut,

        /// <summary>
        /// Up and down.
        /// </summary>
        UpDown
    }

    /// <summary>
    /// A decorator for the space navigator.
    /// </summary>
    public class SpaceNavigator
    {
        /// <summary>
        /// The _input.
        /// </summary>
        private Device _input;

        /// <summary>
        /// The _sensor.
        /// </summary>
        private Sensor _sensor;

        /// <summary>
        /// the _isConnected flag.
        /// </summary>
        private bool _isConnected;

        private string _navigatorName;

        private SpaceNavigatorType _navigatorType;

        private SpaceNavigatorZoomMode _navigatorZoomMode;

        private double _sensitivity;

        private double _zoomSensitivity;

        /// <summary>
        /// Event when a property has been changed
        /// </summary>
        public event EventHandler ConnectionChanged;

        public event Action<Tuple<double, double, double>> MoveCamera;
        public event Action<Tuple<double, double>> PanCamera;
        public event Action<Tuple<double, double>> RotateCamera;
        public event Action<Tuple<double>> ZoomCamera;

        /// <summary>
        /// Initializes a new instance of the <see cref = "SpaceNavigator" /> class.
        /// </summary>
        public SpaceNavigator()
        {
            // connect empty event handler as default to prevent null reference exception
            MoveCamera += (T) => { };
            PanCamera += (T) => { };
            RotateCamera += (T) => { };
            ZoomCamera += (T) => { };

            this.Connect();

            /* todo: try to start driver if not available
             Thread.Sleep(1000);
                        if (!IsConnected)
                        {
                            Disconnect();
                            Thread.Sleep(1000);
                            StartDriver();
                            Connect();
                        }*/
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }

            set
            {
                _isConnected = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the navigator.
        /// </summary>
        /// <value>The name of the navigator.</value>
        public string NavigatorName
        {
            get { return _navigatorName; }
            set { _navigatorName = value; }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public SpaceNavigatorType Type
        {
            get { return _navigatorType; }
            set { _navigatorType = value; }
        }

        /// <summary>
        /// Gets or sets the zoom mode.
        /// </summary>
        /// <value>The zoom mode.</value>
        public SpaceNavigatorZoomMode ZoomMode
        {
            get { return _navigatorZoomMode; }
            set { _navigatorZoomMode = value; }
        }

        public double Sensitivity
        {
            get { return _sensitivity; }
            set { _sensitivity = value; }
        }

        public double ZoomSensitivity
        {
            get { return _zoomSensitivity; }
            set { _zoomSensitivity = value; }
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        public void Disconnect()
        {
            if (this._input != null)
            {
                this._input.Disconnect();
            }

            this._input = null;
            this.IsConnected = false;
            
        }

        /// <summary>
        /// Raise the connection changed event.
        /// </summary>
        protected virtual void RaiseConnectionChanged()
        {
            // e.Handled = true;
            var handler = ConnectionChanged;
            if (handler != null)
            {
                var args = new EventArgs();
                handler(this, args);
            }
        }

        /// <summary>
        /// The connect.
        /// </summary>
        private void Connect()
        {
            try
            {
                this._input = new Device();
                this._sensor = this._input.Sensor;
                this._input.DeviceChange += this.input_DeviceChange;
                this._sensor.SensorInput += this.Sensor_SensorInput;
                this._input.Connect();
            }
            catch (COMException e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        // todo...
        /*
        private void StartDriver()
        {
            string exe = @"C:\Program Files\3Dconnexion\3Dconnexion 3DxSoftware\3DxWare\3dxsrv.exe";
            if (!File.Exists(exe))
                return;
            var p = Process.Start(exe, "-searchWarnDlg");
            Thread.Sleep(2000);
        }

        private void StopDriver()
        {
            string exe = @"C:\Program Files\3Dconnexion\3Dconnexion 3DxSoftware\3DxWare\3dxsrv.exe";
            if (!File.Exists(exe))
                return;
            var p = Process.Start(exe, "-shutdown");
        }
        */

        /// <summary>
        /// The sensor_ sensor input.
        /// </summary>
        private void Sensor_SensorInput()
        {
            RotateCamera(new Tuple<double, double>(
                    this.Sensitivity * this._sensor.Rotation.Y,
                    this.Sensitivity * this._sensor.Rotation.X ));

            if (this.ZoomMode == SpaceNavigatorZoomMode.InOut)
            {
                ZoomCamera(new Tuple<double>(
                    this.ZoomSensitivity * 0.001 * this._input.Sensor.Translation.Z ));
            }

            if (this.ZoomMode == SpaceNavigatorZoomMode.UpDown)
            {
                ZoomCamera(new Tuple<double>(
                    this.ZoomSensitivity * 0.001 * this._sensor.Translation.Y ));

                PanCamera(new Tuple<double,double>(
                    this.Sensitivity * 0.03 * this._sensor.Translation.X,
                    this.Sensitivity * 0.03 * this._sensor.Translation.Z ));                
            }
        }

        /// <summary>
        /// The input_ device change.
        /// </summary>
        /// <param name="reserved">
        /// The reserved.
        /// </param>
        private void input_DeviceChange(int reserved)
        {
            this.IsConnected = true;
            this.Type = (SpaceNavigatorType)this._input.Type;
            this.NavigatorName = this.Type.ToString();
            this.RaiseConnectionChanged();
        }

    }
}