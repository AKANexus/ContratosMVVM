﻿// <auto-generated />
using System;
using ContratosMVVM.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ContratosMVVM.Migrations
{
    [DbContext(typeof(CobrancaDbContext))]
    [Migration("20210518194709_skdjf")]
    partial class skdjf
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("ContratosMVVM.Domain.CLIENTE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Bairro")
                        .HasColumnType("longtext");

                    b.Property<string>("CNPJCPF")
                        .HasColumnType("longtext");

                    b.Property<string>("Cidade")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("Endereço")
                        .HasColumnType("longtext");

                    b.Property<string>("Estado")
                        .HasColumnType("longtext");

                    b.Property<int>("IDFirebird")
                        .HasColumnType("int");

                    b.Property<string>("RazãoSocial")
                        .HasColumnType("longtext");

                    b.Property<string>("Telefone")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("CLIENTE");
                });

            modelBuilder.Entity("ContratosMVVM.Domain.CONTRATO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ClienteId")
                        .HasColumnType("int");

                    b.Property<int>("ContratoBaseId")
                        .HasColumnType("int");

                    b.Property<int>("FirebirdIDCliente")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantidade")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("ValorUnitário")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Vigência")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.HasIndex("ContratoBaseId");

                    b.ToTable("Contratos");
                });

            modelBuilder.Entity("ContratosMVVM.Domain.CONTRATO_BASE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Descrição")
                        .HasColumnType("longtext");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext");

                    b.Property<int>("SetorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SetorId");

                    b.ToTable("ContratoBases");
                });

            modelBuilder.Entity("ContratosMVVM.Domain.SETOR", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Setor")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Setors");
                });

            modelBuilder.Entity("ContratosMVVM.Domain.CONTRATO", b =>
                {
                    b.HasOne("ContratosMVVM.Domain.CLIENTE", "Cliente")
                        .WithMany("Contratos")
                        .HasForeignKey("ClienteId");

                    b.HasOne("ContratosMVVM.Domain.CONTRATO_BASE", "ContratoBase")
                        .WithMany("Contratos")
                        .HasForeignKey("ContratoBaseId");

                    b.Navigation("Cliente");

                    b.Navigation("ContratoBase");
                });

            modelBuilder.Entity("ContratosMVVM.Domain.CONTRATO_BASE", b =>
                {
                    b.HasOne("ContratosMVVM.Domain.SETOR", "Setor")
                        .WithMany("ContratoBases")
                        .HasForeignKey("SetorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Setor");
                });

            modelBuilder.Entity("ContratosMVVM.Domain.CLIENTE", b =>
                {
                    b.Navigation("Contratos");
                });

            modelBuilder.Entity("ContratosMVVM.Domain.CONTRATO_BASE", b =>
                {
                    b.Navigation("Contratos");
                });

            modelBuilder.Entity("ContratosMVVM.Domain.SETOR", b =>
                {
                    b.Navigation("ContratoBases");
                });
#pragma warning restore 612, 618
        }
    }
}
