
--query1

CREATE TABLE UserInfo (
    EmailId VARCHAR(100) PRIMARY KEY,

    UserName VARCHAR(50) NOT NULL,
    
    Role VARCHAR(20) NOT NULL 
        CHECK (Role IN ('Admin','Participant')),

    Password VARCHAR(20) NOT NULL
        CHECK (LEN(Password) BETWEEN 6 AND 20)
);

INSERT INTO UserInfo VALUES
('admin12@gmail.com','AdminUser','Admin','admin098'),
('ishalandge234@gmail.com','isha','Participant','ishis123'),
('nishalandge24@gmail.com','nisha','Participant','nishhh456');

SELECT * FROM UserInfo;

--query2

CREATE TABLE EventDetails (
    EventId INT PRIMARY KEY,

    EventName VARCHAR(50) NOT NULL,

    EventCategory VARCHAR(50) NOT NULL,

    EventDate DATETIME NOT NULL,

    Description VARCHAR(255) NULL,

    Status VARCHAR(20)
        CHECK (Status IN ('Active','In-Active'))
);

INSERT INTO EventDetails VALUES
(1,'Tech Conference','Technology','2026-04-10','Annual tech event','Active'),
(2,'AI Summit','Data Science ','2026-05-15','Data Science focused','Active');

SELECT * FROM EventDetails;

--query3

CREATE TABLE SpeakersDetails (
    SpeakerId INT PRIMARY KEY,

    SpeakerName VARCHAR(50) NOT NULL
);


INSERT INTO SpeakersDetails VALUES
(1,'Shradha khapraa'),
(2,'love babbar');

SELECT * FROM SpeakersDetails;

--query4
CREATE TABLE SessionInfo (
    SessionId INT PRIMARY KEY,

    EventId INT NOT NULL,

    SessionTitle VARCHAR(50) NOT NULL,

    SpeakerId INT NOT NULL,

    Description VARCHAR(255) NULL,

    SessionStart DATETIME NOT NULL,

    SessionEnd DATETIME NOT NULL,

    SessionUrl VARCHAR(255),

    CONSTRAINT FK_Session_Event
        FOREIGN KEY (EventId)
        REFERENCES EventDetails(EventId),

    CONSTRAINT FK_Session_Speaker
        FOREIGN KEY (SpeakerId)
        REFERENCES SpeakersDetails(SpeakerId)
);


INSERT INTO SessionInfo VALUES
(101,1,'Future of AI',1,'Discussion about future AI technologies',
'2026-04-10 10:00:00','2026-04-10 11:00:00',
'https://meet.com/ai-session'),

(102,1,'Web Development Trends',2,'Latest trends in web development',
'2026-04-10 11:30:00','2026-04-10 12:30:00',
'https://meet.com/web-session'),

(103,2,'Machine Learning Basics',1,'Introduction to ML concepts',
'2026-05-15 09:30:00','2026-05-15 10:30:00',
'https://meet.com/ml-session'),

(104,2,'Deep Learning Applications',2,'Real world deep learning use cases',
'2026-05-15 11:00:00','2026-05-15 12:00:00',
'https://meet.com/dl-session');


SELECT * 
FROM SessionInfo;

--query5

CREATE TABLE ParticipantEventDetails (
   Id INT PRIMARY KEY,
 
   ParticipantEmailId VARCHAR(100) NOT NULL,

   EventId INT,

   SpeakerId INT,

   CONSTRAINT FK_Participant_Email
   FOREIGN KEY (ParticipantEmailId)
   REFERENCES UserInfo(EmailId),

   CONSTRAINT FK_Participant_Event
   FOREIGN KEY (EventId)
   REFERENCES EventDetails(EventId),

   CONSTRAINT FK_Participant_Speaker
   FOREIGN KEY (SpeakerId)
   REFERENCES SpeakersDetails(SpeakerId)
);

INSERT INTO ParticipantEventDetails VALUES
(1,'ishalandge234@gmail.com',1,1),
(2,'ishalandge234@gmail.com',2,2),
(3,'nishalandge24@gmail.com',1,2),
(4,'nishalandge24@gmail.com',2,1);


SELECT * 
FROM  ParticipantEventDetails;
