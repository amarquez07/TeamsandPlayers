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

IF OBJECT_ID('uspAddTeamPlayer')						IS NOT NULL DROP PROCEDURE uspAddTeamPlayer
IF OBJECT_ID('uspRemoveTeamPlayer')						IS NOT NULL DROP PROCEDURE uspRemoveTeamPlayer
IF OBJECT_ID('uspSetTeamStatus')						IS NOT NULL DROP PROCEDURE uspSetTeamStatus


-- --------------------------------------------------------------------------------
--	Name: uspAddTeamPlayer
--	Abstract: Stored procedure to add a team to database
-- --------------------------------------------------------------------------------
GO
CREATE PROCEDURE uspAddTeamPlayer
	 @intTeamID					INTEGER
	,@intPlayerID				INTEGER
AS
SET NOCOUNT ON			-- Report errors only
SET XACT_ABORT ON		-- Rollback transaction on error

BEGIN TRANSACTION
	
	-- Create new record
	INSERT INTO TTeamPlayers( intTeamID, intPlayerID )
	VALUES( @intTeamID, @intPlayerID )

COMMIT TRANSACTION
GO


-- --------------------------------------------------------------------------------
--	Name: uspRemoveTeamPlayer
--	Abstract: Stored procedure to remove team in the database
-- --------------------------------------------------------------------------------
GO
CREATE PROCEDURE uspRemoveTeamPlayer
	 @intTeamID					INTEGER
	,@intPlayerID				INTEGER
AS
SET NOCOUNT ON			-- Report errors only
SET XACT_ABORT ON		-- Rollback transaction on error

BEGIN TRANSACTION

	-- Delete the record
	DELETE FROM
		TTeamPlayers
	WHERE
		intTeamID = @intTeamID
	AND intPlayerID = @intPlayerID

COMMIT TRANSACTION
GO
















