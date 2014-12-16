// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Wpf.Input.SpaceNavigatorDecorator.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// <summary>
//   Space navigator class in old namespace. Only used to generate the obsolete error during compile.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;

namespace HelixToolkit.Wpf.Input
{
    private static class Info
    {
        internal static string Message =
            "Namespace 'HelixToolkit.Wpf.Input' is obsolete. Please use Namespace 'HelixToolkit.Wpf.HID' instead (https://github.com/helix-toolkit/helix-toolkit/issues/100)";
    }

    [Obsolete(Info.Message, true)]
    public enum SpaceNavigatorType
    {
        UnknownDevice = 0,
        SpaceNavigator = 6,
        SpaceExplorer = 4,
        SpaceTraveler = 25,
        SpacePilot = 29
    }

    [Obsolete(Info.Message, true)]
    public enum SpaceNavigatorZoomMode
    {
        InOut,
        UpDown
    }

    [Obsolete(Info.Message, true)]
    public class SpaceNavigatorDecorator : Decorator
    {
        [Obsolete(Info.Message, true)]
        public static readonly DependencyProperty CameraControlProperty = DependencyProperty.Register(
            "CameraController", typeof(CameraController), typeof(SpaceNavigatorDecorator), new UIPropertyMetadata(null));

        [Obsolete(Info.Message, true)]
        public static readonly RoutedEvent ConnectionChangedEvent = EventManager.RegisterRoutedEvent(
            "ConnectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SpaceNavigatorDecorator));

        [Obsolete(Info.Message, true)]
        public static readonly DependencyProperty IsConnectedProperty = DependencyProperty.Register(
            "IsConnected", typeof(bool), typeof(SpaceNavigatorDecorator), new UIPropertyMetadata(false));

        [Obsolete(Info.Message, true)]
        public static readonly DependencyProperty IsPanEnabledProperty = DependencyProperty.Register(
            "IsPanEnabled", typeof(bool), typeof(SpaceNavigatorDecorator), new UIPropertyMetadata(false));

        [Obsolete(Info.Message, true)]
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(
            "Type",
            typeof(SpaceNavigatorType),
            typeof(SpaceNavigatorDecorator),
            new UIPropertyMetadata(SpaceNavigatorType.UnknownDevice));

        [Obsolete(Info.Message, true)]
        public static readonly DependencyProperty NavigatorNameProperty = DependencyProperty.Register(
            "NavigatorName", typeof(string), typeof(SpaceNavigatorDecorator), new UIPropertyMetadata(null));

        [Obsolete(Info.Message, true)]
        public static readonly DependencyProperty SensivityProperty = DependencyProperty.Register(
            "Sensitivity", typeof(double), typeof(SpaceNavigatorDecorator), new UIPropertyMetadata(1.0));

        [Obsolete(Info.Message, true)]
        public static readonly DependencyProperty ZoomModeProperty = DependencyProperty.Register(
            "ZoomMode",
            typeof(SpaceNavigatorZoomMode),
            typeof(SpaceNavigatorDecorator),
            new UIPropertyMetadata(SpaceNavigatorZoomMode.UpDown));

        [Obsolete(Info.Message, true)]
        public static readonly DependencyProperty ZoomSensivityProperty = DependencyProperty.Register(
            "ZoomSensitivity", typeof(double), typeof(SpaceNavigatorDecorator), new UIPropertyMetadata(1.0));

        [Obsolete(Info.Message, true)]
        public SpaceNavigatorDecorator()
        {
        }

        [Obsolete(Info.Message, true)]
        public event RoutedEventHandler ConnectionChanged
        {
            add {}
            remove {}
        }

        [Obsolete(Info.Message, true)]
        public CameraController CameraController { get; set; }

        [Obsolete(Info.Message, true)]
        public bool IsConnected { get; set; }

        [Obsolete(Info.Message, true)]
        public bool IsPanEnabled { get; set; }

        [Obsolete(Info.Message, true)]
        public string NavigatorName { get; set; }

        [Obsolete(Info.Message, true)]
        public double Sensitivity  { get; set; }

        [Obsolete(Info.Message, true)]
        public SpaceNavigatorType Type  { get; set; }

        [Obsolete(Info.Message, true)]
        public SpaceNavigatorZoomMode ZoomMode  { get; set; }

        [Obsolete(Info.Message, true)]
        public double ZoomSensitivity  { get; set; }

        [Obsolete(Info.Message, true)]
        private CameraController Controller  { get; }

        [Obsolete(Info.Message, true)]
        public void Disconnect() { }

        [Obsolete(Info.Message, true)]
        protected virtual void RaiseConnectionChanged()  { }

    }
}