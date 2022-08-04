﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BestellserviceWeb.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BestellserviceWeb.Data
{
    public partial class BestellserviceDBContext : DbContext
    {
        public BestellserviceDBContext()
        {
        }

        public BestellserviceDBContext(DbContextOptions<BestellserviceDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblBestellung> TblBestellung { get; set; }
        public virtual DbSet<TblKunde> TblKunde { get; set; }
        public virtual DbSet<TblProdukte> TblProdukte { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=SQLSPSRV01\\SP2K16DEV;Database=BE_TEST;Trusted_Connection=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblBestellung>(entity =>
            {
                entity.HasKey(e => e.BesId)
                    .HasName("pk_tblBestellung");

                entity.Property(e => e.BesZeitstempel)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.BeskunIdrefNavigation)
                    .WithMany(p => p.TblBestellung)
                    .HasForeignKey(d => d.BeskunIdref)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tblBestellung_tblKunde");

                entity.HasOne(d => d.BesproIdrefNavigation)
                    .WithMany(p => p.TblBestellung)
                    .HasForeignKey(d => d.BesproIdref)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tblBestellung_tblProdukte");
            });

            modelBuilder.Entity<TblKunde>(entity =>
            {
                entity.HasKey(e => e.KunId)
                    .HasName("pk_tblKunde");

                entity.Property(e => e.KunGeschlecht).IsUnicode(false);

                entity.Property(e => e.KunNachname).IsUnicode(false);

                entity.Property(e => e.KunVorname).IsUnicode(false);
            });

            modelBuilder.Entity<TblProdukte>(entity =>
            {
                entity.HasKey(e => e.ProId)
                    .HasName("pk_tblProdukte");

                entity.Property(e => e.ProBezeichnung).IsUnicode(false);

                entity.Property(e => e.ProZeitstempel)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
