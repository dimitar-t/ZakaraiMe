namespace ZakaraiMe.Data.Entities.Implementations
{
    using Contracts;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Journey : IBaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string StartPointX { get; set; }

        [Required]
        public string StartPointY { get; set; }

        [Required]
        public string EndPointX { get; set; }

        [Required]
        public string EndPointY { get; set; }

        public decimal Price { get; set; }

        public int Seats { get; set; }

        public virtual Car Car { get; set; }

        public int CarId { get; set; }

        public virtual User Driver { get; set; }

        public int DriverId { get; set; }

        public virtual IEnumerable<UserJourney> Passengers { get; set; } = new List<UserJourney>();

        public DateTime DateTime { get; set; }
    }
}
