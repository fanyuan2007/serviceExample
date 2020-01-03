using BlastService.Private.ModelContract;
using BlastService.Private.Models;

using System;

namespace BlastService.Private.Controllers
{
    public class ProjectResponse
    {
        public ProjectResponse()
        {

        }

        // Database to response
        public ProjectResponse(ProjectDb projectDb)
        {
            Id = projectDb.Id;
            Name = projectDb.Name;
            CreatedAt = projectDb.CreatedAt;
            UpdatedAt = projectDb.UpdatedAt;
            Description = projectDb.Description;
            Unit = projectDb.Unit;
            TimeZone = new UTCTimeZone();
            TimeZone.OffsetHours = projectDb.UTCOffsetHours;
            TimeZone.OffsetMinutes = projectDb.UTCOffsetMinutes;
            TimeZone.IdName = projectDb.UTCIdName;
            TimeZone.IsDaylightSavingTime = projectDb.UTCIsDaylightSavingTime;
            if (projectDb.LocalTransformation != null)
            {
                LocalTransformation = projectDb.LocalTransformation;
            }
            Mapping = projectDb.Mapping;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Description { get; set; }

        public MeasurementUnit Unit { get; set; }

        public UTCTimeZone TimeZone { get; set; }

        /// <summary>
        /// JSON string
        /// </summary>
        public string LocalTransformation { get; set; }

        public CoordinateConvention? Mapping { get; set; }

    }
}