﻿using System;
using Foro;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Foro.Migrations
{
    [DbContext(typeof(ForoContexto))]
    [Migration("20231205003328_Foro")]
    partial class Foro
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Foro.Categoria", b =>
                {
                    b.Property<int>("CategoriaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoriaId"), 1L, 1);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("CategoriaId");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("Foro.Entrada", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("MiembroId")
                        .HasColumnType("int");

                    b.Property<bool>("Privada")
                        .HasColumnType("bit");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("MiembroId");

                    b.ToTable("Entradas");
                });

            modelBuilder.Entity("Foro.MiembrosHabilitados", b =>
                {
                    b.Property<int>("MiembroId")
                        .HasColumnType("int");

                    b.Property<int>("EntradaId")
                        .HasColumnType("int");

                    b.Property<bool>("Habilitado")
                        .HasColumnType("bit");

                    b.HasKey("MiembroId", "EntradaId");

                    b.HasIndex("EntradaId");

                    b.ToTable("MiembrosHabilitados");
                });

            modelBuilder.Entity("Foro.Pregunta", b =>
                {
                    b.Property<int>("PreguntaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PreguntaId"), 1L, 1);

                    b.Property<bool>("Activa")
                        .HasColumnType("bit");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("EntradaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("MiembroId")
                        .HasColumnType("int");

                    b.HasKey("PreguntaId");

                    b.HasIndex("EntradaId");

                    b.HasIndex("MiembroId");

                    b.ToTable("Respuestas");
                });

            modelBuilder.Entity("Foro.Reaccion", b =>
                {
                    b.Property<int>("MiembroId")
                        .HasColumnType("int");

                    b.Property<int>("RespuestaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<bool>("MeGusta")
                        .HasColumnType("bit");

                    b.HasKey("MiembroId", "RespuestaId");

                    b.HasIndex("RespuestaId");

                    b.ToTable("Reacciones");
                });

            modelBuilder.Entity("Foro.Respuesta", b =>
                {
                    b.Property<int>("RespuestaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RespuestaId"), 1L, 1);

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("MiembroId")
                        .HasColumnType("int");

                    b.Property<int>("PreguntaId")
                        .HasColumnType("int");

                    b.HasKey("RespuestaId");

                    b.HasIndex("MiembroId");

                    b.HasIndex("PreguntaId");

                    b.ToTable("Respuestas");
                });

            modelBuilder.Entity("Foro.Usuario", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FechaAlta")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Usuarios");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Usuario");
                });

            modelBuilder.Entity("Foro.Miembro", b =>
                {
                    b.HasBaseType("Foro.Usuario");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasDiscriminator().HasValue("Miembro");
                });

            modelBuilder.Entity("Foro.Entrada", b =>
                {
                    b.HasOne("Foro.Categoria", "Categoria")
                        .WithMany("Entradas")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Foro.Miembro", "Miembro")
                        .WithMany("Entradas")
                        .HasForeignKey("MiembroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("Miembro");
                });

            modelBuilder.Entity("Foro.MiembrosHabilitados", b =>
                {
                    b.HasOne("Foro.Entrada", "Entrada")
                        .WithMany("MiembrosHabilitados")
                        .HasForeignKey("EntradaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Foro.Miembro", "Miembro")
                        .WithMany("miembrosHabilitados")
                        .HasForeignKey("MiembroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entrada");

                    b.Navigation("Miembro");
                });

            modelBuilder.Entity("Foro.Pregunta", b =>
                {
                    b.HasOne("Foro.Entrada", "Entrada")
                        .WithMany("Respuestas")
                        .HasForeignKey("EntradaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Foro.Miembro", "Miembro")
                        .WithMany("Respuestas")
                        .HasForeignKey("MiembroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entrada");

                    b.Navigation("Miembro");
                });

            modelBuilder.Entity("Foro.Reaccion", b =>
                {
                    b.HasOne("Foro.Miembro", "Miembro")
                        .WithMany("PreguntasYRespuestasQueMeGustan")
                        .HasForeignKey("MiembroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Foro.Respuesta", "Respuesta")
                        .WithMany("Reacciones")
                        .HasForeignKey("RespuestaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Miembro");

                    b.Navigation("Respuesta");
                });

            modelBuilder.Entity("Foro.Respuesta", b =>
                {
                    b.HasOne("Foro.Miembro", "Miembro")
                        .WithMany("Respuestas")
                        .HasForeignKey("MiembroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Foro.Pregunta", "Pregunta")
                        .WithMany("Respuestas")
                        .HasForeignKey("PreguntaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Miembro");

                    b.Navigation("Pregunta");
                });

            modelBuilder.Entity("Foro.Categoria", b =>
                {
                    b.Navigation("Entradas");
                });

            modelBuilder.Entity("Foro.Entrada", b =>
                {
                    b.Navigation("MiembrosHabilitados");

                    b.Navigation("Respuestas");
                });

            modelBuilder.Entity("Foro.Pregunta", b =>
                {
                    b.Navigation("Respuestas");
                });

            modelBuilder.Entity("Foro.Respuesta", b =>
                {
                    b.Navigation("Reacciones");
                });

            modelBuilder.Entity("Foro.Miembro", b =>
                {
                    b.Navigation("Entradas");

                    b.Navigation("Respuestas");

                    b.Navigation("PreguntasYRespuestasQueMeGustan");

                    b.Navigation("Respuestas");

                    b.Navigation("miembrosHabilitados");
                });
#pragma warning restore 612, 618
        }
    }
}
