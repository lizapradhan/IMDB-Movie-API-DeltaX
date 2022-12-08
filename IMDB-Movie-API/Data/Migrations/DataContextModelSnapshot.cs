﻿// <auto-generated />
using System;
using IMDB_Movie_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IMDBMovieAPI.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("IMDB_Movie_API.Entities.Actor", b =>
                {
                    b.Property<long>("ActorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ActorName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Bio")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("DOB")
                        .HasColumnType("TEXT");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ActorId");

                    b.ToTable("Actors");
                });

            modelBuilder.Entity("IMDB_Movie_API.Entities.ActorMovies", b =>
                {
                    b.Property<long>("MappingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("actorId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("movieId")
                        .HasColumnType("INTEGER");

                    b.HasKey("MappingId");

                    b.HasIndex("movieId");

                    b.ToTable("ActorMovies");
                });

            modelBuilder.Entity("IMDB_Movie_API.Entities.Movie", b =>
                {
                    b.Property<long>("MovieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateOfRelease")
                        .HasColumnType("TEXT");

                    b.Property<long>("Producer")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("MovieId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("IMDB_Movie_API.Entities.Producer", b =>
                {
                    b.Property<long>("ProducerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProducerName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ProducerId");

                    b.ToTable("Producers");
                });

            modelBuilder.Entity("IMDB_Movie_API.Entities.ActorMovies", b =>
                {
                    b.HasOne("IMDB_Movie_API.Entities.Movie", null)
                        .WithMany("Actors")
                        .HasForeignKey("movieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IMDB_Movie_API.Entities.Movie", b =>
                {
                    b.Navigation("Actors");
                });
#pragma warning restore 612, 618
        }
    }
}
