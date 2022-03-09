INSERT INTO Human (FirstName, LastName)
VALUES 
	('Richard', 'West'),
	('Heather', 'Rodriquez'),
	('Joanne', 'Rowling');

INSERT INTO Human (FirstName, LastName, Patronymic)
VALUES 
	('Иван', 'Иванов', 'Иванович');

INSERT INTO Author (HumanId) 
	VALUES ((SELECT Id FROM Human WHERE FirstName='Joanne' AND LastName='Rowling'));

INSERT INTO Customer (HumanId, Phone) 
	VALUES ((SELECT Id FROM Human WHERE FirstName='Heather' AND  LastName='Rodriquez'), '380962024774');
