using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TestDataDumper.Data.Entities;
using TestDataDumper.TestData;

namespace TestDataDumper.Data;

public partial class DumperContext : DbContext
{
    public DumperContext()
    {
    }

    public DumperContext(DbContextOptions<DumperContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses => Set<Address>();

    public virtual DbSet<Person> People => Set<Person>();

    public virtual DbSet<Pet> Pets => Set<Pet>();

    public virtual DbSet<PetOwner> PetOwners => Set<PetOwner>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite("Data Source=TestDataDumper.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Street)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Street2)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Zip)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Address).WithMany(p => p.People)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_People_Addresses");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PetOwner>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Owner).WithMany(p => p.PetOwners)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PetOwners_People");

            entity.HasOne(d => d.Pet).WithMany(p => p.PetOwners)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PetOwners_Pets");
        });
        OnModelCreatingPartial(modelBuilder);
        //AddSeedData(modelBuilder);
    }
    
    private void AddSeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>().HasData(AddressData.LoadAddresses());
        modelBuilder.Entity<Person>().HasData(PersonData.LoadPeople());
        modelBuilder.Entity<Pet>().HasData(PetData.LoadPets());
        modelBuilder.Entity<PetOwner>().HasData(PetOwnerData.LoadPetOwners());
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
