using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Italia.Lib.Model
{
    [Table("Offers")]
    public sealed class Offer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(256)]
        [Required]
        public string ExternalReference { get; set; }

        public DateTime Created { get; set; }

        public bool Active { get; set; }

        public DateTime Modified { get; set; }

        public decimal Price { get; set; }

        public decimal? OriginalPrice { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public double Rating { get; set; }

        [MaxLength(64)]
        [Required]
        public string HotelName { get; set; }

        [MaxLength(64)]
        [Required]
        public string Country { get; set; }

        [NotMapped]
        public Uri Url
        {
            get => new Uri(UriRaw);
            set => UriRaw = value.ToString();
        }

        [MaxLength(256)]
        [Required]
        [Column("Url")]
        public string UriRaw { get; set; }

        [MaxLength(64)]
        [Required]
        public string Departure { get; set; }

        [MaxLength(32)]
        [Required]
        public string DataProvider { get; set; }

        public ReferenceKey GetReferenceKey()
        {
            return new ReferenceKey(DataProvider, ExternalReference);
        }
    }
}
