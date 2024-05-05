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
    public partial class ShowPrescriptionsOfPatient : Form
    {
        private ApplicationDbContext m_db;

        public ShowPrescriptionsOfPatient()
        {
            InitializeComponent();
        }

        public ShowPrescriptionsOfPatient(ApplicationDbContext t_db, Patient t_patient) :this()
        {
            this.m_db                    = t_db;
            this.egnTextBox.Text         = t_patient.EGN;
            var _allPatientPrescriptions = t_db.Prescriptions.Where(p => p.PatientId == t_patient.Id).ToList();

            if (_allPatientPrescriptions.Count == 0)
            {
                allPatientPrescriptionsListBox.Items.Add("няма");
                prescriptionTextBox.Text = "няма";
            }
            else
            {
                foreach (var _prescription in _allPatientPrescriptions)
                {
                    allPatientPrescriptionsListBox.Items.Add(_prescription.Id);
                }
                allPatientPrescriptionsListBox.SelectedIndexChanged += allPatientPrescriptionsListBox_SelectedIndexChanged;
            }
        }

        private void allPatientPrescriptionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var _prescriptionId       = (int)allPatientPrescriptionsListBox.SelectedItem;
            var _prescription         = m_db.Prescriptions.Single(p => p.Id == _prescriptionId);
            prescriptionTextBox.Text  = _prescription.PrescriptionText;
        }
    }
}
