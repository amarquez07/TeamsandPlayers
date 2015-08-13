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

IF OBJECT_ID('uspAddTeam')								IS NOT NULL DROP PROCEDURE uspAddTeam
IF OBJECT_ID('uspEditTeam')								IS NOT NULL DROP PROCEDURE uspEditTeam
IF OBJECT_ID('uspSetTeamStatus')						IS NOT NULL DROP PROCEDURE uspSetTeamStatus

-- --------------------------------------------------------------------------------
--	Execute stored procedures
-- --------------------------------------------------------------------------------

--EXECUTE uspAddTeam 'anthony', 'marquez';

-- --------------------------------------------------------------------------------
--	Name: uspAddTeam
--	Abstract: Stored procedure to add a team to database
-- --------------------------------------------------------------------------------
GO
CREATE PROCEDURE uspAddTeam
	 @strTeam					VARCHAR(50)
	,@strMascot					VARCHAR(50)
AS
SET NOCOUNT ON							-- Report errors only
SET XACT_ABORT ON						-- Rollback transaction on error

BEGIN TRANSACTION

	DECLARE @intTeamID			INTEGER

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
		 strTeam = @strTeam
		,strMascot = @strMascot
	WHERE
		intTeamID = @intTeamID
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
		intTeamID = @intTeamID
COMMIT TRANSACTION
GO














