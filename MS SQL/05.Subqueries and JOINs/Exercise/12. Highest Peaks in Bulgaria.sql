--USE [Geography]

 SELECT 
 c.[CountryCode],
 m.[MountainRange],
 p.[PeakName],
 p.[Elevation]
 FROM [Peaks] AS p
 INNER JOIN [Mountains] AS m
 ON p.[MountainId] = m.[Id]
 INNER JOIN [MountainsCountries] AS mc
 ON m.[Id] = mc.[MountainId]
INNER JOIN [Countries] AS c
 ON mc.[CountryCode] = c.[CountryCode]
 WHERE c.[CountryCode] ='BG' AND p.[Elevation] > 2835
 ORDER BY p.[Elevation] DESC

 --SELECT 
 --c.[CountryCode],
 --m.[MountainRange],
 --p.[PeakName],
 --p.[Elevation]
 --FROM [Peaks] AS p
 --LEFT JOIN [Mountains] AS m
 --ON p.[MountainId] = m.[Id]
 --LEFT JOIN [MountainsCountries] AS mc
 --ON m.[Id] = mc.[MountainId]
 --LEFT JOIN [Countries] AS c
 --ON mc.[CountryCode] = c.[CountryCode]
 --WHERE c.[CountryCode] ='BG' AND p.[Elevation] > 2835
 --ORDER BY p.[Elevation] DESC

 --SELECT c.CountryCode,
--	   m.MountainRange,
--	   p.PeakName,
--	   p.Elevation
--	   FROM Countries AS c
--INNER JOIN MountainsCountries AS mc
--ON c.CountryCode = mc.CountryCode
--INNER JOIN Mountains AS m
--ON mc.MountainId = m.Id
--INNER JOIN Peaks AS p
--ON p.MountainId = m.Id
--WHERE c.CountryCode = 'BG' AND p.Elevation >= 2835
--ORDER BY p.Elevation DESC

--SELECT c.CountryCode,
--	   m.MountainRange,
--	   p.PeakName,
--	   p.Elevation
--	   FROM MountainsCountries AS mc
--INNER JOIN Countries AS c
--ON c.CountryCode = mc.CountryCode
--INNER JOIN Mountains AS m
--ON mc.MountainId = m.Id
--INNER JOIN Peaks AS p
--ON p.MountainId = m.Id
--WHERE c.CountryCode = 'BG' AND p.Elevation >= 2835
--ORDER BY p.Elevation DESC
