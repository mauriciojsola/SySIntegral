

ALTER TABLE Device 
ADD [DeviceType] [int] NOT NULL DEFAULT 3
GO

--ALTER TABLE [Device] ADD  CONSTRAINT [DF_Device_DeviceType]  DEFAULT ((3)) FOR [DeviceType]
--GO