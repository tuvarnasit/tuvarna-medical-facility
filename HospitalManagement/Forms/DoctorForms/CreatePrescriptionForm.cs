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
    public partial class CreatePrescriptionForm : Form
    {
        private ApplicationDbContext m_db;
        private int                  m_doctorId;
        private int                  m_patientId;

        public CreatePrescriptionForm()
        {
            InitializeComponent();
        }

        // доктор Id и patient Id трябва винаги да са валидни
        public CreatePrescriptionForm(ApplicationDbContext t_db, int t_doctorId, Patient t_patient) : this()
        {
            this.m_db                   = t_db;
            this.m_doctorId             = t_doctorId;
            this.m_patientId            = t_patient.Id;

            this.egnTextBox.Text        = t_patient.EGN;
            this.firstNameTextBox.Text  = t_patient.FirstName;
            this.middleNameTextBox.Text = t_patient.MiddleName;
            this.lastNameTextBox.Text   = t_patient.LastName;
        }
        private async void createPrescriptionButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(prescriptionTextBox.Text))
            {
                MessageBox.Show("Рецептата не може да бъде празна. Моля попълнете я.", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var prescription = new Prescription()
            {
                DoctorId            = m_doctorId,
                PatientId           = m_patientId,
                PrescriptionText    = prescriptionTextBox.Text,
                DateCreated         = DateTime.UtcNow.ToShortDateString()
            };

            await m_db.Prescriptions.AddAsync(prescription);
            await m_db.SaveChangesAsync();
            prescriptionTextBox.Text = "";
            MessageBox.Show("Успешно създадохте рецепта за този пациент.", "Успешно създадена рецепта.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
