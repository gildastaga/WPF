using System;
using System.IO;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Msn.Model;
using PRBD_Framework;

namespace Msn.ViewModel {
    public class MemberDetailViewModel : ViewModelCommon {
        private ImageHelper imageHelper;

        public event Func<string> OnLoadImage;

        public ICommand Save { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Delete { get; set; }
        public ICommand LoadImage { get; set; }
        public ICommand ClearImage { get; set; }
        public ICommand FollowUnfollow { get; set; }

        private Member member;
        public Member Member { get => member; set => SetProperty(ref member, value); }

        private bool isNew;
        public bool IsNew {
            get { return isNew; }
            set {
                isNew = value;
                RaisePropertyChanged(nameof(IsNew));
            }
        }

        public bool IsExisting { get => !isNew; }

        public string Pseudo {
            get { return Member?.Pseudo; }
            set {
                Member.Pseudo = value;
                RaisePropertyChanged(nameof(Pseudo));
                NotifyColleagues(AppMessages.MSG_PSEUDO_CHANGED, Member);
            }
        }

        public string Profile {
            get { return Member?.Profile; }
            set {
                Member.Profile = value;
                RaisePropertyChanged(nameof(Profile));
            }
        }

        public string PicturePath {
            get { return Member?.AbsolutePicturePath; }
            set {
                Member.PicturePath = value;
                RaisePropertyChanged(nameof(PicturePath));
            }
        }

        public string FollowStatus {
            get {
                if (CurrentUser.Pseudo == Member?.Pseudo)
                    return Properties.Resources.MembersDetailView_ThisIsYou;
                return (CurrentUser.GetRelationshipType(Member)) switch {
                    RelationshipType.Mutual => Properties.Resources.MembersDetailView_MutualFriend,
                    RelationshipType.Followee => Properties.Resources.MembersDetailView_YouAreFollowing,
                    RelationshipType.Follower => Properties.Resources.MembersDetailView_IsFollowingYou,
                    _ => Properties.Resources.MembersDetailView_NotRelated,
                };
            }
        }

        public string FollowUnfollowLabel {
            get {
                return (CurrentUser.GetRelationshipType(Member)) switch {
                    RelationshipType.Mutual or RelationshipType.Followee => Properties.Resources.MembersDetailView_Unfollow,
                    _ => Properties.Resources.MembersDetailView_Follow,
                };
            }
        }

        public bool IsNotCurrentMember {
            get => Member?.Pseudo != CurrentUser.Pseudo;
        }

        public MemberDetailViewModel() : base() {
            Save = new RelayCommand(SaveAction, CanSaveAction);
            Cancel = new RelayCommand(CancelAction, CanCancelAction);
            Delete = new RelayCommand(DeleteAction, () => !IsNew);
            LoadImage = new RelayCommand(LoadImageAction);
            ClearImage = new RelayCommand(ClearImageAction, () => PicturePath != null);
            FollowUnfollow = new RelayCommand(FollowUnfollowAction, 
                () => CurrentUser != null && (CurrentUser.Followees.Contains(Member) || CurrentUser.CanFollow));
        }

        public void Init(Member member, bool isNew) {
            Member = member;
            IsNew = isNew;

            imageHelper = new ImageHelper(App.IMAGE_PATH, Member.PicturePath);

            RaisePropertyChanged();
        }

        protected override void OnRefreshData() {
        }

        private void SaveAction() {
            if (IsNew) {
                // Un petit raccourci ;-)
                Member.Password = Member.Pseudo;
                Context.Add(Member);
                IsNew = false;
            }
            imageHelper.Confirm(Member.Pseudo);
            PicturePath = imageHelper.CurrentFile;
            Context.SaveChanges();
            NotifyColleagues(AppMessages.MSG_MEMBER_CHANGED, Member);
        }

        private bool CanSaveAction() {
            if (IsNew)
                return !string.IsNullOrEmpty(Pseudo);
            return Member != null && (Context?.Entry(Member)?.State == EntityState.Modified);
        }

        private void CancelAction() {
            if (imageHelper.IsTransitoryState) {
                imageHelper.Cancel();
            }
            if (IsNew) {
                NotifyColleagues(AppMessages.MSG_CLOSE_TAB, Member);
            } else {
                Context.Reload(Member);
                RaisePropertyChanged();
            }
        }

        private bool CanCancelAction() {
            return Member != null && (IsNew || Context?.Entry(Member)?.State == EntityState.Modified);
        }

        private void LoadImageAction() {
            var res = OnLoadImage?.Invoke();
            if (res != null) {
                imageHelper.Load(res);
                PicturePath = imageHelper.CurrentFile;
            }
        }

        private void ClearImageAction() {
            imageHelper.Clear();
            PicturePath = imageHelper.CurrentFile;
        }

        private void DeleteAction() {
            CancelAction();
            if (File.Exists(PicturePath))
                File.Delete(PicturePath);
            Member.Delete();
            NotifyColleagues(AppMessages.MSG_MEMBER_CHANGED, Member);
            NotifyColleagues(AppMessages.MSG_CLOSE_TAB, Member);
        }

        private void FollowUnfollowAction() {
            CurrentUser.ToggleFollowUnfollow(Member);
            RaisePropertyChanged(nameof(FollowStatus), nameof(FollowUnfollowLabel));
            NotifyColleagues(AppMessages.MSG_MEMBER_CHANGED, Member);
        }

        public override void Dispose() {
            if (imageHelper.IsTransitoryState)
                imageHelper.Cancel();
            base.Dispose();
        }
    }
}
