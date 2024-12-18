﻿// <auto-generated />
using System;
using DA.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace PersonalAccountV2.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DA.Entities.Accomplishment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValue(new Guid("ffb43baf-ceec-4f3e-b102-40c3f0015885"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 11, 16, 19, 6, 42, 229, DateTimeKind.Local).AddTicks(6840));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 11, 16, 19, 6, 42, 229, DateTimeKind.Local).AddTicks(7121));

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Accomplishment");
                });

            modelBuilder.Entity("DA.Entities.Communication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValue(new Guid("258c0e1d-0aed-4565-9133-3e220823f59b"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 11, 16, 19, 6, 42, 229, DateTimeKind.Local).AddTicks(7647));

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 11, 16, 19, 6, 42, 229, DateTimeKind.Local).AddTicks(7865));

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Communication");
                });

            modelBuilder.Entity("DA.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValue(new Guid("7ab66c83-2479-49e7-8682-1a33cd3d1b9e"));

                    b.Property<string>("About")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 11, 16, 19, 6, 42, 229, DateTimeKind.Local).AddTicks(5456));

                    b.Property<DateTime?>("EmploymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Language")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainTelephoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OfficeAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 11, 16, 19, 6, 42, 229, DateTimeKind.Local).AddTicks(5898));

                    b.HasKey("Id");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("DA.Entities.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValue(new Guid("2d24af6e-1c63-4bb9-9b8f-4bee6ce201bb"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 11, 16, 19, 6, 42, 229, DateTimeKind.Local).AddTicks(8296));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsAcrive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 11, 16, 19, 6, 42, 229, DateTimeKind.Local).AddTicks(8555));

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("DA.Entities.Experience", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValue(new Guid("9df9892a-9979-48a5-8223-578195abbf68"));

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 11, 16, 19, 6, 42, 229, DateTimeKind.Local).AddTicks(8999));

                    b.Property<string>("DescriptionCompany")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DescriptionWork")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DismissalDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EmployementDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 11, 16, 19, 6, 42, 229, DateTimeKind.Local).AddTicks(9222));

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Experience");
                });

            modelBuilder.Entity("DA.Entities.Skill", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValue(new Guid("27c5dabf-312d-4665-aea4-d88dd3be0bed"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 11, 16, 19, 6, 42, 229, DateTimeKind.Local).AddTicks(9688));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 11, 16, 19, 6, 42, 229, DateTimeKind.Local).AddTicks(9907));

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Skill");
                });

            modelBuilder.Entity("DA.Entities.Accomplishment", b =>
                {
                    b.HasOne("DA.Entities.Employee", "Employee")
                        .WithMany("AccomplishmentsList")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("DA.Entities.Communication", b =>
                {
                    b.HasOne("DA.Entities.Employee", "Employee")
                        .WithMany("CommunicationsList")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("DA.Entities.Event", b =>
                {
                    b.HasOne("DA.Entities.Employee", "Employee")
                        .WithMany("EventList")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("DA.Entities.Experience", b =>
                {
                    b.HasOne("DA.Entities.Employee", "Employee")
                        .WithMany("ExperienceList")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("DA.Entities.Skill", b =>
                {
                    b.HasOne("DA.Entities.Employee", "Employee")
                        .WithMany("SkillsList")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("DA.Entities.Employee", b =>
                {
                    b.Navigation("AccomplishmentsList");

                    b.Navigation("CommunicationsList");

                    b.Navigation("EventList");

                    b.Navigation("ExperienceList");

                    b.Navigation("SkillsList");
                });
#pragma warning restore 612, 618
        }
    }
}
