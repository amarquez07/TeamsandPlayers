' ------------------------------------------------------------------------------------------
' Name: Anthony Marquez
' Class: VB.Net #2
' Abstract: Edit player form
' ------------------------------------------------------------------------------------------

' ------------------------------------------------------------------------------------------
' Options
' ------------------------------------------------------------------------------------------
Option Explicit On


' ------------------------------------------------------------------------------------------
' Imports
' ------------------------------------------------------------------------------------------
Imports System.Globalization


Public Class FEditPlayer

    ' ------------------------------------------------------------------------------------------
    ' Form Variables
    ' ------------------------------------------------------------------------------------------
    Private f_intPlayerID As Integer
    Private f_blnResult As Boolean


    ' ------------------------------------------------------------------------------------------
    ' Name: SetPlayerID
    ' Name: What player are we going to edit.
    '       called after an instance is created but before it is shown
    ' ------------------------------------------------------------------------------------------
    Public Sub SetPlayerID(ByVal intPlayerID)

        Try

            ' Team ID
            f_intPlayerID = intPlayerID

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

    End Sub



    ' ------------------------------------------------------------------------------------------
    ' Name: FEditPlayer_Shown
    ' Abstract: load the form with values from database
    ' ------------------------------------------------------------------------------------------
    Private Sub FEditPlayer_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        Try

            ' Load the combobox
            LoadComboBoxFromDatabase("TStates", _
                                     "intStateID", "strState", _
                                     cmbState)

            ' Suitcase for the data
            Dim udtPlayer As udtPlayerType
            ' make a suitcase instance
            udtPlayer = New udtPlayerType

            ' Set the ID
            udtPlayer.intPlayerID = f_intPlayerID

            ' We are busy
            SetBusyCursor(Me, True)

            ' Is the data ok (pass in the empty suitcase by ref so it can be filled up?)
            If GetPlayerInformationFromDatabase(udtPlayer) = True Then

                ' Load text and combo boxes with values from database
                With udtPlayer
                    txtFirstName.Text = .strFirstName
                    txtMiddleName.Text = .strMiddleName
                    txtLastName.Text = .strLastName
                    txtStreetAddress.Text = .strStreetAddress
                    txtCity.Text = .strCity
                    txtStreetAddress.Text = .strStreetAddress
                    ' Get the state ID
                    cmbState.SelectedIndex = SelectItemIndexInList(cmbState, udtPlayer.intStateID)                   ' Get the state id from database and offset by -1 on combobox due to 0 based index
                    txtZipCode.Text = .strZipCode
                    txtHomePhoneNumber.Text = .strHomePhoneNumber
                    txtSalary.Text = ConvertDecimalToCurrencyString(.decSalary) ' Convert decimal into a string formatted for currency display
                    txtDateOfBirth.Text = CheckGenericDate(.dtmDateOfBirth)
                    SetPlayerSex(udtPlayer.intSexID)                            ' Check for and set the player's gender
                    GetMostValuablePlayer(.blnMostValuablePlayer)               ' Check for the Most valuable player flag
                    txtEmailAddress.Text = .strEmailAddress
                End With


            Else

                ' Something went wrong. Warn the user...
                MessageBox.Show(Me, "Error: Unable to load information for team to edit." & vbNewLine & _
                                "The form will now close.", "Edit Player Error", _
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

            ' Need a suicase for the state ID
            Dim liState As CListItem
            liState = New CListItem

            ' Get the state currently selected in combobox
            liState = cmbState.SelectedItem

            With udtPlayer
                ' Look it up with the data  from the form
                .strFirstName = txtFirstName.Text
                .strMiddleName = txtMiddleName.Text
                .strLastName = txtLastName.Text
                .strStreetAddress = txtStreetAddress.Text
                .strCity = txtCity.Text
                .intStateID = liState.GetID
                .strZipCode = txtZipCode.Text
                .strHomePhoneNumber = txtHomePhoneNumber.Text
                .decSalary = ConvertSalaryToDecimal(txtSalary.Text)
                .dtmDateOfBirth = PopulateEmptyDate(txtDateOfBirth.Text)
                .intSexID = CheckPlayerSex()
                .blnMostValuablePlayer = SetMostValuablePlayer()
                .strEmailAddress = txtEmailAddress.Text
                .intPlayerID = f_intPlayerID
            End With


            ' Do it
            blnResult = EditPlayerInDatabase2(udtPlayer)

        Catch excError As Exception

            WriteLog(excError)

        End Try

        Return blnResult

    End Function



    ' ------------------------------------------------------------------------------------------
    ' Name: btnCancel_Click
    ' Abstract: Get the new player information
    ' ------------------------------------------------------------------------------------------
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        Try

            ' Close the form
            Me.Close()

        Catch excError As Exception

            ' log and display error message
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

            ' Store a players full name concatenated from the textboxes
            Dim strPlayerFullName As String = txtLastName.Text & ", " & txtFirstName.Text

            ' Assign the player ID and name to list item
            clsPlayer = New CListItem(f_intPlayerID, strPlayerFullName)

        Catch excError As Exception

            ' Log and display errors
            WriteLog(excError)

        End Try

        Return clsPlayer

    End Function




    ' -------------------------------------------------------------------------
    ' Name: SetPlayerSex
    ' Abstract: Set the players sexual orientation
    ' -------------------------------------------------------------------------
    Public Sub SetPlayerSex(ByVal intSexID As Integer)


        Try

            ' is the player a female?
            If intSexID = 2 Then

                ' Yes, set radFemale to true
                radFemale.Checked = True

            ElseIf intSexID = 1 Then

                'No, set radMale to true
                radMale.Checked = True

            End If


        Catch excError As Exception

            ' log and display error message
            WriteLog(excError)

        End Try

    End Sub




    ' -------------------------------------------------------------------------
    ' Name: CheckPlayerSex
    ' Abstract: Check the players sexual orientation
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
    ' Name: GetMostValuablePlayer
    ' Abstract: Get the status of the most valuable player
    ' -------------------------------------------------------------------------
    Public Sub GetMostValuablePlayer(ByVal blnMostValuablePlayer As Boolean)


        Try

            ' Are you an MVP(Most Valuable Player)?
            If blnMostValuablePlayer = True Then

                ' Yes, set result to true
                chkMostValuablePlayer.Checked = True

            Else

                ' No, make sure checkbox reflects that
                chkMostValuablePlayer.Checked = False

            End If

        Catch excError As Exception

            ' log and display error message
            WriteLog(excError)

        End Try

    End Sub



    ' -------------------------------------------------------------------------
    ' Name: SetMostValuablePlayer
    ' Abstract: Set the most valuable player status
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