using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.ComponentModel.DataAnnotations;
using System.IO;
using DevExpress.XtraLayout.Helpers;
using DevExpress.XtraLayout;
using RedSea24.Views;
using RedSea24.Properties;
using RedSea24.Classes;

namespace RedSea24.Views
{
    public partial class EmployeeView : DevExpress.XtraEditors.XtraUserControl
    {
        MainForm parent;
        EmployeeCollectionView employeeCollectionView;
        long employeeId;
        public EmployeeView(MainForm parent, long employeeId, EmployeeCollectionView employeeCollectionView)
        {
            InitializeComponent();
            this.parent = parent;
            this.employeeCollectionView = employeeCollectionView;
            this.employeeId = employeeId;

            // fill data
            FillPositions();
            FillTeams();
            FillDirectReport();
            FillPcs();
            FillLanguages();
            FillSkills();
            FillSystems();
            FillAuthority();
            RecallEmployee();
        }

        private void RecallEmployee()
        {
            
            if (this.employeeId == 0)
                return;

            using (var context = new RedSeaEntities())
            {
                var fte = context.tblFTEs
                    .Where(b => b.Id == this.employeeId)
                    .FirstOrDefault();

                fullNameLabelControl.Text = fte.FirstName + " " + fte.LastName;
                firstNameTextEdit.EditValue = fte.FirstName;
                lastNameTextEdit.EditValue = fte.LastName;
                usernameTextEdit.EditValue = fte.Username;
                positionLookUpEdit.EditValue = fte.Designation;
                teamLookUpEdit.EditValue = fte.Team;
                hireDateDateEdit.EditValue = fte.HireDate;
                statusComboBoxEdit.SelectedIndex = (fte.Active) ? 0 : 1;
                emailTextEdit.EditValue = fte.Email;
                codeTextEdit.Text = fte.Code;
                fingerprintTextEdit.EditValue = fte.FingerPrint;
                directReportLookUpEdit.EditValue = fte.DirectReport;
                phoneTextEdit.EditValue = fte.Phone;
                ciscoTextEdit.EditValue = fte.Cisco;
                pcLookUpEdit.EditValue = fte.PC;
                authorityLookUpEdit.EditValue = fte.Authority;
                notesMemoEdit.EditValue = fte.Notes;
                Image employeeImage = RedSea24.Classes.Logic.ByteArrayToImage(fte.Photo);
                if (employeeImage != null)
                {
                    photoPictureEdit.Image = employeeImage;
                }
                else
                {
                    photoPictureEdit.Image = Resources.images__1_;
                }
                CheckLanguages();
                CheckSkills();
                CheckPrograms();
            }
        }
        
        private void windowsUIButtonPanelCloseButton_Click(object sender, EventArgs e)
        {

            this.parent.navigationFrame.SelectedPageIndex = 0;
        }

        private void FillPositions()
        {
            using (var context = new RedSeaEntities())
            {
                var positions = context.tblDesignations.ToList();
                positionLookUpEdit.Properties.DataSource = positions;
            }
        }

        
        private void FillTeams()
        {
            using (var context = new RedSeaEntities())
            {
                var teams = context.tblTeams.ToList();
                teamLookUpEdit.Properties.DataSource = teams;
            }
        }
        private void FillDirectReport()
        {
            using (var context = new RedSeaEntities())
            {
                var query =
                    from directReports in context.tblFTEs
                    select new
                    {
                        id = directReports.Id,
                        directReprt = directReports.FirstName + " " + directReports.LastName
                    };
                DataTable dt = new DataTable();
                dt.Columns.Add("Id");
                dt.Columns.Add("DirectReport");
                foreach (var record in query)
                {
                    dt.Rows.Add(record.id, record.directReprt);
                }
                directReportLookUpEdit.Properties.DataSource = dt;
            }
        }

        private void FillPcs()
        {
            using (var context = new RedSeaEntities())
            {
                var query =
                    from pcs in context.tblPCs
                    select new
                    {
                        Id = pcs.Id,
                        PCName = pcs.PCName
                    };
                DataTable dt = new DataTable();
                dt.Columns.Add("Id");
                dt.Columns.Add("PCName");

                foreach (var record in query)
                {
                    dt.Rows.Add(record.Id, record.PCName);
                }
                pcLookUpEdit.Properties.DataSource = dt;
            }
        }

        private void FillLanguages()
        {
            using (var context = new RedSeaEntities())
            {
                var languages = context.tblLanguages.ToList();

                foreach (var record in languages)
                {
                    languagesCheckedComboBoxEdit.Properties.Items.Add(record.Id, record.LanguageName);
                }
            }
        }

