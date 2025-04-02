﻿// <auto-generated />
using System;
using CC_Karriarpartner.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CC_Karriarpartner.Migrations
{
    [DbContext(typeof(KarriarPartnerDBContext))]
    partial class KarriarPartnerDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CC_Karriarpartner.Models.Certificate", b =>
                {
                    b.Property<int>("CertificateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CertificateId"));

                    b.Property<string>("CertificateUrl")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("CourseId_FK")
                        .HasColumnType("int");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId_FK")
                        .HasColumnType("int");

                    b.HasKey("CertificateId");

                    b.HasIndex("CourseId_FK");

                    b.HasIndex("UserId_FK");

                    b.ToTable("Certificates");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<bool>("Completed")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CourseId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.CourseReview", b =>
                {
                    b.Property<int>("CourseReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseReviewId"));

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("CourseId_FK")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("UserId_FK")
                        .HasColumnType("int");

                    b.HasKey("CourseReviewId");

                    b.HasIndex("CourseId_FK");

                    b.HasIndex("UserId_FK");

                    b.ToTable("CourseReviews");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.CourseVideo", b =>
                {
                    b.Property<int>("CourseVideoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseVideoId"));

                    b.Property<int>("CourseId_FK")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("VideoUrl")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("CourseVideoId");

                    b.HasIndex("CourseId_FK");

                    b.ToTable("CourseVideos");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.Purchase", b =>
                {
                    b.Property<int>("PurchaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PurchaseId"));

                    b.Property<DateTime>("BuyDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserId_FK")
                        .HasColumnType("int");

                    b.HasKey("PurchaseId");

                    b.HasIndex("UserId_FK");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.PurchaseItem", b =>
                {
                    b.Property<int>("PurchaseItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PurchaseItemId"));

                    b.Property<int?>("CourseId_Fk")
                        .HasColumnType("int");

                    b.Property<int>("PurchaseId_Fk")
                        .HasColumnType("int");

                    b.Property<int?>("TemplateId_Fk")
                        .HasColumnType("int");

                    b.HasKey("PurchaseItemId");

                    b.HasIndex("CourseId_Fk");

                    b.HasIndex("PurchaseId_Fk");

                    b.HasIndex("TemplateId_Fk");

                    b.ToTable("PurchaseItems");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.Template", b =>
                {
                    b.Property<int>("TemplateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TemplateId"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PdfUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("TemplateId");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("EmailVerification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PasswordResetExpire")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("ProfileImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpireTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Verified")
                        .HasColumnType("bit");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.UserSubscriptions", b =>
                {
                    b.Property<int>("SubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubscriptionId"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("UserId_FK")
                        .HasColumnType("int");

                    b.HasKey("SubscriptionId");

                    b.HasIndex("UserId_FK");

                    b.ToTable("UserSubscriptions");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.Certificate", b =>
                {
                    b.HasOne("CC_Karriarpartner.Models.Course", "Course")
                        .WithMany("Certificates")
                        .HasForeignKey("CourseId_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CC_Karriarpartner.Models.User", "User")
                        .WithMany("Certificates")
                        .HasForeignKey("UserId_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.CourseReview", b =>
                {
                    b.HasOne("CC_Karriarpartner.Models.Course", "Course")
                        .WithMany("Reviews")
                        .HasForeignKey("CourseId_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CC_Karriarpartner.Models.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.CourseVideo", b =>
                {
                    b.HasOne("CC_Karriarpartner.Models.Course", "Course")
                        .WithMany("Videos")
                        .HasForeignKey("CourseId_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.Purchase", b =>
                {
                    b.HasOne("CC_Karriarpartner.Models.User", "User")
                        .WithMany("Purchases")
                        .HasForeignKey("UserId_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.PurchaseItem", b =>
                {
                    b.HasOne("CC_Karriarpartner.Models.Course", "Course")
                        .WithMany("PurchaseItems")
                        .HasForeignKey("CourseId_Fk");

                    b.HasOne("CC_Karriarpartner.Models.Purchase", "Purchase")
                        .WithMany("PurchaseItems")
                        .HasForeignKey("PurchaseId_Fk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CC_Karriarpartner.Models.Template", "Template")
                        .WithMany("PurchaseItems")
                        .HasForeignKey("TemplateId_Fk");

                    b.Navigation("Course");

                    b.Navigation("Purchase");

                    b.Navigation("Template");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.UserSubscriptions", b =>
                {
                    b.HasOne("CC_Karriarpartner.Models.User", "User")
                        .WithMany("Subscriptions")
                        .HasForeignKey("UserId_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.Course", b =>
                {
                    b.Navigation("Certificates");

                    b.Navigation("PurchaseItems");

                    b.Navigation("Reviews");

                    b.Navigation("Videos");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.Purchase", b =>
                {
                    b.Navigation("PurchaseItems");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.Template", b =>
                {
                    b.Navigation("PurchaseItems");
                });

            modelBuilder.Entity("CC_Karriarpartner.Models.User", b =>
                {
                    b.Navigation("Certificates");

                    b.Navigation("Purchases");

                    b.Navigation("Reviews");

                    b.Navigation("Subscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
