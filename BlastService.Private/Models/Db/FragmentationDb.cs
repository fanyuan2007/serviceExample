using BlastService.Private.ModelContract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlastService.Private.Models
{
    [Table("Fragmentation")]
    public class FragmentationDb : Fragmentation
    {
        // For database migration
        public FragmentationDb()
        {

        }

        // Request -> Database
        public FragmentationDb(DesignFragmentation request)
            : this()
        {
            Id = Guid.NewGuid();
            P10 = request.P10;
            P20 = request.P20;
            P30 = request.P30;
            P40 = request.P40;
            P50 = request.P50;
            P60 = request.P60;
            P70 = request.P70;
            P80 = request.P80;
            P90 = request.P90;
        }

        public FragmentationDb(ActualFragmentation request)
            :this()
        {
            Id = Guid.NewGuid();
            P10 = request.P10;
            P20 = request.P20;
            P30 = request.P30;
            P40 = request.P40;
            P50 = request.P50;
            P60 = request.P60;
            P70 = request.P70;
            P80 = request.P80;
            P90 = request.P90;
            TopSize = request.TopSize;
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        public double? TopSize { get; set; }
    }
}