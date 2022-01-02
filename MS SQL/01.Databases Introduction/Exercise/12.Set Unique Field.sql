ALTER TABLE [Users]
DROP CONSTRAINT [PK_UserCompositeIdUser];

ALTER TABLE [Users]
ADD CONSTRAINT [PK_Id]
PRIMARY KEY ([Id]);

ALTER TABLE [Users]
ADD CONSTRAINT [CH_UsernameIsAtLeast3Symbols] 
CHECK (LEN([Username]) >= 3);