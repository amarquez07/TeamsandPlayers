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

--sp_helptext uspAddTeam	-- pulls up stored procedure

-- --------------------------------------------------------------------------------
--	Step #0: Drop statements
-- --------------------------------------------------------------------------------
-- IF OBJECT_ID('') IS NOT NULL DROP TABLE
-- IF OBJECT_ID('') IS NOT NULL DROP PROCEDURE


IF OBJECT_ID('uspAddPlayer')				IS NOT NULL DROP PROCEDURE uspAddPlayer
IF OBJECT_ID('uspEditPlayer')				IS NOT NULL DROP PROCEDURE uspEditPlayer
IF OBJECT_ID('uspSetPlayerstatus')			IS NOT NULL DROP PROCEDURE uspSetPlayerstatus

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
	 ,@strCity						VARCHAR(50)
	 ,@intStateID					INTEGER
	 ,@strZipcode					VARCHAR(50)
	 ,@strHomePhoneNumber			VARCHAR(50)
	 ,@curSalary					MONEY
	 ,@dteDateOfBirth				DATE
	 ,@intSexID						INTEGER
	 ,@blnMostValuablePlayer		BIT
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
	INSERT INTO TPlayers ( intPlayerID, strFirstName, strMiddleName, strLastName, strStreetAddress, strCity, intStateID				
,strZipcode, strHomePhoneNumber, curSalary, dteDateOfBirth, intSexID, blnMostValuablePlayer, strEmailAddress, intPlayerStatusID )
	VALUES( @intPlayerID, @strFirstName, @strMiddleName, @strLastName, @strStreetAddress, @strCity, @intStateID				
, @strZipcode, @strHomePhoneNumber, @curSalary, @dteDateOfBirth, @intSexID, @blnMostValuablePlayer, @strEmailAddress , 1 ) -- 1 = Active

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
	,@strCity						VARCHAR(50)
	,@intStateID					INTEGER
	,@strZipcode					VARCHAR(50)
	,@strHomePhoneNumber			VARCHAR(50)
	,@curSalary						MONEY
	,@dteDateOfBirth				DATE
	,@intSexID						INTEGER
	,@blnMostValuablePlayer			BIT
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
		,strCity				= @strCity
		,intStateID				= @intStateID
		,strZipcode				= @strZipcode
		,strHomePhoneNumber		= @strHomePhoneNumber	
		,curSalary				= @curSalary				
		,dteDateOfBirth			= @dteDateOfBirth		
		,intSexID				= @intSexID				
		,blnMostValuablePlayer	= @blnMostValuablePlayer	
		,strEmailAddress		= @strEmailAddress		
	WHERE
		intPlayerID	= @intPlayerID
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














