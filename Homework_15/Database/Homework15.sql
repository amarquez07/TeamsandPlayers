-- --------------------------------------------------------------------------------
--	Name: Anthony Marquez
--	Class: IT-102 VB.Net #2
--	Abstract: 
-- --------------------------------------------------------------------------------

-- --------------------------------------------------------------------------------
-- Options
-- --------------------------------------------------------------------------------
USE dbTeamsAndPlayers		-- moves user out of master database
SET NOCOUNT ON				-- Reports errors only


-- --------------------------------------------------------------------------------
--	Step #0: Drop statements
-- --------------------------------------------------------------------------------
-- IF OBJECT_ID('') IS NOT NULL DROP TABLE
-- IF OBJECT_ID('') IS NOT NULL DROP PROCEDURE

-- Tables
IF OBJECT_ID('TTeamPlayers')						IS NOT NULL DROP TABLE TTeamPlayers
IF OBJECT_ID('TTeams')							IS NOT NULL DROP TABLE TTeams
IF OBJECT_ID('TPlayers')						IS NOT NULL DROP TABLE TPlayers
IF OBJECT_ID('TTeamStatuses')						IS NOT NULL DROP TABLE TTeamStatuses
IF OBJECT_ID('TPlayerStatuses')						IS NOT NULL DROP TABLE TPlayerStatuses
IF OBJECT_ID('TStates')							IS NOT NULL DROP TABLE TStates
IF OBJECT_ID('TSexes')							IS NOT NULL DROP TABLE TSexes

IF OBJECT_ID('VActivePlayers')						IS NOT NULL DROP VIEW VActivePlayers
IF OBJECT_ID('VInactivePlayers')					IS NOT NULL DROP VIEW VInactivePlayers
IF OBJECT_ID('VActiveTeams')						IS NOT NULL DROP VIEW VActiveTeams
IF OBJECT_ID('VInactiveTeams')						IS NOT NULL DROP VIEW VInactiveTeams

-- Stored procedures

-- Players
IF OBJECT_ID('uspAddPlayer')						IS NOT NULL DROP PROCEDURE uspAddPlayer
IF OBJECT_ID('uspEditPlayer')						IS NOT NULL DROP PROCEDURE uspEditPlayer
IF OBJECT_ID('uspSetPlayerstatus')					IS NOT NULL DROP PROCEDURE uspSetPlayerstatus
-- Teams
IF OBJECT_ID('uspAddTeam')						IS NOT NULL DROP PROCEDURE uspAddTeam
IF OBJECT_ID('uspEditTeam')						IS NOT NULL DROP PROCEDURE uspEditTeam
IF OBJECT_ID('uspSetTeamStatus')					IS NOT NULL DROP PROCEDURE uspSetTeamStatus
-- Team players
IF OBJECT_ID('uspAddTeamPlayer')					IS NOT NULL DROP PROCEDURE uspAddTeamPlayer
IF OBJECT_ID('uspRemoveTeamPlayer')					IS NOT NULL DROP PROCEDURE uspRemoveTeamPlayer
IF OBJECT_ID('uspSetTeamStatus')					IS NOT NULL DROP PROCEDURE uspSetTeamStatus
	

-- --------------------------------------------------------------------------------
--	Step #1: Create Tables
-- --------------------------------------------------------------------------------

CREATE TABLE TTeams
(
	 intTeamID							INTEGER								NOT NULL
	,strTeam							VARCHAR(50)							NOT NULL
	,strMascot							VARCHAR(50)							NOT NULL
	,intTeamStatusID						INTEGER								NOT NULL
	,CONSTRAINT TTeams_PK PRIMARY KEY ( intTeamID )
)

CREATE TABLE TTeamPlayers
(
	 intTeamID							INTEGER								NOT NULL
	,intPlayerID							INTEGER								NOT NULL
	,CONSTRAINT TTeamPlayers_PK PRIMARY KEY ( intTeamID, intPlayerID )
)

CREATE TABLE TPlayers
(
	 intPlayerID							INTEGER								NOT NULL
	,strFirstName							VARCHAR(50)							NOT NULL
	,strMiddleName							VARCHAR(50)							NOT NULL
	,strLastName							VARCHAR(50)							NOT NULL
	,strStreetAddress						VARCHAR(50)							NOT NULL
	,strCity							VARCHAR(50)							NOT NULL
	,intStateID							INTEGER								NOT NULL
	,strZipcode							VARCHAR(50)							NOT NULL
	,strHomePhoneNumber						VARCHAR(50)							NOT NULL
	,curSalary							MONEY								NOT NULL
	,dteDateOfBirth							DATE								NOT NULL
	,intSexID							INTEGER								NOT NULL
	,blnMostValuablePlayer						BIT									NOT NULL
	,strEmailAddress						VARCHAR(50)							NOT NULL
	,intPlayerStatusID						INTEGER								NOT NULL
	,CONSTRAINT TPlayers_PK PRIMARY KEY ( intPlayerID )
)

CREATE TABLE TPlayerStatuses
(
	 intPlayerStatusID						INTEGER								NOT NULL
	,strPlayerStatus						VARCHAR(50)							NOT NULL
	,CONSTRAINT TPlayerStatuses_PK PRIMARY KEY ( intPlayerStatusID )
)

