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
            ' \ is the expae character in regular expressions
            Dim strPattern1 As String
            Dim strPattern2 As String

            ' #####
            strPattern1 = strStart & "\d{5}" & strStop

            ' #####-####
            strPattern2 = strStart & "\d{5}" & "\-" & "\d{4}" & strStop

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
    ' Name: IsValidPhoneNumber
    ' Abstract: Use regular expressions to validate the email address:
    '           1:Letter, N: letters/numbers/dot/underscore/dash
    '           1:@
    '           1:letter, N:letters/numbers/dot/dash, 1:dot, 2-6:letters
    ' -------------------------------------------------------------------------
    Public Function IsValidEmailAddress(ByVal strPhoneNumber As String) As Boolean

        Dim blnIsValidEmailAddress As Boolean = False

        Try

            Dim strStart As String = "^"
            Dim strStop As String = "$"
            ' \ is the escape character in regular expressions
            Dim strPattern1 As String

            ' ###-####
            strPattern1 = strStart _
                          & "[a-zA-Z][a-zA-Z0-9\.\_\-]*" _
                          & "@" _
                          & "[a-zA-Z][a-zA-Z0-9\.\-]*" _
                          & "\.[a-zA-Z]{2,6}" _
                          & strStop

            ' Does it match any of the formats?
            If Regex.IsMatch(strPhoneNumber, strPattern1) = True Then

                ' Yes
                blnIsValidEmailAddress = True

            End If

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnIsValidEmailAddress
    End Function