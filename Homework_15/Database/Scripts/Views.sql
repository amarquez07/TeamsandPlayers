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
-- Drop Statements
-- --------------------------------------------------------------------------------

IF OBJECT_ID('VActivePlayers')						IS NOT NULL DROP VIEW VActivePlayers
IF OBJECT_ID('VInactivePlayers')					IS NOT NULL DROP VIEW VInactivePlayers
IF OBJECT_ID('VActiveTeams')						IS NOT NULL DROP VIEW VActiveTeams
IF OBJECT_ID('VInactiveTeams')						IS NOT NULL DROP VIEW VInactiveTeams

-- --------------------------------------------------------------------------------
-- Name: VActivePlayers
-- Abstract: Show active players only
-- --------------------------------------------------------------------------------
GO
CREATE VIEW VActivePlayers
AS
SELECT
	TP.intPlayerID, TP.strFirstName, TP.strLastName
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
	TP.intPlayerID, TP.strFirstName, TP.strLastName
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
	TT.intTeamID, TT.strTeam
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
	TT.intTeamID, TT.strTeam
FROM
	TTeams		AS TT

WHERE
	intTeamStatusID = 2

GO

