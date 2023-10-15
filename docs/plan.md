# Entities

Create database objects and classes along these lines

```
Pdf
	Id
	Title
	Description
	Author
	TotalPages
	CreatedOn
	LastAccessedOn

Tag
	Id
	PdfId
	Name

Progress
	Id
	PdfId
	UserId
	Name
	Page
```