CREATE TABLE TTeamStatuses
(
	 intTeamStatusID						INTEGER								NOT NULL
	,strTeamStatus							VARCHAR(50)							NOT NULL
	,CONSTRAINT TTeamStatuses_PK PRIMARY KEY ( intTeamStatusID )
)

CREATE TABLE TStates
(
	 intStateID							INTEGER								NOT NULL
	,strState							VARCHAR(50)							NOT NULL
	,strStateAbbreviations						VARCHAR(50)							NOT NULL
	,CONSTRAINT TStates_PK PRIMARY KEY ( intStateID )
)

CREATE TABLE TSexes
(
	 intSexID							INTEGER								NOT NULL
	,strSex								VARCHAR(50)							NOT NULL
	,CONSTRAINT TSexes_PK PRIMARY KEY ( intSexID )
)


-- --------------------------------------------------------------------------------
--	Step #2: Identify and create foreign keys
-- --------------------------------------------------------------------------------
--	#	Child			Parent			Column(s)
--	-	-----			------			---------
--	1	TTeams			TTeamStatuses		intTeamStatusID
--	2	TPlayers		TPlayerStatuses		intPlayerStatusID
--	3	TPlayers		TStates			intStateID
--	4	TPlayers		TSexes			intSexID
--	5	TTeamPlayers		TTeams			intTeamID
--	6	TTeamPlayers		TPlayers		intPlayerID


--	#1
ALTER TABLE TTeams ADD CONSTRAINT TTeams_TTeamStatuses_FK
FOREIGN KEY ( intTeamStatusID ) REFERENCES TTeamStatuses( intTeamStatusID )

--	#2
ALTER TABLE TPlayers ADD CONSTRAINT TPlayers_TPlayerStatuses_FK
FOREIGN KEY ( intPlayerStatusID ) REFERENCES TPlayerStatuses( intPlayerStatusID )

--	#3
ALTER TABLE TPlayers ADD CONSTRAINT TPlayers_TStates_FK
FOREIGN KEY ( intStateID ) REFERENCES TStates( intStateID )

--	#4
ALTER TABLE TPlayers ADD CONSTRAINT TPlayers_TSexes_FK
FOREIGN KEY ( intSexID ) REFERENCES TSexes( intSexID )

--	#5
ALTER TABLE TTeamPlayers ADD CONSTRAINT TTeamPlayers_TTeams_FK
FOREIGN KEY ( intTeamID ) REFERENCES TTeams( intTeamID )

--	#6
ALTER TABLE TTeamPlayers ADD CONSTRAINT TTeamPlayers_TPlayers_FK
FOREIGN KEY ( intPlayerID ) REFERENCES TPlayers( intPlayerID )


-- --------------------------------------------------------------------------------
--	Step #3: Views
-- --------------------------------------------------------------------------------

-- --------------------------------------------------------------------------------
-- Name: VActivePlayers
-- Abstract: Show active players only
-- --------------------------------------------------------------------------------
GO
CREATE VIEW VActivePlayers
AS
SELECT
	 TP.intPlayerID
	,TP.strFirstName
	,TP.strLastName
	,TP.strLastName + ', ' + TP.strFirstName AS strPlayer
FROM
	TPlayers		AS TP

WHERE
	intPlayerStatusID = 1

GO


-- --------------------------------------------------------------------------------
-- Name: VInactivePlayers
-- Abstract: Show inactive players only
-- --------------------------------------------------------------------------------
GO
CREATE VIEW VInactivePlayers
AS
SELECT
	 TP.intPlayerID
	,TP.strFirstName
	,TP.strLastName
	,TP.strLastName + ', ' + TP.strFirstName AS strPlayer
FROM
	TPlayers		AS TP

WHERE
	intPlayerStatusID = 2
GO

-- --------------------------------------------------------------------------------
-- Name: VActiveTeams
-- Abstract: Show active players only
-- --------------------------------------------------------------------------------
GO
CREATE VIEW VActiveTeams
AS
SELECT
	 TT.intTeamID
	,TT.strTeam
FROM
	TTeams		AS TT

WHERE
	intTeamStatusID = 1

GO


-- --------------------------------------------------------------------------------
-- Name: VInactiveTeams
-- Abstract: Show active players only
-- --------------------------------------------------------------------------------
GO
CREATE VIEW VInactiveTeams
AS
SELECT
	 TT.intTeamID
	,TT.strTeam
FROM
	TTeams		AS TT

WHERE
	intTeamStatusID = 2

GO


-- --------------------------------------------------------------------------------
--	Step #4: Stored procedures
-- --------------------------------------------------------------------------------

-- --------------------------------------------------------------------------------
--	Name: uspAddPlayer
--	Abstract: Stored procedure to add a player to database
-- --------------------------------------------------------------------------------
GO
CREATE PROCEDURE uspAddPlayer
	  @strFirstName					VARCHAR(50)			
	 ,@strMiddleName				VARCHAR(50)
	 ,@strLastName					VARCHAR(50)
	 ,@strStreetAddress				VARCHAR(50)
	 ,@strCity					VARCHAR(50)
	 ,@intStateID					INTEGER
	 ,@strZipcode					VARCHAR(50)
	 ,@strHomePhoneNumber				VARCHAR(50)
	 ,@curSalary					MONEY
	 ,@dteDateOfBirth				DATE
	 ,@intSexID					INTEGER
	 ,@blnMostValuablePlayer			BIT
	 ,@strEmailAddress				VARCHAR(50)

