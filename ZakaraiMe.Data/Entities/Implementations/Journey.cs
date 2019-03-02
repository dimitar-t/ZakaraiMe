﻿namespace ZakaraiMe.Data.Entities.Implementations
{
    using Contracts;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Journey : IBaseEntity
    {
        public int Id { get; set; }
        
        public decimal StartPointX { get; set; }
        
        public decimal StartPointY { get; set; }
        
        public decimal EndPointX { get; set; }
        
        public decimal EndPointY { get; set; }

        public decimal Price { get; set; }

        public int Seats { get; set; }

        public virtual Car Car { get; set; }

        public int CarId { get; set; }

        public virtual User Driver { get; set; }

        public int DriverId { get; set; }

        public virtual IEnumerable<UserJourney> Passengers { get; set; } = new List<UserJourney>();

        public DateTime SetOffTime { get; set; }
    }
}
