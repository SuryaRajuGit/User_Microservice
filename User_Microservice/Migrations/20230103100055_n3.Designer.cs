﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using User_Microservice.Entity.Models;

namespace User_Microservice.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20230103100055_n3")]
    partial class n3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("User_Microservice.Entity.Models.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Line1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Line2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StateName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Zipcode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Address");

                    b.HasData(
                        new
                        {
                            Id = new Guid("1a2a103d-17e3-46e4-b38a-0bcbd9064bba"),
                            City = "vizag",
                            Country = "India",
                            Line1 = "s-street",
                            Line2 = "ss-street",
                            StateName = "Andhra",
                            Type = "ADMIN",
                            UserId = new Guid("fdce4d6a-226b-474c-976f-87f63084d791"),
                            Zipcode = "531116"
                        });
                });

            modelBuilder.Entity("User_Microservice.Entity.Models.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CardNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExpiryDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("User_Microservice.Entity.Models.Phone", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Phone");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f794c6cb-eb2e-4bfa-862b-0b0291a0b905"),
                            PhoneNumber = "8142255769",
                            Type = "ADMIN",
                            UserId = new Guid("fdce4d6a-226b-474c-976f-87f63084d791")
                        });
                });

            modelBuilder.Entity("User_Microservice.Entity.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = new Guid("fdce4d6a-226b-474c-976f-87f63084d791"),
                            EmailAddress = "surya@gamil.com",
                            FirstName = "Surya",
                            LastName = "Raju",
                            Role = "ADMIN"
                        });
                });

            modelBuilder.Entity("User_Microservice.Entity.Models.UserSecret", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserSecret");

                    b.HasData(
                        new
                        {
                            Id = new Guid("47b15044-a2a2-4981-a472-734b45a91300"),
                            Password = "7CtkAg/X1ImgPy1BBb61+XUzs6b3iWzI",
                            UserId = new Guid("fdce4d6a-226b-474c-976f-87f63084d791")
                        });
                });

            modelBuilder.Entity("User_Microservice.Entity.Models.Address", b =>
                {
                    b.HasOne("User_Microservice.Entity.Models.User", "User")
                        .WithOne("Address")
                        .HasForeignKey("User_Microservice.Entity.Models.Address", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("User_Microservice.Entity.Models.Payment", b =>
                {
                    b.HasOne("User_Microservice.Entity.Models.User", "User")
                        .WithOne("Payment")
                        .HasForeignKey("User_Microservice.Entity.Models.Payment", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("User_Microservice.Entity.Models.Phone", b =>
                {
                    b.HasOne("User_Microservice.Entity.Models.User", "User")
                        .WithOne("Phone")
                        .HasForeignKey("User_Microservice.Entity.Models.Phone", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("User_Microservice.Entity.Models.UserSecret", b =>
                {
                    b.HasOne("User_Microservice.Entity.Models.User", "User")
                        .WithOne("UserSecret")
                        .HasForeignKey("User_Microservice.Entity.Models.UserSecret", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}