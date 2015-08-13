' --------------------------------------------------------------------------------
' Name: Anthony Marquez
' Abstract: Suitcases for traveling with data ( Structures )
' --------------------------------------------------------------------------------

' --------------------------------------------------------------------------------
' Options
' --------------------------------------------------------------------------------
Option Explicit On
Option Strict Off


Public Module modUserDataTypes

    ' Strucutures are like suitcases.
    ' It's a lot easier to carry one suitcase intead of a whole bunch of loose items
    ' Same with passing data to a procedure. Instead of having 4+ variables just use
    ' a structure


    ' --------------------------------------------------------------------------------
    ' Name: udtTeamType
    ' Abstract: Structure to hold the values for any team entries/modifications in database
    ' --------------------------------------------------------------------------------
    Public Structure udtTeamType

        Dim intTeamID As Integer
        Dim strTeam As String
        Dim strMascot As String

    End Structure



    ' --------------------------------------------------------------------------------
    ' Name: udtPlayerType
    ' Abstract: Structure to hold the values for any player entries/modifications in database
    ' --------------------------------------------------------------------------------
    Public Structure udtPlayerType

        Dim intPlayerID As Integer
        Dim strFirstName As String
        Dim strMiddleName As String
        Dim strLastName As String
        Dim strStreetAddress As String
        Dim strCity As String
        Dim intStateID As Integer
        Dim strZipCode As String
        Dim strHomePhoneNumber As String
        Dim decSalary As Decimal
        Dim dtmDateOfBirth As DateTime
        Dim intSexID As Integer
        Dim blnMostValuablePlayer As Boolean
        Dim strEmailAddress As String

    End Structure

End Module
