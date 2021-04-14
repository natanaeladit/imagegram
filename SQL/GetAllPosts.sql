DECLARE @pageNumber int = 1;
DECLARE @pageSize int = 2;

SELECT post.*, comment.* FROM 
	-- Get post data ordered by number of comments and based on the page number/page size
	(SELECT 
		p.*, 
		(SELECT Count(Id) from [Imagegram].[dbo].[Comment] where PostId = p.Id) AS NumberOfComments
	 FROM 
		[Imagegram].[dbo].[Post] p
	 ORDER BY NumberOfComments DESC
	 OFFSET     ((@pageNumber-1) * @pageSize) ROWS       
	 FETCH NEXT @pageSize ROWS ONLY) post
JOIN
	-- Get last 3 comments to each post
	(SELECT *,
	 row_number() over (partition by PostId order by CreatedAt desc) AS RowNumber
	 FROM [Imagegram].[dbo].[Comment]) comment
ON post.Id = comment.PostId
WHERE comment.RowNumber <= 3
