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
    public partial class MedicalConditionForm : Form
    {
        private ApplicationDbContext m_db;

        public MedicalConditionForm()
        {
            InitializeComponent();
        }

        public MedicalConditionForm(ApplicationDbContext t_db):this()
        {
            this.m_db = t_db;
            LoadMedicalConditionListBoxData();
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

        private async void createMedicalConditionButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(medicalConditionNameTextBox.Text))
            {
                MessageBox.Show("Въведи име за новото заболяване!", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // винаги запаметяваме заболяванията с главни букви -> .ToUpperInvariant()
                var _medicalCondition = new MedicalCondition()
                {
                    Name = medicalConditionNameTextBox.Text.ToUpperInvariant()
                };

                if (m_db.MedicalConditions.FirstOrDefault(x => x.Name == _medicalCondition.Name) != null)
                {
                    MessageBox.Show("Вече съществува такова заболяване!", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    await m_db.MedicalConditions.AddAsync(_medicalCondition);
                    await m_db.SaveChangesAsync();
                    LoadMedicalConditionListBoxData();
                    MessageBox.Show("Вие успешно създадохте ново заболяване. Вече можете да създавате пациенти, които го притежават!", "Успешно създадено ново заболяване.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private async void deleteMedicalConditionButton_Click(object sender, EventArgs e)
        {
            if (medicalConditionListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Моля изберете заболяване, което желаете да изтриете.", "Изберете заболяване за изтриване.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var _result = MessageBox.Show("Наистина ли искате да изтриете това заболяване? Всички пациенти страдащи от него вече няма да го имат. Искате ли да продължите?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (_result == DialogResult.Yes)
                {
                    // дай ми единственото заболяване с такова име
                    var _selectedMedicalCondition = m_db.MedicalConditions.Single(x => x.Name == medicalConditionListBox.SelectedItem.ToString());
                    var _patientsWithMedicalCondition = m_db.Patients.Include(x => x.MedicalCondition).Where(x => x.MedicalConditionId == _selectedMedicalCondition.Id);
                    foreach (var _patient in _patientsWithMedicalCondition)
                    {
                        _patient.MedicalConditionId = null;
                    }
                    m_db.MedicalConditions.Remove(_selectedMedicalCondition);
                    await m_db.SaveChangesAsync();
                    LoadMedicalConditionListBoxData();
                }
            }
        }
    }
}
