using FileStorage.ImportMap;
using FileStorage.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace FileStorage.EntityFrameworkCore
{
    public static class FileStorageDbContextModelCreatingExtensions
    {
        public static void ConfigureFileStorage(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
            var options = new FileStorageModelBuilderConfigurationOptions(
               FileStorageDbProperties.DbTablePrefix,
               FileStorageDbProperties.DbSchema
           );

            builder.Entity<UploadFileInfo>(b =>
            {
                b.ToTable("mr_file_info");

                b.ConfigureByConvention();

                b.Property(x => x.FileName).IsRequired().HasMaxLength(500);
                //b.Property(x => x.Size).IsRequired().HasMaxLength(100);
                //b.Property(x => x.Suffix).IsRequired().HasMaxLength(50);
                b.Property(x => x.FileUrl).IsRequired().HasMaxLength(256);
                //创建索引
                b.HasIndex(q => q.FileName);
            });
            ConfigureImportMap(builder, options);

        }
        private static void ConfigureImportMap(this ModelBuilder builder, FileStorageModelBuilderConfigurationOptions options)
        {
            builder.Entity<ImportColumnMap>(b =>
            {
                b.ToTable($"{options.TablePrefix}_importmap", options.Schema);

                b.ConfigureByConvention();
                b.Property(x => x.NewColumnName).HasMaxLength(36);
                b.Property(x => x.OldColumnName).HasMaxLength(36);

                b.Property(x => x.ProjectName).HasMaxLength(36);

            });
        }
    }
}