AS
SET NOCOUNT ON			-- Report errors only
SET XACT_ABORT ON		-- Rollback transaction on error

BEGIN TRANSACTION

	DECLARE @intPlayerID			INTEGER

	-- Get the next highest ID and lock the table until the end of the transaction
	SELECT @intPlayerID = MAX( intPlayerID ) + 1 FROM TPlayers ( TABLOCKX )

	-- Default to 1 if the table is empty
	SELECT @intPlayerID = COALESCE( @intPlayerID, 1 )
	
	-- Create new record
	INSERT INTO TPlayers ( intPlayerID, strFirstName, strMiddleName, strLastName, strStreetAddress, strCity, intStateID, strZipcode, strHomePhoneNumber, curSalary, dteDateOfBirth, intSexID, blnMostValuablePlayer, strEmailAddress, intPlayerStatusID )
	VALUES( @intPlayerID, @strFirstName, @strMiddleName, @strLastName, @strStreetAddress, @strCity, @intStateID, @strZipcode, @strHomePhoneNumber, @curSalary, @dteDateOfBirth, @intSexID, @blnMostValuablePlayer, @strEmailAddress , 1 ) -- 1 = Active

	-- Return ID Calling to program
	SELECT @intPlayerID AS intPlayerID

COMMIT TRANSACTION
GO

-- --------------------------------------------------------------------------------
--	Name: uspEditPlayer
--	Abstract: Stored procedure to edit player in the database
-- --------------------------------------------------------------------------------
GO
CREATE PROCEDURE uspEditPlayer
	 @intPlayerID					INTEGER
	,@strFirstName					VARCHAR(50)			
	,@strMiddleName					VARCHAR(50)
	,@strLastName					VARCHAR(50)
	,@strStreetAddress				VARCHAR(50)
	,@strCity					VARCHAR(50)
	,@intStateID					INTEGER
	,@strZipcode					VARCHAR(50)
	,@strHomePhoneNumber				VARCHAR(50)
	,@curSalary					MONEY
	,@dteDateOfBirth				DATE
	,@intSexID					INTEGER
	,@blnMostValuablePlayer				BIT
	,@strEmailAddress				VARCHAR(50)
AS
SET NOCOUNT ON			-- Report errors only
SET XACT_ABORT ON		-- Rollback transaction on error

BEGIN TRANSACTION

	-- Update the records
	UPDATE
		TPlayers
	SET
		 strFirstName			= @strFirstName
		,strMiddleName			= @strMiddleName
		,strLastName			= @strLastName
		,strStreetAddress		= @strStreetAddress
		,strCity			= @strCity
		,intStateID			= @intStateID
		,strZipcode			= @strZipcode
		,strHomePhoneNumber		= @strHomePhoneNumber	
		,curSalary			= @curSalary				
		,dteDateOfBirth			= @dteDateOfBirth		
		,intSexID			= @intSexID				
		,blnMostValuablePlayer		= @blnMostValuablePlayer	
		,strEmailAddress		= @strEmailAddress		
	WHERE
		intPlayerID			= @intPlayerID
COMMIT TRANSACTION
GO

-- --------------------------------------------------------------------------------
--	Name: uspSetPlayerstatus
--	Abstract: Stored procedure to edit player in the database
-- --------------------------------------------------------------------------------
GO
CREATE PROCEDURE uspSetPlayerstatus
	 @intPlayerID				INTEGER
	,@intPlayerstatusID			INTEGER
AS
SET NOCOUNT ON			-- Report errors only
SET XACT_ABORT ON		-- Rollback transaction on error

BEGIN TRANSACTION

	-- Update the records
	UPDATE
		TPlayers
	SET
		intPlayerstatusID = @intPlayerstatusID
	WHERE
		intPlayerID = @intPlayerID
COMMIT TRANSACTION
GO

-- --------------------------------------------------------------------------------
--	Name: uspAddTeam
--	Abstract: Stored procedure to add a team to database
-- --------------------------------------------------------------------------------
GO
CREATE PROCEDURE uspAddTeam
	 @strTeam				AS	VARCHAR(50)
	,@strMascot				AS	VARCHAR(50)
AS
SET NOCOUNT ON							-- Report errors only
SET XACT_ABORT ON						-- Rollback transaction on error

BEGIN TRANSACTION

	DECLARE @intTeamID		AS	INTEGER

	-- Get the next highest ID and lock the table until the end of the transaction
	SELECT @intTeamID = MAX( intTeamID ) + 1 FROM TTeams ( TABLOCKX )

	-- Default to 1 if the table is empty
	SELECT @intTeamID = COALESCE( @intTeamID, 1 )
	
	-- Create new record
	INSERT INTO TTeams ( intTeamID, strTeam, strMascot, intTeamStatusID )
	VALUES( @intTeamID, @strTeam, @strMascot, 1 )			-- 1 = Active

	-- Return ID Calling to program
	SELECT @intTeamID AS intTeamID

COMMIT TRANSACTION
GO

-- --------------------------------------------------------------------------------
--	Name: uspEditTeam
--	Abstract: Stored procedure to edit team in the database
-- --------------------------------------------------------------------------------
GO
CREATE PROCEDURE uspEditTeam
	 @intTeamID					INTEGER
	,@strTeam					VARCHAR(50)
	,@strMascot					VARCHAR(50)
