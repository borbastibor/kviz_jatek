﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using kviz_jatek.Data;

namespace kviz_jatek.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20200212191359_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1");

            modelBuilder.Entity("kviz_jatek.Model.QuizContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("GoodAnswer")
                        .HasColumnType("TEXT");

                    b.Property<string>("Question")
                        .HasColumnType("TEXT");

                    b.Property<string>("WrongAnswer1")
                        .HasColumnType("TEXT");

                    b.Property<string>("WrongAnswer2")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("QuizContents");
                });

            modelBuilder.Entity("kviz_jatek.Model.TopScore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Score")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("TopScores");
                });
#pragma warning restore 612, 618
        }
    }
}
