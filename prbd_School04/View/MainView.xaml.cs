﻿using PRBD_Framework;
using School04.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace School04.View {
    /// <summary>
    /// Logique d'interaction pour MainView.xaml
    /// </summary>
    public partial class MainView : WindowBase {
        public MainView() {
            InitializeComponent();
        }

        private void Vm_DisplayCourse(Course course, bool isNew) {
            if (course != null) {
                var tab = tabControl.FindByTag(course.Title);
                if (tab == null)
                    tabControl.Add(
                        new CourseDetailsView(course, isNew),
                        isNew ? "<new course>" : course.Title, course.Title
                    );
                else
                    tabControl.SetFocus(tab);
            }
        }

        private void Vm_DisplayQuizz(Quizz quizz, bool isNew) {
            if (quizz != null) {
                var tab = tabControl.FindByTag(quizz.QuizzId.ToString());
                if (tab == null)
                    tabControl.Add(
                        new QuizzView(quizz, isNew),
                        isNew ? "<new quizz>" : quizz.Title, quizz.QuizzId.ToString()
                    );
                else
                    tabControl.SetFocus(tab);
            }
        }

        //KeyDown="WindowBase_KeyDown" -> handler à l'événement KeyDown afin de pouvoir fermer 
        //la fenêtre quand on presse la touche 
        private void WindowBase_KeyDown( object sender, KeyEventArgs e ) {
            if (e.Key == Key.Escape)
                Close();
        }
    }
}
