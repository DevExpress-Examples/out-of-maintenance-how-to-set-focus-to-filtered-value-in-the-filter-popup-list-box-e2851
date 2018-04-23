using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Data.Filtering;

namespace Q303649 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("ID", typeof(int));
            tbl.Columns.Add("Item", typeof(string));
            tbl.Columns.Add("Name", typeof(string));
            tbl.Columns.Add("Payment", typeof(decimal));
            for(int i = 1; i < 200; i++)
                tbl.Rows.Add(i, string.Format("Item{0}", i % 30), string.Format("Name{0}", i), i * 3.3);
            gridControl1.DataSource = tbl;
            gridView1.ActiveFilterString = "[Item] = 'Item5'";
        }

        private void gridView1_ShowFilterPopupListBox(object sender, DevExpress.XtraGrid.Views.Grid.FilterPopupListBoxEventArgs e) {
            e.ComboBox.Tag = e.Column;
            e.ComboBox.Popup += new EventHandler(ComboBox_Popup);
        }

        void ComboBox_Popup(object sender, EventArgs e) {
            ColumnFilterPopup.FilterComboBox popup = sender as ColumnFilterPopup.FilterComboBox;
            object value = GetFilterValueByColumn(popup.Properties.Tag as GridColumn);
            if(value == null) return;
            for(int i = 0; i < popup.Properties.Items.Count; i++) {
                FilterItem fi = popup.Properties.Items[i] as FilterItem;
                if(fi != null && value.Equals(fi.Value)) {
                    popup.SelectedIndex = i;
                    return;
                }
            }
        }
        object GetFilterValueByColumn(GridColumn column) {
            BinaryOperator bo = column.FilterInfo.FilterCriteria as BinaryOperator;
            if(ReferenceEquals(bo, null)) return null;
            OperandValue value = bo.RightOperand as OperandValue;
            if(ReferenceEquals(value, null)) return null;
            return value.Value;
        }
    }
}