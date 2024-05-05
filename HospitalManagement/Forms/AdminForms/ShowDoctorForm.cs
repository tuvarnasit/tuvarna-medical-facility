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
    public partial class ShowDoctorForm : Form
    {
        private ApplicationDbContext m_db;
        public ShowDoctorForm()
        {
            InitializeComponent();
        }
        // тази форма трябва да се ползва само със User-и, които са в роля Doctor
        // ако сложим user с друга роля ще получим грешка, имай го на предвид
        public ShowDoctorForm(ApplicationDbContext t_db, User t_user) :this()
        {
            this.m_db                    = t_db;
            this.emailLabel.Text        += t_user.Email;
            this.passwordLabel.Text     += t_user.Password;
            // вземи информацията на доктора за съответния user
            // знаем че ползваме .Single() понеже имаме връзка едно към едно
            // точно това би хвърлило грешката ако user-а всъщност няма докторски акаунт
            var _doctorInfo = m_db.Doctors
                .Include(d => d.DoctorSpeciality)
                .Single(d => d.UserId == t_user.Id);

            // вземи колко рецепти е изписал този доктор
            var _doctorPrescriptionCount = m_db.Prescriptions
                .Where(p => p.DoctorId == _doctorInfo.Id)
                .Count();

            this.firstNameLabel.Text            += _doctorInfo.FirstName;
            this.middleNameLabel.Text           += _doctorInfo.MiddleName;
            this.lastNameLabel.Text             += _doctorInfo.LastName;
            this.specialityLabel.Text           += _doctorInfo.DoctorSpeciality.Name;
            this.prescriptionsAmountLabel.Text  += _doctorPrescriptionCount;
        }
    }
}
