CREATE TABLE users (
    id INT PRIMARY KEY IDENTITY(1,1),
    firstname VARCHAR(50) NOT NULL,
    lastname VARCHAR(50) NOT NULL,
    username VARCHAR(50) NOT NULL,
    createdAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT unique_username UNIQUE(username)
);

CREATE TABLE scores (
    id INT PRIMARY KEY IDENTITY(1,1),
    userId INT NOT NULL,
    scoreValue DECIMAL(5, 2) NOT NULL,
    createdAt DATE NOT NULL,

    CONSTRAINT fk_user_id FOREIGN KEY(userId) 
    REFERENCES users(id) ON DELETE CASCADE
);


-- table type for sp_UploadUserScores
CREATE TYPE dbo.ScoreType AS TABLE
(
    userId INT,
    scoreValue INT,
    createdAt DATE
);