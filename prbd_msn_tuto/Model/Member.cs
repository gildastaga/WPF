using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using PRBD_Framework;

namespace Msn.Model {
    public enum RelationshipType {
        NotRelated,
        Followee,
        Follower,
        Mutual
    }

    public class Member : EntityBase<MsnContext> {
        public const int MAX_FOLLOWEES = 4;

        [Key]
        public string Pseudo { get; set; }
        public string Password { get; set; }
        public string Profile { get; set; }
        public bool IsAdmin { get; set; }
        public string PicturePath { get; set; }

        [NotMapped]
        public string AbsolutePicturePath {
            get { return PicturePath != null ? App.IMAGE_PATH + "\\" + PicturePath : null; }
        }

        public virtual ICollection<Member> Followees { get; set; } = new HashSet<Member>();
        public virtual ICollection<Member> Followers { get; set; } = new HashSet<Member>();

        public virtual ICollection<Message> MessagesSent { get; set; } = new HashSet<Message>();
        public virtual ICollection<Message> MessagesReceived { get; set; } = new HashSet<Message>();

        public Member(string pseudo, string password, string profile = "", bool isAdmin = false, string picturePath = null) {
            Pseudo = pseudo;
            Password = password;
            Profile = profile;
            IsAdmin = isAdmin;
            PicturePath = picturePath;
        }

        protected Member() { }

        public override string ToString() {
            return $"<Member: Pseudo={Pseudo}, " +
                $"#followees={Followees.Count}, " +
                $"#followers={Followers.Count}, " +
                $"#sent={MessagesSent.Count}, " +
                $"#received={MessagesReceived.Count}>";
        }

        public void Follow(Member member) {
            if (!Followees.Contains(member) && CanFollow) {
                Followees.Add(member);
                member.Followers.Add(this);
                Context.SaveChanges();
            }
        }

        public void Unfollow(Member member) {
            Followees.Remove(member);
            member.Followers.Remove(this);
            Context.SaveChanges();
        }

        public RelationshipType GetRelationshipType(Member otherMember) {
            if (otherMember == null)
                return RelationshipType.NotRelated;
            var followee = Followees.Any(m => m.Pseudo == otherMember.Pseudo);
            var follower = Followers.Any(m => m.Pseudo == otherMember.Pseudo);
            if (followee && follower)
                return RelationshipType.Mutual;
            else if (followee)
                return RelationshipType.Followee;
            else if (follower)
                return RelationshipType.Follower;
            else
                return RelationshipType.NotRelated;
        }

        public void ToggleFollowUnfollow(Member otherMember) {
            if (otherMember == null)
                throw new Exception("member may not be null");
            switch (GetRelationshipType(otherMember)) {
                case RelationshipType.Mutual:
                case RelationshipType.Followee:
                    Unfollow(otherMember);
                    break;
                case RelationshipType.Follower:
                case RelationshipType.NotRelated:
                default:
                    Follow(otherMember);
                    break;
            }
        }

        public Message Send(Member recipient, string body, bool isPrivate = false) {
            var msg = new Message(this, recipient, body, isPrivate);
            Context.Messages.Add(msg);
            Context.SaveChanges();
            return msg;
        }

        public void Delete() {
            Followees.Clear();
            Followers.Clear();
            // Supprime les messages envoyés ou reçus
            Context.Messages.RemoveRange(MessagesSent);
            MessagesSent.Clear();
            Context.Messages.RemoveRange(MessagesReceived);
            MessagesReceived.Clear();
            // Supprime le membre lui-même
            Context.Members.Remove(this);
            Context.SaveChanges();
        }
		
		public static IQueryable<Member> GetAll() {
            return Context.Members.OrderBy(m => m.Pseudo);
        }

        public static IQueryable<Member> GetFiltered(string Filter) {
            var filtered = from m in Context.Members
                           where m.Pseudo.Contains(Filter) || m.Profile.Contains(Filter)
                           orderby m.Pseudo
                           select m;
            return filtered;
        }

        public bool CanFollow {
            get => Followees.Count < MAX_FOLLOWEES;
        }
    }
}