AS
SET NOCOUNT ON			-- Report errors only
SET XACT_ABORT ON		-- Rollback transaction on error

BEGIN TRANSACTION

	-- Update the records
	UPDATE
		TTeams
	SET
		 strTeam	= @strTeam
		,strMascot	= @strMascot
	WHERE
		intTeamID	= @intTeamID

COMMIT TRANSACTION
GO


-- --------------------------------------------------------------------------------
--	Name: uspSetTeamStatus
--	Abstract: Stored procedure to edit team in the database
-- --------------------------------------------------------------------------------
GO
CREATE PROCEDURE uspSetTeamStatus
	 @intTeamID					INTEGER
	,@intTeamStatusID			INTEGER
AS
SET NOCOUNT ON			-- Report errors only
SET XACT_ABORT ON		-- Rollback transaction on error

BEGIN TRANSACTION

	-- Update the records
	UPDATE
		TTeams
	SET
		intTeamStatusID = @intTeamStatusID
	WHERE
		intTeamID	= @intTeamID

COMMIT TRANSACTION
GO

-- --------------------------------------------------------------------------------
--	Name: uspAddTeamPlayer
--	Abstract: Stored procedure to add a team to database
-- --------------------------------------------------------------------------------
GO
CREATE PROCEDURE uspAddTeamPlayer
	 @intTeamID				INTEGER
	,@intPlayerID				INTEGER
AS
SET NOCOUNT ON			-- Report errors only
SET XACT_ABORT ON		-- Rollback transaction on error

BEGIN TRANSACTION
	
	DECLARE @blnAlreadyExists AS BIT = 0

	-- Check to see if it already exists
	SELECT @blnAlreadyExists = 1 FROM TTeamPlayers (TABLOCKX)
	WHERE
			intTeamID	= @intTeamID
		AND	intPlayerID	= @intPlayerID

	-- Does it exist?
	IF @blnAlreadyExists = 0 
	BEGIN

		-- No, so add it
		INSERT INTO TTeamPlayers( intTeamID, intPlayerID )
		VALUES( @intTeamID, @intPlayerID )

	END

COMMIT TRANSACTION
GO


-- --------------------------------------------------------------------------------
--	Name: uspRemoveTeamPlayer
--	Abstract: Stored procedure to remove team in the database
-- --------------------------------------------------------------------------------
GO
CREATE PROCEDURE uspRemoveTeamPlayer
	 @intTeamID				INTEGER
	,@intPlayerID				INTEGER
AS
SET NOCOUNT ON			-- Report errors only
SET XACT_ABORT ON		-- Rollback transaction on error

BEGIN TRANSACTION

	-- Delete the record
	DELETE FROM
		TTeamPlayers
	WHERE
		intTeamID	= @intTeamID
	AND 	intPlayerID	= @intPlayerID

COMMIT TRANSACTION
GO



-- --------------------------------------------------------------------------------
--	Step #5: Insert Sample data
-- --------------------------------------------------------------------------------
INSERT INTO TTeamStatuses ( intTeamStatusID, strTeamStatus )
VALUES	 ( 1, 'Active')
	,( 2, 'Inactive')


INSERT INTO TPlayerStatuses ( intPlayerStatusID, strPlayerStatus )
VALUES	 ( 1, 'Active')
	,( 2, 'Inactive')


INSERT INTO TSexes ( intSexID, strSex )
VALUES	 ( 1, 'Male' )
	,( 2, 'Female' )


INSERT INTO TTeams ( intTeamID, strTeam, strMascot, intTeamStatusID )
VALUES	 ( 1, 'The East blue pirates', '', 1 )
	,( 2, 'Grand Line Sluggers', '', 2 )
	,( 3, 'New World Bullies', '', 1 )
	,( 4, 'Raftel Big Dogs', '', 1 )


INSERT INTO TStates ( intStateID, strState, strStateAbbreviations )
VALUES	 ( 1, 'Alabama', 'AL' )
	,( 2, 'Alaska', 'AK' )
	,( 3, 'Arizona', 'AZ' )
	,( 4, 'Arkansas', 'AR' )
	,( 5, 'California', 'CA' )
	,( 6, 'Colorado', 'CO' )
	,( 7, 'Connecticut', 'CT' )
	,( 8, 'Delaware', 'DE' )
	,( 9, 'Florida', 'FL' )
	,( 10, 'Georgia', 'GA' )
	,( 11, 'Hawaii', 'HI' )
	,( 12, 'Idaho', 'ID' )
	,( 13, 'Illinois', 'IL' )
	,( 14, 'Indiana', 'IN' )
	,( 15, 'Iowa', 'IA' )
	,( 16, 'Kansas', 'KS' )
	,( 17, 'Kentucky', 'KY' )
	,( 18, 'Louisiana', 'LA' )
	,( 19, 'Maine', 'ME' )
	,( 20, 'Maryland', 'MD' )
	,( 21, 'Massachusetts', 'MA' )
	,( 22, 'Michigan', 'MI' )
	,( 23, 'Minnesota', 'MN' )
	,( 24, 'Mississippi', 'MS' )
	,( 25, 'Missouri', 'MO' )
	,( 26, 'Montana', 'MT' )
	,( 27, 'Nebraska', 'NE' )
	,( 28, 'Nevada', 'NV' )
	,( 29, 'New Hampshire', 'NH' )
	,( 30, 'New Jersey', 'NJ' )
	,( 31, 'New Mexico', 'NM' )
	,( 32, 'New York', 'NY' )
	,( 33, 'North Carolina', 'NC' )
	,( 34, 'North Dakota', 'ND' )
	,( 35, 'Ohio', 'OH' )
	,( 36, 'Oklahoma', 'OK' )
	,( 37, 'Oregon', 'OR' )
	,( 38, 'Pennsylvania', 'PA' )
	,( 39, 'Rhode Island', 'RI' )
	,( 40, 'South Carolina', 'SC' )
	,( 41, 'South Dakota', 'SD' )
	,( 42, 'Tennessee', 'TN' )
	,( 43, 'Texas', 'TX' )
	,( 44, 'Utah', 'UT' )
	,( 45, 'Vermont', 'VT' )
	,( 46, 'Virginia', 'VA' )
	,( 47, 'Washington', 'WA' )
	,( 48, 'West Virginia', 'WV' )
	,( 49, 'Wisconsin', 'WI' )
	,( 50, 'Wyoming', 'WY' )
	,( 51, 'American Samoa', 'AS' )
	,( 52, 'District of Columbia', 'DC' )
	,( 53, 'Federated States of Micronesia', 'FM' )
	,( 54, 'Guam', 'GU' )
	,( 55, 'Marshall Islands', 'MH' )
	,( 56, 'Northern Mariana Islands', 'MP' )
	,( 57, 'Palau', 'PW' )
	,( 58, 'Puerto Rico', 'PR' )
	,( 59, 'Virgin Islands', 'VI' )

