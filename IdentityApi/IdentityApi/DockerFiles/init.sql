-- Create the Database
CREATE
DATABASE IdentityMovie;
GO

USE IdentityMovie;
GO

-- Create additional users if needed
CREATE
LOGIN olivia WITH PASSWORD = 'olivia0123';
GO

-- Example table creation and data seeding
CREATE TABLE ExampleTable
(
    ID            INT PRIMARY KEY,
    ExampleColumn NVARCHAR(50)
);
GO

INSERT INTO ExampleTable (ID, ExampleColumn) VALUES (1, 'Example Data');
GO
