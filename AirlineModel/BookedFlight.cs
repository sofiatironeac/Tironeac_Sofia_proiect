namespace AirlineModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BookedFlight
    {
        public int Id { get; set; }

        public int? ClientId { get; set; }

        public int? FlightId { get; set; }

        public virtual Client Client { get; set; }

        public virtual Flight Flight { get; set; }
    }
}
