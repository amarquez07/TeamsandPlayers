<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FAssignTeamPlayers
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblTeams = New System.Windows.Forms.Label()
        Me.grpPlayers = New System.Windows.Forms.GroupBox()
        Me.btnNone = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnAll = New System.Windows.Forms.Button()
        Me.lblAvailable = New System.Windows.Forms.Label()
        Me.lstAvailablePlayers = New System.Windows.Forms.ListBox()
        Me.lblSelected = New System.Windows.Forms.Label()
        Me.lstSelectedPlayers = New System.Windows.Forms.ListBox()
        Me.cmbTeams = New System.Windows.Forms.ComboBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.grpPlayers.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTeams
        '
        Me.lblTeams.AutoSize = True
        Me.lblTeams.Location = New System.Drawing.Point(23, 20)
        Me.lblTeams.Name = "lblTeams"
        Me.lblTeams.Size = New System.Drawing.Size(42, 13)
        Me.lblTeams.TabIndex = 0
        Me.lblTeams.Text = "Teams:"
        '
        'grpPlayers
        '
        Me.grpPlayers.Controls.Add(Me.btnNone)
        Me.grpPlayers.Controls.Add(Me.btnRemove)
        Me.grpPlayers.Controls.Add(Me.btnAdd)
        Me.grpPlayers.Controls.Add(Me.btnAll)
        Me.grpPlayers.Controls.Add(Me.lblAvailable)
        Me.grpPlayers.Controls.Add(Me.lstAvailablePlayers)
        Me.grpPlayers.Controls.Add(Me.lblSelected)
        Me.grpPlayers.Controls.Add(Me.lstSelectedPlayers)
        Me.grpPlayers.Location = New System.Drawing.Point(25, 47)
        Me.grpPlayers.Name = "grpPlayers"
        Me.grpPlayers.Size = New System.Drawing.Size(617, 325)
        Me.grpPlayers.TabIndex = 2
        Me.grpPlayers.TabStop = False
        Me.grpPlayers.Text = "Players"
        '
        'btnNone
        '
        Me.btnNone.Location = New System.Drawing.Point(252, 237)
        Me.btnNone.Name = "btnNone"
        Me.btnNone.Size = New System.Drawing.Size(112, 30)
        Me.btnNone.TabIndex = 7
        Me.btnNone.Text = "&None >>"
        Me.btnNone.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Location = New System.Drawing.Point(252, 183)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(112, 30)
        Me.btnRemove.TabIndex = 6
        Me.btnRemove.Text = "&Remove >"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(252, 129)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(112, 30)
        Me.btnAdd.TabIndex = 5
        Me.btnAdd.Text = "< A&dd"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnAll
        '
        Me.btnAll.Location = New System.Drawing.Point(252, 75)
        Me.btnAll.Name = "btnAll"
        Me.btnAll.Size = New System.Drawing.Size(112, 30)
        Me.btnAll.TabIndex = 4
        Me.btnAll.Text = "<< &All"
        Me.btnAll.UseVisualStyleBackColor = True
        '
        'lblAvailable
        '
        Me.lblAvailable.AutoSize = True
        Me.lblAvailable.Location = New System.Drawing.Point(391, 28)
        Me.lblAvailable.Name = "lblAvailable"
        Me.lblAvailable.Size = New System.Drawing.Size(53, 13)
        Me.lblAvailable.TabIndex = 2
        Me.lblAvailable.Text = "Available:"
        '
        'lstAvailablePlayers
        '
        Me.lstAvailablePlayers.FormattingEnabled = True
        Me.lstAvailablePlayers.Location = New System.Drawing.Point(394, 44)
        Me.lstAvailablePlayers.Name = "lstAvailablePlayers"
        Me.lstAvailablePlayers.Size = New System.Drawing.Size(196, 251)
        Me.lstAvailablePlayers.Sorted = True
        Me.lstAvailablePlayers.TabIndex = 3
        '
        'lblSelected
        '
        Me.lblSelected.AutoSize = True
        Me.lblSelected.Location = New System.Drawing.Point(26, 28)
        Me.lblSelected.Name = "lblSelected"
        Me.lblSelected.Size = New System.Drawing.Size(52, 13)
        Me.lblSelected.TabIndex = 0
        Me.lblSelected.Text = "Selected:"
        '
        'lstSelectedPlayers
        '
        Me.lstSelectedPlayers.FormattingEnabled = True
        Me.lstSelectedPlayers.Location = New System.Drawing.Point(29, 44)
        Me.lstSelectedPlayers.Name = "lstSelectedPlayers"
        Me.lstSelectedPlayers.Size = New System.Drawing.Size(196, 251)
        Me.lstSelectedPlayers.Sorted = True
        Me.lstSelectedPlayers.TabIndex = 1
        '
        'cmbTeams
        '
        Me.cmbTeams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTeams.FormattingEnabled = True
        Me.cmbTeams.Location = New System.Drawing.Point(68, 17)
        Me.cmbTeams.Name = "cmbTeams"
        Me.cmbTeams.Size = New System.Drawing.Size(121, 21)
        Me.cmbTeams.Sorted = True
        Me.cmbTeams.TabIndex = 1
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(211, 394)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(245, 35)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "&Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'FAssignTeamPlayers
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(667, 456)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.cmbTeams)
        Me.Controls.Add(Me.grpPlayers)
        Me.Controls.Add(Me.lblTeams)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FAssignTeamPlayers"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Assign Team Players"
        Me.grpPlayers.ResumeLayout(False)
        Me.grpPlayers.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblTeams As System.Windows.Forms.Label
    Friend WithEvents grpPlayers As System.Windows.Forms.GroupBox
    Friend WithEvents btnNone As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnAll As System.Windows.Forms.Button
    Friend WithEvents lblAvailable As System.Windows.Forms.Label
    Friend WithEvents lstAvailablePlayers As System.Windows.Forms.ListBox
    Friend WithEvents lblSelected As System.Windows.Forms.Label
    Friend WithEvents lstSelectedPlayers As System.Windows.Forms.ListBox
    Friend WithEvents cmbTeams As System.Windows.Forms.ComboBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
