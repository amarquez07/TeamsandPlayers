' ------------------------------------------------------------------------------------------
' Name: Anthony Marquez
' Class: VB.Net #2
' Abstract: edit Team form
' ------------------------------------------------------------------------------------------

' ------------------------------------------------------------------------------------------
' Options
' ------------------------------------------------------------------------------------------
Option Explicit On


' ------------------------------------------------------------------------------------------
' Imports
' ------------------------------------------------------------------------------------------



Public Class FEditTeam

    ' ------------------------------------------------------------------------------------------
    ' Form Variables
    ' ------------------------------------------------------------------------------------------
    Private f_intTeamID As Integer
    Private f_blnResult As Boolean


    ' ------------------------------------------------------------------------------------------
    ' Name: SetTeamID
    ' Name: What team are we going to edit.
    '       called after an instance is created but before it is shown
    ' ------------------------------------------------------------------------------------------
    Public Sub SetTeamID(ByVal intTeamID)

        Try

            ' Team ID
            f_intTeamID = intTeamID

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

    End Sub



    ' ------------------------------------------------------------------------------------------
    ' Name: FEditTeam_Shown
    ' Abstract: load the form with values from database
    ' ------------------------------------------------------------------------------------------
    Private Sub FEditTeam_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        Try

            Dim udtTeam As udtTeamType

            ' make a suitcase instance
            udtTeam = New udtTeamType

            ' Set the ID
            udtTeam.intTeamID = f_intTeamID

            ' We are busy
            SetBusyCursor(Me, True)

            ' Is the data ok (pass in the empty suitcase by ref so it can be filled up?)
            If GetTeamInformationFromDatabase(udtTeam) = True Then

                txtTeam.Text = udtTeam.strTeam
                txtMascot.Text = udtTeam.strMascot

            Else

                ' Something went wrong. Warn the user...
                MessageBox.Show(Me, "Error: Unable to load information for team to edit." & vbNewLine & _
                                "The form will now close.", "Edit Team Error", _
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                ' Close the form
                Me.Hide()

            End If

        Catch excError As Exception

            WriteLog(excError)

        Finally

            ' We are not busy
            SetBusyCursor(Me, False)

        End Try

    End Sub



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
            udtTeam.intTeamID = f_intTeamID
            udtTeam.strTeam = txtTeam.Text
            udtTeam.strMascot = txtMascot.Text

            ' Do it
            blnResult = EditTeamInDatabase2(udtTeam)

        Catch excError As Exception

            WriteLog(excError)

        End Try

        Return blnResult

    End Function



    ' ------------------------------------------------------------------------------------------
    ' Name: btnCancel_Click
    ' Abstract: Get the new team information
    ' ------------------------------------------------------------------------------------------
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        Try
            ' Close the form
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

            clsTeam = New CListItem(f_intTeamID, txtTeam.Text)

        Catch excError As Exception

            ' Log and display errors
            WriteLog(excError)

        End Try

        Return clsTeam

    End Function

End Class