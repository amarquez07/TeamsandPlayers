' -------------------------------------------------------------------------
' Module: modUtilities
' Author: Patrick Callahan
' Abstract: General purpose utilities
'
' Revision        Owner   Changes:
' 2013/04/07      P.C.    For Book
' -------------------------------------------------------------------------

' -------------------------------------------------------------------------
' Options
' -------------------------------------------------------------------------
Option Explicit On


' -------------------------------------------------------------------------
' Imports
' -------------------------------------------------------------------------
Imports System
Imports System.IO
Imports System.Text.RegularExpressions              ' Regular expressions source file


Public Module modUtilities

    ' -------------------------------------------------------------------------
    '  Module constants
    ' -------------------------------------------------------------------------
    ' What log file should we use
    Private Const strLOG_FILE_EXTENSION As String = ".Log"

    ' -------------------------------------------------------------------------
    '  Module variables
    ' -------------------------------------------------------------------------
    Private m_strOldLogFilePath As String           ' Name of the last log file opened
    Private m_fsLogFile As FileStream = Nothing     ' File handle of the last log file opened



    ' -------------------------------------------------------------------------
    ' Name: SetBusyCursor
    ' Abstract: Enable/Disable the form and set the cursor to nomal or busy
    ' -------------------------------------------------------------------------
    Public Sub SetBusyCursor(ByRef frmForm As Form, ByVal blnBusy As Boolean)

        Try

            ' Busy?
            If blnBusy = True Then

                ' Yes
                frmForm.Cursor = Cursors.WaitCursor
                frmForm.Enabled = False

            Else

                ' No
                frmForm.Cursor = Cursors.Default
                frmForm.Enabled = True

            End If

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

    End Sub



    ' -------------------------------------------------------------------------
    ' Name: intStateSelectedIndex
    ' Abstract: 
    ' -------------------------------------------------------------------------
    Public Function SelectItemIndexInList(ByVal cmbSource As ComboBox, ByVal intStateID As Integer)

        Dim intStateSelectedIndex = 0

        Try

            Dim intIndex As Integer = 0
            Dim liState As New CListItem

            For intIndex = 0 To cmbSource.Items.Count - 1

                liState = cmbSource.Items(intIndex)

                If (liState.GetID = intStateID) Then

                    intStateSelectedIndex = intIndex
                    Exit For

                End If

            Next

            Return intStateSelectedIndex

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

    End Function



    ' --------------------------------------------------------------------------------
    ' Name: HighlightNextItemInList
    ' Abstract: Get all the teams from the TTeams table
    ' --------------------------------------------------------------------------------
    Public Sub HighlightNextItemInList(ByRef lstTarget As ListBox,
                                       ByVal intSelectedIndex As Integer)

        Try

            ' Are we past the end of the list?
            If intSelectedIndex > lstTarget.Items.Count - 1 Then

                ' Yes, move to the last item
                intSelectedIndex = lstTarget.Items.Count - 1

            End If

            ' Select it
            lstTarget.SelectedIndex = intSelectedIndex

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

    End Sub



    ' -------------------------------------------------------------------------
    ' Name: IsValidZipCode
    ' Abstract: Use regular expressions to validate the zip code:
    '           5 digits or 5 plus 4 digits
    '           ##### or #####-####
    ' -------------------------------------------------------------------------
    Public Function IsValidZipCode(ByVal strZipCode As String) As Boolean

        Dim blnIsValidZipCode As Boolean = False

        Try

            Dim strStart As String = "^"
            Dim strStop As String = "$"
            Dim strDash As String = "\-"
            ' \ is the expae character in regular expressions
            Dim strPattern1 As String
            Dim strPattern2 As String

            ' No, validate data
            ' #####
            strPattern1 = strStart & "\d{5}" & strStop

            ' #####-####
            strPattern2 = strStart & "\d{5}" & strDash & "\d{4}" & strStop

            ' Does it match any of the formats?
            If Regex.IsMatch(strZipCode, strPattern1) = True Or _
                Regex.IsMatch(strZipCode, strPattern2) = True Then

                ' Yes
                blnIsValidZipCode = True

            End If

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnIsValidZipCode
    End Function



    ' -------------------------------------------------------------------------
    ' Name: IsValidPhoneNumber
    ' Abstract: Use regular expressions to validate the Phone number:
    '           5 digits or 5 plus 4 digits
    '           ###-####
    '           ###-###-####
    '           (###)-###-####
    ' -------------------------------------------------------------------------
    Public Function IsValidPhoneNumber(ByVal strPhoneNumber As String) As Boolean

        Dim blnIsValidPhoneNumber As Boolean = False

        Try

            Dim strStart As String = "^"
            Dim strStop As String = "$"
            Dim strDash As String = "\-"
            ' \ is the expae character in regular expressions
            Dim strPattern1 As String
            Dim strPattern2 As String
            Dim strPattern3 As String

            ' ###-####
            strPattern1 = strStart & "\d{3}" & strDash & "\d{4}" & strStop

            ' ###-###-####
            strPattern2 = strStart & "\d{3}" & strDash & "\d{3}" & strDash & "\d{4}" & strStop

            ' (###)-###-####
            strPattern3 = strStart & "\(\d{3}\)" & strDash & "\d{3}" & strDash & "\d{4}" & strStop

            ' Does it match any of the formats?
            If Regex.IsMatch(strPhoneNumber, strPattern1) = True Or _
                Regex.IsMatch(strPhoneNumber, strPattern2) = True Or _
                Regex.IsMatch(strPhoneNumber, strPattern3) = True Then

                ' Yes
                blnIsValidPhoneNumber = True

            End If

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnIsValidPhoneNumber

    End Function



    ' -------------------------------------------------------------------------
    ' Name: IsValidEmailAddress
    ' Abstract: Use regular expressions to validate the email address:
    ' -------------------------------------------------------------------------
    Public Function IsValidEmailAddress(ByVal strEmailAddress As String) As Boolean

        Dim blnIsValidEmailAddress As Boolean = False

        Try

            Dim strStart As String = "^"
            Dim strStop As String = "$"
            ' \ is the escape character in regular expressions
            Dim strPattern1 As String

            '           1:Letter, N: letters/numbers/dot/underscore/dash
            '           1:@
            '           1:letter, N:letters/numbers/dot/dash, 1:dot, 2-6:letters
            strPattern1 = strStart _
                                      & "[a-zA-Z][a-zA-Z0-9.%_-]*" _
                                      & "@" _
                                      & "[a-zA-Z][a-zA-Z0-9.-]*" & "\.[a-zA-Z]{2,6}" _
                                      & strStop

            ' Does it match any of the formats?
            If Regex.IsMatch(strEmailAddress, strPattern1) = True Then

                ' Yes
                blnIsValidEmailAddress = True

            End If

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnIsValidEmailAddress

    End Function



    ' -------------------------------------------------------------------------
    ' Name: IsValidSalary
    ' Abstract: Use regular expressions to validate the email address:
    '           1:Letter, N: letters/numbers/dot/underscore/dash
    '           1:@
    '           1:letter, N:letters/numbers/dot/dash, 1:dot, 2-6:letters
    ' -------------------------------------------------------------------------
    Public Function IsValidSalary(ByVal strSalary As String) As Boolean

        Dim blnIsValidCurrency As Boolean = False

        Try

            Dim strStart As String = "^"
            Dim strStop As String = "$"
            Dim strOptionalPlusOrMinus As String = "[\+\-]?"
            Dim strOptionalDollarSign As String = "\$?"
            Dim strOptionalDecimal As String = "(\.\d+)?"

            ' \ is the escape character in regular expressions
            Dim strPattern1 As String
            Dim strPattern2 As String

            ' No, validate data
            ' Optional +/-, Optional $, one or more digits, optional decimal
            strPattern1 = strStart _
                          & strOptionalPlusOrMinus _
                          & strOptionalDollarSign _
                          & "\d+" _
                          & strOptionalDecimal

            ' Same as above but with commas every 3 digits
            strPattern2 = strStart _
                          & strOptionalPlusOrMinus _
                          & strOptionalDollarSign _
                          & "\d{1,3}" _
                          & "(,\d{3})*" _
                          & strOptionalDecimal _
                          & strStop

            ' Does it match any of the formats?
            If Regex.IsMatch(strSalary, strPattern1) = True Or _
                Regex.IsMatch(strSalary, strPattern2) = True Then

                ' Yes
                blnIsValidCurrency = True

            End If

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnIsValidCurrency

    End Function


    ' -------------------------------------------------------------------------
    ' Name: ConvertSalaryToDecimal
    ' Abstract: check salary string and convert to decimal
    ' -------------------------------------------------------------------------
    Public Function ConvertSalaryToDecimal(ByVal strSourceSalary As String) As String

        Dim decTargetSalary As String = ""

        Try
            If strSourceSalary = "" Then


                ' Yes, set salary variable to 0
                decTargetSalary = 0

            Else

                ' Pat's way
                'strSourceSalary = strSourceSalary.Replace("$", "")
                'strSourceSalary = strSourceSalary.Replace(",", "")
                'decTargetSalary = Val(strSourceSalary)

                ' Convert string to Decimal (NumberStyles.Currency handles/removes dollar signs and commas)
                decTargetSalary = Decimal.Parse(strSourceSalary, Globalization.NumberStyles.Currency)

            End If
        Catch excError As Exception

            ' log and display error message
            WriteLog(excError)

        End Try

        Return decTargetSalary

    End Function



    ' -------------------------------------------------------------------------
    ' Name: ConvertDecimalToCurrencyString
    ' Abstract: Convert decimal to string for salary, if salary is 0 return empty string
    ' -------------------------------------------------------------------------
    Public Function ConvertDecimalToCurrencyString(ByVal decSalary As Decimal) As String

        Dim strTargetSalary As String = ""

        Try

            ' Is salary 0?
            If decSalary <= 0 Then

                ' Yes, set string to empty
                strTargetSalary = ""

            Else

                ' No, Convert decimal to string
                strTargetSalary = decSalary.ToString("c2")


            End If

        Catch excError As Exception

            ' log and display error message
            WriteLog(excError)

        End Try

        Return strTargetSalary

    End Function



    ' -------------------------------------------------------------------------
    ' Name: IsValidDate
    ' Abstract: Use regular expressions to validate the email address:
    '           1:Letter, N: letters/numbers/dot/underscore/dash
    '           1:@
    '           1:letter, N:letters/numbers/dot/dash, 1:dot, 2-6:letters
    ' -------------------------------------------------------------------------
    Public Function IsValidDate(ByVal strDate As String) As Boolean

        Dim blnIsValidDate As Boolean = False

        Try

            Dim strStart As String = "^"
            Dim strStop As String = "$"
            Dim strYears As String = "\d{4}"
            Dim strMonth As String = "\d{1,2}"
            Dim strDays As String = "\d{1,2}"
            Dim strForwardSlash As String = "\/"
            Dim strDash As String = "\-"

            ' \ is the escape character in regular expressions
            Dim strPattern1 As String
            Dim strPattern2 As String

                ' No, validate data
                ' Optional +/-, Optional $, one or more digits, optional decimal
                strPattern1 = strStart _
                              & strYears _
                              & strForwardSlash _
                              & strMonth _
                              & strForwardSlash _
                              & strDays _
                              & strStop

                strPattern2 = strStart _
                              & strYears _
                              & strDash _
                              & strMonth _
                              & strDash _
                              & strDays _
                              & strStop

                ' Does it match any of the formats?
                If Regex.IsMatch(strDate, strPattern1) = True Or _
                    Regex.IsMatch(strDate, strPattern2) = True Then

                    ' Yes
                    blnIsValidDate = True

                End If

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnIsValidDate

    End Function



    ' -------------------------------------------------------------------------
    ' Name: PopulateEmptyDate
    ' Abstract: Populate empty string with generic date for database
    ' -------------------------------------------------------------------------
    Public Function PopulateEmptyDate(ByVal strDate As String) As DateTime

        Dim dtmConvertedDateOfBirth As DateTime

        Try

            ' Is string empty?
            If strDate = "" Then

                ' Yes, populate with generic value
                strDate = "#1800/01/01#"

            End If

            ' Format string into "yyyy/MM/dd"
            dtmConvertedDateOfBirth = strDate

        Catch excError As Exception

            ' log and display error message
            WriteLog(excError)

        End Try

        Return dtmConvertedDateOfBirth

    End Function



    ' -------------------------------------------------------------------------
    ' Name: CheckGenericDate
    ' Abstract: Check for default input on a generic date time value
    '           and return empty string to be displayed for textbox
    ' -------------------------------------------------------------------------
    Public Function CheckGenericDate(ByVal dtmDate As DateTime) As String

        Dim strConvertedDateOfBirth As String = ""

        Try

            Dim strDate As String

            ' store source date as string, convert to just dd/mm/yyyy format
            strDate = dtmDate.ToString("yyyy/MM/dd")

            ' Does this string match the generic value?
            If strDate = "1800/01/01" Then

                ' Yes, assign empty string
                strConvertedDateOfBirth = ""

            Else

                ' Format string into "yyyy/MM/dd"
                strConvertedDateOfBirth = strDate

            End If

        Catch excError As Exception

            ' log and display error message
            WriteLog(excError)

        End Try

        Return strConvertedDateOfBirth

    End Function



    ' -------------------------------------------------------------------------
    ' Name: IsValidSocialSecurityNumber
    ' Abstract: 
    ' -------------------------------------------------------------------------
    Public Function IsValidSocialSecurityNumber(ByVal strSocialSecurityNumber As String) As Boolean

        Dim blnIsValidSocialSecurityNumber As Boolean = False

        Try

            Dim strStart As String = "^"
            Dim strStop As String = "$"
            Dim strDash As String = "\-"

            ' \ is the escape character in regular expressions
            Dim strPattern1 As String
            Dim strPattern2 As String

            ' #########
            ' Optional +/-, Optional $, one or more digits, optional decimal
            strPattern1 = strStart & "\d{9}" & strStop

            ' ### - ## - ####
            strPattern2 = strStart _
                          & "\d{3}" _
                          & strDash _
                          & "\d{2}" _
                          & strDash _
                          & "\d{4}" _
                          & strStop

            ' Does it match any of the formats?
            If Regex.IsMatch(strSocialSecurityNumber, strPattern1) = True Or _
                Regex.IsMatch(strSocialSecurityNumber, strPattern2) = True Then

                ' Yes
                blnIsValidSocialSecurityNumber = True

            End If

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnIsValidSocialSecurityNumber
    End Function



    ' -------------------------------------------------------------------------
    ' Name: TrimAllTextBoxes
    ' Abstract: Delete any files older than 10 days.
    ' -------------------------------------------------------------------------
    Public Sub TrimAllFormTextBoxes(frmTarget As Form)

        Try

            Dim intIndex As Integer
            Dim ctlCurrentControl As Control
            Dim txtCurrentTextBox As TextBox

            For intIndex = 0 To frmTarget.Controls.Count - 1

                ' Get the next control from  the list of all the  controls on the form
                ctlCurrentControl = frmTarget.Controls.Item(intIndex)

                ' is it a textbox?
                If TypeOf (ctlCurrentControl) Is TextBox Then

                    ' Yes, explicit cast to textbox so
                    txtCurrentTextBox = ctlCurrentControl

                    ' we can access the text property and trim
                    txtCurrentTextBox.Text = txtCurrentTextBox.Text.Trim

                End If

            Next

        Catch excError As Exception

            ' Log and display errors
            WriteLog(excError)

        End Try

    End Sub



    ' -------------------------------------------------------------------------
    ' Name: WriteLog
    ' Abstract: Overload withd blnDisplay set to true
    ' -------------------------------------------------------------------------
    Public Sub WriteLog(ByVal excErrorToLog As Exception, _
               Optional ByVal blnDisplayWarning As Boolean = True)

        Try

            WriteLog(excErrorToLog.ToString(), blnDisplayWarning)

        Catch excError As Exception

            ' Display error message
            MessageBox.Show("Error:" & vbNewLine & excError.ToString(), _
                            Application.ProductName, _
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)

        End Try

    End Sub



    ' -------------------------------------------------------------------------
    ' Name: WriteLog
    ' Abstract: Write a message to the error log.
    ' -------------------------------------------------------------------------
    Public Sub WriteLog(ByVal strMessageToLog As String, _
               Optional ByVal blnDisplayWarning As Boolean = True)

        Try

            Dim fsLogFile As FileStream = Nothing
            Dim encConvertToByteArray As New System.Text.UTF8Encoding

            ' Warn the user?
            If blnDisplayWarning = True Then

                ' Yes( ProductName is set in AssemblyInfo )
                MessageBox.Show(strMessageToLog, Application.ProductName, _
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)

            End If

            ' Append a date/time stamp
            strMessageToLog = (DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss") _
                              & " - " & strMessageToLog & vbNewLine & _
                              vbNewLine

            ' Get a free file handle
            fsLogFile = GetLogFile()

            ' Is the file OK?
            If Not fsLogFile Is Nothing Then

                ' Yes, Log it
                fsLogFile.Write(encConvertToByteArray.GetBytes(strMessageToLog), _
                                0, strMessageToLog.Length)

                ' Flush the buffer so we can immediately see results in file.  Very important.
                ' Otherwise we have to wait for flush which might be when application closes
                ' or we get another error.  Waiting for the application to close may not be
                ' a good idea if the application is in a production environment (e.g. a web
                '  app running on a remote server)
                fsLogFile.Flush()

            End If

        Catch excError As Exception

            ' Display error message
            MessageBox.Show("Error:" & vbNewLine & excError.ToString(), _
                            Application.ProductName, _
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)

        End Try

    End Sub


    ' -------------------------------------------------------------------------
    ' Name: DeleteOldFiles
    ' Abstract: Delete any files older than 10 days.
    ' -------------------------------------------------------------------------
    Private Sub DeleteOldFiles()

        Try

            Dim strLogFilePath As String = ""
            Dim dirLogDirectory As DirectoryInfo = Nothing
            Dim dtmFileCreated As DateTime = Now
            Dim intDaysOld As Integer = 0

            ' Path
            strLogFilePath = Application.StartupPath & "\Log\"

            ' Look for any files
            dirLogDirectory = New DirectoryInfo(strLogFilePath)

            ' Are there any?
            For Each finLogFile As FileInfo _
                In dirLogDirectory.GetFiles("*" & strLOG_FILE_EXTENSION)

                ' When was the file created?
                dtmFileCreated = finLogFile.CreationTime

                ' How old is the file?
                intDaysOld = (dtmFileCreated.Subtract(DateTime.Now)).Days

                ' Is the file older than 10 days?
                If intDaysOld > 10 Then

                    ' Yes.  Delete it.
                    finLogFile.Delete()

                End If

            Next

        Catch excError As Exception

            ' Display error message
            MessageBox.Show("Error:" & vbNewLine & excError.ToString(), _
                            Application.ProductName, _
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)

        End Try

    End Sub


    ' -------------------------------------------------------------------------
    ' Name: GetLogFile
    ' Abstract: Open the log file for writing.  Use today's date as part of
    '           the file name.  Each day a new log file will be created.
    '           Makes debug easier.
    '           Use a filestream object so we can specify file read share
    '           during the open call.
    ' -------------------------------------------------------------------------
    Private Function GetLogFile() As FileStream

        Try
            Dim strToday As String = (DateTime.Now).ToString("yyyyMMdd")
            Dim strLogFilePath As String = ""

            ' Log everything in a log directory off of the current application directory
            strLogFilePath = Application.StartupPath & _
                             "\Log\" & strToday & strLOG_FILE_EXTENSION

            ' Is this a new day?
            If m_strOldLogFilePath <> strLogFilePath Then

                ' Save the log file name
                m_strOldLogFilePath = strLogFilePath

                ' Does the log directory exist?
                If Directory.Exists(Application.StartupPath & "\Log") = False Then

                    ' No, so create it
                    Directory.CreateDirectory(Application.StartupPath & "\Log")

                End If

                ' Close old log file( if there is one )
                If Not m_fsLogFile Is Nothing Then m_fsLogFile.Close()

                ' Delete old log files
                DeleteOldFiles()

                ' Does the file exist?
                If File.Exists(strLogFilePath) = False Then

                    ' No, create with shared read access so it can be read while application has it open
                    m_fsLogFile = New FileStream(strLogFilePath, FileMode.Create, _
                                                 FileAccess.Write, FileShare.Read)

                Else

                    ' Yes, append with shared read access so it can be read while application has it open
                    m_fsLogFile = New FileStream(strLogFilePath, FileMode.Append, _
                                                 FileAccess.Write, FileShare.Read)

                End If

            End If

        Catch excError As Exception

            ' Display error message
            MessageBox.Show("Error:" & vbNewLine & excError.ToString(), _
                            Application.ProductName, _
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)

        End Try

        ' Return result
        Return m_fsLogFile

    End Function

End Module
