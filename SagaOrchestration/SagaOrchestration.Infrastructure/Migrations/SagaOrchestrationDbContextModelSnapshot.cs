﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SagaOrchestration.Infrastructure.Data;

#nullable disable

namespace SagaOrchestration.Infrastructure.Migrations
{
    [DbContext(typeof(SagaOrchestrationDbContext))]
    partial class SagaOrchestrationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SagaOrchestration.Domain.State.UserRegistrationSagaState", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uuid");

                    b.Property<string>("CurrentState")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<Guid>("UserId")
                        .HasMaxLength(64)
                        .HasColumnType("uuid");

                    b.HasKey("CorrelationId");

                    b.ToTable("UserRegistrationStates", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
