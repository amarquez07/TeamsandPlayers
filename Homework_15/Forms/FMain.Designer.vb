<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FMain
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
        Me.btnManageTeams = New System.Windows.Forms.Button()
        Me.btnManagePlayers = New System.Windows.Forms.Button()
        Me.btnAssignTeamPlayers = New System.Windows.Forms.Button()
        Me.grpManageAssign = New System.Windows.Forms.GroupBox()
        Me.mnuMenu = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuManageTeams = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAssignTeamPlayers = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuManagePlayers = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.grpManageAssign.SuspendLayout()
        Me.mnuMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnManageTeams
        '
        Me.btnManageTeams.Location = New System.Drawing.Point(31, 31)
        Me.btnManageTeams.Name = "btnManageTeams"
        Me.btnManageTeams.Size = New System.Drawing.Size(213, 49)
        Me.btnManageTeams.TabIndex = 0
        Me.btnManageTeams.Text = "Manage Teams"
        Me.btnManageTeams.UseVisualStyleBackColor = True
        '
        'btnManagePlayers
        '
        Me.btnManagePlayers.Location = New System.Drawing.Point(31, 209)
        Me.btnManagePlayers.Name = "btnManagePlayers"
        Me.btnManagePlayers.Size = New System.Drawing.Size(213, 49)
        Me.btnManagePlayers.TabIndex = 2
        Me.btnManagePlayers.Text = "Manage Players"
        Me.btnManagePlayers.UseVisualStyleBackColor = True
        '
        'btnAssignTeamPlayers
        '
        Me.btnAssignTeamPlayers.Location = New System.Drawing.Point(31, 120)
        Me.btnAssignTeamPlayers.Name = "btnAssignTeamPlayers"
        Me.btnAssignTeamPlayers.Size = New System.Drawing.Size(213, 49)
        Me.btnAssignTeamPlayers.TabIndex = 1
        Me.btnAssignTeamPlayers.Text = "Assign Team Players"
        Me.btnAssignTeamPlayers.UseVisualStyleBackColor = True
        '
        'grpManageAssign
        '
        Me.grpManageAssign.Controls.Add(Me.btnManageTeams)
        Me.grpManageAssign.Controls.Add(Me.btnAssignTeamPlayers)
        Me.grpManageAssign.Controls.Add(Me.btnManagePlayers)
        Me.grpManageAssign.Location = New System.Drawing.Point(33, 37)
        Me.grpManageAssign.Name = "grpManageAssign"
        Me.grpManageAssign.Size = New System.Drawing.Size(275, 288)
        Me.grpManageAssign.TabIndex = 1
        Me.grpManageAssign.TabStop = False
        Me.grpManageAssign.Text = "Manage/Assign"
        '
        'mnuMenu
        '
        Me.mnuMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuTools, Me.mnuHelp})
        Me.mnuMenu.Location = New System.Drawing.Point(0, 0)
        Me.mnuMenu.Name = "mnuMenu"
        Me.mnuMenu.Size = New System.Drawing.Size(340, 24)
        Me.mnuMenu.TabIndex = 0
        Me.mnuMenu.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(37, 20)
        Me.mnuFile.Text = "File"
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(152, 22)
        Me.mnuExit.Text = "Exit"
        '
        'mnuTools
        '
        Me.mnuTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuManageTeams, Me.mnuAssignTeamPlayers, Me.mnuManagePlayers})
        Me.mnuTools.Name = "mnuTools"
        Me.mnuTools.Size = New System.Drawing.Size(48, 20)
        Me.mnuTools.Text = "&Tools"
        '
        'mnuManageTeams
        '
        Me.mnuManageTeams.Name = "mnuManageTeams"
        Me.mnuManageTeams.Size = New System.Drawing.Size(182, 22)
        Me.mnuManageTeams.Text = "Manage Teams"
        '
        'mnuAssignTeamPlayers
        '
        Me.mnuAssignTeamPlayers.Name = "mnuAssignTeamPlayers"
        Me.mnuAssignTeamPlayers.Size = New System.Drawing.Size(182, 22)
        Me.mnuAssignTeamPlayers.Text = "Assign Team Players"
        '
        'mnuManagePlayers
        '
        Me.mnuManagePlayers.Name = "mnuManagePlayers"
        Me.mnuManagePlayers.Size = New System.Drawing.Size(182, 22)
        Me.mnuManagePlayers.Text = "Manage Players"
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHelpAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(44, 20)
        Me.mnuHelp.Text = "Help"
        '
        'mnuHelpAbout
        '
        Me.mnuHelpAbout.Name = "mnuHelpAbout"
        Me.mnuHelpAbout.Size = New System.Drawing.Size(152, 22)
        Me.mnuHelpAbout.Text = "Help/About"
        '
        'FMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(340, 355)
        Me.Controls.Add(Me.grpManageAssign)
        Me.Controls.Add(Me.mnuMenu)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MainMenuStrip = Me.mnuMenu
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Manage Teams and Players"
        Me.grpManageAssign.ResumeLayout(False)
        Me.mnuMenu.ResumeLayout(False)
        Me.mnuMenu.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnManageTeams As System.Windows.Forms.Button
    Friend WithEvents btnManagePlayers As System.Windows.Forms.Button
    Friend WithEvents btnAssignTeamPlayers As System.Windows.Forms.Button
    Friend WithEvents grpManageAssign As System.Windows.Forms.GroupBox
    Friend WithEvents mnuMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTools As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuManageTeams As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAssignTeamPlayers As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuManagePlayers As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
End Class
