using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace api_enterprise.Models
{
    public partial class transactContext : DbContext
    {
        public transactContext()
        {
        }

        public transactContext(DbContextOptions<transactContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Enterprise> Enterprises { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<Region> Regions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Latin1_General_CI_AS");

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("countries");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CountryName)
                    .HasMaxLength(50)
                    .HasColumnName("countryName");
            });

            modelBuilder.Entity<Enterprise>(entity =>
            {
                entity.ToTable("enterprises");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CountryId).HasColumnName("countryId");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DatePublished)
                    .HasColumnType("datetime")
                    .HasColumnName("datePublished");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Justification).HasColumnName("justification");

                entity.Property(e => e.MemberId).HasColumnName("memberId");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.RegionId).HasColumnName("regionId");

                entity.Property(e => e.Summary)
                    .HasMaxLength(50)
                    .HasColumnName("summary");

                entity.Property(e => e.TimeStamp)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("timeStamp");

                entity.Property(e => e.VendorFinancing).HasColumnName("vendorFinancing");

                entity.Property(e => e.VendorImplication).HasColumnName("vendorImplication");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Enterprises)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_enterprises_countries");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Enterprises)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_enterprises_members");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Enterprises)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_enterprises_regions");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("members");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AddressCity)
                    .HasMaxLength(50)
                    .HasColumnName("addressCity");

                entity.Property(e => e.AddressCodePostal)
                    .HasMaxLength(10)
                    .HasColumnName("addressCodePostal")
                    .IsFixedLength();

                entity.Property(e => e.AddressNumber)
                    .HasMaxLength(10)
                    .HasColumnName("addressNumber")
                    .IsFixedLength();

                entity.Property(e => e.AddressStreet)
                    .HasMaxLength(50)
                    .HasColumnName("addressStreet");

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .HasColumnName("fax");

                entity.Property(e => e.MemberName)
                    .HasMaxLength(50)
                    .HasColumnName("memberName");

                entity.Property(e => e.PhoneBusiness)
                    .HasMaxLength(50)
                    .HasColumnName("phoneBusiness");

                entity.Property(e => e.PhoneCell)
                    .HasMaxLength(50)
                    .HasColumnName("phoneCell");

                entity.Property(e => e.PhoneHome)
                    .HasMaxLength(50)
                    .HasColumnName("phoneHome");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("regions");

                entity.Property(e => e.RegionId).HasColumnName("regionId");

                entity.Property(e => e.RegionName)
                    .HasMaxLength(50)
                    .HasColumnName("regionName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
