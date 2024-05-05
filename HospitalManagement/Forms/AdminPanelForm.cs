using HospitalManagement.Forms.AdminForms;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Forms
{
    public partial class AdminPanelForm : Form
    {
        private ApplicationDbContext m_db;
        private Color                m_activeButtonColor;
        private Color                m_notActiveButtonColor;
        private Form                 m_activeForm;
        public AdminPanelForm()
        {
            InitializeComponent();
        }
        public AdminPanelForm(User user):this()
        {
            this.m_db = new ApplicationDbContext();
            m_activeButtonColor = Color.FromArgb(0, 151, 230);
            m_notActiveButtonColor = Color.FromArgb(0, 168, 255);

            // това е default-ната форма която е отворена в началото
            MakeFormActive(new HomeForm(user.Email, user.Role.Name));
        }

        private void OpenChildForm(Form t_childForm)
        {
            if (t_childForm.Text == m_activeForm.Text)
            {
                // ако искаме отново да отворим същата форма, няма нужда да я презареждаме.
                return;
            }

            // затвори предишната форма
            if (m_activeForm != null)
            {
                m_activeForm.Close();
            }
            // направи новата форма да е активна
            MakeFormActive(t_childForm);
        }
        private void MakeFormActive(Form t_childForm)
        {
            m_activeForm                = t_childForm;
            t_childForm.TopLevel        = false;
            t_childForm.FormBorderStyle = FormBorderStyle.None;
            t_childForm.Dock            = DockStyle.Fill;
            contentPanel.Controls.Add(t_childForm);
            titleLabel.Text             = t_childForm.Text;
            t_childForm.BringToFront();
            t_childForm.Show();
        }
        private void createDoctorMenuButton_Click(object sender, EventArgs e)
        {
            createDoctorMenuButton.BackColor     = m_activeButtonColor;
            searchDoctorMenuButton.BackColor     = m_notActiveButtonColor;
            createSpecialityMenuButton.BackColor = m_notActiveButtonColor;

            OpenChildForm(new CreateDoctorForm(m_db));
        }
        private void searchDoctorMenuButton_Click(object sender, EventArgs e)
        {
            searchDoctorMenuButton.BackColor     = m_activeButtonColor;
            createDoctorMenuButton.BackColor     = m_notActiveButtonColor;
            createSpecialityMenuButton.BackColor = m_notActiveButtonColor;

            // Тъй като искаме винаги да изпълняваме метода OpenChildForm за всяка една нова отворена форма,
            // а нямаме достъп до този метод вътре в друг клас, ползваме функционално програмиране
            // за да предадем този метод като параметър на конструктора на класа, който се нуждае от него
            // в нашия случай имаме класа SearchDoctorForm, който ТРЯБВА ДА МОЖЕ да отваря форми
            // затова му предоставяме метода OpenChildForm като параметър
            
            // създаваме си нов Action който като параметър приема инстанция на Form класа
            // и изпълнява метода в този клас OpenChildForm
            // след това просто даваме този Action<Form> като параметър при създаването на
            // инстанция на SearchDoctorForm
            // и вече SearchDoctorForm може да изпълнява форми както този клас
            var _openChildFormMethod = (Form form) => OpenChildForm(form);
            var _searchDoctorForm    = new SearchDoctorForm(m_db, _openChildFormMethod);
            OpenChildForm(_searchDoctorForm);
        }

        private void createSpecialityMenuButton_Click(object sender, EventArgs e)
        {
            createSpecialityMenuButton.BackColor = m_activeButtonColor;
            searchDoctorMenuButton.BackColor     = m_notActiveButtonColor;
            createDoctorMenuButton.BackColor     = m_notActiveButtonColor;

            OpenChildForm(new DoctorSpecialityForm(m_db));
        }
    }
}
