﻿// <auto-generated />
using System;
using ContratosMVVM.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ContratosMVVM.Migrations
{
    [DbContext(typeof(CobrancaDbContext))]
    partial class CobrancaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("ContratosMVVM.Domain.CLIENTE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bairro")
                        .HasColumnType("TEXT");

                    b.Property<int>("BlingID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CEP")
                        .HasColumnType("TEXT");

                    b.Property<string>("CNPJCPF")
                        .HasColumnType("TEXT");

                    b.Property<string>("CPFDoRepresentante")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cidade")
                        .HasColumnType("TEXT");

                    b.Property<int>("DataMelhorVencimento")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Endereço")
                        .HasColumnType("TEXT");

                    b.Property<string>("Estado")
                        .HasColumnType("TEXT");

                    b.Property<int>("IDFirebird")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RazãoSocial")
                        .HasColumnType("TEXT");

                    b.Property<string>("Representante")
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefone")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("ContratosMVVM.Domain.CONTRATO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CLIENTEId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ContratoBaseId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ContratoPDF")
                        .HasColumnType("TEXT");

                    b.Property<int>("FirebirdIDCliente")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Quantidade")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ValorUnitário")
                        .HasColumnType("TEXT");

                    b.Property<int>("Vigência")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CLIENTEId");

                    b.HasIndex("ContratoBaseId");

                    b.ToTable("Contratos");
                });

            modelBuilder.Entity("ContratosMVVM.Domain.CONTRATO_BASE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descrição")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.Property<int>("SetorId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SetorId");

                    b.ToTable("ContratoBases");
                });

            modelBuilder.Entity("ContratosMVVM.Domain.OBSERVACAO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FirebirdId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Texto")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Observacoes");
                });

            modelBuilder.Entity("ContratosMVVM.Domain.SETOR", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Setor")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Setors");
                });

            modelBuilder.Entity("ContratosMVVM.Domain.CONTRATO", b =>
                {
                    b.HasOne("ContratosMVVM.Domain.CLIENTE", null)
                        .WithMany("Contratos")
                        .HasForeignKey("CLIENTEId");

                    b.HasOne("ContratosMVVM.Domain.CONTRATO_BASE", "ContratoBase")
                        .WithMany("Contratos")
                        .HasForeignKey("ContratoBaseId");

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
