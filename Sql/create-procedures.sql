GO
-- sp_CheckUserExistsBy
CREATE PROCEDURE sp_CheckUserExistsBy
    @column VARCHAR(50),
	@value VARCHAR(50)
AS
BEGIN
	DECLARE @sql NVARCHAR(MAX)
	SET @sql = 'SELECT 1 FROM Users WHERE ' + QUOTENAME(@column) + ' = ' + QUOTENAME(@value, '''');

	EXEC sp_executesql @sql
END

GO

-- sp_GetScoresByDay
CREATE PROCEDURE sp_UploadUserData
    @firstname VARCHAR(50),
    @lastname VARCHAR(50),
    @username VARCHAR(50)
AS
BEGIN
    INSERT INTO users(firstname, lastname, username) VALUES (@firstname, @lastname, @username);
END

GO


-- sp_GetScoresByDay
CREATE PROCEDURE sp_UploadUserScores
    @scores dbo.ScoreType READONLY,
    @affectedRows INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO scores (userId, scoreValue, createdAt)
    SELECT userId, scoreValue, createdAt
    FROM @scores;

    SET @affectedRows = @@ROWCOUNT;
END;

GO

-- sp_GetScoresByDay
CREATE PROCEDURE sp_GetScoresByDay
    @targetDay DATE
AS
BEGIN
    SELECT
        u.id AS UserID,
        u.username AS UserName,
        SUM(s.scoreValue) AS ScoreValue
    FROM
        users u
    INNER JOIN
        scores s ON u.id = s.userId
    WHERE
        s.createdAt = @targetDay
    GROUP BY
        u.id, u.username
    ORDER BY
        ScoreValue DESC;
END;

GO

-- sp_GetScoresByMonth
CREATE PROCEDURE sp_GetScoresByMonth
    @targetYear INT,
    @targetMonth INT
    
AS
BEGIN
    SELECT
        u.id AS UserID,
        u.username AS UserName,
        SUM(s.scoreValue) AS ScoreValue
    FROM
        users u
    INNER JOIN
        scores s ON u.id = s.userId
    WHERE
        YEAR(s.createdAt) = @targetYear 
        AND MONTH(s.createdAt) = @targetMonth
    GROUP BY
        u.id, u.username
    ORDER BY
        ScoreValue DESC;
END;

GO

-- sp_GetAllData
CREATE PROCEDURE sp_GetAllData
AS
BEGIN
	SELECT
		u.id AS UserID,
		u.username AS UserName,
        s.scoreValue AS ScoreValue
	FROM
		users u
	INNER JOIN
		scores s ON u.id = s.userId
    ORDER BY
	    u.id;
END;

GO

-- sp_GetStats
CREATE PROCEDURE sp_GetStats
AS
BEGIN
     WITH DailyScores AS (
        SELECT scoreValue
        FROM scores
        WHERE createdAt = CAST(GETDATE() AS DATE)
    ),
    MonthlyScores AS (
        SELECT scoreValue
        FROM scores
        WHERE MONTH(createdAt) = MONTH(GETDATE()) AND YEAR(createdAt) = YEAR(GETDATE())
    ),
    WeeklyScores AS (
        SELECT scoreValue
        FROM scores
        WHERE createdAt >= DATEADD(DAY, -7, GETDATE())
    )
    SELECT
        ROUND((SELECT AVG(scoreValue) FROM DailyScores),2) AS AverageDaily,
        ROUND((SELECT AVG(scoreValue) FROM MonthlyScores),2) AS AverageMonthly,
        (SELECT MAX(scoreValue) FROM DailyScores) AS MaximumDaily,
        (SELECT MAX(scoreValue) FROM WeeklyScores) AS MaximumWeekly,
        (SELECT MAX(scoreValue) FROM MonthlyScores) AS MaximumMonthly;
END;

GO

-- sp_GetUserInfo
CREATE PROCEDURE sp_GetUserInfo
    @UserID INT
AS
BEGIN
    DECLARE @StartDate DATE, @EndDate DATE;

    SET @StartDate = DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1);
    SET @EndDate = DATEADD(DAY, -1, DATEADD(MONTH, 1, @StartDate));

    WITH UserStats AS (
        SELECT 
            userId,
            SUM(scoreValue) AS TotalScore,
            RANK() OVER (ORDER BY SUM(scoreValue) DESC) AS UserRank
        FROM scores
        WHERE
             createdAt >= @StartDate
            AND createdAt <= @EndDate
        GROUP BY userId
    )
    SELECT TOP 1
		us.UserRank,
		us.TotalScore
    FROM
	    users u
    INNER JOIN
	    UserStats us ON u.id = us.userId
    WHERE
	    u.id = @UserID;

END;

GO