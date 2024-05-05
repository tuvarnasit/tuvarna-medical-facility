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

namespace HospitalManagement.Forms.DoctorForms
{
    public partial class EditPatientForm : Form
    {
        private ApplicationDbContext m_db;
        private List<Control>        m_editPatientControls;
        private Patient              m_patientInfoToEdit;
        public EditPatientForm()
        {
            InitializeComponent();
        }
        public EditPatientForm(ApplicationDbContext t_db, Patient t_patient) :this()
        {
            this.m_db = t_db;
            LoadMedicalConditionListBoxData();
            PopulateEditPatientControls();

            // ако пациента има заболяване
            if (t_patient.MedicalCondition != null)
            {
                // сложи индекса на избраното заболяване да е заболяването на пациента
                var _index = this.medicalConditionListBox.Items.IndexOf(t_patient.MedicalCondition.Name);
                medicalConditionListBox.SelectedIndex = _index;
            }
            else
            {
                // тъй като знаем че опцията НЯМА e index 0
                medicalConditionListBox.SelectedIndex = 0;
            }

            // попълни и другите данни на пациента
            egnTextBox.Text          = t_patient.EGN;
            firstNameTextBox.Text    = t_patient.FirstName;
            middleNameTextBox.Text   = t_patient.MiddleName;
            lastNameTextBox.Text     = t_patient.LastName;

            this.m_patientInfoToEdit = t_patient;

        }

        private void LoadMedicalConditionListBoxData()
        {
            medicalConditionListBox.Items.Clear();
            var _allMedicalConditions = m_db.MedicalConditions.ToList();

            // това винаги ще е index 0
            medicalConditionListBox.Items.Add("НЯМА");

            foreach (var _medicalCondition in _allMedicalConditions)
            {
                medicalConditionListBox.Items.Add(_medicalCondition.Name);
            }
        }

        private void PopulateEditPatientControls()
        {
            m_editPatientControls = new List<Control>();
            m_editPatientControls.Add(egnTextBox);
            m_editPatientControls.Add(firstNameTextBox);
            m_editPatientControls.Add(middleNameTextBox);
            m_editPatientControls.Add(lastNameTextBox);
        }

        private async void editPatientButton_Click(object sender, EventArgs e)
        {
            if (!CheckIfAllInfoIsFilled())
            {
                MessageBox.Show("Моля попълни всички полета нужни за редактиране на пациент!", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // ако егнто не е точно 10 символа ИЛИ на егнто всичките му символи НЕ са числа
                if (egnTextBox.Text.Length != 10 || !(egnTextBox.Text.All(char.IsDigit)))
                {
                    MessageBox.Show("Невалидно ЕГН. Егнто трябва да е точно 10 символа дълго и да съдържа само цифри", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var _dbPatient                       = m_db.Patients.Single(p => p.Id == m_patientInfoToEdit.Id);
                var _currentSelectedMedicalCondition = medicalConditionListBox.SelectedItem.ToString();

                if (_currentSelectedMedicalCondition != "НЯМА")
                {
                    var _patientMedicalCondition  = m_db.MedicalConditions.Single(x => x.Name == _currentSelectedMedicalCondition);
                    _dbPatient.MedicalConditionId = _patientMedicalCondition.Id;
                }
                else
                {
                    _dbPatient.MedicalConditionId = null;
                }

                // провери дали вече има друг patient с такова EGN

                var _dbPatientWithNewEGN = m_db.Patients.SingleOrDefault(p => p.EGN == egnTextBox.Text);
                // ако не е NULL значи има такъв пациент с такова EGN
                if (_dbPatientWithNewEGN != null) 
                {
                    // тази проверка я правим, понеже когато доктор редактира пациент
                    // той може да иска да му смени всичко останало освен ЕГНто
                    // провери дали това егн принадлежи на пациента, който редактираме сега
                    // ако това егн принадлежи на друг пациент, то тогава хвърли грешка
                    // понеже не може да имаме 2 еднакви егн-та в базата
                    // а ако егн-то е на пациента, който редактираме вмомента няма никакъв проблем
                    // мини нататък и го редактирай
                    if (_dbPatient.Id != _dbPatientWithNewEGN.Id)
                    {
                        MessageBox.Show("Вече има пациент с такова ЕГН.", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                // редактирай информацията на пациента
                _dbPatient.EGN          = egnTextBox.Text;
                _dbPatient.FirstName    = firstNameTextBox.Text;
                _dbPatient.MiddleName   = middleNameTextBox.Text;
                _dbPatient.LastName     = lastNameTextBox.Text;
                await m_db.SaveChangesAsync();

                MessageBox.Show("Вие успешно редактирахте информацията на този пациент.", "Успешно редактиран пациент", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private bool CheckIfAllInfoIsFilled()
        {
            foreach (var _control in m_editPatientControls)
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