        private void FillSkills()
        {
            using (var context = new RedSeaEntities())
            {
                var skills = context.tblSkills.ToList();

                foreach (var record in skills)
                {
                    skillsCheckedComboBoxEdit.Properties.Items.Add(record.Id, record.SkillName);
                }
            }
        }
        private void FillSystems()
        {
            using (var context = new RedSeaEntities())
            {
                var systems = context.tblSystems.ToList();

                foreach (var record in systems)
                {
                    programsCheckedComboBoxEdit.Properties.Items.Add(record.Id, record.SystemName);
                }
            }
        }

        private void FillAuthority()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Authority");

            dt.Rows.Add("user", "USER");
            dt.Rows.Add("admin", "ADMIN");
            dt.Rows.Add("hr", "HR");
            authorityLookUpEdit.Properties.DataSource = dt;

        }

        private void CheckLanguages()
        {
            using (var context = new RedSeaEntities())
            {
                var query = context.tblLinkLanguages
                    .Where(b => b.FTE == this.employeeId);

                foreach (var record in query)
                {
                    for (var i = 0; i < languagesCheckedComboBoxEdit.Properties.Items.Count; i++ )
                    {
                        if (languagesCheckedComboBoxEdit.Properties.Items[i].Value.ToString() == record.cLanguage.ToString())
                            languagesCheckedComboBoxEdit.Properties.Items[i].CheckState = CheckState.Checked;
                    }
                }

            }
        }

        private void CheckSkills()
        {
            using (var context = new RedSeaEntities())
            {
                var query = context.tblLinkSkills
                    .Where(b => b.FTE == this.employeeId);

                foreach (var record in query)
                {
                    for (var i = 0; i < skillsCheckedComboBoxEdit.Properties.Items.Count; i++)
                    {
                        if (skillsCheckedComboBoxEdit.Properties.Items[i].Value.ToString() == record.Skill.ToString())
                            skillsCheckedComboBoxEdit.Properties.Items[i].CheckState = CheckState.Checked;
                    }
                }

            }
        }

        private void CheckPrograms()
        {
            using (var context = new RedSeaEntities())
            {
                var query = context.tblLinkSystems
                    .Where(b => b.FTE == this.employeeId);

                foreach (var record in query)
                {
                    for (var i = 0; i < programsCheckedComboBoxEdit.Properties.Items.Count; i++)
                    {
                        if (programsCheckedComboBoxEdit.Properties.Items[i].Value.ToString() == record.cSystem.ToString())
                            programsCheckedComboBoxEdit.Properties.Items[i].CheckState = CheckState.Checked;
                    }
                }

            }
        }

