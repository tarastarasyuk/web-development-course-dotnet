﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheaterCashRegister.DAL.Data;

#nullable disable

namespace TheaterCashRegister.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231210130059_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("TheaterCashRegister.DAL.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("TicketId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UUID")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TicketId")
                        .IsUnique();

                    b.ToTable("Booking");
                });

            modelBuilder.Entity("TheaterCashRegister.DAL.Models.Performance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Performance");
                });

            modelBuilder.Entity("TheaterCashRegister.DAL.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PerformanceId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int>("SeatNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PerformanceId", "SeatNumber")
                        .IsUnique();

                    b.ToTable("Ticket");
                });

            modelBuilder.Entity("TheaterCashRegister.DAL.Models.Booking", b =>
                {
                    b.HasOne("TheaterCashRegister.DAL.Models.Ticket", "Ticket")
                        .WithOne("Booking")
                        .HasForeignKey("TheaterCashRegister.DAL.Models.Booking", "TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("TheaterCashRegister.DAL.Models.Ticket", b =>
                {
                    b.HasOne("TheaterCashRegister.DAL.Models.Performance", "Performance")
                        .WithMany("Tickets")
                        .HasForeignKey("PerformanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Performance");
                });

            modelBuilder.Entity("TheaterCashRegister.DAL.Models.Performance", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("TheaterCashRegister.DAL.Models.Ticket", b =>
                {
                    b.Navigation("Booking")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