-- Need to find a way to make this WAAAAY better looking
INSERT INTO TPlayers ( intPlayerID, strFirstName, strMiddleName, strLastName, strStreetAddress, strCity, intStateID, strZipcode				
			,strHomePhoneNumber, curSalary, dteDateOfBirth, intSexID, blnMostValuablePlayer, strEmailAddress, intPlayerStatusID )
VALUES	 ( 1, 'Fiann', '', 'Erba', '', 'Carol Stream', 4, '60188', '', 20377, '1965/05/25', 1, 0, '', 1 )
	,( 2, 'Cullie', '', 'Scepan', '', 'Redford', 49, '48239', '', 1595, '1988/02/29', 1, 0, '', 1 )
	,( 3, 'Darlleen', '', 'Boyd', '', 'Durham', 16, '27703', '', 34774, '1974/05/17', 2, 1, '', 1 )
	,( 4, 'Washington', '', 'Belser', '', 'Nanuet', 24, '10954', '', 47804, '2008/02/22', 2, 1, '', 1 )
	,( 5, 'Gayel', '', 'Bonadies', '', 'Chicago', 10, '60621', '', 8858, '2011/02/23', 2, 1, '', 1 )
	,( 6, 'Aylmer', '', 'Muello', '', 'Southington', 20, '6489', '', 28524, '2001/09/14', 2, 0, '', 1 )
