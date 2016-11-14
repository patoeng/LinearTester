Imports System.Windows.Forms

Public Class DialogNewOrder

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        If TextBoxOrder.Text.Length <> 23 Then
            MessageBox.Show("Barcode incorrect, please try again.")
            Exit Sub
        End If

        ' Validate the ref and call the SQL server
        ' Barcode like XS612B1PAM12   12340001
        FormMain.ReferencesTableAdapter.FillByrefComm(FormMain.MyBdd_badgesDataSet.references, TextBoxOrder.Text.Substring(0, 15).Trim.ToUpper)
        If FormMain.MyBdd_badgesDataSet.references.Rows.Count = 0 Then
            MessageBox.Show("reference not found.")
            Exit Sub
        End If

        FormMain.currentRefRecord = FormMain.MyBdd_badgesDataSet.references.Rows(0)

        ' It's OK for the ref, now find the order
        FormMain.ManufacturingordersTableAdapter.FillByManufacturingOrder(FormMain.MyDataacquisitionDataSet.manufacturingorders, TextBoxOrder.Text.Substring(15, 8))

        If FormMain.MyDataacquisitionDataSet.manufacturingorders.Rows.Count = 0 Then
            MessageBox.Show("Order not found in the XS156 information system.")
            Exit Sub
        End If

        FormMain.currentManufacturingOrder = FormMain.MyDataacquisitionDataSet.manufacturingorders.Rows(0)
        TextBoxOrder.Text = ""

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click

        FormMain.referenceLaunched = ""
        FormMain.qtyLaunched = 0
        FormMain.qtySent = 0
        FormMain.currentRefRecord = Nothing
        FormMain.currentManufacturingOrder = Nothing

        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub DialogNewOrder_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        TextBoxOrder.Select()
        TextBoxOrder.Text = ""

    End Sub
End Class
