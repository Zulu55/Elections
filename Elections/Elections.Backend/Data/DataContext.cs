﻿using Elections.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elections.Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {
            
        }

        public DbSet<ElectoralJourney> ElectoralJourneys { get; set; }

        public DbSet<ElectoralPosition> ElectoralPositions { get; set; }
        public DbSet<VotingStation> VotingStations { get; set; }
        public DbSet<Zoning> Zonings { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ElectoralJourney>().HasIndex(x => x.Date).IsUnique();
            modelBuilder.Entity<ElectoralPosition>().HasIndex(x => x.Name).IsUnique();

            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<State>().HasIndex(s => new { s.CountryId, s.Name }).IsUnique();
            modelBuilder.Entity<City>().HasIndex(c => new { c.StateId, c.Name }).IsUnique();

            //modelBuilder.Entity<VotingStation>().HasIndex(x => new { x.CityId, x.Name }).IsUnique();
            modelBuilder.Entity<VotingStation>().HasIndex(x => new { x.Name, x.Code }).IsUnique();
            modelBuilder.Entity<Zoning>().HasIndex(x => new { x.VotingStationId, x.ZoningNumber }).IsUnique();
            DisableCascadingDelete(modelBuilder);
        }
        private void DisableCascadingDelete(ModelBuilder modelBuilder)
        {
            var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
            foreach (var relationship in relationships)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }

}