--		,( 7, 'Alla', '', 'Massam', '', 'Columbus', 1, '31904', '', 57322, '1999/10/19', 2, 0, '', 1 )
--		,( 8, 'Matthus', '', 'Axelrod', '', 'Sunnyside', 55, '11104', '', 1020, '1966/04/21', 2, 0, '', 1 )
--		,( 9, 'Farrand', '', 'Ghemawat', '', 'Birmingham', 43, '35209', '', 74828, '1999/08/19', 1, 0, '', 1 )
--		,( 10, 'Fran', '', 'Cipriano', '', 'Fair Lawn', 40, '7410', '', 48060, '1968/03/23', 2, 0, '', 1 )
--		,( 11, 'Brandy', '', 'Scippacercola', '', 'Casselberry', 6, '32707', '', 76774, '2003/01/07', 1, 0, '', 1 )
--		,( 12, 'Dylan', '', 'Balaban', '', 'Nazareth', 59, '18064', '', 23762, '1981/12/07', 1, 1, '', 1 )
--		,( 13, 'Latrena', '', 'Takemi', '', 'Saint Johns', 54, '32259', '', 77416, '1973/11/10', 2, 0, '', 1 )
--		,( 14, 'Verene', '', 'Shields', '', 'Enfield', 45, '6082', '', 22367, '1996/03/31', 2, 1, '', 1 )
--		,( 15, 'Mathew', '', 'Sturm', '', 'Media', 8, '19063', '', 51245, '1963/09/03', 2, 0, '', 1 )
--		,( 16, 'Hamel', '', 'Cuno', '', 'Chatsworth', 58, '30705', '', 31567, '1970/07/24', 2, 1, '', 1 )
--		,( 17, 'Angelique', '', 'Berliner', '', 'Alliance', 16, '44601', '', 26653, '2000/12/30', 1, 1, '', 1 )
--		,( 18, 'Webster', '', 'Marubini', '', 'Norristown', 59, '19401', '', 79645, '1983/04/24', 2, 0, '', 1 )
--		,( 19, 'Kinna', '', 'Pudney', '', 'Ithaca', 3, '14850', '', 23069, '2007/02/06', 1, 1, '', 1 )
--		,( 20, 'Bradford', '', 'Orszak', '', 'Wasilla', 27, '99654', '', 55994, '1993/01/25', 1, 1, '', 1 )
--		,( 21, 'Dionis', '', 'Thivierge', '', 'New Milford', 19, '6776', '', 14655, '1989/07/24', 2, 1, '', 1 )
--		,( 22, 'Garv', '', 'Losick', '', 'Bay City', 8, '48706', '', 50326, '1987/07/21', 1, 1, '', 1 )
--		,( 23, 'Frieda', '', 'High', '', 'Ozone Park', 5, '11417', '', 73050, '1969/11/04', 1, 0, '', 1 )
--		,( 24, 'Logan', '', 'Leslie', '', 'Sun Prairie', 33, '53590', '', 76162, '2014/09/22', 1, 1, '', 1 )
--		,( 25, 'Dionne', '', 'Geman', '', 'Menomonee Falls', 54, '53051', '', 20565, '1961/08/24', 1, 0, '', 1 )
--		,( 26, 'Gasparo', '', 'Boyd', '', 'New Albany', 32, '47150', '', 23999, '2007/12/13', 2, 0, '', 1 )
--		,( 27, 'Addi', '', 'Ogden', '', 'Saint Paul', 24, '55104', '', 33248, '1972/03/17', 2, 1, '', 1 )
--		,( 28, 'Benson', '', 'Massam', '', 'Mentor', 30, '44060', '', 49975, '2005/07/17', 2, 0, '', 1 )
--		,( 29, 'Marijo', '', 'Kimmerly', '', 'Evansville', 31, '47711', '', 4759, '1953/08/24', 2, 0, '', 1 )
--		,( 30, 'Whit', '', 'Scippacercola', '', 'Fleming Island', 23, '32003', '', 46940, '2003/04/04', 1, 1, '', 1 )
--		,( 31, 'Valma', '', 'Jagers', '', 'Monsey', 43, '10952', '', 67484, '1985/11/24', 1, 0, '', 1 )
--		,( 32, 'Lemuel', '', 'Shields', '', 'Ft Mitchell', 46, '41017', '', 56714, '1975/07/22', 1, 1, '', 1 )
--		,( 33, 'Joni', '', 'Berlioz', '', 'Mason', 2, '45040', '', 5304, '1990/06/14', 1, 1, '', 1 )
--		,( 34, 'Ezri', '', 'Pudney', '', 'Mcdonough', 18, '30252', '', 41159, '1965/08/06', 1, 1, '', 1 )
--		,( 35, 'Joelly', '', 'Osher', '', 'Floral Park', 17, '11001', '', 33967, '1979/05/29', 1, 1, '', 1 )
--		,( 36, 'Tyson', '', 'High', '', 'Lithonia', 15, '30038', '', 72068, '1993/07/28', 2, 0, '', 1 )
--		,( 37, 'Jan', '', 'Lampros', '', 'Franklin', 39, '2038', '', 21546, '1954/09/14', 1, 1, '', 1 )
--		,( 38, 'Sayres', '', 'Ogden', '', 'Melrose', 3, '2176', '', 65149, '2009/02/06', 1, 1, '', 1 )
--		,( 39, 'Ailina', '', 'Campedelli', '', 'Charlotte', 18, '28205', '', 33867, '1956/01/03', 2, 0, '', 1 )
--		,( 40, 'Marion', '', 'Jagers', '', 'Long Branch', 2, '7740', '', 76401, '2006/08/25', 2, 1, '', 1 )
--		,( 41, 'Blisse', '', 'Sweeting', '', 'San Antonio', 7, '78213', '', 45419, '1973/08/17', 2, 1, '', 1 )
--		,( 42, 'Fletch', '', 'Fridlund', '', 'Hermitage', 12, '37076', '', 29437, '1990/07/01', 2, 0, '', 1 )
--		,( 43, 'Dorri', '', 'Gelbart', '', 'Carmel', 57, '10512', '', 36394, '1996/07/06', 2, 1, '', 1 )
--		,( 44, 'Dana', '', 'Perloff', '', 'Prattville', 48, '36067', '', 22464, '2013/09/03', 1, 1, '', 1 )
--		,( 45, 'Darelle', '', 'Hagen', '', 'Havertown', 38, '19083', '', 28997, '2000/12/01', 2, 0, '', 1 )
--		,( 46, 'Granthem', '', 'Glynn', '', 'Butler', 14, '16001', '', 78229, '1986/06/19', 2, 0, '', 1 )
--		,( 47, 'Heddi', '', 'Geissler', '', 'Saint Petersburg', 50, '33702', '', 74061, '1951/02/28', 1, 1, '', 1 )
--		,( 48, 'Vinnie', '', 'Davidson', '', 'Hartselle', 48, '35640', '', 23324, '1975/04/21', 1, 1, '', 1 )
--		,( 49, 'Tracie', '', 'Devellis', '', 'Conway', 34, '29526', '', 31210, '1974/08/06', 2, 1, '', 1 )
--		,( 50, 'Morie', '', 'Kumins', '', 'Newington', 32, '6111', '', 4812, '1998/11/19', 2, 0, '', 1 )
--		,( 51, 'Carmencita', '', 'Liander', '', 'Sterling Heights', 44, '48310', '', 64936, '2010/02/13', 1, 1, '', 1 )
--		,( 52, 'Arri', '', 'Lovell', '', 'Vienna', 29, '22180', '', 1186, '1980/07/07', 1, 1, '', 1 )
--		,( 53, 'Marti', '', 'Tawn', '', 'Bensalem', 45, '19020', '', 67534, '1975/10/19', 1, 0, '', 1 )
--		,( 54, 'Dicky', '', 'Mahjub', '', 'Bemidji', 46, '56601', '', 18913, '2013/02/20', 2, 0, '', 1 )
--		,( 55, 'Susannah', '', 'Halverson', '', 'Camden', 18, '8105', '', 36315, '2012/09/02', 1, 0, '', 1 )
--		,( 56, 'Rocky', '', 'Loret', '', 'Muscatine', 59, '52761', '', 62257, '1962/06/07', 2, 1, '', 1 )
--		,( 57, 'Eran', '', 'Naylor', '', 'Englewood', 6, '7631', '', 57439, '1997/07/21', 2, 1, '', 1 )
--		,( 58, 'Roberto', '', 'Karasik', '', 'Bountiful', 20, '84010', '', 19923, '1973/09/04', 2, 0, '', 1 )
--		,( 59, 'Shea', '', 'Barth', '', 'Cantonment', 12, '32533', '', 33529, '1994/04/28', 1, 0, '', 1 )
--		,( 60, 'Fabien', '', 'Bichel', '', 'Lake Worth', 37, '33460', '', 14042, '1999/04/10', 2, 1, '', 1 )
--		,( 61, 'Charis', '', 'Boezi', '', 'West Hempstead', 11, '11552', '', 40190, '2011/03/30', 1, 1, '', 1 )
--		,( 62, 'Talbot', '', 'Levasseur', '', 'Howell', 31, '7731', '', 32727, '1992/03/02', 2, 0, '', 1 )
--		,( 63, 'Theadora', '', 'Manalis', '', 'Highland Park', 7, '60035', '', 968, '2005/11/24', 2, 1, '', 1 )
--		,( 64, 'Paddie', '', 'Valois', '', 'Meadville', 14, '16335', '', 58710, '1991/11/17', 1, 0, '', 1 )
--		,( 65, 'Evita', '', 'Pallant', '', 'Mokena', 54, '60448', '', 5610, '1960/06/07', 1, 1, '', 1 )
--		,( 66, 'Far', '', 'Hunink', '', 'Lansdale', 57, '19446', '', 4843, '2000/02/07', 2, 0, '', 1 )
--		,( 67, 'Netta', '', 'Brockenbrough', '', 'Yorktown', 58, '23693', '', 6479, '1973/06/03', 2, 1, '', 1 )
--		,( 68, 'Jared', '', 'Hagen', '', 'Haines City', 27, '33844', '', 57198, '2004/12/02', 1, 0, '', 1 )
--		,( 69, 'Kassey', '', 'Heifetz', '', 'Severna Park', 41, '21146', '', 4144, '1970/05/11', 2, 0, '', 1 )
--		,( 70, 'Angelico', '', 'Devellis', '', 'Henderson', 4, '42420', '', 41785, '1989/09/27', 1, 0, '', 1 )
--		,( 71, 'Mandi', '', 'Liest', '', 'East Haven', 10, '6512', '', 35684, '1986/12/30', 2, 0, '', 1 )
--		,( 72, 'Eldredge', '', 'Tawn', '', 'Port Charlotte', 39, '33952', '', 44761, '1951/03/12', 2, 1, '', 1 )
--		,( 73, 'Betty', '', 'Shimin', '', 'Owosso', 2, '48867', '', 67006, '1974/02/18', 2, 1, '', 1 )
--		,( 74, 'Raphael', '', 'Naylor', '', 'Avon Lake', 53, '44012', '', 33221, '1986/06/04', 2, 0, '', 1 )
--		,( 75, 'Mae', '', 'Suzuki', '', 'Altoona', 37, '16601', '', 54761, '1960/02/02', 1, 0, '', 1 )
--		,( 76, 'Pauly', '', 'Boezi', '', 'Appleton', 56, '54911', '', 15087, '1982/02/09', 2, 1, '', 1 )
--		,( 77, 'Daveta', '', 'Goodridge', '', 'Manitowoc', 25, '54220', '', 6326, '1974/06/14', 1, 0, '', 1 )
--		,( 78, 'Kendall', '', 'Pallant', '', 'Gibsonia', 8, '15044', '', 76572, '1989/03/29', 2, 0, '', 1 )
--		,( 79, 'Bertine', '', 'Demaio', '', 'Vineland', 25, '8360', '', 45079, '1962/08/06', 1, 1, '', 1 )
--		,( 80, 'Lyman', '', 'Heifetz', '', 'Vernon Rockville', 8, '6066', '', 40941, '2006/03/10', 1, 1, '', 1 )
--		,( 81, 'Noell', '', 'Robertucci', '', 'Milford', 10, '1757', '', 6666, '1979/01/16', 2, 0, '', 1 )
--		,( 82, 'Siegfried', '', 'Shimin', '', 'Eastpointe', 34, '48021', '', 33411, '1981/02/27', 2, 1, '', 1 )
--		,( 83, 'Melodie', '', 'Mcgee', '', 'Quakertown', 35, '18951', '', 25360, '1951/11/20', 1, 1, '', 1 )
--		,( 84, 'Sander', '', 'Goodridge', '', 'Stuart', 11, '34997', '', 54078, '2008/09/12', 1, 1, '', 1 )
--		,( 85, 'May', '', 'Mcnulty', '', 'Jacksonville', 14, '28540', '', 62241, '1988/11/08', 2, 1, '', 1 )
--		,( 86, 'Matthieu', '', 'Robertucci', '', 'Reston', 16, '20191', '', 4415, '1965/09/20', 1, 0, '', 1 )
--		,( 87, 'Freddy', '', 'Roegner', '', 'Rossville', 24, '30741', '', 28144, '2004/11/21', 1, 1, '', 1 )
--		,( 88, 'Lowe', '', 'Mcnulty', '', 'Deerfield Beach', 13, '33442', '', 9906, '1999/10/16', 1, 1, '', 1 )
--		,( 89, 'Evangelin', '', 'Bovet', '', 'Zanesville', 6, '43701', '', 14074, '1957/03/04', 2, 0, '', 1 )
--		,( 90, 'Judon', '', 'Bovet', '', 'Fayetteville', 15, '28303', '', 28817, '2015/05/27', 2, 1, '', 1 )
--		,( 91, 'Farly', '', 'Stiefez', '', 'Pueblo', 52, '81001', '', 1658, '1956/06/29', 2, 1, '', 1 )
--		,( 92, 'Othilia', '', 'Grummell', '', 'Absecon', 23, '8205', '', 70724, '1958/06/09', 2, 0, '', 1 )
--		,( 93, 'Jeth', '', 'Papacosta', '', 'Lawrenceville', 6, '30043', '', 53514, '1950/04/06', 2, 0, '', 1 )
--		,( 94, 'Philippine', '', 'Mehrabi', '', 'Doylestown', 6, '18901', '', 76176, '2012/11/16', 2, 1, '', 1 )
--		,( 95, 'Stephen', '', 'Elam', '', 'Trussville', 29, '35173', '', 73940, '1988/11/08', 1, 1, '', 1 )
--		,( 96, 'Lissie', '', 'Swofford', '', 'Mount Juliet', 53, '37122', '', 30097, '1954/03/08', 1, 0, '', 1 )
--		,( 97, 'Everett', '', 'Mortimer', '', 'Colonial Heights', 57, '23834', '', 69233, '1962/02/02', 2, 0, '', 1 )
--		,( 98, 'Dianna', '', 'Monteiro', '', 'Fort Mill', 45, '29708', '', 67199, '1962/09/01', 1, 1, '', 1 )
--		,( 99, 'Gare', '', 'Jamil', '', 'Bridgewater', 58, '8807', '', 76098, '1998/12/18', 2, 0, '', 1 )
--		,( 100, 'Danice', '', 'Racioppi', '', 'Evans', 1, '30809', '', 11976, '1980/03/20', 1, 1, '', 1 )



