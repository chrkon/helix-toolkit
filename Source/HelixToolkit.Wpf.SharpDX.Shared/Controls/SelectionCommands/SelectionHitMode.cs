// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectionHitMode.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// <summary>
//   Specifies the selection hit mode.
// </summary>
// <remark>
//   Adapted for ShaprDX based WPF Windows in October 2020
// </remark>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixToolkit.Wpf.SharpDX
{
    /// <summary>
    /// Specifies the selection hit mode.
    /// </summary>
    public enum SelectionHitMode
    {
        /// <summary>
        /// Selects models touching the selection range.
        /// </summary>
        Touch,

        /// <summary>
        /// Selects models completely inside selection range.
        /// </summary>
        Inside,

    }
}