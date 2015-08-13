' ------------------------------------------------------------------------------------------
' Name: Anthony Marquez
' Class: VB.Net #2
' Abstract: Add player form
' ------------------------------------------------------------------------------------------

' ------------------------------------------------------------------------------------------
' Options
' ------------------------------------------------------------------------------------------
Option Explicit On


' ------------------------------------------------------------------------------------------
' Imports
' ------------------------------------------------------------------------------------------
Imports System.Globalization


Public Class FAddPlayer

    ' ------------------------------------------------------------------------------------------
    ' Form Variables
    ' ------------------------------------------------------------------------------------------
    Private f_intNewPlayerID As Integer
    Private f_intNewStateID As Integer
    Private f_blnResult As Boolean



    ' ------------------------------------------------------------------------------------------
    ' Name:FAddPlayer_Shown
    ' Abstract: Add the player to the database if the data is valid
    ' ------------------------------------------------------------------------------------------
    Private Sub FAddPlayer_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown

        Try

            ' Load the combobox
            LoadComboBoxFromDatabase("TStates", _
                                     "intStateID", "strState", _
                                     cmbState)

            ' Set female radio button to checked on shown
            radFemale.Checked = True

        Catch excError As Exception

            WriteLog(excError)

        End Try

    End Sub



    ' ------------------------------------------------------------------------------------------
    ' Name:btnOK_Click
    ' Abstract: Add the player to the database if the data is valid
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

            ' First Name
            If txtFirstName.Text = "" Then

                strErrorMessage &= "-First Name cannot be blank" & vbNewLine
                blnIsValidData = False

            End If

            ' Last Name
            If txtLastName.Text = "" Then

                strErrorMessage &= "-Last Name cannot be blank" & vbNewLine
                blnIsValidData = False

            End If

            ' Zip Code
            If txtZipCode.Text <> "" Then

                If IsValidZipCode(txtZipCode.Text) = False Then

                    strErrorMessage &= "-Zip Code is incorrect" & vbNewLine
                    blnIsValidData = False

                End If

            End If

            ' Home Phone Number
            If txtHomePhoneNumber.Text <> "" Then

                If IsValidPhoneNumber(txtHomePhoneNumber.Text) = False Then

                    strErrorMessage &= "-Phone number is invalid" & vbNewLine
                    blnIsValidData = False

                End If

            End If

            ' Salary
            If txtSalary.Text <> "" Then

                If IsValidSalary(txtSalary.Text) = False Then

                    strErrorMessage &= "-Salary amount is invalid"
                    blnIsValidData = False

                End If

            End If

            ' Date
            If txtDateOfBirth.Text <> "" Then

                If IsValidDate(txtDateOfBirth.Text) = False Then

                    strErrorMessage &= "-Date is incorrect" & vbNewLine
                    blnIsValidData = False

                End If

            End If

            ' Email
            If txtEmailAddress.Text <> "" Then

                If IsValidEmailAddress(txtEmailAddress.Text) = False Then

                    strErrorMessage &= "-Email address is invalid"
                    blnIsValidData = False

                End If

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
            Dim udtPlayer As New udtPlayerType

            Dim liState As New CListItem

            With udtPlayer
                ' Look it up with the data from the form
                .strFirstName = txtFirstName.Text
                .strMiddleName = txtMiddleName.Text
                .strLastName = txtLastName.Text
                .strStreetAddress = txtStreetAddress.Text
                .strCity = txtCity.Text
                ' Gather state info and add to udtPlayer structure
                liState = cmbState.SelectedItem
                .intStateID = liState.GetID
                .strZipCode = txtZipCode.Text
                .strHomePhoneNumber = txtHomePhoneNumber.Text
                .decSalary = ConvertSalaryToDecimal(txtSalary.Text)            ' Converts salary string to decimal and is stored in the suitcase
                .dtmDateOfBirth = PopulateEmptyDate(txtDateOfBirth.Text)       ' Check for empty date and populate if so
                .intSexID = CheckPlayerSex()                                   ' Checks for the players gender and stores it
                .blnMostValuablePlayer = SetMostValuablePlayer()
                .strEmailAddress = txtEmailAddress.Text
            End With

            ' We are busy
            SetBusyCursor(Me, True)

            ' Do it
            blnResult = AddPlayerToDatabase2(udtPlayer)

            ' Was it successful?
            If blnResult = True Then

                ' Yes, add was succesful
                f_blnResult = blnResult

                ' Save the new team & state ID
                f_intNewPlayerID = udtPlayer.intPlayerID
                f_intNewStateID = udtPlayer.intStateID

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
    ' Name: GetNewPlayerInformation
    ' Abstract: Get the new player information
    ' ------------------------------------------------------------------------------------------
    Public Function GetNewPlayerInformation() As CListItem

        Dim clsPlayer As CListItem = Nothing

        Try
            Dim strPlayerFullName As String = txtLastName.Text + ", " + txtFirstName.Text

            clsPlayer = New CListItem(f_intNewPlayerID, strPlayerFullName)

        Catch excError As Exception

            ' Log and display errors
            WriteLog(excError)

        End Try

        Return clsPlayer

    End Function



    ' -------------------------------------------------------------------------
    ' Name: CheckPlayerSex
    ' Abstract: Check the player's gender
    ' -------------------------------------------------------------------------
    Public Function CheckPlayerSex() As Integer

        Dim intSexID As Integer

        Try

            ' is the player a female?
            If radFemale.Checked = True Then

                ' Yes, set intsexID to 2 for male
                intSexID = 2

            ElseIf radMale.Checked = True Then

                'No, set to intSexId to 1 for male
                intSexID = 1

            End If


        Catch excError As Exception

            ' log and display error message
            WriteLog(excError)

        End Try

        Return intSexID

    End Function



    ' -------------------------------------------------------------------------
    ' Name: SetMostValuablePlayer
    ' Abstract: Set the most valuable player
    ' -------------------------------------------------------------------------
    Public Function SetMostValuablePlayer() As Boolean

        Dim blnResult As Boolean = False

        Try

            ' Is this player an MVP (Most valuable player)?
            If chkMostValuablePlayer.Checked = True Then

                ' Yes,
                blnResult = True

            End If

        Catch excError As Exception

            ' log and display error message
            WriteLog(excError)

        End Try

        Return blnResult

    End Function

End Class