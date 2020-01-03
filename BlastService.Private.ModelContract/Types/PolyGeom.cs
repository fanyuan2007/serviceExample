using System;
using System.Collections.Generic;
using System.Text;

namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// This class defines a list of points
    /// </summary>
    public class PolyGeom
    {
        /// <summary>
        /// The collection of GPS Coordinates that make up the PolyGeom.
        /// </summary>
        public IEnumerable<GPSCoordinate> PolyGeometry { get; set; }
    }
}
