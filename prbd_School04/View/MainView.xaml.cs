using PRBD_Framework;
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
                if (isNew)
                    tab = tabControl.FindByTag("<new course>");
                if (tab == null)
                    tabControl.Add(
                        new CourseDetailsView(course, isNew),
                        isNew ? "<new course>" : course.Title, isNew ? "<new course>" : course.Title
                    );
                else
                    tabControl.SetFocus(tab);
            }
        }
        private void Vm_DisplayQuizzTeacher(Quizz quizz, bool isNew) {
            if (quizz != null) {
                var tab = tabControl.FindByTag(quizz.Title);
                if (isNew)
                    tab = tabControl.FindByTag("<new quizz>");
                if (tab == null)
                    tabControl.Add(
                        new QuizzView(quizz, isNew),
                        isNew ? "<new quizz>" : quizz.Title, isNew ? "<new quizz>" : quizz.Title
                    );
                else
                    tabControl.SetFocus(tab);
            }
        }

        private void Vm_DisplayQuizzStudent(Quizz quizz, bool isNew) {
            if (quizz != null) {
                var tab = tabControl.FindByTag(quizz.Title);
                if (tab == null)
                    tabControl.Add(
                        new ResponseQuizzView(quizz, isNew),
                        quizz.Title, quizz.Title
                    );
                else
                    tabControl.SetFocus(tab);
            }
        }

        private void Vm_DisplayQuestion(Question question, bool isNew) {
            if (question != null) {
                var tab = tabControl.FindByTag(question.Enonce);
                if (tab == null)
                    tabControl.Add(
                        new NewQuestionView(question, isNew),
                        isNew ? "<NewQuestion>" : question.Enonce, question.Enonce
                    );
                else
                    tabControl.SetFocus(tab);
            }
        }

        //handler pour l'en-tete de l'onglet du nouveau course
        private void Vm_RenameTabCourseDetail( Course course, string header ) {
            var tab = tabControl.SelectedItem as TabItem;
            if (tab != null) {
                tab.Header = tab.Tag = header = string.IsNullOrEmpty(header) ? "<new course>" : header;
            }
        }
        private void Vm_RenameTabQuizz(Quizz quizz, string header) {
            var tab = tabControl.SelectedItem as TabItem;
            if (tab != null) {
                tab.Header = tab.Tag = header = string.IsNullOrEmpty(header) ? "<new quizz>" : header;
            }
        }
        private void Vm_CloseTabQuizz(Quizz quizz) {
            var tab = tabControl.FindByTag(quizz.Title);
            if(tab == null)
                tab = tabControl.FindByTag("<new quizz>");
            tabControl.Items.Remove(tab);
        }
        private void Vm_CloseTabCourse( Course course ) {
            var tab = tabControl.FindByTag(course.Title);
            if (tab == null)
                tab = tabControl.FindByTag("<new course>");
            tabControl.Items.Remove(tab);
        }
        private void Vm_CloseTabProfile() {
            var tab = tabControl.FindByTag("Profile");
            tabControl.Items.Remove(tab);
        }
        private void Menu_ProfileUser_Click( object sender, RoutedEventArgs e ) {
            var tab = tabControl.FindByTag("Profile");
            if (tab == null)
                tabControl.Add(new ProfileView(), "Profile", "Profile");
            else
                tabControl.SetFocus(tab);
        }
        //handler qui réalise le logout simplement en "redirigeant" vers la page de login
        private void Vm_OnLogout() {
            App.NavigateTo<LoginView>();
        }
        //handler qui de fermer la fenêtre courant (Close()), ce qui a pour effet de 
        //quitter l'application car c'est la fenêtre principale de l'application.
        private void WindowBase_KeyDown( object sender, KeyEventArgs e ) {
            if (e.Key == Key.Escape)
                Close();
        }
    }
}
