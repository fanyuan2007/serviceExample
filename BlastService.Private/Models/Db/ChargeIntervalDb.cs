using BlastService.Private.ModelContract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlastService.Private.Models
{
    [Table("ChargingIntervals")]
    public class ChargeIntervalDb
    {
        // For database migration
        public ChargeIntervalDb()
        {

        }

        public ChargeIntervalDb(ChargeInterval profileRequest)
            :this()
        {
            Id = Guid.NewGuid();
            From = profileRequest.From;
            To = profileRequest.To;
            Consumable = profileRequest.Consumable;
            Amount = profileRequest.Amount;
            Deck = profileRequest.Deck;
            ProfileType = ChargeProfileType.Design;
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public double From { get; set; }

        [Required]
        public double To { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public ChargeProfileType ProfileType { get; set; }

        [Required]
        public string Consumable { get; set; }

        [Required]
        public double Amount { get; set; }

        public string Deck { get; set; }

        [ForeignKey("HoleId")]
        public Guid HoleId { get; set; }
        /// <summary>
        /// The Charging table has a FK constrait to Holes table
        /// No need to set this as it's used for table relationships
        /// </summary>
        public HoleDb Hole { get; set; }

    }
}