INSERT INTO TTeamPlayers ( intTeamID, intPlayerID )
VALUES	 ( 1, 1 )
		,( 1, 2 )
		,( 1, 3 )
		,( 2, 4 )
		,( 2, 5 )
		,( 3, 6 )


GO
-- --------------------------------------------------------------------------------
--	Step #6: Test player stored procedures
---- --------------------------------------------------------------------------------
---- Add Player
--SELECT
--	*
--FROM
--	VActivePlayers

--EXECUTE uspAddPlayer 'Danice', '', 'Racioppi', '', 'Evans', 1, '30809', '', 11976, '1980/03/20', 1, 1, '';

--SELECT
--	*
--FROM
--	VActivePlayers

---- Edit Player
----SELECT
----	*
----FROM
----	TPlayers
----WHERE 
----	intPlayerID = 101

----EXECUTE uspEditPlayer 101, 'Dan', '', 'Brown', '', 'Evans', 1, '30809', '', 11976, '1980/03/20', 1, 1, '';

--SELECT
--	*
--FROM
--	TPlayers
--WHERE 
--	intPlayerID = 101


---- Set player status
--SELECT
--	*
--FROM
--	VActivePlayers

----EXECUTE uspSetPlayerStatus 101, 2;

--SELECT
--	*
--FROM
--	VInactivePlayers



