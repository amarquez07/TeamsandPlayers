' ------------------------------------------------------------------------------------------
' Name: Anthony Marquez
' Class: VB.Net #2
' Abstract: Assign team players form
' ------------------------------------------------------------------------------------------

' ------------------------------------------------------------------------------------------
' Options
' ------------------------------------------------------------------------------------------
Option Explicit On


' ------------------------------------------------------------------------------------------
' Imports
' ------------------------------------------------------------------------------------------


Public Class FAssignTeamPlayers


    ' ------------------------------------------------------------------------------------------
    ' Name: FAssignTeamPlayers_Shown
    ' Abstract: Execute code on display form
    ' ------------------------------------------------------------------------------------------
    Private Sub FAssignTeamPlayers_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown

        Try
            ' We are busy
            SetBusyCursor(Me, True)

            ' Load the combobox with teams from the database
            LoadComboBoxFromDatabase("VActiveTeams", "intTeamID", "strTeam", cmbTeams)

        Catch excError As Exception

            ' Log and display error messages
            WriteLog(excError)

        Finally

            ' We are not busy
            SetBusyCursor(Me, False)

        End Try

    End Sub



    ' ------------------------------------------------------------------------------------------
    ' Name: cmbTeams_SelectedIndexChanged
    ' Abstract: Execute code on index change
    ' ------------------------------------------------------------------------------------------
    Private Sub cmbTeams_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTeams.SelectedIndexChanged

        Try

            ' We are busy
            SetBusyCursor(Me, True)

            ' load the list box with updated list of players
            LoadTeamPlayers()

        Catch excError As Exception

            ' Log and display error messages
            WriteLog(excError)

        Finally

            ' We are not busy
            SetBusyCursor(Me, False)

        End Try
    End Sub



    ' ------------------------------------------------------------------------------------------
    ' Name: LoadTeamPlayers
    ' Abstract: Load the selected and available player lists for the current team
    ' ------------------------------------------------------------------------------------------
    Private Sub LoadTeamPlayers()

        Try

            Dim liSelectedTeam As New CListItem
            Dim intTeamID As Integer

            ' We are busy
            SetBusyCursor(Me, True)

            ' Is a team selected?
            If cmbTeams.SelectedIndex >= 0 Then

                ' Get the selected team ID
                liSelectedTeam = cmbTeams.SelectedItem
                intTeamID = liSelectedTeam.GetID()

                ' Selected Players
                LoadListWithPlayersFromDatabase(intTeamID, lstSelectedPlayers, True)

                ' Available Players
                LoadListWithPlayersFromDatabase(intTeamID, lstAvailablePlayers, False)

                ' Enable/Disable add/remove buttons
                EnableButtons()

            End If

        Catch excError As Exception

            ' Log and display the error message
            WriteLog(excError)

        Finally

            ' We are no longer busy
            SetBusyCursor(Me, False)

        End Try

    End Sub



    ' ------------------------------------------------------------------------------------------
    ' Name: EnableButtons
    ' Abstract: Enable/disable the ok and add/remove buttons
    ' ------------------------------------------------------------------------------------------
    Private Sub EnableButtons()

        Try

            ' All
            btnAll.Enabled = False
            If lstAvailablePlayers.Items.Count > 0 Then btnAll.Enabled = True
            ' Remove
            btnAdd.Enabled = False
            If lstAvailablePlayers.Items.Count > 0 Then btnAdd.Enabled = True
            ' Remove
            btnRemove.Enabled = False
            If lstSelectedPlayers.Items.Count > 0 Then btnRemove.Enabled = True
            ' None
            btnNone.Enabled = False
            If lstSelectedPlayers.Items.Count > 0 Then btnNone.Enabled = True

        Catch excError As Exception

            ' Log and display the error message
            WriteLog(excError)

        End Try

    End Sub



    ' ------------------------------------------------------------------------------------------
    ' Name: btnAll_Click
    ' Abstract: Add all players to the team
    ' ------------------------------------------------------------------------------------------
    Private Sub btnAll_Click(sender As Object, e As EventArgs) Handles btnAll.Click

        Try

            MessageBox.Show("Under construction")

        Catch excError As Exception

            ' Log and display the error message
            WriteLog(excError)

        End Try

    End Sub



    ' ------------------------------------------------------------------------------------------
    ' Name: btnAdd_Click
    ' Abstract: Add a player to the team
    ' ------------------------------------------------------------------------------------------
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        Try

            Dim liSelectedItem As CListItem
            Dim intTeamID As Integer
            Dim intPlayerID As Integer
            Dim intIndex As Integer

            ' Is a player selected?
            If lstAvailablePlayers.SelectedIndex >= 0 Then

                ' Yes

                ' We are busy
                SetBusyCursor(Me, True)

                ' Get team and player IDs from lists (which are populated with instances of CListItem)
                liSelectedItem = cmbTeams.SelectedItem
                intTeamID = liSelectedItem.GetID
                liSelectedItem = lstAvailablePlayers.SelectedItem
                intPlayerID = liSelectedItem.GetID

                ' Add the player
                If AddPlayerToTeamInDatabase2(intTeamID, intPlayerID) = True Then

                    ' Add to selected players
                    intIndex = lstSelectedPlayers.Items.Add(lstAvailablePlayers.SelectedItem)
                    lstSelectedPlayers.SelectedIndex = intIndex

                    ' Remove from available players
                    intIndex = lstAvailablePlayers.SelectedIndex
                    lstAvailablePlayers.Items.RemoveAt(intIndex)

                    ' Highlight next in list
                    HighlightNextItemInList(lstAvailablePlayers, intIndex)

                    EnableButtons()

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



    ' ------------------------------------------------------------------------------------------
    ' Name: btnRemove_Click
    ' Abstract: Remove the currently selected player from the team
    ' ------------------------------------------------------------------------------------------
    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click

        Try

            Dim liSelectedItem As CListItem
            Dim intTeamID As Integer
            Dim intPlayerID As Integer
            Dim intIndex As Integer

            ' Is a player selected?
            If lstSelectedPlayers.SelectedIndex >= 0 Then

                ' Yes

                ' We are busy
                SetBusyCursor(Me, True)

                ' Get team and player IDs from lists (which are populated with instances of CListItem)
                liSelectedItem = cmbTeams.SelectedItem
                intTeamID = liSelectedItem.GetID
                liSelectedItem = lstSelectedPlayers.SelectedItem
                intPlayerID = liSelectedItem.GetID

                ' Add the player
                If RemovePlayerFromTeamInDatabase2(intTeamID, intPlayerID) = True Then

                    ' Add to available players
                    intIndex = lstAvailablePlayers.Items.Add(liSelectedItem)
                    lstAvailablePlayers.SelectedIndex = intIndex

                    ' Remove from available players
                    intIndex = lstSelectedPlayers.SelectedIndex
                    lstSelectedPlayers.Items.RemoveAt(intIndex)

                    ' Highlight next in list
                    HighlightNextItemInList(lstSelectedPlayers, intIndex)

                    EnableButtons()

                End If

            End If

        Catch excError As Exception

            ' Log and display the error message
            WriteLog(excError)

        Finally

            ' We are no longer busy
            SetBusyCursor(Me, False)

        End Try

    End Sub



    ' ------------------------------------------------------------------------------------------
    ' Name: btnNone_Click
    ' Abstract: Remove all players from a team
    ' ------------------------------------------------------------------------------------------
    Private Sub btnNone_Click(sender As Object, e As EventArgs) Handles btnNone.Click

        Try

            MessageBox.Show("Under construction")

        Catch excError As Exception

            ' Log and display the error message
            WriteLog(excError)

        End Try
    End Sub



    ' ------------------------------------------------------------------------------------------
    ' Name: btnClose_Click
    ' Abstract: Close the form
    ' ------------------------------------------------------------------------------------------
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        Try

            Me.Hide()

        Catch excError As Exception

            ' Log and display the error message
            WriteLog(excError)

        End Try

    End Sub

End Class