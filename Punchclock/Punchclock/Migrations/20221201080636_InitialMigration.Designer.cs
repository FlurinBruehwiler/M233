// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Punchclock.Models;
using Punchclock.Models.Db;

#nullable disable

namespace Punchclock.Migrations
{
    [DbContext(typeof(PunchclockDbContext))]
    [Migration("20221201080636_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("EntryTag", b =>
                {
                    b.Property<long>("EntriesId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("TagsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("EntriesId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("EntryTag");
                });

            modelBuilder.Entity("Punchclock.Models.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Punchclock.Models.Entry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CheckIn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CheckOut")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Entries");
                });

            modelBuilder.Entity("Punchclock.Models.Tag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("EntryTag", b =>
                {
                    b.HasOne("Punchclock.Models.Entry", null)
                        .WithMany()
                        .HasForeignKey("EntriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Punchclock.Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Punchclock.Models.Entry", b =>
                {
                    b.HasOne("Punchclock.Models.Category", "Category")
                        .WithMany("Entries")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Punchclock.Models.Category", b =>
                {
                    b.Navigation("Entries");
                });
#pragma warning restore 612, 618
        }
    }
}
