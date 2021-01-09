namespace AirlineModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AirlineEntitiesModel : DbContext
    {
        public AirlineEntitiesModel()
            : base("name=AirlineEntitiesModel")
        {
        }

        public virtual DbSet<BookedFlight> BookedFlights { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany(e => e.BookedFlights)
                .WithOptional(e => e.Client)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Flight>()
                .Property(e => e.Duration)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Flight>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Flight>()
                .HasMany(e => e.BookedFlights)
                .WithOptional(e => e.Flight)
                .WillCascadeOnDelete();
        }
    }
}
