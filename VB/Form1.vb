Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.Data.Filtering

Namespace Q303649
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			Dim tbl As New DataTable()
			tbl.Columns.Add("ID", GetType(Integer))
			tbl.Columns.Add("Item", GetType(String))
			tbl.Columns.Add("Name", GetType(String))
			tbl.Columns.Add("Payment", GetType(Decimal))
			For i As Integer = 1 To 199
				tbl.Rows.Add(i, String.Format("Item{0}", i Mod 30), String.Format("Name{0}", i), i * 3.3)
			Next i
			gridControl1.DataSource = tbl
			gridView1.ActiveFilterString = "[Item] = 'Item5'"
		End Sub

		Private Sub gridView1_ShowFilterPopupListBox(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.FilterPopupListBoxEventArgs) Handles gridView1.ShowFilterPopupListBox
			e.ComboBox.Tag = e.Column
			AddHandler e.ComboBox.Popup, AddressOf ComboBox_Popup
		End Sub

		Private Sub ComboBox_Popup(ByVal sender As Object, ByVal e As EventArgs)
			Dim popup As ColumnFilterPopup.FilterComboBox = TryCast(sender, ColumnFilterPopup.FilterComboBox)
			Dim value As Object = GetFilterValueByColumn(TryCast(popup.Properties.Tag, GridColumn))
			If value Is Nothing Then
				Return
			End If
			For i As Integer = 0 To popup.Properties.Items.Count - 1
				Dim fi As FilterItem = TryCast(popup.Properties.Items(i), FilterItem)
				If fi IsNot Nothing AndAlso value.Equals(fi.Value) Then
					popup.SelectedIndex = i
					Return
				End If
			Next i
		End Sub
		Private Function GetFilterValueByColumn(ByVal column As GridColumn) As Object
			Dim bo As BinaryOperator = TryCast(column.FilterInfo.FilterCriteria, BinaryOperator)
			If ReferenceEquals(bo, Nothing) Then
				Return Nothing
			End If
			Dim value As OperandValue = TryCast(bo.RightOperand, OperandValue)
			If ReferenceEquals(value, Nothing) Then
				Return Nothing
			End If
			Return value.Value
		End Function
	End Class
End Namespace