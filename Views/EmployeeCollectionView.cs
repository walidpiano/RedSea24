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
using RedSea24.Views;
using RedSea24.Classes;

namespace RedSea24
{
    public partial class EmployeeCollectionView : DevExpress.XtraEditors.XtraUserControl
    {
        MainForm parent;
        public EmployeeCollectionView(MainForm parent)
        {
            InitializeComponent();
            this.parent = parent;
            GetDataSource();
        }
        void windowsUIButtonPanel_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Caption == "Print") gridControl1.ShowRibbonPrintPreview();
            if (e.Button.Properties.Caption == "New") ShowEmployeeDetails(0);
            if (e.Button.Properties.Caption == "Edit")
            {
                long currentId = Convert.ToInt64(tileView1.GetFocusedRowCellValue("Id"));
                ShowEmployeeDetails(currentId);
            }
            if (e.Button.Properties.Caption == "Ascending") SortEmployees(true);
            if (e.Button.Properties.Caption == "Descending") SortEmployees(false);
            if (e.Button.Properties.Caption == "Refresh") ResetSorting();
            if (e.Button.Properties.Caption == "Delete")
            {
                if (!parent.DialogMessage("Delete", "Are you sure you want to delete this employee?"))
                    return;
                DeleteCurrentEmployee();
            }
        }

        public void GetDataSource()
        {


            using (var context = new RedSeaEntities())
            {
                var empoyeeList = context.vAllEmpoyees.ToList();
                gridControl1.DataSource = empoyeeList;
            }
            tileGroup1.Items.Clear();
            CreateAllGroup();
            CreateGroupFilters();
            CreateInactiveGroup();
            tileItem_Filter("All Teams");
        }

        private void CreateAllGroup()
        {
            using (var context = new RedSeaEntities())
            {
                var query = context.tblFTEs
                    .Where(b => b.Active == true).Count();
                createTileItem(0, "All Teams", query, global::RedSea24.Properties.Resources.all_active);

            }
        }

        private void CreateInactiveGroup()
        {
            using (var context = new RedSeaEntities())
            {
                var query = context.tblFTEs
                    .Where(b => b.Active == false).Count();
                createTileItem(100, "Inactive Users", query, global::RedSea24.Properties.Resources.inactive);

            }
        }

        private void CreateGroupFilters()
        {
            using (var context = new RedSeaEntities())
            {
                var query = (from team in context.tblTeams
                             .OrderBy(b=> b.Team)
                             let cCount =
                             (
                             from fte in context.tblFTEs
                             where fte.Team == team.Id
                             select fte).Count()
                             select new
                             {
                                 id = team.Id,
                                 team = team.Team,
                                 logo = team.Logo,
                                 number = cCount
                             }).ToList();

                for (var i = 0; i < query.Count; i++)
                {
                    createTileItem(query[i].id, query[i].team, query[i].number, 
                        RedSea24.Classes.Logic.ByteArrayToImage(query[i].logo));
                }

                
            }
        }

        

        // to sort tile view
        void SortEmployee(bool ascending)
        {
            var lastName = tileView1.Columns["FullName"];
            if (lastName == null)
                return;
            tileView1.SortInfo.Clear();
            tileView1.SortInfo.Add(new DevExpress.XtraGrid.Columns.GridColumnSortInfo(lastName, ascending ? DevExpress.Data.ColumnSortOrder.Ascending : DevExpress.Data.ColumnSortOrder.Descending));
            
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            long currentId = Convert.ToInt64(tileView1.GetFocusedRowCellValue("Id"));
            ShowEmployeeDetails(currentId);
        }

        private void ShowEmployeeDetails(long employeeId)
        {
            XtraUserControl ucEmployeeView = new EmployeeView(this.parent, employeeId, this);
            ucEmployeeView.Dock = DockStyle.Fill;
            this.parent.navigationFrame.Pages[4].Controls.Clear();
            this.parent.navigationFrame.Pages[4].Controls.Add(ucEmployeeView);

            this.parent.navigationFrame.SelectedPageIndex = 4;
        }

        private void SortEmployees(bool ascending)
        {
            var firstName = tileView1.Columns["FirstName"];
            if (firstName == null)
                return;
            tileView1.SortInfo.Clear();
            tileView1.SortInfo.Add(new DevExpress.XtraGrid.Columns.GridColumnSortInfo(firstName, ascending ? DevExpress.Data.ColumnSortOrder.Ascending : DevExpress.Data.ColumnSortOrder.Descending));
        }

        private void ResetSorting()
        {
            tileView1.SortInfo.Clear();
        }

        private void DeleteCurrentEmployee()
        {
            long currentId = Convert.ToInt64(tileView1.GetFocusedRowCellValue("Id"));

            using (var context = new RedSeaEntities())
            {

                var oldLanguages = context.tblLinkLanguages
                    .Where(b => b.FTE == currentId);

                context.tblLinkLanguages.RemoveRange(oldLanguages);

                var employeeLanguages = context.tblLinkLanguages
                    .Where(b => b.FTE == currentId);
                context.tblLinkLanguages.RemoveRange(employeeLanguages);

                var employeeSkills = context.tblLinkSkills
                    .Where(b => b.FTE == currentId);
                context.tblLinkSkills.RemoveRange(employeeSkills);

                var employeePrograms = context.tblLinkSystems
                    .Where(b => b.FTE == currentId);
                context.tblLinkSystems.RemoveRange(employeePrograms);

                var employeeToDelete = context.tblFTEs
                    .Where(b => b.Id == currentId).FirstOrDefault();
                context.tblFTEs.Remove(employeeToDelete);
                
                context.SaveChanges();
                GetDataSource();
            }
        }


        // test for runtime creating tile items
        private void tileItem1_ItemClick(object sender, TileItemEventArgs e)
        {
            tileItem_Filter(e.Item.Elements[2].Text);
        }

        private void createTileItem(int id, string groupName, int number, Image logo)
        {
            TileItem item = new TileItem();
            item.AppearanceItem.Normal.BackColor = System.Drawing.Color.White;
            item.AppearanceItem.Normal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            item.AppearanceItem.Normal.Options.UseBackColor = true;
            item.AppearanceItem.Normal.Options.UseBorderColor = true;

            TileItemElement tileItemElement1 = new TileItemElement();
            tileItemElement1.AnchorAlignment = DevExpress.Utils.AnchorAlignment.Top;
            tileItemElement1.Appearance.Normal.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tileItemElement1.Appearance.Normal.ForeColor = System.Drawing.Color.Gray;
            tileItemElement1.Appearance.Normal.Options.UseFont = true;
            tileItemElement1.Appearance.Normal.Options.UseForeColor = true;
            tileItemElement1.Image = (logo == null) ? global::RedSea24.Properties.Resources.empty_team : logo;
            tileItemElement1.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleLeft;
            tileItemElement1.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Stretch;
            tileItemElement1.ImageSize = new System.Drawing.Size(100, 60);
            tileItemElement1.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Top;
            tileItemElement1.Text = " ";

            TileItemElement tileItemElement2 = new TileItemElement();
            tileItemElement2.AnchorAlignment = DevExpress.Utils.AnchorAlignment.Top;
            tileItemElement2.Appearance.Normal.Font = new System.Drawing.Font("Segoe UI Semibold", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tileItemElement2.Appearance.Normal.ForeColor = System.Drawing.Color.Gray;
            tileItemElement2.Appearance.Normal.Options.UseFont = true;
            tileItemElement2.Appearance.Normal.Options.UseForeColor = true;
            tileItemElement2.ColumnIndex = 1;
            tileItemElement2.Text = number.ToString();

            TileItemElement tileItemElement3 = new TileItemElement();
            tileItemElement3.Appearance.Normal.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tileItemElement3.Appearance.Normal.ForeColor = System.Drawing.Color.Gray;
            tileItemElement3.Appearance.Normal.Options.UseFont = true;
            tileItemElement3.Appearance.Normal.Options.UseForeColor = true;
            tileItemElement3.Text = groupName;

            item.Elements.Add(tileItemElement1);
            item.Elements.Add(tileItemElement2);
            item.Elements.Add(tileItemElement3);

            item.Id = id;
            item.ItemSize = DevExpress.XtraEditors.TileItemSize.Wide;
            item.Name = groupName;
            item.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.tileItem1_ItemClick);

            tileGroup1.Items.Add(item);

        }

        private void tileItem_Filter(string filter)
        {
            tileView1.ActiveFilter.Clear();
            searchControl.EditValue = null;
            if (filter == "All Teams")
            {
                tileView1.ActiveFilterString = "([Active] = true)";
            }
            else if (filter == "Inactive Users")
            {
                tileView1.ActiveFilterString = "([Active] = false)";
            }
            else
            {
                tileView1.ActiveFilterString = "([Team] = '" + filter + "' AND [Active] = true)";
            }
            
        }
    }
}
