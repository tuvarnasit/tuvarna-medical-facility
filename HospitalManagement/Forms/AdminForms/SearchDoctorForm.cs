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
    public partial class SearchDoctorForm : Form
    {
        private ApplicationDbContext m_db;

        // тези полета ги използваме за да може да предадем от AdminPanel методи, които
        // да се изпълнят при изпълняването на event handler-ите, които отговарят
        // при кликане на съответен бутон
        // накратко даваме код ОТВЪН, който да се изпълни при кликането на бутоните.
        private Action<Form>         m_openChildForm;
        public SearchDoctorForm()
        {
            InitializeComponent();
        }
        // в конструктора задължаваме на външния клас, който прави инстанция на този клас
        // да предаде методите, които да се изпълнят при кликане на бутоните
        // за показване на доктор/ редактиране на доктор/ изтриване на доктор
        public SearchDoctorForm(ApplicationDbContext t_db, Action<Form> t_openChildForm) : this()
        {
            this.m_db = t_db;
            this.m_openChildForm = t_openChildForm;
            PopulateSearchCriteriaListBox();
        }
        private void PopulateSearchCriteriaListBox()
        {
            searchCriteriaListBox.Items.Add("Имейл");
            searchCriteriaListBox.Items.Add("Първо име");
            searchCriteriaListBox.Items.Add("Презиме");
            searchCriteriaListBox.Items.Add("Фамилия");
            searchCriteriaListBox.Items.Add("Специалност");
        }
        private void searchDoctorButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                MessageBox.Show("Моля попълнете полето за търсене на доктор", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (searchCriteriaListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Моля изберете критерий по който да се извърши търсенето на доктор.", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var _foundDoctorsList = new List<Doctor>();
            var _searchCriteria = searchCriteriaListBox.SelectedItem.ToString();
            var _searchTerm = searchTextBox.Text;
            switch (_searchCriteria)
            {
                case "Имейл":
                    {
                        // намери дали такъв User е регистриран с този имейл
                        // ползваме SingleOrDefault тъй като
                        // трябва да имаме само един User с такъв имейл
                        // ако не бъде намерен 1 такъв User, то ми върни NULL
                        // също така зареди ми Role полето на User-а с .Include() че да имам достъп до името на ролята на usera
                        var _user = m_db.Users.Include(u => u.Role).SingleOrDefault(u => u.Email == _searchTerm);
                        if (_user != null)
                        {
                            // ако ролята е доктор
                            if (_user.Role.Name.ToLower() == "doctor")
                            {
                                // намери информацията за доктора, който отговаря на съответния user
                                // използваме Single(), тъй като връзката ни е едно към едно
                                // и не би трябвало да има друг доктор с Id на намерения user
                                // всеки един доктор отговаря на точно един user - не по малко/ не повече

                                var doctor = m_db.Doctors.Single(d => d.UserId == _user.Id);
                                _foundDoctorsList.Add(doctor);
                            }
                            // ако ролята не му е доктор, не бива да го добавяме към списъка с намерени доктори
                        }
                        break;
                    }
                case "Първо име":
                    {
                        // ползваме .Where за да намерим колекция от доктори, които притежават това име
                        var _doctors = m_db.Doctors.Where(d => d.FirstName == _searchTerm).ToList();
                        if (_doctors != null && _doctors.Count != 0)
                        {
                            _foundDoctorsList.AddRange(_doctors);
                        }
                        break;
                    }
                case "Презиме":
                    {
                        var _doctors = m_db.Doctors.Where(d => d.MiddleName == _searchTerm).ToList();
                        if (_doctors != null && _doctors.Count != 0)
                        {
                            _foundDoctorsList.AddRange(_doctors);
                        }
                        break;
                    }
                case "Фамилия":
                    {
                        var _doctors = m_db.Doctors.Where(d => d.LastName == _searchTerm).ToList();
                        if (_doctors != null && _doctors.Count != 0)
                        {
                            _foundDoctorsList.AddRange(_doctors);
                        }
                        break;
                    }
                case "Специалност":
                    {
                        // отново ползваме SingleOrDefault
                        // защото би трябвало да имаме не повече от 1 специалност с такова име в базата
                        // и защото ако не бъде намерена 1 специалност, то да ми върне NULL

                        var _doctorSpeciality = m_db.DoctorSpecialities.SingleOrDefault(x => x.Name == _searchTerm.ToUpper());
                        if (_doctorSpeciality == null)
                        {
                            MessageBox.Show("Такава докторска специалност не съществува.", "Грешка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var _doctors = m_db.Doctors.Include(d => d.DoctorSpeciality).Where(d => d.DoctorSpeciality.Name == _searchTerm.ToUpper()).ToList();
                        if (_doctors != null && _doctors.Count != 0)
                        {
                            _foundDoctorsList.AddRange(_doctors);
                        }
                        break;
                    }
                default:
                    // тука даже не би трябвало да стигне програмата тъй като имаме
                    // само тези 5 неща в searchCriteriaListBox заредени
                    return;

            }

            if (_foundDoctorsList.Count() == 0)
            {
                foundDoctorsListBox.Items.Clear();
                MessageBox.Show("Няма намерени доктори по този критерий.", "Не бяха намерени доктори.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                foundDoctorsListBox.Items.Clear();

                foreach (var _doctor in _foundDoctorsList)
                {
                    // зареди ми имейла на User профила на намерения доктор
                    // ползваме Single() тъй като знаем че щом имаме намерени доктори
                    // всеки един от тях принадлежи само на един User профил
                    // нямаме един User профил, който да може да се използва от много доктори
                    // тъй като отново -> имаме връзка едно към едно
                    // един User съответства на един Doctor
                    // и е невъзможно един и същи User да отговаря на повече от един доктор
                    // заради дизайна на базата (foreign key-a към User-ите е UNIQUE, не може да се повтаря)
                    var _user = m_db.Users.Single(x => x.Id == _doctor.UserId);
                    foundDoctorsListBox.Items.Add(_user.Email);
                }
                MessageBox.Show("Бяха намерени " + _foundDoctorsList.Count() + " доктора.", "Има намерени доктори.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        private bool CheckIfDoctorSelected()
        {
            if (foundDoctorsListBox.SelectedIndex == -1)
            {
                return false;
            }
            return true;
        }
        private User GetSelectedUser()
        {
            var _selectedEmail = foundDoctorsListBox.SelectedItem.ToString();
            return m_db.Users.Single(u => u.Email == _selectedEmail);
        }
        private void showSelectedDoctorButton_Click(object sender, EventArgs e)
        {
            if (!CheckIfDoctorSelected())
            {
                MessageBox.Show("Трябва да изберете доктор първо.", "Не сте избрали доктор.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var _selectedUser = GetSelectedUser();
            m_openChildForm(new ShowDoctorForm(m_db, _selectedUser));
        }
        private void editSelectedDoctorButton_Click(object sender, EventArgs e)
        {
            if (!CheckIfDoctorSelected())
            {
                MessageBox.Show("Трябва да изберете доктор първо.", "Не сте избрали доктор.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var _selectedUser = GetSelectedUser();
            m_openChildForm(new EditDoctorForm(m_db, _selectedUser));
        }

        private async void deleteSelectedDoctorButton_Click(object sender, EventArgs e)
        {
            if (!CheckIfDoctorSelected())
            {
                MessageBox.Show("Трябва да изберете доктор първо.", "Не сте избрали доктор.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var _selectedUser = GetSelectedUser();
            var _result = MessageBox.Show("Вмомента сте на път да изтриете доктора " + _selectedUser.Email + ". Това ще изтрие и всичките негови рецепти. Искате ли да изтриете този доктор?", "Сигурни ли сте че искате да изтриете този доктор?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (_result == DialogResult.Yes)
            {
                var userToDelete = m_db.Users.Single(u => u.Id == _selectedUser.Id);
                m_db.Users.Remove(userToDelete);
                await m_db.SaveChangesAsync();
                foundDoctorsListBox.Items.Remove(_selectedUser.Email);
            }
        }
    }
}
