using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace RedSea24.Classes
{
    public class Designation
    {
        public int id;
        public string designation;

        public Designation(int id, string designation)
        {
            this.id = id;
            this.designation = Logic.CleanString(designation, 3);
        }

        public int Add()
        {
            using (var context = new RedSeaEntities())
            {
                var designation = new tblDesignation()
                {
                    Designation = this.designation
                };
                context.tblDesignations.Add(designation);
                context.SaveChanges();
                this.id = designation.Id;
            }
            return this.id;
        }

        public bool Update()
        {
            bool result = false;
            using (var context = new RedSeaEntities())
            {
                var designation = context.tblDesignations.SingleOrDefault(i => i.Id == this.id);
                if (designation != null)
                {
                    designation.Designation = this.designation;
                    context.SaveChanges();
                    result = true;
                }
            }
            return result;
        }

        public bool Delete()
        {
            bool result = false;
            using (var context = new RedSeaEntities())
            {
                var designation = context.tblDesignations.SingleOrDefault(i => i.Id == this.id);
                if (designation != null)
                {
                    context.tblDesignations.Remove(designation);
                    context.SaveChanges();
                    result = true;
                }
                
            }
            return result;
        }
    }

    public class Language
    {
        public int id;
        public string language;

        public Language(int id, string language)
        {
            this.id = id;
            this.language = Logic.CleanString(language, 1);
        }
    }

    public class Skill
    {
        public int id;
        public string skill;

        public Skill(int id, string skill)
        {
            this.id = id;
            this.skill = Logic.CleanString(skill, 3);
        }
    }

    public class Pc
    {
        public int id;
        public string pcName;
        public string screen1;
        public string screen2;
        public string notes;

        public Pc(int id, string pcName, string screen1, string screen2, string notes)
        {
            this.id = id;
            this.pcName = Logic.CleanString(pcName, 0);
            this.screen1 = Logic.CleanString(screen1, 0);
            this.screen2 = Logic.CleanString(screen2, 0);
            this.notes = notes;
        }
    }

    public class Program
    {
        public int id;
        public string systemName;

        public Program(int id, string systemName)
        {
            this.id = id;
            this.systemName = Logic.CleanString(systemName, 3);
        }
    }

    public class TeamGroup
    {
        public int id;
        public string teamGroup;
        public Image logo;

        public TeamGroup(int id, string teamGroup, Image logo)
        {
            this.id = id;
            this.teamGroup = Logic.CleanString(teamGroup, 3);
            this.logo = logo;
        }
    }

    public class Team
    {
        public int id;
        public int teamGroup;
        public string team;
        public Image logo;

        public Team(int id, int teamGroup, string team, Image logo)
        {
            this.id = id;
            this.teamGroup = teamGroup;
            this.team = Logic.CleanString(team, 3);
            this.logo = logo;
        }
    }

    public class Empoyee
    {
        public long id;
        public int team;
        public int designation;
        public string username;
        public string firstName;
        public string lastName;
        public string cisco;
        public int pc;
        public string code;
        public string fingerprint;
        public string phone;
        public Image photo;
        public string email;
        public string passKey;
        public string authority;
        public bool active;
        public string notes;

        public Empoyee(long id, int team, int designation, string username, string firstName, string lastName,
            string cisco, int pc, string code, string fingerprint, string phone, Image photo, string email, string passKey,
            string authority, bool active, string notes)
        {
            this.id = id;
            this.team = team;
            this.designation = designation;
            this.username = Logic.CleanString(username, 0);
            this.firstName = Logic.CleanString(firstName, 1);
            this.lastName = Logic.CleanString(lastName, 1);
            this.cisco = Logic.CleanString(cisco, 0);
            this.pc = pc;
            this.code = code;
            this.fingerprint = fingerprint;
            this.phone = phone;
            this.photo = photo;
            this.email = email;
            this.passKey = passKey;
            this.authority = authority;
            this.active = active;
            this.notes = notes;
        }

        
    }
}
