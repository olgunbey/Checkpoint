﻿// <auto-generated />
using System;
using Checkpoint.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Checkpoint.API.Migrations
{
    [DbContext(typeof(CheckpointDbContext))]
    partial class CheckpointDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Checkpoint.API.Entities.Action", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ActionPath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Body")
                        .HasColumnType("text");

                    b.Property<int>("ControllerId")
                        .HasColumnType("integer");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Header")
                        .HasColumnType("text");

                    b.Property<string>("Query")
                        .HasColumnType("text");

                    b.Property<int>("RequestType")
                        .HasColumnType("integer");

                    b.Property<int>("UpdateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ControllerId");

                    b.ToTable("Action");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.BaseUrl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BasePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.Property<int>("UpdateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("BaseUrl");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.Controller", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BaseUrlId")
                        .HasColumnType("integer");

                    b.Property<string>("ControllerPath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UpdateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("BaseUrlId");

                    b.ToTable("Controller");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.Corporate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CreateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UpdateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Corporate");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.Individual", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CreateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UpdateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Individual");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CreateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UpdateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CorporateId")
                        .HasColumnType("integer");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("IndividualId")
                        .HasColumnType("integer");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UpdateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CorporateId");

                    b.HasIndex("IndividualId");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CreateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UpdateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.RolePermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CreateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PermissionId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<int>("UpdateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermission");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.UserPermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CorporateId")
                        .HasColumnType("integer");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("IndividualId")
                        .HasColumnType("integer");

                    b.Property<int>("PermissionId")
                        .HasColumnType("integer");

                    b.Property<int>("UpdateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CorporateId");

                    b.HasIndex("IndividualId");

                    b.HasIndex("PermissionId");

                    b.ToTable("UserPermission");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CorporateId")
                        .HasColumnType("integer");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("IndividualId")
                        .HasColumnType("integer");

                    b.Property<int?>("RoleId")
                        .HasColumnType("integer");

                    b.Property<int>("UpdateUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CorporateId");

                    b.HasIndex("IndividualId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("CorporateRolePermission", b =>
                {
                    b.Property<int>("CorporateId")
                        .HasColumnType("integer");

                    b.Property<int>("RolePermissionsId")
                        .HasColumnType("integer");

                    b.HasKey("CorporateId", "RolePermissionsId");

                    b.HasIndex("RolePermissionsId");

                    b.ToTable("CorporateRolePermission");
                });

            modelBuilder.Entity("IndividualRolePermission", b =>
                {
                    b.Property<int>("IndividualId")
                        .HasColumnType("integer");

                    b.Property<int>("RolePermissionsId")
                        .HasColumnType("integer");

                    b.HasKey("IndividualId", "RolePermissionsId");

                    b.HasIndex("RolePermissionsId");

                    b.ToTable("IndividualRolePermission");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.Action", b =>
                {
                    b.HasOne("Checkpoint.API.Entities.Controller", "Controller")
                        .WithMany("Actions")
                        .HasForeignKey("ControllerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Controller");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.BaseUrl", b =>
                {
                    b.HasOne("Checkpoint.API.Entities.Project", "Project")
                        .WithMany("BaseUrls")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.Controller", b =>
                {
                    b.HasOne("Checkpoint.API.Entities.BaseUrl", "BaseUrl")
                        .WithMany("Controllers")
                        .HasForeignKey("BaseUrlId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BaseUrl");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.Project", b =>
                {
                    b.HasOne("Checkpoint.API.Entities.Corporate", "Corporate")
                        .WithMany("Projects")
                        .HasForeignKey("CorporateId");

                    b.HasOne("Checkpoint.API.Entities.Individual", "Individual")
                        .WithMany("Projects")
                        .HasForeignKey("IndividualId");

                    b.Navigation("Corporate");

                    b.Navigation("Individual");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.RolePermission", b =>
                {
                    b.HasOne("Checkpoint.API.Entities.Permission", "Permission")
                        .WithMany("RolePermission")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Checkpoint.API.Entities.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.UserPermission", b =>
                {
                    b.HasOne("Checkpoint.API.Entities.Corporate", "Corporate")
                        .WithMany("UserPermission")
                        .HasForeignKey("CorporateId");

                    b.HasOne("Checkpoint.API.Entities.Individual", "Individual")
                        .WithMany("UserPermissions")
                        .HasForeignKey("IndividualId");

                    b.HasOne("Checkpoint.API.Entities.Permission", "Permission")
                        .WithMany("UserPermission")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Corporate");

                    b.Navigation("Individual");

                    b.Navigation("Permission");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.UserRole", b =>
                {
                    b.HasOne("Checkpoint.API.Entities.Corporate", "Corporate")
                        .WithMany("UserRoles")
                        .HasForeignKey("CorporateId");

                    b.HasOne("Checkpoint.API.Entities.Individual", "Individual")
                        .WithMany("UserRoles")
                        .HasForeignKey("IndividualId");

                    b.HasOne("Checkpoint.API.Entities.Role", null)
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId");

                    b.Navigation("Corporate");

                    b.Navigation("Individual");
                });

            modelBuilder.Entity("CorporateRolePermission", b =>
                {
                    b.HasOne("Checkpoint.API.Entities.Corporate", null)
                        .WithMany()
                        .HasForeignKey("CorporateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Checkpoint.API.Entities.RolePermission", null)
                        .WithMany()
                        .HasForeignKey("RolePermissionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IndividualRolePermission", b =>
                {
                    b.HasOne("Checkpoint.API.Entities.Individual", null)
                        .WithMany()
                        .HasForeignKey("IndividualId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Checkpoint.API.Entities.RolePermission", null)
                        .WithMany()
                        .HasForeignKey("RolePermissionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Checkpoint.API.Entities.BaseUrl", b =>
                {
                    b.Navigation("Controllers");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.Controller", b =>
                {
                    b.Navigation("Actions");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.Corporate", b =>
                {
                    b.Navigation("Projects");

                    b.Navigation("UserPermission");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.Individual", b =>
                {
                    b.Navigation("Projects");

                    b.Navigation("UserPermissions");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.Permission", b =>
                {
                    b.Navigation("RolePermission");

                    b.Navigation("UserPermission");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.Project", b =>
                {
                    b.Navigation("BaseUrls");
                });

            modelBuilder.Entity("Checkpoint.API.Entities.Role", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
