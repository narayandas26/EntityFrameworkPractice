using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkPractice.Models;

public partial class NarCosmosContext : DbContext
{
    private string ConnectionString = "";
    public NarCosmosContext(string connectionString)
    {
        this.ConnectionString = connectionString;
    }

    public NarCosmosContext(DbContextOptions<NarCosmosContext> options)
        : base(options)
    {
    }

    private void Initialize()
    {
        //optionsBuilder
        //.UseLazyLoadingProxies(false) //by default the navigation properties are virtual and lazy loading in enabled
        //depending on the system disable lazy loading and implement eager loading by using include and projections
        //or remove the virtual from navigation properties to make them eager loaded

        //.UseChangeTrackingProxies(false) // either disable change tracking like this or use AsNoTracking on each query
        //then for insert,update/delete, use context.dbset.attach(), then set context.Entry(T).State = EntityState.Modified/deleted etc

        //listners can be set to the optionBuilder as well for logging and stuff
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(this.ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Shadow properties can be set here which can be used for where conditions but won't be part of the model
        //mapping TPH (table per heirarchy can be done here) like one parent model class and 2 child model with different property on same column depending on value

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DeptId).HasName("PK__Departme__014881AEFEC96FC0");

            entity.ToTable("Department");

            entity.Property(e => e.DeptName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F11BEEBFA77");

            entity.ToTable("Employee");

            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Dept).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DeptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Employee_Dapartment_FK");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("Employee_Employee_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
