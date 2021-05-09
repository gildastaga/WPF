﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School04.Model;

namespace School04.ViewModel {
    public class MainViewModel : ViewModelCommon {
        public event Action<Course, bool> DisplayCourse;
        public event Action<Quizz, bool> DisplayQuizz;
        public event Action<Quizz, string> RenameTabQuizz;
        public MainViewModel() : base() {
            Register<Course>(this, AppMessages.MSG_DISPLAY_COURSE, course => {
                Console.WriteLine("Test");
                DisplayCourse?.Invoke(course, false);
            });

            Register<Quizz>(this, AppMessages.MSG_DISPLAY_QUIZZ, quizz => {
                Console.WriteLine("Test");
                DisplayQuizz?.Invoke(quizz, false);
            });

            Register<Quizz>(this, AppMessages.MSG_NEW_QUIZZ, quizz => {
                Console.WriteLine("Test");
                DisplayQuizz?.Invoke(quizz, true);
            });

            Register<Quizz>(this, AppMessages.MSG_TITLE_QUIZZ_CHANGED, quizz => {
                RenameTabQuizz?.Invoke(quizz, quizz.Title);
            });

            Register(this, AppMessages.MSG_NEW_COURSE, () => {
                //créer une nouvelle instance pour un nouveau cours "vide"
                //var course = new Course(null,"","",null,(Teacher)CurrentUser);
                //demande à la vue de créer dynamiquement un nouvel onglet avec le titre "new course"
                //DisplayCourse?.Invoke(course, true);
            });
        }

        public string TitleWindow {
            get => $"School04 ({CurrentUser.ToString()})";
        }

        protected override void OnRefreshData() {
            //pour plus tard
        }
    }
}
