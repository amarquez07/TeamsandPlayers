' ------------------------------------------------------------------------------------------
' Name: Anthony Marquez
' Class: VB.Net #2
' Abstract: Add Team Forms
' ------------------------------------------------------------------------------------------

' ------------------------------------------------------------------------------------------
' Options
' ------------------------------------------------------------------------------------------
Option Explicit On


' ------------------------------------------------------------------------------------------
' Imports
' ------------------------------------------------------------------------------------------



Public Class FAddTeam

    ' ------------------------------------------------------------------------------------------
    ' Form Variables
    ' ------------------------------------------------------------------------------------------
    Private f_intNewTeamID As Integer
    Private f_blnResult As Boolean


    ' ------------------------------------------------------------------------------------------
    ' Name:btnOK_Click
    ' Abstract: Add the team to the database if the data is valid
    ' ------------------------------------------------------------------------------------------
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click

        Try

            If IsValidData() = True Then

                If SaveData() = True Then

                    ' Yes, success
                    f_blnResult = True

                    ' Hide the form
                    Me.Hide()

                End If

            End If

        Catch excError As Exception

            ' Log and display errors
            WriteLog(excError)

        End Try

    End Sub



    ' ------------------------------------------------------------------------------------------
    ' Name: IsValidData
    ' Abstract:
    ' ------------------------------------------------------------------------------------------
    Private Function IsValidData() As Boolean

        Dim blnIsValidData As Boolean = True                ' Easier to assume true and have one error to turn off

        Try

            Dim strErrorMessage As String = "Please correct the following error(s): " & vbNewLine

            TrimAllFormTextBoxes(Me)

            ' Team
            If txtTeam.Text = "" Then

                strErrorMessage &= "-Team cannot be blank" & vbNewLine
                blnIsValidData = False

            End If

            ' Mascot
            If txtMascot.Text = "" Then

                strErrorMessage &= "-Mascot cannot be blank" & vbNewLine
                blnIsValidData = False

            End If

            ' Bad Data
            If blnIsValidData = False Then

                ' Yes, warn the user
                MessageBox.Show(strErrorMessage, Me.Text & " Error", _
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            End If

        Catch excError As Exception

            ' Log and display errors
            WriteLog(excError)

        End Try

        Return blnIsValidData

    End Function



    ' ------------------------------------------------------------------------------------------
    ' Name: SaveData
    ' Abstract: Save the data to the Database
    ' ------------------------------------------------------------------------------------------
    Private Function SaveData() As Boolean

        Dim blnResult As Boolean

        Try

            ' Need a suitcase since we are traveling to the database
            Dim udtTeam As New udtTeamType

            ' Look it up with the data  from the form
            udtTeam.strTeam = txtTeam.Text
            udtTeam.strMascot = txtMascot.Text

            ' We are busy
            SetBusyCursor(Me, True)

            ' Do it
            blnResult = AddTeamToDatabase2(udtTeam)

            ' Was it successful?
            If blnResult = True Then

                ' Yes, add was succesful
                f_blnResult = blnResult

                ' Save the new team ID
                f_intNewTeamID = udtTeam.intTeamID

            End If

        Catch excError As Exception

            WriteLog(excError)

        Finally

            ' We are not busy
            SetBusyCursor(Me, False)

        End Try

        Return blnResult

    End Function



    ' ------------------------------------------------------------------------------------------
    ' Name: btnCancel_Click
    ' Abstract: Close form
    ' ------------------------------------------------------------------------------------------
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        Try

            Me.Close()

        Catch excError As Exception

            ' Log and display errors
            WriteLog(excError)

        End Try

    End Sub



    ' ------------------------------------------------------------------------------------------
    ' Name: GetResult
    ' Abstract: Was the add/edit succesful?
    ' ------------------------------------------------------------------------------------------
    Public Function GetResult() As Boolean

        Try

        Catch excError As Exception

            ' Log and display errors
            WriteLog(excError)

        End Try

        Return f_blnResult

    End Function



    ' ------------------------------------------------------------------------------------------
    ' Name: GetNewTeamInformation
    ' Abstract: Get the new team information
    ' ------------------------------------------------------------------------------------------
    Public Function GetNewTeamInformation() As CListItem

        Dim clsTeam As CListItem = Nothing

        Try

            clsTeam = New CListItem(f_intNewTeamID, txtTeam.Text)

        Catch excError As Exception

            ' Log and display errors
            WriteLog(excError)

        End Try

        Return clsTeam

    End Function
End Class