        private void windowsUIButtonPanel_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Caption == "Save")
            {
                if (this.employeeId == 0)
                {
                    SaveNew();
                }
                else
                {
                    EditCurrent();
                }
            }
            
        }

        private void SaveNew()
        {
            // check validation first
            if (!dxValidationProvider1.Validate())
                return;

            using (var context = new RedSeaEntities())
            {
                tblFTE currentFte = new tblFTE();

                currentFte.FirstName = Classes.Logic.CleanString(firstNameTextEdit.Text, 1);
                currentFte.LastName = Classes.Logic.CleanString(lastNameTextEdit.Text, 1);
                currentFte.Username = usernameTextEdit.Text;
                currentFte.Designation = Convert.ToInt32(positionLookUpEdit.EditValue);
                currentFte.Team = Convert.ToInt32(teamLookUpEdit.EditValue);
                currentFte.HireDate = Convert.ToDateTime(hireDateDateEdit.EditValue);
                currentFte.Active = (statusComboBoxEdit.SelectedIndex == 0) ? true : false;
                currentFte.Email = emailTextEdit.Text;
                currentFte.Code = codeTextEdit.Text;
                currentFte.FingerPrint = fingerprintTextEdit.Text;
                currentFte.DirectReport = Convert.ToInt64(directReportLookUpEdit.EditValue);
                currentFte.Phone = phoneTextEdit.Text;
                currentFte.Cisco = ciscoTextEdit.Text;
                currentFte.PC = Convert.ToInt32(pcLookUpEdit.EditValue);
                currentFte.Authority = Convert.ToString(authorityLookUpEdit.EditValue);
                currentFte.Notes = notesMemoEdit.Text;
                currentFte.PassKey = "12345";
                if (photoPictureEdit.Image != Resources.images__1_)
                    currentFte.Photo = RedSea24.Classes.Logic.ImageToByteArray(photoPictureEdit.Image);

                context.tblFTEs.Add(currentFte);
                context.SaveChanges();
                this.employeeId = currentFte.Id;
                SaveLanguages();
                SaveSkills();
                SavePrograms();
                this.employeeCollectionView.GetDataSource();
                this.parent.ShowMessage("Saved", "New employee has been added successfully!");
            }
        }

        private void EditCurrent()
        {
            // check validation first
            if (!dxValidationProvider1.Validate())
                return;

            using (var context = new RedSeaEntities())
            {
                var currentFte = context.tblFTEs
                    .Where(b => b.Id == this.employeeId).FirstOrDefault();

                currentFte.FirstName = Classes.Logic.CleanString(firstNameTextEdit.Text, 1);
                currentFte.LastName = Classes.Logic.CleanString(lastNameTextEdit.Text, 1);
                currentFte.Username = usernameTextEdit.Text;
                currentFte.Designation = Convert.ToInt32(positionLookUpEdit.EditValue);
                currentFte.Team = Convert.ToInt32(teamLookUpEdit.EditValue);
                currentFte.HireDate = Convert.ToDateTime(hireDateDateEdit.EditValue);
                currentFte.Active = (statusComboBoxEdit.SelectedIndex == 0) ? true : false;
                currentFte.Email = emailTextEdit.Text;
                currentFte.Code = codeTextEdit.Text;
                currentFte.FingerPrint = fingerprintTextEdit.Text;
                currentFte.DirectReport = Convert.ToInt64(directReportLookUpEdit.EditValue);
                currentFte.Phone = phoneTextEdit.Text;
                currentFte.Cisco = ciscoTextEdit.Text;
                currentFte.PC = Convert.ToInt32(pcLookUpEdit.EditValue);
                currentFte.Authority = Convert.ToString(authorityLookUpEdit.EditValue);
                currentFte.Notes = notesMemoEdit.Text;
                if (photoPictureEdit.Image != Resources.images__1_)
                    currentFte.Photo = RedSea24.Classes.Logic.ImageToByteArray(photoPictureEdit.Image);
                
                context.SaveChanges();
                SaveLanguages();
                SaveSkills();
                SavePrograms();
                this.employeeCollectionView.GetDataSource();
                this.parent.ShowMessage("Saved", "The employee has been updated successfully!");
            }

            
        }
        
        private void SaveLanguages()
        {
            using (var context = new RedSeaEntities())
            {
                // delete old rows
                var oldLanguages = context.tblLinkLanguages
                    .Where(b => b.FTE == this.employeeId);

                context.tblLinkLanguages.RemoveRange(oldLanguages);

                // add new rows
                for (var i = 0; i < languagesCheckedComboBoxEdit.Properties.Items.Count; i++)
                {
                    if (languagesCheckedComboBoxEdit.Properties.Items[i].CheckState == CheckState.Checked)
                    {
                        tblLinkLanguage newLanguage = new tblLinkLanguage();
                        newLanguage.FTE = this.employeeId;
                        newLanguage.cLanguage = Convert.ToInt32(languagesCheckedComboBoxEdit.Properties.Items[i].Value);
                        context.tblLinkLanguages.Add(newLanguage);                        
                    }
                }
                context.SaveChanges();
            }
        }

        private void SaveSkills()
        {
            using (var context = new RedSeaEntities())
            {
                // delete old rows
                var oldSkills = context.tblLinkSkills
                    .Where(b => b.FTE == this.employeeId);

                context.tblLinkSkills.RemoveRange(oldSkills);

                // add new rows
                for (var i = 0; i < skillsCheckedComboBoxEdit.Properties.Items.Count; i++)
                {
                    if (skillsCheckedComboBoxEdit.Properties.Items[i].CheckState == CheckState.Checked)
                    {
                        tblLinkSkill newSkill = new tblLinkSkill();
                        newSkill.FTE = this.employeeId;
                        newSkill.Skill = Convert.ToInt32(skillsCheckedComboBoxEdit.Properties.Items[i].Value);
                        context.tblLinkSkills.Add(newSkill);
                    }
                }
                context.SaveChanges();
            }
        }

        private void SavePrograms()
        {
            using (var context = new RedSeaEntities())
            {
                // delete old rows
                var oldPrograms = context.tblLinkSystems
                    .Where(b => b.FTE == this.employeeId);

                context.tblLinkSystems.RemoveRange(oldPrograms);

                // add new rows
                for (var i = 0; i < programsCheckedComboBoxEdit.Properties.Items.Count; i++)
                {
                    if (programsCheckedComboBoxEdit.Properties.Items[i].CheckState == CheckState.Checked)
                    {
                        tblLinkSystem newProgram = new tblLinkSystem();
                        newProgram.FTE = this.employeeId;
                        newProgram.cSystem = Convert.ToInt32(programsCheckedComboBoxEdit.Properties.Items[i].Value);
                        context.tblLinkSystems.Add(newProgram);
                    }
                }
                context.SaveChanges();
            }
        }
    }


}
