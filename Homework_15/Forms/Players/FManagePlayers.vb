' --------------------------------------------------------------------------------
' Name: Anthony Marquez
' Class: VB.Net 2
' Abstract: Manage players form
' --------------------------------------------------------------------------------

' --------------------------------------------------------------------------------
' Options
' --------------------------------------------------------------------------------
Option Explicit On
Option Strict Off



' --------------------------------------------------------------------------------
' Imports
' --------------------------------------------------------------------------------



Public Class FManagePlayers

    ' --------------------------------------------------------------------------------
    ' Constants
    ' --------------------------------------------------------------------------------


    ' --------------------------------------------------------------------------------
    ' Name: FMain_Shown
    ' Abstract: Execute code as long as form is open
    ' --------------------------------------------------------------------------------
    Private Sub FManagePlayers_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        Try

            Dim blnResult As Boolean = False

            ' Load the players lists
            blnResult = LoadPlayersList()

            ' Did it work?
            If blnResult = False Then

                ' No, warn the user
                MessageBox.Show(Me, "Unable to load the players list" & vbNewLine & _
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
    ' Name: LoadPlayersList
    ' Abstract: Execute code as long as form is open
    ' --------------------------------------------------------------------------------
    Private Function LoadPlayersList() As Boolean

        Dim blnResult As Boolean = False

        Try

            Dim strSourceTable As String = ""

            If chkShowDeleted.Checked = False Then

                strSourceTable = "VActivePlayers"

            Else

                strSourceTable = "VInactivePlayers"

            End If

            ' We are busy
            SetBusyCursor(Me, True)

            ' Load listbox with custom sql select statement
            blnResult = LoadListBoxFromDatabase(strSourceTable, "intPlayerID", "strLastName + ', ' + strFirstName", lstPlayers)

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
    ' Abstract: Add the player to database
    ' --------------------------------------------------------------------------------
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        Try
            Dim intNewItemIndex As Integer = 0
            ' Wrong Wrong wrong this is a blue print not an instance
            ' Always create a new instance of the class
            ' FAddPlayer.ShowDialog() !!!WRONG!!!

            ' Make instance of FAddTeam
            Dim frmAddPlayer As New FAddPlayer
            ' Make instance of CListItem
            Dim liNewPlayer As New CListItem


            ' Show modally
            frmAddPlayer.ShowDialog()

            ' Ddd it work?
            If frmAddPlayer.GetResult = True Then

                ' Yes, get the new player info
                liNewPlayer = frmAddPlayer.GetNewPlayerInformation

                ' Add new record to listbox (True = select)
                intNewItemIndex = lstPlayers.Items.Add(liNewPlayer)

                ' Select new player added
                lstPlayers.SelectedIndex = intNewItemIndex

            End If

        Catch excError As Exception

            ' Display error message
            WriteLog(excError)

        End Try

    End Sub



    ' --------------------------------------------------------------------------------
    ' Name: btnEdit_Click
    ' Abstract: Edit the selected player
    ' --------------------------------------------------------------------------------
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click

        Try

            Dim intSelectedPlayerID As Integer
            Dim liSelectedPlayer As CListItem
            Dim frmEditPlayer As FEditPlayer
            Dim liNewPlayerInformation As CListItem
            Dim intIndex As Integer

            ' Is the team selected?
            If lstPlayers.SelectedIndex < 0 Then

                ' No, Warn the user
                MessageBox.Show("You must select a team to edit.", Me.Text & " Error", _
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            Else

                ' Yes, get the team to edit ID
                liSelectedPlayer = lstPlayers.SelectedItem
                intSelectedPlayerID = liSelectedPlayer.GetID

                ' Create an instance
                frmEditPlayer = New FEditPlayer

                ' Set, the form values
                frmEditPlayer.SetPlayerID(intSelectedPlayerID)

                ' Show it modally
                frmEditPlayer.ShowDialog(Me)

                ' Was the add succesful?
                If frmEditPlayer.GetResult = True Then

                    ' Get new team values
                    liNewPlayerInformation = frmEditPlayer.GetNewPlayerInformation

                    ' Yes, remove and re-add from list so it get's sorted correctly
                    lstPlayers.Items.RemoveAt(lstPlayers.SelectedIndex)

                    ' Add item returns index of newly added item
                    intIndex = lstPlayers.Items.Add(liNewPlayerInformation)

                    ' which we can use to select it
                    lstPlayers.SelectedIndex = intIndex

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
                DeletePlayer()

            Else

                ' No, undelete
                UndeletePlayer()

            End If
            

        Catch excError As Exception

            WriteLog(excError)

        End Try

    End Sub



    ' --------------------------------------------------------------------------------
    ' Name: DeletePlayer
    ' Abstract: Delete record from database and update listbox
    ' --------------------------------------------------------------------------------
    Private Sub DeletePlayer()

        Try

            Dim liSelectedPlayer As CListItem
            Dim intSelectedPlayerID As Integer
            Dim strSelectedPlayerName As String
            Dim intSelectedPlayerIndex As Integer
            Dim drConfirm As DialogResult
            Dim blnResult As Boolean

            ' Is a team selected?
            If lstPlayers.SelectedIndex < 0 Then

                ' No, Warn the user
                MessageBox.Show("You must selected a player to delete.", Me.Text & " Error", _
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            Else

                ' Get the player ID, name and index
                liSelectedPlayer = lstPlayers.SelectedItem
                intSelectedPlayerID = liSelectedPlayer.GetID
                strSelectedPlayerName = liSelectedPlayer.GetName
                intSelectedPlayerIndex = lstPlayers.SelectedIndex

                ' Yes, confirm they want to delete (Use name for user configuration)
                drConfirm = MessageBox.Show("Are you sure?", "Delete player: " & strSelectedPlayerName, _
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                ' Yes?
                If drConfirm = Windows.Forms.DialogResult.Yes Then

                    ' We are busy
                    SetBusyCursor(Me, True)

                    ' yes, delete the team (use ID for database command)
                    blnResult = DeletePlayerFromDatabase(intSelectedPlayerID)

                    ' Was the delete successful
                    If blnResult = True Then

                        ' Yes, remove the Player from the list
                        lstPlayers.Items.RemoveAt(intSelectedPlayerIndex)

                        ' Select the next team on the list
                        HighlightNextItemInList(lstPlayers, intSelectedPlayerIndex)

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
    ' Name: UndeletePlayer
    ' Abstract: mark player as active
    ' --------------------------------------------------------------------------------
    Private Sub UndeletePlayer()

        Try

            Dim intSelectedPlayerID As Integer
            Dim liSelectedPlayer As CListItem
            Dim intSelectedPlayerIndex As Integer
            Dim blnResult As Boolean

            ' Is a team selected?
            If lstPlayers.SelectedIndex < 0 Then

                ' No, Warn the user
                MessageBox.Show("You must selected a player to delete.", Me.Text & " Error", _
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            Else

                ' Get the player ID and index
                liSelectedPlayer = lstPlayers.SelectedItem
                intSelectedPlayerID = liSelectedPlayer.GetID
                intSelectedPlayerIndex = lstPlayers.SelectedIndex

                ' We are busy
                SetBusyCursor(Me, True)

                ' Yes undelete the player
                blnResult = UndeletePlayerFromDatabase(intSelectedPlayerID)

                ' Was the delete succesful?
                If blnResult = True Then

                    ' Yes, remove the team from the listbox
                    lstPlayers.Items.RemoveAt(intSelectedPlayerIndex)

                    ' Select the next team in the list
                    HighlightNextItemInList(lstPlayers, intSelectedPlayerIndex)


                End If

            End If

        Catch excError As Exception

            WriteLog(excError)

        Finally

            ' We are no longer busy
            SetBusyCursor(Me, False)

        End Try
    End Sub



    ' --------------------------------------------------------------------------------
    ' Name: chkShowDeleted_CheckedChanged
    ' Abstract: Toggle between active and inactive players
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

            LoadPlayersList()

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

End Class
