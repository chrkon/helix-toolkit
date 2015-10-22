// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinesVisual3D.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// <summary>
//   A visual element that contains a set of line segments. The thickness of the lines is defined in screen space.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixToolkit.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;
  
    /// <summary>
    /// A visual element that contains a set of line segments. The thickness of the lines is defined in screen space.
    /// </summary>
    public class LinesVisual3D : ScreenSpaceVisual3D
    {
        /// <summary>
        /// Identifies the <see cref="Thickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(
            "Thickness", typeof(double), typeof(LinesVisual3D), new UIPropertyMetadata(1.0, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="DashArray"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DashArrayProperty = DependencyProperty.Register(
            "DashArray", typeof(DoubleCollection), typeof(LinesVisual3D), new UIPropertyMetadata(new DoubleCollection {1.0}, GeometryChanged));

        /// <summary>
        /// The builder.
        /// </summary>
        private readonly LineGeometryBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref = "LinesVisual3D" /> class.
        /// </summary>
        public LinesVisual3D()
        {
            this.builder = new LineGeometryBuilder(this);
        }

        /// <summary>
        /// Gets or sets the thickness of the lines.
        /// </summary>
        /// <value>
        /// The thickness.
        /// </value>
        public double Thickness
        {
            get
            {
                return (double)this.GetValue(ThicknessProperty);
            }

            set
            {
                this.SetValue(ThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the DashArray of the line.
        /// </summary>
        /// <value>
        /// The dashArray.
        /// </value>
        public DoubleCollection DashArray
        {
            get
            {
                return (DoubleCollection)this.GetValue(DashArrayProperty);
            }

            set
            {
                this.SetValue(DashArrayProperty, value);
            }
        }


        /// <summary>
        /// Updates the geometry.
        /// </summary>
        protected override void UpdateGeometry()
        {
            if (this.Points == null)
            {
                this.Mesh.Positions = null;
                return;
            }

           var linePoints = createLinePointsForDottedLine(this.Points, this.Thickness, this.DashArray);      

            int n = linePoints.Count;
            if (n > 0)
            {
                if (this.Mesh.TriangleIndices.Count != n * 3)
                {
                    this.Mesh.TriangleIndices = this.builder.CreateIndices(n);
                }

                //this.Mesh.Positions = this.builder.CreatePositions(this.Points, this.Thickness, this.DepthOffset);
                this.Mesh.Positions = this.builder.CreatePositions(linePoints, this.Thickness, this.DepthOffset);
            }
            else
            {
                this.Mesh.Positions = null;
            }
        }

        /// <summary>
        /// Updates the transforms.
        /// </summary>
        /// <returns>
        /// True if the transform is updated.
        /// </returns>
        protected override bool UpdateTransforms()
        {
            return this.builder.UpdateTransforms();
        }


        private static IList<Point3D> createLinePointsForDottedLine(IList<Point3D> points, double thickness, DoubleCollection dashArray)
        {
            // create subpoints für dotted/dashed line
            int dashparts = dashArray == null ? 1 : dashArray.Count;

            IList<Point3D> linePoints = null;

            if (dashparts < 2)
            {
                // normal line
                linePoints = points;
            }
            else
            {
                // create sub points for dotted line
                linePoints = new List<Point3D>();
                var subsegments = points.Count / 2;

                for (int i = 0; i < subsegments; i++)
                {
                    int segment = i * 2;

                    var segmentStartPoint = points[segment];
                    var segmentEndPoint = points[segment + 1];

                    var direction = segmentEndPoint - segmentStartPoint;
                    direction.Normalize();

                    var point = segmentStartPoint;
                    linePoints.Add(point);

                    do
                    {
                        for (int j = 0; j < dashparts; j++)
                        {
                            var nextPoint = point + direction * thickness * dashArray[j] / 10.0;

                            if (pointBetweenStartAndEnd(nextPoint, segmentStartPoint, segmentEndPoint))
                            {
                                point = nextPoint;
                                linePoints.Add(point);
                            }
                            else
                            {
                                if ((j % 2) == 0)
                                {
                                    point = segmentEndPoint;
                                    linePoints.Add(point);
                                }
                                break; // for loop
                            }
                        }
                    }
                    while (pointBetweenStartAndEnd(point, segmentStartPoint, segmentEndPoint));
                }
            }
            return linePoints;
        }

        private static bool pointBetweenStartAndEnd(Point3D point, Point3D startPoint, Point3D endPoint)
        {
            var result = true;

            var directionLine = endPoint - startPoint;
            var directionSegment = endPoint - point;

            if (Math.Sign(directionLine.X) != Math.Sign(directionSegment.X)) result = false;
            if (Math.Sign(directionLine.Y) != Math.Sign(directionSegment.Y)) result = false;
            if (Math.Sign(directionLine.Z) != Math.Sign(directionSegment.Z)) result = false;

            return result;
        }

    }
}