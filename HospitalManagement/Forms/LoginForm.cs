using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Forms
{
    public partial class LoginForm : Form
    {
        private ApplicationDbContext m_db;
        public LoginForm()
        {
            InitializeComponent();
        }
        public LoginForm(ApplicationDbContext t_db) : this() // преизползвай празния конструктор
        {
            this.m_db = t_db;
        }
        private void loginButton_Click(object sender, EventArgs e)
        {
            var _email = emailTextBox.Text;
            var _password = passwordTextBox.Text;

            User _user = LoginUser(_email, _password);
            if (_user != null)
            {
                Form _formToDisplay;
                switch (_user.Role.Name.ToLower()) // за всеки случай ако ролите са в друг case
                {
                    case "admin":
                        _formToDisplay = new AdminPanelForm(_user);
                        break;
                    case "doctor":
                        _formToDisplay = new DoctorPanelForm(_user);
                        break;
                    default:
                        throw new ArgumentException("Това приложение не подържа други роли.");
                }

                this.Hide(); // скрий сегашната форма
                _formToDisplay.Closed += (s, args) => this.Close(); // при затваряне на следваща форма затвори и тази (която е скрита вмомента)
                _formToDisplay.Show(); // покажи новата форма
            }
            else
            {
                errorLabel.Visible = true;
            }
        }
        private User LoginUser(string t_email, string t_password)
        {
            // тук използваме SingleOrDefault за да намерим от таблицата Users ЕДИНСТВЕНИЯ
            // user с такъв имейл и парола. Ако има повече от един user с такъв имейл
            // ще получим грешка / но това няма как да стане тъй като в самата база от данни
            // сме задали Email полето да е UNIQUE/ да няма еднакви имейли
            // Ако не бъде намерен един user с такъв имейл и парола ще бъде върнано NULL
            var _user = m_db.Users
                .Include(u => u.Role) // зареди ми navigational property-то за ролите/ за да имам достъп до тях
                .SingleOrDefault(u => u.Email == t_email && u.Password == t_password);
            return _user;
        }
    }
}