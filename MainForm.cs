using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraBars.Docking2010.Customization;
using RedSea24.Classes;
using RedSea24.Views;

namespace RedSea24
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        EmployeeCollectionView employeeCollectionView;
        public MainForm()
        {
            InitializeComponent();
        }
        private void tileBar_SelectedItemChanged(object sender, TileItemEventArgs e)
        {
            // Load layers
            int pageIndex = tileBarGroupTables.Items.IndexOf(e.Item);
            switch (pageIndex)
            {
                case 1:

                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    break;
            }
            navigationFrame.SelectedPageIndex = pageIndex;
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
            //XtraUserControl ucEmployee = new EmployeeCollectionView(this);
            this.employeeCollectionView = new EmployeeCollectionView(this);

            employeeCollectionView.Dock = DockStyle.Fill;
            employeesNavigationPage.Controls.Add(employeeCollectionView);
            AddLayer(employeeCollectionView, 0);
 

            //tests
            //Designation desi = new Designation(24, "wll");
            //desi.Delete();
            
            
           
                       
        }

        public void AddLayer(XtraUserControl layerName, short layerPage)
        {
            if (!GV.layers[layerPage])
            {
                layerName.Dock = DockStyle.Fill;
                navigationFrame.Pages[layerPage].Controls.Add(layerName);

                GV.layers[layerPage] = true;
            }

        }

    
        public void ShowMessage(string caption, string messageText)
        {
            FlyoutAction action = new FlyoutAction();
            action.Caption = caption;
            action.Description = messageText;
            action.Commands.Add(FlyoutCommand.OK);
            FlyoutDialog.Show(this, action);
        }

        public bool DialogMessage(string caption, string messageText)
        {
            bool result;
            FlyoutAction action = new FlyoutAction();
            action.Caption = caption;
            action.Description = messageText;
            action.Commands.Add(FlyoutCommand.Yes);
            action.Commands.Add(FlyoutCommand.No);

            result = (FlyoutDialog.Show(this, action) == System.Windows.Forms.DialogResult.Yes);
            
            return result;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyCode == Keys.F & navigationFrame.SelectedPageIndex == 0)
            {
                this.employeeCollectionView.searchControl.Focus();
                this.employeeCollectionView.searchControl.SelectAll();
            }

        }

    }


   
}