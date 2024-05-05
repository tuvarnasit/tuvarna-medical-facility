using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalManagement.Forms.AdminForms
{
    public partial class EditDoctorForm : Form
    {
        private ApplicationDbContext m_db;
        private List<Control>        m_editDoctorControls;
        private User                 m_userInfoToEdit;
        private Doctor               m_doctorInfoToEdit;
        public EditDoctorForm()
        {
            InitializeComponent();
        }
        // тази форма трябва да се ползва само със User-и, които са в роля Doctor
        // ако сложим user с друга роля ще получим грешка, имай го на предвид
        public EditDoctorForm(ApplicationDbContext t_db, User t_user):this()
        {
            this.m_db = t_db;
            LoadSpecialityListBoxData();
            PopulateEditDoctorControls();

            // вземи информацията на доктора за съответния user
            // знаем че ползваме .Single() понеже имаме връзка едно към едно
            // точно това би хвърлило грешката ако user-а всъщност няма докторски акаунт
            var _doctorInfo = m_db.Doctors
                .Include(d => d.DoctorSpeciality)
                .Single(d => d.UserId == t_user.Id);

            // сложи индекса на избраната специалност да е специалността на доктора
            var _index = this.specialityListBox.Items.IndexOf(_doctorInfo.DoctorSpeciality.Name);
            specialityListBox.SelectedIndex = _index;

            // попълни и другите данни на доктора
            emailTextBox.Text       = t_user.Email;
            passwordTextBox.Text    = t_user.Password;
            firstNameTextBox.Text   = _doctorInfo.FirstName;
            middleNameTextBox.Text  = _doctorInfo.MiddleName;
            lastNameTextBox.Text    = _doctorInfo.LastName;

            this.m_userInfoToEdit   = t_user;
            this.m_doctorInfoToEdit = _doctorInfo;

        }

        private void LoadSpecialityListBoxData()
        {
            specialityListBox.Items.Clear();
            var allDoctorSpecialities = m_db.DoctorSpecialities.ToList();

            foreach (var speciality in allDoctorSpecialities)
            {
                specialityListBox.Items.Add(speciality.Name);
            }
        }

        private void PopulateEditDoctorControls()
        {
            m_editDoctorControls = new List<Control>();
            m_editDoctorControls.Add(emailTextBox);
            m_editDoctorControls.Add(passwordTextBox);
            m_editDoctorControls.Add(firstNameTextBox);
            m_editDoctorControls.Add(middleNameTextBox);
            m_editDoctorControls.Add(lastNameTextBox);
        }

        private async void editDoctorButton_Click(object sender, EventArgs e)
        {
            if (!CheckIfAllInfoIsFilled())
            {
                MessageBox.Show("Моля попълни всички полета нужни за редактиране на доктор!", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var _dbUser     = m_db.Users.Single(u => u.Id == m_userInfoToEdit.Id);
                var _dbDoctor   = m_db.Doctors.Single(d => d.Id == m_userInfoToEdit.Id);

                var _currentSelectedSpeciality  = specialityListBox.SelectedItem.ToString();
                var _doctorSpeciality           = m_db.DoctorSpecialities.Single(x => x.Name == _currentSelectedSpeciality);

                // редактирай User профила на доктора

                // провери дали вече има друг user с такъв имейл

                var _dbUserWithNewEmail = m_db.Users.SingleOrDefault(u => u.Email == emailTextBox.Text);
                // ако не е NULL значи има такъв user с такъв имейл
                if (_dbUserWithNewEmail != null) 
                {
                    // ако Id-то на Userа който едитваме НЕ Е равно на Id-то на usera с новия имейл
                    // то тогава значи че това са два различни Usera
                    // тоест вече има регистриран User с този имейл
                    // и съответно не можем да променим този който редактираме на него.
                    if (_dbUser.Id != _dbUserWithNewEmail.Id)
                    {
                        MessageBox.Show("Вече има такъв регистриран потребител с този имейл. Моля пробвайте друг.", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                _dbUser.Email       = emailTextBox.Text;
                _dbUser.Password    = passwordTextBox.Text;

                await m_db.SaveChangesAsync();

                // редактирай самата информация на доктора
                _dbDoctor.FirstName             = firstNameTextBox.Text;
                _dbDoctor.MiddleName            = middleNameTextBox.Text;
                _dbDoctor.LastName              = lastNameTextBox.Text;
                _dbDoctor.DoctorSpecialityId    = _doctorSpeciality.Id;

                await m_db.SaveChangesAsync();

                MessageBox.Show("Вие успешно редактирахте този докторски акаунт.", "Успешно редактиран докторски акаунт", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private bool CheckIfAllInfoIsFilled()
        {
            // ако не е избрана специалност за доктора
            if (specialityListBox.SelectedIndex == -1)
            {
                return false;
            }

            foreach (var _control in m_editDoctorControls)
            {
                // за всеки един TextBox ако текста само на един даже да не е попълнен
                if (string.IsNullOrWhiteSpace(_control.Text))
                {
                    // върни false че не е попълнен
                    return false;
                }

            }
            // ако всички полета за добавяне на доктор са попълнени коректно върни true
            return true;
        }
    }
}
