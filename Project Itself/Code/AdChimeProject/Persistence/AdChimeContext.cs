namespace AdChimeProject
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AdChimeContext : DbContext
    {
        public AdChimeContext()
            : base("name=AdChimeContext")
        {
        }

        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<panelContactsVariable> panelContactsVariables { get; set; }
        public virtual DbSet<sLink> sLinks { get; set; }
        public virtual DbSet<Campaings> Campaings { get; set; }
        public virtual DbSet<tCampaignSend> tCampaignSends { get; set; }
        public virtual DbSet<tRecipientNumber> tRecipientNumbers { get; set; }
        public virtual DbSet<RecipientsLists> RecipientsLists { get; set; }
        public virtual DbSet<tSMSCounter> tSMSCounters { get; set; }
        public virtual DbSet<TemplateSMS> TemplateSMS { get; set; }
        public virtual DbSet<AppUsers> AppUsers { get; set; }
        public virtual DbSet<tVarContact> tVarContacts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contacts>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Contacts>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Contacts>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Contacts>()
                .Property(e => e.CountryCodePhone)
                .IsUnicode(false);

            modelBuilder.Entity<Contacts>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<Contacts>()
                .Property(e => e.updatedbyuser)
                .IsUnicode(false);

            modelBuilder.Entity<panelContactsVariable>()
                .Property(e => e.sValue)
                .IsUnicode(false);

            modelBuilder.Entity<panelContactsVariable>()
                .Property(e => e.updatedbyuser)
                .IsUnicode(false);

            modelBuilder.Entity<sLink>()
                .Property(e => e.sOriginalLink)
                .IsUnicode(false);

            modelBuilder.Entity<sLink>()
                .Property(e => e.sShorterLink)
                .IsUnicode(false);

            modelBuilder.Entity<Campaings>()
                .Property(e => e.TitleCampaing)
                .IsUnicode(false);

            modelBuilder.Entity<Campaings>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Campaings>()
                .Property(e => e.Sender)
                .IsUnicode(false);

            modelBuilder.Entity<Campaings>()
                .Property(e => e.Text)
                .IsUnicode(false);

            modelBuilder.Entity<Campaings>()
                .Property(e => e.updatedbyuser)
                .IsUnicode(false);

            modelBuilder.Entity<Campaings>()
                .HasMany(e => e.tCampaignSends)
                .WithOptional(e => e.tCampaign)
                .HasForeignKey(e => e.idcampaing);

            modelBuilder.Entity<tCampaignSend>()
                .Property(e => e.tEmailtoGroup)
                .IsUnicode(false);

            modelBuilder.Entity<tCampaignSend>()
                .Property(e => e.sShorterLink)
                .IsUnicode(false);

            modelBuilder.Entity<tCampaignSend>()
                .Property(e => e.sSendbyWho)
                .IsUnicode(false);

            modelBuilder.Entity<RecipientsLists>()
                .Property(e => e.TitleRecipient)
                .IsUnicode(false);

            modelBuilder.Entity<RecipientsLists>()
                .Property(e => e.updatedbyuser)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateSMS>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateSMS>()
                .Property(e => e.Text)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateSMS>()
                .Property(e => e.updatedbyuser)
                .IsUnicode(false);

            modelBuilder.Entity<tVarContact>()
                .Property(e => e.VarName)
                .IsUnicode(false);

            modelBuilder.Entity<tVarContact>()
                .Property(e => e.colTypeType)
                .IsUnicode(false);

            modelBuilder.Entity<tVarContact>()
                .Property(e => e.colTypeFilter)
                .IsUnicode(false);
        }
    }
}
