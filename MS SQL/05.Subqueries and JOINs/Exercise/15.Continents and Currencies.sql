--USE [Geography]

--SELECT  ContinentCode, 
--	    CurrencyCode, 
--		COUNT(*) AS [CurrencyCount], 
--		DENSE_RANK() OVER
--		(PARTITION BY ContinentCode ORDER BY COUNT(*) DESC)
--FROM Countries
--GROUP BY ContinentCode, CurrencyCode 

SELECT ContinentCode, CurrencyCode, CurrencyCount AS [CurrencyUsage] 
		FROM (SELECT	ContinentCode, 
		CurrencyCode, 
		[CurrencyCount],
		DENSE_RANK() OVER (PARTITION BY ContinentCode ORDER BY CurrencyCount DESC) AS [CurrencyRank]
		FROM (
				SELECT  ContinentCode, 
						CurrencyCode, 
						COUNT(*) AS [CurrencyCount] 
				FROM Countries
				GROUP BY ContinentCode, CurrencyCode 
				) AS [CurrencyCountQuery]
				WHERE CurrencyCount > 1) AS [CurrencyRankingQuery]
WHERE CurrencyRank = 1
ORDER BY ContinentCode