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
    public partial class SearchPatientForm : Form
    {
        private ApplicationDbContext m_db;
        private Action<Form>         m_openChildForm;
        private int                  m_currentLoggedInDoctorId;
        public SearchPatientForm()
        {
            InitializeComponent();
        }
        public SearchPatientForm(ApplicationDbContext db, Action<Form> openChildForm, int currentLoggedInDoctorId) : this()
        {
            this.m_db = db;
            this.m_openChildForm = openChildForm;
            this.m_currentLoggedInDoctorId = currentLoggedInDoctorId;
            PopulateSearchCriteriaListBox();
        }
        private void PopulateSearchCriteriaListBox()
        {
            searchCriteriaListBox.Items.Add("ЕГН");
            searchCriteriaListBox.Items.Add("Първо име");
            searchCriteriaListBox.Items.Add("Презиме");
            searchCriteriaListBox.Items.Add("Фамилия");
            searchCriteriaListBox.Items.Add("Заболяване");
        }
        private void searchPatientButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                MessageBox.Show("Моля попълнете полето за търсене на пациент", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (searchCriteriaListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Моля изберете критерий по който да се извърши търсенето на пациент.", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var _foundPatientsList = new List<Patient>();
            var _searchCriteria = searchCriteriaListBox.SelectedItem.ToString();
            var _searchTerm = searchTextBox.Text;
            switch (_searchCriteria)
            {
                case "ЕГН":
                    {
                        var patient = m_db.Patients.SingleOrDefault(p => p.EGN == _searchTerm);
                        if (patient != null)
                        {
                            _foundPatientsList.Add(patient);
                        }
                        break;
                    }
                case "Първо име":
                    {
                        var patients = m_db.Patients.Where(p => p.FirstName == _searchTerm).ToList();
                        foreach (var patient in patients)
                        {
                            _foundPatientsList.Add(patient);
                        }
                        break;
                    }
                case "Презиме":
                    {
                        var patients = m_db.Patients.Where(p => p.MiddleName == _searchTerm).ToList();
                        foreach (var patient in patients)
                        {
                            _foundPatientsList.Add(patient);
                        }
                        break;
                    }
                case "Фамилия":
                    {
                        var patients = m_db.Patients.Where(p => p.LastName == _searchTerm).ToList();
                        foreach (var patient in patients)
                        {
                            _foundPatientsList.Add(patient);
                        }
                        break;
                    }
                case "Заболяване":
                    {
                        var medicalCondition = m_db.MedicalConditions.SingleOrDefault(x => x.Name == _searchTerm.ToUpper());
                        if (medicalCondition == null)
                        {
                            MessageBox.Show("Таково заболяване не съществува.", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var patients = m_db.Patients.Include(p=>p.MedicalCondition).Where(p => p.MedicalCondition.Name == _searchTerm.ToUpper()).ToList();
                        foreach (var patient in patients)
                        {
                            _foundPatientsList.Add(patient);
                        }
                        break;
                    }
                default:
                    return;

            }

            if (_foundPatientsList.Count() == 0)
            {
                foundPatientsListBox.Items.Clear();
                MessageBox.Show("Няма намерени пациенти по този критерий.", "Не бяха намерени пациенти.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                foundPatientsListBox.Items.Clear();

                foreach (var _patient in _foundPatientsList)
                {
                    foundPatientsListBox.Items.Add(_patient.EGN);
                }
                MessageBox.Show("Бяха намерени " + _foundPatientsList.Count() + " пациента.", "Има намерени пациенти.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        private bool CheckIfPatientSelected()
        {
            if (foundPatientsListBox.SelectedIndex == -1)
            {
                return false;
            }
            return true;
        }
        private Patient GetSelectedPatient()
        {
            var _selectedPatientEGN = foundPatientsListBox.SelectedItem.ToString();
            // .Single() тъй като егн-тата неможе да са еднакви на двама пациента
            // .Include() зада заредим заболяването на пациента/ ако той има такова ествествено
            return m_db.Patients.Include(p=>p.MedicalCondition).Single(p => p.EGN == _selectedPatientEGN);
        }
        private void showSelectedPatientButton_Click(object sender, EventArgs e)
        {
            if (!CheckIfPatientSelected())
            {
                MessageBox.Show("Трябва да изберете пациент първо.", "Не сте избрали пациент.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var _selectedPatient = GetSelectedPatient();
            m_openChildForm(new ShowPatientForm(m_db, _selectedPatient));
        }
        private void editSelectedPatientButton_Click(object sender, EventArgs e)
        {
            if (!CheckIfPatientSelected())
            {
                MessageBox.Show("Трябва да изберете пациент първо.", "Не сте избрали пациент.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var _selectedPatient = GetSelectedPatient();
            m_openChildForm(new EditPatientForm(m_db, _selectedPatient));
        }

        private async void deleteSelectedPatientButton_Click(object sender, EventArgs e)
        {
            if (!CheckIfPatientSelected())
            {
                MessageBox.Show("Трябва да изберете пациент първо.", "Не сте избрали пациент.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var _selectedPatient = GetSelectedPatient();
            var _result = MessageBox.Show("Вмомента сте на път да изтриете пациента с ЕГН " + _selectedPatient.EGN + ". Това ще изтрие и всичките му рецепти. Искате ли да изтриете този пациент?", "Сигурни ли сте че искате да изтриете този пациент?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (_result == DialogResult.Yes)
            {
                var _patientToDelete = m_db.Patients.Single(p => p.EGN == _selectedPatient.EGN);
                m_db.Patients.Remove(_patientToDelete);
                await m_db.SaveChangesAsync();
                foundPatientsListBox.Items.Remove(_patientToDelete.EGN);
            }
        }

        private void createSelectedPatientPrescription_Click(object sender, EventArgs e)
        {
            if (!CheckIfPatientSelected())
            {
                MessageBox.Show("Трябва да изберете пациент първо.", "Не сте избрали пациент.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var _selectedPatient = GetSelectedPatient();
            m_openChildForm(new CreatePrescriptionForm(m_db, m_currentLoggedInDoctorId, _selectedPatient));
        }

        private void showSelectedPatientPrescriptionsButton_Click(object sender, EventArgs e)
        {
            if (!CheckIfPatientSelected())
            {
                MessageBox.Show("Трябва да изберете пациент първо.", "Не сте избрали пациент.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var _selectedPatient = GetSelectedPatient();
            m_openChildForm(new ShowPrescriptionsOfPatient(m_db, _selectedPatient));
        }
    }
}
