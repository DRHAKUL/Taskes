﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tasques.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class tasquesEntities : DbContext
    {
        public tasquesEntities()
            : base("name=tasquesEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<GROUPS> GROUPS { get; set; }
        public virtual DbSet<TASKS> TASKS { get; set; }
        public virtual DbSet<USERGROUP> USERGROUP { get; set; }
        public virtual DbSet<USERS> USERS { get; set; }
        public virtual DbSet<USERTASK> USERTASK { get; set; }
        public virtual DbSet<USERVALUES> USERVALUES { get; set; }
        public virtual DbSet<ROLS> ROLS { get; set; }
        public virtual DbSet<USERROL> USERROL { get; set; }
    }
}
