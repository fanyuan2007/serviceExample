using System.Collections.Generic;

namespace BlastService.Private.ModelContract
{
    /// <summary>
    /// Request for Scoring.
    /// </summary>
    public class Scoring
    {
        /// <summary>
        /// Required
        /// </summary>
        public double TotalScore { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public IEnumerable<MetricScore> MetricScore { get; set; }
    }
}
