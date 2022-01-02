--USE [Geography]

SELECT TOP(5) CountryName,
		MAX(p.Elevation) AS [Highest Peak Elevation],
		MAX(r.[Length]) AS [Longest River Length]
		FROM Countries AS c
LEFT OUTER JOIN CountriesRivers AS cr
ON cr.CountryCode = c.ContinentCode
LEFT OUTER JOIN Rivers AS r
ON cr.RiverId = r.Id
LEFT OUTER JOIN MountainsCountries AS mc
ON c.CountryCode = mc.CountryCode
LEFT OUTER JOIN Mountains AS m
ON mc.MountainId = m.Id
LEFT OUTER JOIN Peaks AS p
ON p.MountainId = m.Id
GROUP BY c.CountryName
ORDER BY [Highest Peak Elevation] DESC, [Longest River Length] DESC, CountryName ASC