INSERT INTO Book ([Name], AuthorId, PublisherId, Pages, GenreId, YearPublishing, CostPrice, Price) 
VALUES ('Harry Potter and the Philosopher''s Stone', 
	(SELECT Id FROM Human WHERE FirstName='Joanne' AND LastName='Rowling'),
	(SELECT Id FROM Publisher WHERE [Name]='Bloomsbury Publishing'), 400,
	(SELECT Id FROM Genre WHERE [Name]='Fantasy'), '1997-06-26', 30, 45.50);




