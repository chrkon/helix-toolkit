﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HIDDecorator.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// <summary>
//   Decorator for HelixViewport3D which connects an HID Device
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixToolkit.Wpf.Input
{
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using HelixToolkit;

    /// <summary>
    /// A decorator for the space navigator.
    /// </summary>
    public class HIDDecorator : Decorator
    {
        /// <summary>
        /// The HID property.
        /// </summary>
        public static readonly DependencyProperty HIDProperty = DependencyProperty.Register(
            "HumanInterfaceDevice", typeof(IHumanInterfaceDevice), typeof(HIDDecorator), new UIPropertyMetadata(null));
        
        /// <summary>
        /// The camera control property.
        /// </summary>
        public static readonly DependencyProperty CameraControlProperty = DependencyProperty.Register(
            "CameraController", typeof(CameraController), typeof(HIDDecorator), new UIPropertyMetadata(null));

        /// <summary>
        /// The connection changed event.
        /// </summary>
        public static readonly RoutedEvent ConnectionChangedEvent = EventManager.RegisterRoutedEvent(
            "ConnectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(HIDDecorator));

        /// <summary>
        /// The is connected property.
        /// </summary>
        public static readonly DependencyProperty IsConnectedProperty = DependencyProperty.Register(
            "IsConnected", typeof(bool), typeof(HIDDecorator), new UIPropertyMetadata(false));

        /// <summary>
        /// The navigator name property.
        /// </summary>
        public static readonly DependencyProperty HIDNameProperty = DependencyProperty.Register(
            "NavigatorName", typeof(string), typeof(HIDDecorator), new UIPropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref = "HIDDecorator" /> class.
        /// </summary>
        public HIDDecorator()
        {
            this.Connect();
        }

        /// <summary>
        /// Gets or sets the HID (Human Interface Device).
        /// </summary>
        /// <value>The camera controller.</value>
        public IHumanInterfaceDevice HID
        {
            get
            {
                return (IHumanInterfaceDevice)this.GetValue(HIDProperty);
            }

            set
            {
                this.SetValue(HIDProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the camera controller.
        /// </summary>
        /// <value>The camera controller.</value>
        public CameraController CameraController
        {
            get
            {
                return (CameraController)this.GetValue(CameraControlProperty);
            }

            set
            {
                this.SetValue(CameraControlProperty, value);
            }
        }

        /// <summary>
        /// Event when a property has been changed
        /// </summary>
        public event RoutedEventHandler ConnectionChanged
        {
            add
            {
                this.AddHandler(ConnectionChangedEvent, value);
            }

            remove
            {
                this.RemoveHandler(ConnectionChangedEvent, value);
            }
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
                return (bool)this.GetValue(IsConnectedProperty);
            }

            set
            {
                this.SetValue(IsConnectedProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the navigator.
        /// </summary>
        /// <value>The name of the navigator.</value>
        public string HIDName
        {
            get
            {
                return (string)this.GetValue(HIDNameProperty);
            }

            set
            {
                this.SetValue(HIDNameProperty, value);
            }
        }

        /// <summary>
        /// Gets the controller.
        /// </summary>
        /// <value>The controller.</value>
        private CameraController Controller
        {
            get
            {
                // if CameraController is set, use it
                if (this.CameraController != null)
                {
                    return this.CameraController;
                }

                // otherwise use the Child of the Decorator
                var view = this.Child as HelixViewport3D;
                return view == null ? null : view.CameraController;
            }
        }

        /// <summary>
        /// The raise connection changed.
        /// </summary>
        protected virtual void RaiseConnectionChanged()
        {
            // e.Handled = true;
            var args = new RoutedEventArgs(ConnectionChangedEvent);
            this.RaiseEvent(args);
        }

        /// <summary>
        /// The connect action
        /// </summary>
        private void Connect()
        {
            try
            {
                if (this.HID == null) return;
                this.HID.CameraZoom += HID_CameraZoom;
                this.HID.CameraTrack += HID_CameraTrack;
                this.HID.CameraTilt += HID_CameraTilt;
                this.HID.CameraRoll += HID_CameraRoll;
                this.HID.CameraPan += HID_CameraPan;
                this.HID.CameraDolly += HID_CameraDolly;
                this.HID.CameraCrane += HID_CameraCrane;

                this.IsConnected = true;
                this.HIDName = HID.Name;
                RaiseConnectionChanged();
            }
            catch (COMException e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// The disconnect action
        /// </summary>
        private void Disconnect()
        {
            try
            {
                if (this.HID == null) return;
                this.HID.CameraZoom -= HID_CameraZoom;
                this.HID.CameraTrack -= HID_CameraTrack;
                this.HID.CameraTilt -= HID_CameraTilt;
                this.HID.CameraRoll -= HID_CameraRoll;
                this.HID.CameraPan -= HID_CameraPan;
                this.HID.CameraDolly -= HID_CameraDolly;
                this.HID.CameraCrane -= HID_CameraCrane;

                this.IsConnected = false;
                this.HIDName = string.Empty;
                RaiseConnectionChanged();
            }
            catch (COMException e)
            {
                Trace.WriteLine(e.Message);
            }
        }

         /// <summary>
        /// Camera Zoom = increase/decrease Field of View angle
        /// </summary>
        void HID_CameraZoom(double delta)
        {
            Controller.AddZoomForce(delta);
        }

        /// <summary>
        /// Camera Track = moving camera left/right (perpendicular to ViewDirection and UpDirection)
        /// </summary>
        void HID_CameraTrack(double delta)
        {
            var moveDirection = System.Windows.Media.Media3D.Vector3D.CrossProduct(Controller.CameraLookDirection, Controller.CameraUpDirection);
            moveDirection = moveDirection * delta;
            Controller.AddMoveForce(moveDirection);
        }

        /// <summary>
        /// Camera Dolly = moving camera forward/backward (along ViewDirection)
        /// </summary>
        void HID_CameraDolly(double delta)
        {
            var moveDirection = Controller.CameraLookDirection * delta;
            Controller.AddMoveForce(moveDirection);
        }

        /// <summary>
        /// Camera Crane = moving camera vertical up/down (along UpDirection)
        /// </summary>
        void HID_CameraCrane(double delta)
        {
            var moveDirection = Controller.CameraUpDirection * delta;
            Controller.AddMoveForce(moveDirection);
        }
        
        /// <summary>
        /// Camera Tilt = turning camera up/down around Vector perpendicular to UpDirection and ViewDirection (Aeroplane : Pitch)
        /// ViewDirection and UpDirection will be changed
        /// </summary>
        void HID_CameraTilt(double angle)
        {
            var rotationAxis = System.Windows.Media.Media3D.Vector3D.CrossProduct(Controller.CameraLookDirection, Controller.CameraUpDirection);
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Camera Rolling = rotate camera left/right around ViewDirection (Aeroplane : Roll)
        /// only UpDirection will be changed
        /// </summary>
        void HID_CameraRoll(double angle)
        {
            var rotationAxis = Controller.CameraLookDirection;
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Camera Panning = turning camera left/right around UpDirection (Aeroplane : Yaw)
        /// only ViewDirection will be changed
        /// </summary>
        void HID_CameraPan(double angle)
        {
            var rotationAxis = Controller.CameraUpDirection;
            throw new System.NotImplementedException();
        }


    }
}