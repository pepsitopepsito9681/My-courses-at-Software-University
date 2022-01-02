--USE [Geography]

  SELECT p.[PeakName],
		 r.[RiverName],
		 LOWER(CONCAT(LEFT(p.[PeakName], LEN(p.[PeakName]) - 1), r.[RiverName])) 
				  AS [Mix]
    FROM [Peaks]  AS p,
		 [Rivers] AS r
   WHERE LOWER(RIGHT(p.[PeakName], 1)) = LOWER(LEFT(r.[RiverName], 1))
ORDER BY [Mix]

--SELECT p.PeakName, r.RiverName, 
--	LOWER(CONCAT(
--	p.PeakName, 
--	SUBSTRING(r.RiverName, 2, LEN(r.RiverName)-1))) AS [MIX]
--FROM 
--	Peaks AS p, 
--	Rivers AS r
--WHERE Right(p.PeakName, 1) = Left(r.RiverName, 1)
--ORDER BY [MIX]

--SELECT p.PeakName, 
--	   r.RiverName, 
--	   LOWER(CONCAT(
--	   p.PeakName, 
--	   SUBSTRING(r.RiverName, 2, LEN(r.RiverName)-1))) 
--	   AS [MIX] FROM Peaks AS p
--JOIN Rivers AS r ON RIGHT(p.PeakName, 1) = LEFT(r.RiverName, 1)
--ORDER BY [MIX]