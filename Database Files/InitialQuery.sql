CREATE TABLE Event(
	ID int PRIMARY KEY NOT NULL identity(1,1),
	EventName varchar(128),
	Date DATETIME,
	Location varchar(128),
	City varchar(128),
	State varchar(128),
	Zip int,
	MorningHunt bit DEFAULT(0),
	EveningHunt bit DEFAULT(0),
	BenchShow bit DEFAULT(0), 
	BarkingContest bit DEFAULT(0)

)

CREATE TABLE Handler(
	ID int PRIMARY KEY NOT NULL identity (1000, 1),
	FristName varchar(45),
	LastName varchar(45),

)

CREATE TABLE Dog(
	ID int PRIMARY KEY NOT NULL identity (1,1),
	Name varchar(128),
	Handler int FOREIGN KEY REFERENCES Handler(ID)
)


CREATE TABLE Result(
	Event int, /* Defines the exact Overall Event  */
	EventType int, /* 1-4 Defines the exact Event Type (morning hunt, evening hunt, bench show, barking contest)  */
	Place int, 
	Handler int,
	Dog int, 
)

INSERT INTO Event (EventName, Location, Date, City, State, Zip, MorningHunt, EveningHunt, BenchShow, BarkingContest) VALUES ('TN State Hunt', 'Buffalo River Mangm. Area', '02/02/1994', 'Columbia', 'Tennesse', 30678, 1, 1, 0, 1) 
INSERT INTO Event (EventName, Location, City, State, Zip, MorningHunt, EveningHunt, BenchShow, BarkingContest) VALUES ('Franklin County Hunt', 'Ed Hardy Woods', 'Franklin', 'Tennesse', 30009, 1, 1, 0, 0)

CREATE TABLE Result(

	ID INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	EventID INT NOT NULL,
	EventType INT NOT NULL,
	Place INT NOT NULL, 
	Handler INT NOT NULL,
	DOG INT NOT NULL
)

CREATE TABLE EventEnum(
	ID INT PRIMARY KEY NOT NULL,
	EventType VARCHAR(45) NOT NULL
) 