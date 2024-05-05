using HospitalManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalManagement.Forms.DoctorForms
{
    public partial class CreatePatientForm : Form
    {
        private List<Control>        m_createPatientControls;
        private ApplicationDbContext m_db;

        public CreatePatientForm()
        {
            InitializeComponent();
        }

        public CreatePatientForm(ApplicationDbContext t_db):this()
        {
            this.m_db = t_db;
            LoadMedicalConditionListBoxData();
            PopulateCreatePatientControls();
        }

        private void LoadMedicalConditionListBoxData()
        {
            medicalConditionListBox.Items.Clear();
            var _allMedicalConditions = m_db.MedicalConditions.ToList();

            foreach (var _medicalCondition in _allMedicalConditions)
            {
                medicalConditionListBox.Items.Add(_medicalCondition.Name);
            }
        }

        private void PopulateCreatePatientControls()
        {
            m_createPatientControls = new List<Control>();
            m_createPatientControls.Add(egnTextBox);
            m_createPatientControls.Add(firstNameTextBox);
            m_createPatientControls.Add(middleNameTextBox);
            m_createPatientControls.Add(lastNameTextBox);
        }

        private async void createPatientButton_Click(object sender, EventArgs e)
        {
            if (!CheckIfAllInfoIsFilled())
            {
                MessageBox.Show("Моля попълни всички полета нужни за създаването на пациент!", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // ако егнто не е точно 10 символа ИЛИ на егнто всичките му символи НЕ са числа
                if (egnTextBox.Text.Length != 10 || !(egnTextBox.Text.All(char.IsDigit)))
                {
                    MessageBox.Show("Невалидно ЕГН. Егнто трябва да е точно 10 символа дълго и да съдържа само цифри", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // провери дали има пациент с регистрирано такова егн
                var _patientAlreadyExists = m_db.Patients.FirstOrDefault(x => x.EGN == egnTextBox.Text);
                if (_patientAlreadyExists != null)
                {
                    MessageBox.Show("Вече има регистриран пациент с такова ЕГН.", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // ако пациента НЕ съществува ще продъжи надолу

                // създай пациент
                var _patient = new Patient()
                {
                    FirstName   = firstNameTextBox.Text,
                    MiddleName  = middleNameTextBox.Text,
                    LastName    = lastNameTextBox.Text,
                    EGN         = egnTextBox.Text,
                };

                // ако има избрано заболяване за съответния пациент, запамети го
                if (medicalConditionListBox.SelectedIndex != -1)
                {
                    var _medicalCondition = m_db.MedicalConditions.Single(x => x.Name == medicalConditionListBox.SelectedItem.ToString());
                    _patient.MedicalConditionId = _medicalCondition.Id;
                }

                await m_db.Patients.AddAsync(_patient);
                await m_db.SaveChangesAsync();

                foreach (var _control in m_createPatientControls)
                {
                    _control.Text = "";
                }
                medicalConditionListBox.SelectedIndex = -1;

                MessageBox.Show("Вие успешно създадохте нов пациент. Вече можете да му изписвате рецепти.", "Успешно създаден пациент.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool CheckIfAllInfoIsFilled()
        {
            foreach (var _control in m_createPatientControls)
            {
                // за всеки един TextBox ако текста само на един даже да не е попълнен
                if (string.IsNullOrWhiteSpace(_control.Text))
                {
                    // върни false че не е попълнен
                    return false;
                }

            }
            // ако всички полета за добавяне на пациент са попълнени коректно върни true
            return true;
        }
    }
}
