using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CreditCardSimulator.Models;

public partial class MagicBankContext : DbContext
{
    public MagicBankContext()
    {
    }

    public MagicBankContext(DbContextOptions<MagicBankContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CreditCard> CreditCards { get; set; }

    public virtual DbSet<CreditCardTransaction> CreditCardTransactions { get; set; }

    public virtual DbSet<Merchant> Merchants { get; set; }

    public virtual DbSet<PosMachine> PosMachines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=Database:ConnectionString");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<CreditCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("credit_card_pkey");

            entity.ToTable("credit_card");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(16)
                .HasColumnName("card_number");
            entity.Property(e => e.ClientName).HasColumnName("client_name");
            entity.Property(e => e.ExpirationMonth).HasColumnName("expiration_month");
            entity.Property(e => e.ExpirationYear).HasColumnName("expiration_year");
            entity.Property(e => e.IssuingBank).HasColumnName("issuing_bank");
        });

        modelBuilder.Entity<CreditCardTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("credit_card_transaction_pkey");

            entity.ToTable("credit_card_transaction");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CreditCardId).HasColumnName("credit_card_id");
            entity.Property(e => e.PosMachineId).HasColumnName("pos_machine_id");

            entity.HasOne(d => d.CreditCard).WithMany(p => p.CreditCardTransactions)
                .HasForeignKey(d => d.CreditCardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_credit_card_transactions_credit_card");

            entity.HasOne(d => d.PosMachine).WithMany(p => p.CreditCardTransactions)
                .HasForeignKey(d => d.PosMachineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_credit_card_transactions_pos_machine");
        });

        modelBuilder.Entity<Merchant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("merchant_pkey");

            entity.ToTable("merchant");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<PosMachine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pos_machine_pkey");

            entity.ToTable("pos_machine");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.MerchantId).HasColumnName("merchant_id");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(20)
                .HasColumnName("zip_code");

            entity.HasOne(d => d.Merchant).WithMany(p => p.PosMachines)
                .HasForeignKey(d => d.MerchantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_pos_machine_merchant");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
