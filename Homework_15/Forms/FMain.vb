' ------------------------------------------------------------------------------------------
' Name: Anthony Marquez
' Class: VB.Net #2
' Abstract: Homework 15
' ------------------------------------------------------------------------------------------

' ------------------------------------------------------------------------------------------
' Options
' ------------------------------------------------------------------------------------------
Option Explicit On


Public Class FMain

    ' --------------------------------------------------------------------------------
    ' Name: FMain_Shown
    ' Abstract: Execute code as long as form is open
    ' --------------------------------------------------------------------------------
    Private Sub FMain_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown

        Try

            ' We are busy
            SetBusyCursor(Me, True)

            ' Did the database connection fail?
            If OpenDatabaseConnectionSQLServer() = False Then

                ' Close the application
                Application.Exit()

            End If

        Catch excError As Exception

            WriteLog(excError)

        Finally

            ' We are not busy
            SetBusyCursor(Me, False)

        End Try

    End Sub



    ' --------------------------------------------------------------------------------
    ' Name: btnManageTeams_Click
    ' Abstract: Open & display the manage teams form
    ' --------------------------------------------------------------------------------
    Private Sub btnManageTeams_Click(sender As Object, e As EventArgs) Handles btnManageTeams.Click

        Try

            ' Create an instance of the form
            Dim frmManageTeam As New FManageTeams

            ' Display the form modaly
            frmManageTeam.ShowDialog()

        Catch excError As Exception

            ' Log and Displayt error message
            WriteLog(excError)

        End Try



    End Sub



    ' --------------------------------------------------------------------------------
    ' Name: btnAssignTeamPlayers_Click
    ' Abstract: Open & display the manage players form
    ' --------------------------------------------------------------------------------
    Private Sub btnAssignTeamPlayers_Click(sender As Object, e As EventArgs) Handles btnAssignTeamPlayers.Click

        Try

            ' Create an instance of the form
            Dim frmAssignTeamPlayers As New FAssignTeamPlayers

            ' Display the form modaly
            frmAssignTeamPlayers.ShowDialog()

        Catch excError As Exception

            ' Log and Displayt error message
            WriteLog(excError)

        End Try

    End Sub



    ' --------------------------------------------------------------------------------
    ' Name: btnManagePlayers_Click
    ' Abstract: Open & display the manage players form
    ' --------------------------------------------------------------------------------
    Private Sub btnManagePlayers_Click(sender As Object, e As EventArgs) Handles btnManagePlayers.Click

        Try

            ' Create an instance of the form
            Dim frmManagePlayers As New FManagePlayers

            ' Display the form modaly
            frmManagePlayers.ShowDialog()

        Catch excError As Exception

            ' Log and Displayt error message
            WriteLog(excError)

        End Try

    End Sub



    ' --------------------------------------------------------------------------------
    ' Name: FMain_Closed
    ' Abstract: Execute code as long as form is open
    ' --------------------------------------------------------------------------------
    Private Sub FMain_Closed(sender As Object, e As EventArgs) Handles Me.FormClosed

        Try

            ' We are busy
            SetBusyCursor(Me, True)

            ' Make sure connection is closed
            modDatabaseUtilities.CloseDatabaseConnection()

        Catch excError As Exception

            WriteLog(excError)

        Finally

            ' We are not busy
            SetBusyCursor(Me, False)

        End Try

    End Sub

End Class