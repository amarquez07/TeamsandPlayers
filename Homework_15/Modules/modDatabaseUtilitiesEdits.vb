' --------------------------------------------------------------------------------
' Name: Anthony Marquez
' Class: VB.Net 2
' Abstract: modDatabaseUtilities
' --------------------------------------------------------------------------------

' --------------------------------------------------------------------------------
' Options
' --------------------------------------------------------------------------------
Option Explicit On
Option Strict Off



Public Module modDatabaseUtilities
    ' --------------------------------------------------------------------------------
    ' Constants
    ' --------------------------------------------------------------------------------

    ' --------------------------------------------------------------------------------
    ' Form Variables
    ' --------------------------------------------------------------------------------
    ' Access connection string (notice use of the relative path); New Microsoft database drivers compatible with windows 7 & 8
    Private f_strDatabaseConnectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;" & _
                                                      "Data Source=" & Application.StartupPath & "\..\..\Database\TeamsAndPlayers3.accdb;" & _
                                                      "User ID=Admin;" & _
                                                      "Password=;"
    '  Access connection string (notice use of the relative path); Old microsoft database drivers
    Private f_strDatabaseConnectionString2 As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                                                      "Data Source=" & Application.StartupPath & "\..\..\Database\TeamsAndPlayers3.accdb;" & _
                                                      "User ID=Admin;" & _
                                                      "Password=;"

    Private m_conAdministrator As OleDb.OleDbConnection

    ' --------------------------------------------------------------------------------
    ' OpenDatabaseConnectionMSAcess
    ' Abstract: Open a connection to the database.
    '           In a 2-Tier(Client-Server) application we connect once in FMain
    '           and hold the connection open until FMain closes
    '
    '           ***********READ ME***********
    '
    '           For MS Access  on windows vista/7/8 you must set target CPU to "x86"
    '           under "Project/Properties/Compiler/Advanced Compiler Options (button at bottom)/Target CPU"
    ' --------------------------------------------------------------------------------
    Public Function OpenDatabaseConnectionMSAcess() As Boolean

        Dim blnResult As Boolean = False

        Try

            ' Open a connection to the database
            m_conAdministrator = New OleDb.OleDbConnection
            m_conAdministrator.ConnectionString = f_strDatabaseConnectionString
            m_conAdministrator.Open()

            ' Success
            blnResult = True

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

            MessageBox.Show("Unable to connection to database!" & vbNewLine & _
                            "The Application will now close." & vbNewLine & _
                            vbNewLine & _
                            "See modDatabaseUtilities.OpenDatabaseConnection for more details", _
                            "Database Connection Error", _
                            MessageBoxButtons.OK, _
                            MessageBoxIcon.Information)

        End Try

        Return blnResult

    End Function



    ' --------------------------------------------------------------------------------
    ' Name: CloseDatabaseConnection
    ' Abstract: If the database connection is open then close it
    ' --------------------------------------------------------------------------------
    Public Function CloseDatabaseConnection() As Boolean

        Dim blnResult As Boolean = False

        Try

            ' Anything there?
            If m_conAdministrator IsNot Nothing Then

                ' Open?
                If m_conAdministrator.State <> ConnectionState.Closed Then

                    ' Yes, close it
                    m_conAdministrator.Close()

                End If

                ' Clean up
                m_conAdministrator = Nothing

            End If

            ' Success
            blnResult = True

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnResult

    End Function



    ' --------------------------------------------------------------------------------
    ' Name: LoadListBoxFromDatabase
    ' Abstract: Get all the teams from the TTeams table
    ' --------------------------------------------------------------------------------
    Public Function LoadListBoxFromDatabase(ByVal strTable As String, _
                                            ByVal strPrimaryKey As String, _
                                            ByVal strNameColumn As String, _
                                            ByRef lstTarget As ListBox, _
								   Optional	ByVal strSortColumn As String = "", _
								   Optional ByVal strCUstomSQL As String = "") As Boolean

        Dim blnResult As Boolean = False

        Try

            Dim strSelect As String = ""
            Dim cmdSelect As OleDb.OleDbCommand
            Dim drSourceTable As OleDb.OleDbDataReader
            Dim liNewItem As CListItem
            Dim intID As Integer
            Dim strName As String

            ' Show Changes all at once at end (Much easier)
            lstTarget.BeginUpdate()

            ' Clear out the list
            lstTarget.Items.Clear()

            ' Build the select statement
            strSelect = BuildSelectStatement(strTable, strPrimaryKey, _
											 strNameColumn, strSortColumn, _
											 strCustomSQL)

            ' Wrap a command object around the select statement
            cmdSelect = New OleDb.OleDbCommand(strSelect, m_conAdministrator)
			
			' Retrieve all the records
            drSourceTable = cmdSelect.ExecuteReader

			' Loop through rocrds one at a time
			' Each call to read, moves to the next record
            Do While drSourceTable.Read = True

                ' Make a list item to hold the information
                intID = drSourceTable.Item(0)       ' Primary Key
                strName = drSourceTable.Item(1)     ' Name column
                liNewItem = New CListItem(intID, strName)

                ' Add the item to the list
                lstTarget.Items.Add(liNewItem)

            Loop

            ' Select the first item in the list by default
            If lstTarget.Items.Count > 0 Then lstTarget.SelectedIndex = 0

            ' Show any changes
            lstTarget.EndUpdate()

            ' Clean up
            drSourceTable.Close()

            ' Success
            blnResult = True

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnResult

    End Function


	
    ' --------------------------------------------------------------------------------
    ' Name: BuildSelectStatement
    ' Abstract: Build the select statement
    ' --------------------------------------------------------------------------------
	Private Function BuildSelectStatement(ByVal strTable As String, _
										  ByVal strPrimaryKey As String, _
										  ByVal strNameColumn As String, _
										  ByVal strSortColumn As String, _
								 Optional ByVal strCustomSQL As String = "") As String
								 
		Dim strSelectStatement As String = ""
		
		Try
		
			' Custom Select statement?
			If strCustomSQL = "" Then
			
				' No, so build one
				
				' Sort by name column if nothing provided
				If strSortColumn = "" Then strSortColumn = strNameColumn
				
					' Put it all together
					strSelectStatement = "SELECT " & _
											strPrimaryKey & ", " & strNameColumn & _
											" FROM " & _
												strTable & _
											" ORDER BY " & strSortColumn
											
				Else
				
					' Yes, use it
					strSelectStatement = strCustomSQL
					
				End If
				
		Catch excError As Exception
		
			' Display and log error message
			WriteLog(excError)
			
		End Try
		
	End Function
	
	
	
	' ---------------------------------------------------------------------------------------------------
	' Name: LoadListWithPlayersFromDatabase
	' Abstract: Load all the players on.not on the specified team.
	' ---------------------------------------------------------------------------------------------------
	Public Function LoadListWithPlayersFromDatabase(ByVal intTeamID As Integer, _
													ByRef lstTarget As ListBox, _
													ByVal blnPlayersOnTeam As Boolean) As Boolean
													
		Dim blnResult As Boolean = False
		
		Try
		
			Dim strCustomSQL As String = ""
			Dim strNot As String = "NOT"
			
			' Selected players?
			If blnPlayersOnTeam = True Then strNot = ""
			
			' Build the custom SQL Select statement. See the chapter on subqueires in SQL Server for programmers.
			'				Load all the players that are/are not already on the team
			strCustomSQL = "SELECT " & _
							"	intPlayerID, strLastName + ', ' + strFirstName " & _
							" FROM " & _
							"	VActivePlayers " & _
							" WHERE intPlayerID " & strNot & " IN " & _
							"	( " & _
							"	  SELECT intPlayerID " & _
							"	  FROM TTeamPlayers " & _
							"	  WHERE intTeamID = " & intTeamID & _
							"	 ) " & _
							" ORDER BY " & _
							"	 strLastName, strFirstName" 
							
			blnResult = LoadListboxFromDatabase("", "", "", lstTarget, "", strCustomSQL)
			
		Catch excError As Exception
		
			' Log and display error message
			WriteLog(excError)
			
		End Try
		
		Return blnResult

	End Function



    ' --------------------------------------------------------------------------------
    ' Name: LoadComboBoxFromDatabase
    ' Abstract: Load all states from TStates
    ' --------------------------------------------------------------------------------
    Public Function LoadComboBoxFromDatabase(ByVal strTable As String,
                                            ByVal strPrimaryKey As String,
                                            ByVal strNameColumn As String,
                                            ByRef cmbTarget As ComboBox) As Boolean

        Dim blnResult As Boolean = False

        Try

            Dim strSelect As String = ""
            Dim cmdSelect As OleDb.OleDbCommand
            Dim drSourceTable As OleDb.OleDbDataReader
            Dim liNewItem As CListItem
            Dim intID As Integer
            Dim strName As String

            ' Show Changes all at once at end (Much easier)
            cmbTarget.BeginUpdate()

            ' Clear out the combobox
            cmbTarget.Items.Clear()

            ' Build the select statement
            strSelect = "SELECT " & strPrimaryKey & ", " & strNameColumn & _
                        " FROM " & strTable & _
                        " ORDER BY " & strPrimaryKey

            ' Retrieve all the records from the database
            cmdSelect = New OleDb.OleDbCommand(strSelect, m_conAdministrator)
            drSourceTable = cmdSelect.ExecuteReader

            Do While drSourceTable.Read = True

                ' Make a list item to hold the information
                intID = drSourceTable.Item(0)       ' Primary Key
                strName = drSourceTable.Item(1)     ' Name column
                liNewItem = New CListItem(intID, strName)

                ' Add the item to the list
                cmbTarget.Items.Add(liNewItem)

            Loop

            ' Select the first item in the list by default
            If cmbTarget.Items.Count > 0 Then cmbTarget.SelectedIndex = 0

            ' Show any changes
            cmbTarget.EndUpdate()

            ' Clean up
            drSourceTable.Close()

            ' Success
            blnResult = True

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnResult

    End Function



    ' --------------------------------------------------------------------------------
    ' Name: GetNextHighestRecordID
    ' Abstract: Get the next highest ID from the table in the database
    '           Danger: Potential race condition
    '           Why do we have this? SO we can see the mechanics of how
    '           everything works
    ' --------------------------------------------------------------------------------
    Public Function GetNextHighestRecordID(ByVal strPrimaryKey As String,
                                            ByVal strTable As String,
                                            ByRef intNextHighestRecordID As Integer) As Boolean

        Dim blnResult As Boolean = False

        Try

            Dim strSelect As String
            Dim cmdSelect As OleDb.OleDbCommand
            Dim drSourceTable As OleDb.OleDbDataReader


            strSelect = "SELECT MAX( " & strPrimaryKey & " ) + 1 AS intNextHighestRecordID " & _
                        " FROM " & strTable

            ' Execute the command
            cmdSelect = New OleDb.OleDbCommand(strSelect, m_conAdministrator)
            drSourceTable = cmdSelect.ExecuteReader

            ' Read the results
            drSourceTable.Read()

            ' Null? (Empty table)
            If drSourceTable.IsDBNull(0) Then

                ' Yes, start numbering at 1
                intNextHighestRecordID = 1

            Else

                ' No get the next highest ID
                intNextHighestRecordID = drSourceTable.Item(0)

            End If

            ' Clean up
            drSourceTable.Close()

            ' Success
            blnResult = True

        Catch excError As Exception

            ' Log and display exception errors
            WriteLog(excError)

        End Try

        Return blnResult

    End Function



    ' --------------------------------------------------------------------------------
    ' Name: DeleteRecordsFromTable
    ' Abstract: Delete all records from table that match the ID
    ' --------------------------------------------------------------------------------
    Private Function DeleteRecordsFromTable(ByVal intRecordID As Integer, _
                                            ByVal strPrimaryKey As String, _
                                            ByVal strTable As String) As Boolean

        Dim blnResult = False

        Try

            Dim strDelete As String
            Dim cmdDelete As OleDb.OleDbCommand
            Dim intRowsAffected As Integer = 0

            ' Build the SQL Command
            strDelete = "DELETE FROM " & strTable & _
                        " WHERE " & strPrimaryKey & " = " & intRecordID

            ' Delete the records
            cmdDelete = New OleDb.OleDbCommand(strDelete, m_conAdministrator)
            intRowsAffected = cmdDelete.ExecuteNonQuery()

            ' Did it work?
            If intRowsAffected > 0 Then

                'yes, success
                blnResult = True

            End If

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnResult

    End Function



    ' --------------------------------------------------------------------------------
    ' Name: AddTeamToDatabase
    ' Abstract: Add the team to the database
    ' --------------------------------------------------------------------------------
    Public Function AddTeamToDatabase(ByRef udtTeam As udtTeamType) As Boolean

        Dim blnResult As Boolean = False

        Try

            Dim strInsert As String
            Dim cmdInsert As OleDb.OleDbCommand

            ' Get the next Highest team ID
            ' Race condition. Need an atomic object but it is not possible in access
            If GetNextHighestRecordID("intTeamID", "TTeams", udtTeam.intTeamID) = True Then


                ' Build the insert command  never build the command with raw user input to prevent sql injections.
                strInsert = "INSERT INTO TTeams ( intTeamID, strTeam, strMascot )" & _
                            " VALUES ( ?, ?, ? )"

                ' Make the command instance
                cmdInsert = New OleDb.OleDbCommand(strInsert, m_conAdministrator)

                ' Add column values here instead of above to prevent SQL injection attacks
                With cmdInsert.Parameters
                    .AddWithValue("1", udtTeam.intTeamID)
                    .AddWithValue("2", udtTeam.strTeam)
                    .AddWithValue("3", udtTeam.strMascot)
                End With

                ' Insert the Row
                cmdInsert.ExecuteNonQuery()

                ' Success
                blnResult = True

            End If

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnResult

    End Function



    ' --------------------------------------------------------------------------------
    ' Name: GetTeamInformationFromDatabase
    ' Abstract: Get data for the specified team from the database
    ' --------------------------------------------------------------------------------
    Public Function GetTeamInformationFromDatabase(ByRef udtTeam As udtTeamType) As Boolean

        Dim blnResult As Boolean = False

        Try

            Dim strSelect As String = ""
            Dim cmdSelect As New OleDb.OleDbCommand
            Dim drTTeams As OleDb.OleDbDataReader

            ' Build the select string
            strSelect = "SELECT *" & _
                        " FROM TTeams" & _
                        " WHERE intTeamID = " & udtTeam.intTeamID

            ' Retrieve the record
            cmdSelect = New OleDb.OleDbCommand(strSelect, m_conAdministrator)
            drTTeams = cmdSelect.ExecuteReader

            ' Read (there should be 1 and only 1)
            drTTeams.Read()
            With drTTeams
                udtTeam.strTeam = .Item("strTeam")
                udtTeam.strMascot = .Item("strMascot")
            End With

            ' Clean up()
            drTTeams.Close()

            ' Success
            blnResult = True

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnResult

    End Function



    ' --------------------------------------------------------------------------------
    ' Name: EditTeamInDatabase
    ' Abstract: Get data for the specified team from the database
    ' --------------------------------------------------------------------------------
    Public Function EditTeamInDatabase(ByRef udtTeam As udtTeamType) As Boolean

        Dim blnResult As Boolean = False

        Try

            Dim strUpdate As String = ""
            Dim cmdUpdate As New OleDb.OleDbCommand
            Dim intRowsAffected As Integer = 0

            ' Build the select string
            strUpdate = "UPDATE TTeams" & _
                        " SET" & _
                        "    strTeam = ?" & _
                        "   ,strMascot = ?" & _
                        " WHERE" & _
                        "   intTeamID = ?"

            ' Make the command word instance
            cmdUpdate = New OleDb.OleDbCommand(strUpdate, m_conAdministrator)

            ' Add columns values here instead above to prevent sql injection attacks
            With cmdUpdate.Parameters
                .AddWithValue("1", udtTeam.strTeam)
                .AddWithValue("2", udtTeam.strMascot)
                .AddWithValue("3", udtTeam.intTeamID)
            End With

            ' Insert the row
            intRowsAffected = cmdUpdate.ExecuteNonQuery

            ' Success
            If intRowsAffected = 1 Then blnResult = True

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnResult

    End Function



    ' --------------------------------------------------------------------------------
    ' Name: DeleteTeamFromDatabase
    ' Abstract: Delete team from database
    ' --------------------------------------------------------------------------------
    Public Function DeleteTeamFromDatabase(ByVal intTeamID As Integer) As Boolean

        Dim blnResult As Boolean = False

        Try

            blnResult = DeleteRecordsFromTable(intTeamID, "intTeamID", "TTeams")

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnResult

    End Function



    ' --------------------------------------------------------------------------------
    ' Name: AddPlayerToDatabase
    ' Abstract: Add the player to the database
    ' --------------------------------------------------------------------------------
    Public Function AddPlayerToDatabase(ByRef udtPlayer As udtPlayerType) As Boolean

        Dim blnResult As Boolean = False

        Try

            Dim strInsert As String
            Dim cmdInsert As OleDb.OleDbCommand

            ' Get the next Highest team ID
            ' Race condition. Need an atomic object but it is not possible in access
            If GetNextHighestRecordID("intPlayerID", "TPlayers", udtPlayer.intPlayerID) = True Then


                ' Build the insert command  never build the command with raw user input to prevent sql injections.
                strInsert = "INSERT INTO TPlayers (      intPlayerID" & _
                                                    "   ,strFirstName" & _
                                                    "   ,strMiddleName" & _
                                                    "   ,strLastName" & _
                                                    "   ,strStreetAddress" & _
                                                    "   ,strCity" & _
                                                    "   ,intStateID" & _
                                                    "   ,strZipCode" & _
                                                    "   ,strHomePhoneNumber" & _
                                                    "   ,curSalary" & _
                                                    "   ,dtmDateOfBirth" & _
                                                    "   ,intSexID" & _
                                                    "   ,blnMostValuablePlayer" & _
                                                    "   ,strEmailAddress )" & _
                                                    " VALUES ( ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ? )"          ' You dumbass parentheses on mvp

                ' Make the command instance
                cmdInsert = New OleDb.OleDbCommand(strInsert, m_conAdministrator)

                ' Add column values here instead of above to prevent SQL injection attacks
                With cmdInsert.Parameters
                    .AddWithValue("1", udtPlayer.intPlayerID)
                    .AddWithValue("2", udtPlayer.strFirstName)
                    .AddWithValue("3", udtPlayer.strMiddleName)
                    .AddWithValue("4", udtPlayer.strLastName)
                    .AddWithValue("5", udtPlayer.strStreetAddress)
                    .AddWithValue("6", udtPlayer.strCity)
                    .AddWithValue("7", udtPlayer.intStateID)
                    .AddWithValue("8", udtPlayer.strZipCode)
                    .AddWithValue("9", udtPlayer.strHomePhoneNumber)
                    .AddWithValue("10", udtPlayer.decSalary)
                    .AddWithValue("11", udtPlayer.dtmDateOfBirth)
                    .AddWithValue("12", udtPlayer.intSexID)
                    .AddWithValue("13", udtPlayer.blnMostValuablePlayer)
                    .AddWithValue("14", udtPlayer.strEmailAddress)
                End With

                ' Insert the Row
                cmdInsert.ExecuteNonQuery()

                ' Success
                blnResult = True

            End If

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnResult

    End Function




    ' --------------------------------------------------------------------------------
    ' Name: GetPlayerInformationFromDatabase
    ' Abstract: Get data for the specified team from the database
    ' --------------------------------------------------------------------------------
    Public Function GetPlayerInformationFromDatabase(ByRef udtPlayer As udtPlayerType) As Boolean

        Dim blnResult As Boolean = False

        Try

            Dim strSelect As String = ""
            Dim cmdSelect As New OleDb.OleDbCommand
            Dim drTPlayers As OleDb.OleDbDataReader

            ' Build the select string
            strSelect = "SELECT *" & _
                        " FROM TPlayers" & _
                        " WHERE intPlayerID = " & udtPlayer.intPlayerID

            ' Retrieve the record
            cmdSelect = New OleDb.OleDbCommand(strSelect, m_conAdministrator)
            drTPlayers = cmdSelect.ExecuteReader

            ' Read (there should be 1 and only 1)
            drTPlayers.Read()
            With drTPlayers
                udtPlayer.strFirstName = .Item("strFirstName")
                udtPlayer.strMiddleName = .Item("strMiddleName")
                udtPlayer.strLastName = .Item("strLastName")
                udtPlayer.strStreetAddress = .Item("strStreetAddress")
                udtPlayer.strCity = .Item("strCity")
                udtPlayer.intStateID = .Item("intStateID")
                udtPlayer.strZipCode = .Item("strZipCode")
                udtPlayer.strHomePhoneNumber = .Item("strHomePhoneNumber")
                udtPlayer.decSalary = .Item("curSalary")
                udtPlayer.dtmDateOfBirth = .Item("dtmDateOfBirth")
                udtPlayer.intSexID = .Item("intSexID")
                udtPlayer.blnMostValuablePlayer = .Item("blnMostValuablePlayer")
                udtPlayer.strEmailAddress = .Item("strEmailAddress")
            End With

            ' Clean up()
            drTPlayers.Close()

            ' Success
            blnResult = True

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnResult

    End Function



    ' --------------------------------------------------------------------------------
    ' Name: EditPlayerInDatabase
    ' Abstract: Get data for the specified player from the database
    ' --------------------------------------------------------------------------------
    Public Function EditPlayerInDatabase(ByRef udtPlayer As udtPlayerType) As Boolean

        Dim blnResult As Boolean = False

        Try

            Dim strUpdate As String = ""
            Dim cmdUpdate As New OleDb.OleDbCommand
            Dim intRowsAffected As Integer = 0

            ' Build the select string
            strUpdate = "UPDATE TPlayers" & _
                        " SET" & _
                        "    strFirstName = ?" & _
                        "   ,strMiddleName = ?" & _
                        "   ,strLastName = ?" & _
                        "   ,strStreetAddress = ?" & _
                        "   ,strCity = ?" & _
                        "   ,intStateID = ?" & _
                        "   ,strZipCode = ?" & _
                        "   ,strHomePhoneNumber = ?" & _
                        "   ,curSalary = ?" & _
                        "   ,dtmDateOfBirth = ?" & _
                        "   ,intSexID = ?" & _
                        "   ,blnMostValuablePlayer = ?" & _
                        "   ,strEmailAddress = ?" & _
                        " WHERE" & _
                        "    intPlayerID = ?"

            ' Make the command word instance
            cmdUpdate = New OleDb.OleDbCommand(strUpdate, m_conAdministrator)

            ' Add columns values here instead above to prevent sql injection attacks
            With cmdUpdate.Parameters

                .AddWithValue("1", udtPlayer.strFirstName)
                .AddWithValue("2", udtPlayer.strMiddleName)
                .AddWithValue("3", udtPlayer.strLastName)
                .AddWithValue("4", udtPlayer.strStreetAddress)
                .AddWithValue("5", udtPlayer.strCity)
                .AddWithValue("6", udtPlayer.intStateID)
                .AddWithValue("7", udtPlayer.strZipCode)
                .AddWithValue("8", udtPlayer.strHomePhoneNumber)
                .AddWithValue("9", udtPlayer.decSalary)
                .AddWithValue("10", udtPlayer.dtmDateOfBirth)
                .AddWithValue("11", udtPlayer.intSexID)
                .AddWithValue("12", udtPlayer.blnMostValuablePlayer)
                .AddWithValue("13", udtPlayer.strEmailAddress)
                .AddWithValue("14", udtPlayer.intPlayerID)

            End With

            ' Insert the row
            intRowsAffected = cmdUpdate.ExecuteNonQuery

            ' Success
            If intRowsAffected = 1 Then blnResult = True

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnResult

    End Function



    ' --------------------------------------------------------------------------------
    ' Name: DeletePlayerFromDatabase
    ' Abstract: Delete team from database
    ' --------------------------------------------------------------------------------
    Public Function DeletePlayerFromDatabase(ByVal intPlayerID As Integer) As Boolean

        Dim blnResult As Boolean = False

        Try

            blnResult = DeleteRecordsFromTable(intPlayerID, "intPlayerID", "TPlayers")

        Catch excError As Exception

            ' Log and display error message
            WriteLog(excError)

        End Try

        Return blnResult

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

End Module
