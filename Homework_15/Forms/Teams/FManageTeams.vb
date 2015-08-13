' --------------------------------------------------------------------------------
' Name: Anthony Marquez
' Class: VB.Net 2
' Abstract: Homework #11
' --------------------------------------------------------------------------------

' --------------------------------------------------------------------------------
' Options
' --------------------------------------------------------------------------------
Option Explicit On
Option Strict Off


' --------------------------------------------------------------------------------
' Imports
' --------------------------------------------------------------------------------



Public Class FManageTeams

    ' --------------------------------------------------------------------------------
    ' Constants
    ' --------------------------------------------------------------------------------


    ' --------------------------------------------------------------------------------
    ' Name: FMain_Shown
    ' Abstract: Execute code as long as form is open
    ' --------------------------------------------------------------------------------
    Private Sub FManageTeams_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        Try

            Dim blnResult As Boolean = False

            ' Load the teams list
            blnResult = LoadTeamsList()

            ' Did it work?
            If blnResult = False Then

                ' No, warn the user
                MessageBox.Show(Me, "Unable to load the teams list" & vbNewLine & _
                                "The form will now close", _
                                Me.Text & " Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                ' and close the form
                Me.Close()

            End If

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

    End Sub



    ' --------------------------------------------------------------------------------
    ' Name: LoadTeamsList
    ' Abstract: Load the team list
    ' --------------------------------------------------------------------------------
    Private Function LoadTeamsList() As Boolean

        Dim blnResult As Boolean = False

        Try

            Dim strSourceTable As String = ""

            If chkShowDeleted.Checked = False Then

                strSourceTable = "VActiveTeams"

            Else

                strSourceTable = "VInactiveTeams"

            End If

            ' We are busy
            SetBusyCursor(Me, True)

            blnResult = LoadListBoxFromDatabase(strSourceTable, _
                                                "intTeamID", _
                                                "strTeam", _
                                                lstTeams)

        Catch excError As Exception

            WriteLog(excError)

        Finally

            ' We are not busy
            SetBusyCursor(Me, False)

        End Try

        Return blnResult
    End Function



    ' --------------------------------------------------------------------------------
    ' Name: btnAdd_Click
    ' Abstract: Add the team to database
    ' --------------------------------------------------------------------------------
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        Try
            Dim intNewItemIndex As Integer = 0
            ' Wrong Wrong wrong this is a blue print not an instance
            ' Always create a new instance of the class
            ' FAddTeam.ShowDialog() !!!WRONG!!!

            ' Make instance of FAddTeam
            Dim frmAddTeam As New FAddTeam

            ' Make instance of liNewTeam
            Dim liNewTeam As New CListItem

            ' Show modally
            frmAddTeam.ShowDialog()

            ' Ddd it work?
            If frmAddTeam.GetResult = True Then

                ' Yes, get the new team info
                liNewTeam = frmAddTeam.GetNewTeamInformation()

                ' Add new record to listbox (True = select)
                intNewItemIndex = lstTeams.Items.Add(liNewTeam)

                ' Select new team added
                lstTeams.SelectedIndex = intNewItemIndex

            End If

        Catch excError As Exception

            ' Display error message
            WriteLog(excError)

        End Try

    End Sub



    ' --------------------------------------------------------------------------------
    ' Name: btnEdit_Click
    ' Abstract: Edit the selected team
    ' --------------------------------------------------------------------------------
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click

        Try

            Dim intSelectedTeamID As Integer
            Dim liSelectedTeam As CListItem
            Dim frmEditTeam As FEditTeam
            Dim liNewTeamInformation As CListItem
            Dim intIndex As Integer

            ' Is the team selected?
            If lstTeams.SelectedIndex < 0 Then

                ' No, Warn the user
                MessageBox.Show("You must select a team to edit.", Me.Text & " Error", _
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            Else

                ' Yes, get the team to edit ID
                liSelectedTeam = lstTeams.SelectedItem
                intSelectedTeamID = liSelectedTeam.GetID

                ' Create an instance
                frmEditTeam = New FEditTeam

                ' Set, the form values
                frmEditTeam.SetTeamID(intSelectedTeamID)

                ' Show it modally
                frmEditTeam.ShowDialog(Me)

                ' Was the add succesful?
                If frmEditTeam.GetResult = True Then

                    ' Get new team values
                    liNewTeamInformation = frmEditTeam.GetNewTeamInformation

                    ' Yes, remove and re-add from list so it get's sorted correctly
                    lstTeams.Items.RemoveAt(lstTeams.SelectedIndex)

                    ' Add item returns index of newly added item
                    intIndex = lstTeams.Items.Add(liNewTeamInformation)

                    ' which we can use to select it
                    lstTeams.SelectedIndex = intIndex

                End If

            End If

        Catch excError As Exception

            ' Display error message
            WriteLog(excError)

        End Try


    End Sub



    ' --------------------------------------------------------------------------------
    ' Name: btnDelete_Click
    ' Abstract: Delete record from database and update listbox
    ' --------------------------------------------------------------------------------
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        Try

            ' Delete?
            If chkShowDeleted.Checked = False Then

                ' Yes
                DeleteTeam()

            Else

                ' No, undelete
                UndeleteTeam()

            End If

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

    End Sub



    ' --------------------------------------------------------------------------------
    ' Name: DeleteTeam
    ' Abstract: Mark the currently selected team as inactive
    ' --------------------------------------------------------------------------------
    Private Sub DeleteTeam()

        Try

            Dim intSelectedTeamID As Integer
            Dim liSelectedTeam As CListItem
            Dim strSelectedTeamName As String
            Dim intSelectedTeamIndex As Integer
            Dim drConfirm As DialogResult
            Dim blnResult As Boolean

            ' Is a team selected?
            If lstTeams.SelectedIndex < 0 Then

                ' No, Warn the user
                MessageBox.Show("You must selected a team to delete.", Me.Text & " Error", _
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            Else

                ' Get the team ID, name and index
                liSelectedTeam = lstTeams.SelectedItem
                intSelectedTeamID = liSelectedTeam.GetID
                strSelectedTeamName = liSelectedTeam.GetName
                intSelectedTeamIndex = lstTeams.SelectedIndex

                ' Yes, confirm they want to delete (Use name for user configuration)
                drConfirm = MessageBox.Show("Are you sure?", "Delete Team: " & strSelectedTeamName, _
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                ' Yes?
                If drConfirm = Windows.Forms.DialogResult.Yes Then

                    ' We are busy
                    SetBusyCursor(Me, True)

                    ' yes, delete the team (use ID for database command)
                    blnResult = DeleteTeamFromDatabase(intSelectedTeamID)

                    ' Was the delete successful
                    If blnResult = True Then

                        ' Yes, remove the team from the list
                        lstTeams.Items.RemoveAt(intSelectedTeamIndex)

                        ' Select the next team on the list
                        HighlightNextItemInList(lstTeams, intSelectedTeamIndex)

                    End If

                End If

            End If

        Catch excError As Exception

            WriteLog(excError)

        Finally

            ' We are not busy
            SetBusyCursor(Me, False)

        End Try

    End Sub



    ' --------------------------------------------------------------------------------
    ' Name: UndeleteTeam
    ' Abstract: Delete the currently selected team
    ' --------------------------------------------------------------------------------
    Private Sub UndeleteTeam()

        Try

            Dim liSelectedTeam As CListItem
            Dim intSelectedTeamID As Integer
            Dim intSelectedTeamIndex As Integer
            Dim blnResult As Boolean

            ' Is a team selected?
            If lstTeams.SelectedIndex < 0 Then

                ' No, warn the user
                MessageBox.Show("You must select a team to undelete.", Me.Text & " Error", _
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            Else

                ' Get the team ID and list index
                liSelectedTeam = lstTeams.SelectedItem
                intSelectedTeamID = liSelectedTeam.GetID()
                intSelectedTeamIndex = lstTeams.SelectedIndex

                ' We are busy
                SetBusyCursor(Me, True)

                ' Yes, undelete the team
                blnResult = UndeleteTeamFromDatabase(intSelectedTeamID)

                ' Was the undelete succesful?
                If blnResult = True Then

                    ' Yes, remove the team from the list
                    lstTeams.Items.RemoveAt(intSelectedTeamIndex)

                    ' Select the next team in the list
                    HighlightNextItemInList(lstTeams, intSelectedTeamIndex)

                End If
            End If

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        Finally

            ' We are no longer busy
            SetBusyCursor(Me, False)

        End Try

    End Sub



    ' --------------------------------------------------------------------------------
    ' Name: chkShowDeleted_CheckedChanged
    ' Abstract: Toggle between the active and inactive teams
    ' --------------------------------------------------------------------------------
    Private Sub chkShowDeleted_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowDeleted.CheckedChanged

        Try

            ' Is show deleted checked?
            If chkShowDeleted.Checked = False Then

                ' No, enable buttons
                btnAdd.Enabled = True
                btnEdit.Enabled = True
                btnDelete.Text = "&Delete"

            Else

                ' Yes, disable buttons
                btnAdd.Enabled = False
                btnEdit.Enabled = False
                btnDelete.Text = "&Undelete"

            End If

            LoadTeamsList()

        Catch excError As Exception

            WriteLog(excError)

        End Try

    End Sub



    ' --------------------------------------------------------------------------------
    ' Name: btnClose_Click
    ' Abstract: Execute code as long as form is open
    ' --------------------------------------------------------------------------------
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        Try

            Me.Close()

        Catch excError As Exception

            WriteLog(excError)

        End Try

    End Sub

    Private Sub FManageTeams_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
