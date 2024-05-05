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
    public partial class ShowPatientForm : Form
    {
        private ApplicationDbContext m_db;
        public ShowPatientForm()
        {
            InitializeComponent();
        }
        public ShowPatientForm(ApplicationDbContext t_db, Patient t_patient):this()
        {
            this.m_db = t_db;

            var _patientPrescriptionsCount = t_db.Prescriptions
                .Where(p => p.PatientId == t_patient.Id)
                .Count();

            this.firstNameLabel.Text            += t_patient.FirstName;
            this.middleNameLabel.Text           += t_patient.MiddleName;
            this.lastNameLabel.Text             += t_patient.LastName;
            this.prescriptionsAmountLabel.Text  += _patientPrescriptionsCount;

            if (t_patient.MedicalCondition != null)
            {
                this.medicalConditionLabel.Text += t_patient.MedicalCondition.Name;
            }
            else
            {
                this.medicalConditionLabel.Text += "няма";
            }
        }
    }
}
