using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.Property(b => b.DaysPerWeek)
                .HasConversion(
                    x => string.Join(", ", x),
                    x => x.Split(", ", StringSplitOptions.None).Select(x => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), x)).ToList(),
                    new ValueComparer<List<DayOfWeek>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()));

            builder
                .HasMany<Session>()
                .WithOne()
                .HasForeignKey(x => x.ScheduleId)
                .IsRequired();
        }
    }
}