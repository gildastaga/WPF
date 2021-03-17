using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRBD_Framework;

namespace prbd_msn_tuto {
    public class Model : DbContextBase {

        public static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => {
            builder.AddConsole();
        });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=msn")
                .EnableSensitiveDataLogging()
                //.UseLoggerFactory(_loggerFactory)
                .UseLazyLoadingProxies(true)
                ;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // l'entité Member participe à une relation one-to-many ...
            modelBuilder.Entity<Member>()
                // avec, du côté many, la propriété MessagesSent ...
                .HasMany(member => member.MessagesSent)
                // avec, du côté one, la propriété Author ...
                .WithOne(msg => msg.Author)
                // et pour laquelle on désactive le delete en cascade
                .OnDelete(DeleteBehavior.Restrict);

            // l'entité Member participe à une relation one-to-many ...
            modelBuilder.Entity<Member>()
                // avec, du côté many, la propriété MessagesReceived ...
                .HasMany(member => member.MessagesReceived)
                // avec, du côté one, la propriété Recipient ...
                .WithOne(msg => msg.Recipient)
                // et pour laquelle on désactive le delete en cascade
                .OnDelete(DeleteBehavior.Restrict);
        }

        public void SeedData() {
            Database.BeginTransaction();

            var admin = new Member("admin", "admin", "Je suis l'admin !!!", true, "admin.jpg");
            var alain = new Member("alain", "alain", null, true, "alain.jpg");
            var angelina = new Member("angelina", "angelina", null, false, "angelina.jpg");
            var audrey = new Member("audrey", "audrey", null, false, "audrey.jpg");
            var ben = new Member("ben", "ben", "Je suis Benoît.", false, "ben.jpg");
            var beyonce = new Member("beyonce", "beyonce", null, false, "beyonce.jpg");
            var bob = new Member("bob", "bob", null, false, "bob.jpg");
            var boris = new Member("boris", "boris", null, true, "boris.jpg");
            var brad = new Member("brad", "brad", null, false, "brad.jpg");
            var bruno = new Member("bruno", "bruno", "Je suis Bruno.", false, "bruno.jpg");
            var caro = new Member("caro", "caro", null, false, "caro.jpg");
            var dany = new Member("dany", "dany", null, false, "dany.jpg");
            var donald = new Member("donald", "donald", null, false, "donald.jpg");
            var fred = new Member("fred", "fred", null, false, "fred.jpg");
            var george = new Member("george", "george", null, false, "george.jpg");
            var guest = new Member("guest", "guest", null, false, "guest.jpg");
            var marilyn = new Member("marilyn", "marilyn", null, false, "marilyn.jpg");
            var test = new Member("test", "test", "Hello, this is Test!", false);
            var uma = new Member("uma", "uma", null, false, "uma.jpg");
            var will = new Member("will", "will", null, false, "will.jpg");
            Members.AddRange(new[] {
                    admin, alain, angelina, audrey, ben, beyonce, bob, boris, brad, bruno, caro, dany,
                    donald, fred, george, guest, marilyn, test, uma, will
                });

            bob.Follow(ben);
            caro.Follow(ben);
            ben.Follow(caro);
            ben.Follow(fred);
            caro.Follow(fred);
            admin.Follow(guest);
            ben.Follow(guest);

            Messages.AddRange(new[] {
                new Message(ben, ben, "message 1", false, new DateTime(2020, 02, 09, 10, 11, 33)),
                new Message(ben, ben, "message 2", false, new DateTime(2020, 02, 09, 10, 12, 59)),
                new Message(caro, ben, "message de caro", false, new DateTime(2020, 02, 09, 10, 14, 03)),
                new Message(ben, ben, "test", true, new DateTime(2020, 02, 09, 10, 58, 10)),
                new Message(ben, ben, "test", false, new DateTime(2020, 02, 09, 10, 58, 15)),
                new Message(caro, caro, "myself", false, new DateTime(2020, 02, 09, 11, 29, 20)),
                new Message(ben, caro, "a longer message for caro in order to see how it wrapped around in the message table.",
                    false, new DateTime(2020, 02, 09, 11, 34, 44)),
                new Message(ben, fred, "this is a message to fred", false, new DateTime(2020, 02, 09, 18, 15, 27)),
                new Message(ben, fred, "this is a private message to fred", true, new DateTime(2020, 02, 09, 18, 15, 36)),
                new Message(ben, fred, "hello", false, new DateTime(2020, 02, 10, 00, 16, 01)),
                new Message(ben, fred, "aaa", false, new DateTime(2020, 02, 10, 00, 17, 41)),
                new Message(bruno, admin, "test", false, new DateTime(2020, 02, 10, 11, 32, 37)),
                new Message(ben, caro, "ben to caro", false, new DateTime(2020, 02, 10, 12, 05, 44)),
            });

            SaveChanges();

            Database.CommitTransaction();
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}