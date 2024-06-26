﻿using HospitalManagement.Models;
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
    public partial class CreateDoctorForm : Form
    {
        private List<Control>        m_createDoctorControls;
        private ApplicationDbContext m_db;

        public CreateDoctorForm()
        {
            InitializeComponent();
        }

        public CreateDoctorForm(ApplicationDbContext t_db) : this()
        {
            this.m_db = t_db;
            LoadSpecialityListBoxData();
            PopulateCreateDoctorControls();
        }

        private void LoadSpecialityListBoxData()
        {
            specialityListBox.Items.Clear();
            var _allDoctorSpecialities = m_db.DoctorSpecialities.ToList();

            foreach (var speciality in _allDoctorSpecialities)
            {
                specialityListBox.Items.Add(speciality.Name);
            }
        }

        private void PopulateCreateDoctorControls()
        {
            m_createDoctorControls = new List<Control>();
            m_createDoctorControls.Add(emailTextBox);
            m_createDoctorControls.Add(passwordTextBox);
            m_createDoctorControls.Add(firstNameTextBox);
            m_createDoctorControls.Add(middleNameTextBox);
            m_createDoctorControls.Add(lastNameTextBox);
        }

        private async void createDoctorButton_Click(object sender, EventArgs e)
        {
            if (!CheckIfAllInfoIsFilled())
            {
                MessageBox.Show("Моля попълни всички полета нужни за създаването на доктор!", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var _userAlreadyExists = m_db.Users.FirstOrDefault(x => x.Email == emailTextBox.Text);
                if (_userAlreadyExists != null)
                {
                    MessageBox.Show("Вече има регистриран User с такъв имейл.", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ако ролята doctor не съществува ще хвърли грешка,
                // което е напълно коректно, тъй като предстоящата логика
                // е за доктори
                var _doctorRole = m_db.Roles.Single(x => x.Name.ToLower() == "doctor");

                var _user = new User()
                {
                    Email       = emailTextBox.Text,
                    Password    = passwordTextBox.Text,
                    RoleId      = _doctorRole.Id
                };

                var _savedUser = await m_db.AddAsync(_user);
                await m_db.SaveChangesAsync();

                var _currentSelectedSpeciality = specialityListBox.SelectedItem.ToString();

                // тук отново метода .Single() би хвърлил грешка ако не бъде намерена докторска специалност
                // или има повече от 1 докторска специалност с такова име
                var _doctorSpeciality = m_db.DoctorSpecialities.Single(x => x.Name == _currentSelectedSpeciality);
                var _doctor = new Doctor()
                {
                    FirstName           = firstNameTextBox.Text,
                    MiddleName          = middleNameTextBox.Text,
                    LastName            = lastNameTextBox.Text,
                    UserId              = _savedUser.Entity.Id,
                    DoctorSpecialityId  = _doctorSpeciality.Id
                };

                await m_db.Doctors.AddAsync(_doctor);
                await m_db.SaveChangesAsync();

                foreach (var _control in m_createDoctorControls)
                {
                    _control.Text = "";
                }
                specialityListBox.SelectedIndex = -1;

                MessageBox.Show("Вие успешно създадохте нов докторски акаунт. Този доктор вече може да влиза с въведените от вас имейл и парола.", "Успешно създаден докторски акаунт", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool CheckIfAllInfoIsFilled()
        {
            // ако не е избрана специалност за доктора
            if (specialityListBox.SelectedIndex == -1)
            {
                return false;
            }

            foreach (var control in m_createDoctorControls)
            {
                // за всеки един TextBox ако текста само на един даже да не е попълнен
                if (string.IsNullOrWhiteSpace(control.Text))
                {
                    // върни false че не е попълнен
                    return false;
                }

            }
            // ако всички полета за добавяне на доктор са попълнени коректно върни true
            return true;
        }

        private void emailTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
