using BlastService.Private.ModelContract;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlastService.Private.Models
{
    [Table("Projects")]
    public class ProjectDb
    {
        // For database migration
        public ProjectDb()
        {

        }

        // Request -> Database
        public ProjectDb(ProjectRequest request)
        {
            #region Base Properties
            Id = request.NameBasedProperties.BaseProperties.Id;
            CreatedAt = request.NameBasedProperties.BaseProperties.CreatedAt;
            UpdatedAt = request.NameBasedProperties.BaseProperties.UpdatedAt;
            Name = request.NameBasedProperties.Name;
            #endregion

            Description = request.Description;
            Unit = request.Unit;

            #region UTCOffset
            UTCOffsetHours = request.TimeZone.OffsetHours;
            UTCOffsetMinutes = request.TimeZone.OffsetMinutes;
            UTCIdName = request.TimeZone.IdName;
            UTCIsDaylightSavingTime = request.TimeZone.IsDaylightSavingTime;
            #endregion

            #region Local Transformation
            if (request.LocalTransformation != null)
            {
                LocalTransformation = JsonConvert.SerializeObject(request.LocalTransformation);
            }
            #endregion

            #region Mapping
            Mapping = request.Mapping;
            #endregion
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public MeasurementUnit Unit { get; set; }

        [Required]
        public int UTCOffsetHours { get; set; }

        [Required]
        public int UTCOffsetMinutes { get; set; }

        [Required]
        public string UTCIdName { get; set; }

        [Required]
        public bool UTCIsDaylightSavingTime { get; set; }

        [Column(TypeName = "jsonb")]
        public string LocalTransformation { get; set; }

        [Column(TypeName = "text")]
        public CoordinateConvention? Mapping { get; set; }

    }
}