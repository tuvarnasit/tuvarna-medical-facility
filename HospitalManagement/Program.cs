using HospitalManagement.Forms;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement
{
    internal static class Program
    {
        // това е първият метод, който се изпълнява при стартиране на приложението
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            SeedDatabase();
            
            Application.Run(new LoginForm(new ApplicationDbContext()));
        }

        private async static void SeedDatabase()
        {
            // създаваме нова инстанция на нашия custom db context клас
            // при създаването на инстанция се изпълнява метода OnConfiguring(),
            // който сме настроили
            using (var _dataBase = new ApplicationDbContext())
            {
                // чрез .Migrate() казваме на Ef Core, ако не съществува такава база от данни
                // да бъде създадена нова, като бъдат изпълнени всички миграции на приложението в папката Migrations
                await _dataBase.Database.MigrateAsync();

                if (_dataBase.Roles.Count() < 2) // ако имаме по-малко от две роли, значи таблицата ни е празна
                {
                    // създаване на ролите в приложението
                    var _adminRole = new Role();
                    _adminRole.Name = "admin";

                    var doctorRole = new Role();
                    doctorRole.Name = "doctor";

                    var _savedAdminRole = await _dataBase.Roles.AddAsync(_adminRole);
                    var _savedDoctorRole = await _dataBase.Roles.AddAsync(doctorRole);
                    await _dataBase.SaveChangesAsync();

                    // създаване на специалностите на докторите ВИНАГИ С ГЛАВНИ БУКВИ
                    var _doctorSpecialityEarDoctor = new DoctorSpeciality();
                    _doctorSpecialityEarDoctor.Name = "УШЕН ЛЕКАР";

                    var _doctorSpecialityEyeDoctor = new DoctorSpeciality();
                    _doctorSpecialityEyeDoctor.Name = "ОЧЕН ЛЕКАР";

                    var _doctorSpecialitySurgeon = new DoctorSpeciality();
                    _doctorSpecialitySurgeon.Name = "ХИРУРГ";

                    var _doctorSpecialitySportsDoctor = new DoctorSpeciality();
                    _doctorSpecialitySportsDoctor.Name = "СПОРТНА МЕДИЦИНА";

                    await _dataBase.DoctorSpecialities.AddAsync(_doctorSpecialityEarDoctor);
                    await _dataBase.DoctorSpecialities.AddAsync(_doctorSpecialityEyeDoctor);
                    await _dataBase.DoctorSpecialities.AddAsync(_doctorSpecialitySurgeon);
                    var _doctorSpeciality = await _dataBase.DoctorSpecialities.AddAsync(_doctorSpecialitySportsDoctor); // запаметяваме тази специалност в променлива за да я използваме при създаването на доктор
                    
                    await _dataBase.SaveChangesAsync();

                    // създаване по един акаунт от всяка роля (Admin и Doctor)
                    var _adminUser = new User();
                    _adminUser.RoleId = _savedAdminRole.Entity.Id;
                    _adminUser.Email = "admin@gmail.com";
                    _adminUser.Password = "admin123";

                    await _dataBase.Users.AddAsync(_adminUser);

                    var _doctorUser = new User();
                    _doctorUser.RoleId = _savedDoctorRole.Entity.Id;
                    _doctorUser.Email = "doctor@gmail.com";
                    _doctorUser.Password = "doctor123";

                    await _dataBase.SaveChangesAsync();

                    // запаметяваме в променлива doctor user-a за да може
                    // като създадем доктор, да кажем данните на доктора на кой
                    // user account съответства
                    var _addedDoctorUser = await _dataBase.Users.AddAsync(_doctorUser);

                    await _dataBase.SaveChangesAsync();

                    // създаване на доктор
                    var _doctor = new Doctor()
                    {
                        FirstName = "Мартин",
                        MiddleName = "Омаров",
                        LastName = "Хабоян",
                        UserId = _addedDoctorUser.Entity.Id, // id-то на user-a който ще съответства на тази информация
                        DoctorSpecialityId = _doctorSpeciality.Entity.Id, // id-то на специалността на този доктор
                    };

                    await _dataBase.Doctors.AddAsync(_doctor);

                    await _dataBase.SaveChangesAsync();

                    // създаване на заболяване
                    var _medicalCondition = new MedicalCondition()
                    {
                        Name = "ДЕМЕНЦИЯ"
                    };

                    // запаметяване на добавеното заболяване в променлива
                    var _addedMedicalCondition = await _dataBase.MedicalConditions.AddAsync(_medicalCondition);
                    await _dataBase.SaveChangesAsync();

                    var _patient = new Patient()
                    {
                        FirstName = "Явор",
                        MiddleName = "Йорданов",
                        LastName = "Чамов",
                        EGN = "4369283746",
                        MedicalConditionId = _addedMedicalCondition.Entity.Id // id-то на заболяването на този пациент
                    };

                    await _dataBase.Patients.AddAsync(_patient);

                    await _dataBase.SaveChangesAsync();
                }
            }
        }
    }
}