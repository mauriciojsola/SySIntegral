SELECT * FROM [CheckPoint] AS cp
SELECT *  FROM CheckPointCount

INSERT INTO CheckPointCount
(
	CheckPointId,
	EggRegistryId
)
SELECT 1,r.Id FROM EggRegistry AS r WHERE r.InputDeviceId=1

INSERT INTO CheckPointCount
(
	CheckPointId,
	EggRegistryId
)
SELECT 2,r.Id FROM EggRegistry AS r WHERE r.InputDeviceId=11

SELECT * FROM Organization AS o

SELECT CAST(r.ReadTimestamp AS DATE) AS RegistryDate, MAX(r.WhiteEggsCount) AS WhiteEggsCount, MAX(r.ColorEggsCount) AS ColorEggsCount,
                        d.UniqueId AS UniqueId,cp.Id AS CheckPointId, cp.CheckPointType,
                        cp.ParentId AS CheckPointParentId,cp.[Description] AS CheckPointDescription ,d.[Description] AS DeviceDescription, a.Name AS AssetName
                            FROM EggRegistry AS r
                            INNER JOIN InputDevice AS d ON d.Id = r.InputDeviceId
                            INNER JOIN [CheckPoint] AS cp ON cp.InputDeviceId = d.Id
                            INNER JOIN Asset AS a ON a.Id = d.AssetId
                            INNER JOIN Organization AS org ON org.Id = a.OrganizationId
                        WHERE r.ReadTimeStamp IS NOT NULL
                        AND org.Id = 2
                        AND d.Id IN(1,11)
                        AND r.ReadTimestamp BETWEEN '2022-01-01' AND '2022-12-31'
                        GROUP BY CAST(r.ReadTimestamp as DATE), d.UniqueId,cp.[Description],d.[Description],a.Name
                            ORDER BY CAST(r.ReadTimestamp as DATE) DESC,a.Name, d.UniqueId
                            
                            
SELECT CAST(r.ReadTimestamp AS DATE) AS RegistryDate, MAX(r.WhiteEggsCount) AS WhiteEggsCount, MAX(r.ColorEggsCount) AS ColorEggsCount,
                        d.UniqueId AS DeviceUniqueId, cp.Id AS CheckPointId,
                        cp.ParentId AS CheckPointParentId,a.Id AS AssetId
                            FROM EggRegistry AS r
                            INNER JOIN InputDevice AS d ON d.Id = r.InputDeviceId
                            INNER JOIN [CheckPoint] AS cp ON cp.InputDeviceId = d.Id
                            INNER JOIN Asset AS a ON a.Id = d.AssetId
                            INNER JOIN Organization AS org ON org.Id = a.OrganizationId
                        WHERE r.ReadTimeStamp IS NOT NULL
                        AND org.Id = 2
                        AND d.Id IN(1,11)
                        AND r.ReadTimestamp BETWEEN '2022-01-01' AND '2022-12-31'
                        GROUP BY CAST(r.ReadTimestamp as DATE), d.UniqueId, cp.Id, cp.ParentId, a.Id
                            ORDER BY CAST(r.ReadTimestamp as DATE) DESC,a.Id, d.UniqueId  
                            
                            
SELECT * FROM 
(SELECT CAST(r.ReadTimestamp AS DATE) AS RegistryDate, MAX(r.WhiteEggsCount) AS WhiteEggsCount, MAX(r.ColorEggsCount) AS ColorEggsCount,
                        d.UniqueId AS DeviceUniqueId, cp.Id AS CheckPointId,
                        cp.ParentId AS CheckPointParentId,a.Id AS AssetId
                            FROM EggRegistry AS r
                            INNER JOIN InputDevice AS d ON d.Id = r.InputDeviceId
                            INNER JOIN [CheckPoint] AS cp ON cp.InputDeviceId = d.Id
                            INNER JOIN Asset AS a ON a.Id = d.AssetId
                            INNER JOIN Organization AS org ON org.Id = a.OrganizationId
                        WHERE r.ReadTimeStamp IS NOT NULL
                        AND org.Id = 2
                        AND d.Id IN(1,11)
                        AND r.ReadTimestamp BETWEEN '2022-01-01' AND '2022-12-31'
                        GROUP BY CAST(r.ReadTimestamp as DATE), d.UniqueId, cp.Id, cp.ParentId, a.Id) AS Registries


	SELECT cp.Id AS CheckPointId, 
		cp.CheckPointType, 
		cp.[Description] AS CheckPointDescription, 
		cp.ParentId AS CheckPointParentId, 
		cp.AssetId,
		cp.InputDeviceId,
		a.Id AS AssetId, 
		a.Name AS AssetName,
		org.Id AS OrganizationId
	  FROM [CheckPoint] AS cp
	 INNER JOIN Asset AS a ON a.Id = cp.AssetId
	 INNER JOIN Organization AS org ON org.Id = a.OrganizationId
	WHERE org.Id = 2
                        
                            