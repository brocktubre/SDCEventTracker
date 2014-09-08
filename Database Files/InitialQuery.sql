CREATE TABLE Event(
	ID INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	EventName varchar(128),
	Date DATETIME,
	Location varchar(128),
	City varchar(128),
	State varchar(128),
	Zip int,
	MorningHunt bit DEFAULT(0),
	EveningHunt bit DEFAULT(0),
	BenchShow bit DEFAULT(0), 
	BarkingContest bit DEFAULT(0),
	Details VARCHAR(8000)

)

CREATE TABLE Handler(
	ID INT PRIMARY KEY NOT NULL IDENTITY(1000, 1),
	FirstName varchar(45),
	LastName varchar(45),

)

CREATE TABLE Dog(
	ID INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	Name varchar(128),
	Handler int FOREIGN KEY REFERENCES Handler(ID)
)

CREATE TABLE Result(
	ID INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	EventID INT NOT NULL,
	EventType INT NOT NULL,
	Place INT NOT NULL, 
	HandlerID INT NOT NULL,
	DogID INT NOT NULL
)

CREATE TABLE EventEnum(
	ID INT PRIMARY KEY NOT NULL,
	EventType VARCHAR(45) NOT NULL
)

ALTER TABLE Result
ADD FOREIGN KEY (DogID)
REFERENCES Dog(ID)

ALTER TABLE Result
ADD FOREIGN KEY (HandlerID)
REFERENCES Handler(ID)

ALTER TABLE Result
ADD FOREIGN KEY (EventID)
REFERENCES Event(ID)

ALTER TABLE Result
ADD FOREIGN KEY (EventType)
REFERENCES EventEnum(ID)

ALTER TABLE Dog
ADD FOREIGN KEY (HandlerID)
REFERENCES Handler(ID)