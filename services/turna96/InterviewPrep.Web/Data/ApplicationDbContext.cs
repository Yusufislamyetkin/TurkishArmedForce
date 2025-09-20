using InterviewPrep.Web.Models;
using InterviewPrep.Web.Models.Questions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InterviewPrep.Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Question> Questions => Set<Question>();

    public DbSet<QuestionOption> QuestionOptions => Set<QuestionOption>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Question>(entity =>
        {
            entity.Property(q => q.Type)
                .HasConversion<string>()
                .HasMaxLength(32);

            entity.Property(q => q.Title)
                .HasMaxLength(180)
                .IsRequired();

            entity.Property(q => q.Category)
                .HasMaxLength(128);

            entity.Property(q => q.Difficulty)
                .HasMaxLength(64);

            entity.HasMany(q => q.Options)
                .WithOne(o => o.Question)
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<QuestionOption>(entity =>
        {
            entity.Property(o => o.Text)
                .HasMaxLength(512)
                .IsRequired();
        });
    }
}