-- --------------------------------------------------------------------------------
--	Step #7: Test team stored procedures
-- --------------------------------------------------------------------------------
---- Add Team
--SELECT
--	*
--FROM
--	VActiveTeams

--EXECUTE uspAddTeam 'The Straw-Hats', 'Monster Island Apes';

--SELECT
--	*
--FROM
--	VActiveTeams

---- Edit Team
--SELECT
--	*
--FROM
--	VActiveTeams
--WHERE 
--	intTeamID = 5

--EXECUTE uspEditTeam 5, 'Marine Happy', 'Dogs';

--SELECT
--	*
--FROM
--	VActiveTeams
--WHERE 
--	intTeamID = 5


---- Set team status
--SELECT
--	*
--FROM
--	VActiveTeams
--WHERE 
--	intTeamID = 5

--EXECUTE uspSetTeamStatus 5, 2;

--SELECT
--	*
--FROM
--	VInactiveTeams



---- --------------------------------------------------------------------------------
----	Step #8: Test teamplayer stored procedures
---- --------------------------------------------------------------------------------

---- Add teamPlayer
--SELECT
--	*
--FROM
--	TTeamPlayers

--EXECUTE uspAddTeamPlayer 2, 101;


--SELECT
--	*
--FROM
--	TTeamPlayers


---- remove teamPlayer
--SELECT
--	*
--FROM
--	TTeamPlayers

--EXECUTE uspRemoveTeamPlayer 2, 101;


--SELECT
--	*
--FROM
--	TTeamPlayers















