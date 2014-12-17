using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelixToolkit.HID.SpaceNavigator
{
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
}
