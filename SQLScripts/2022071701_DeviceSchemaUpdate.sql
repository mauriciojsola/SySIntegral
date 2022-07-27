
EXEC sp_rename 'Device', 'InputDevice';  
GO 

sp_rename 'EggRegistry.DeviceId', 'InputDeviceId', 'COLUMN';
GO  

ALTER TABLE EggRegistry
DROP CONSTRAINT FK_EggRegistry_Device;
GO

ALTER TABLE [EggRegistry]  WITH CHECK ADD  CONSTRAINT [FK_EggRegistry_InputDevice] FOREIGN KEY([InputDeviceId])
REFERENCES [InputDevice] ([Id])
GO

DROP INDEX IX_DeviceId   
    ON [EggRegistry];  
GO

CREATE INDEX IX_InputDeviceId
ON [EggRegistry] (InputDeviceId);
GO


--ALTER TABLE Device 
--ADD [DeviceType] [int] NOT NULL DEFAULT 3
--GO

--ALTER TABLE [Device] ADD  CONSTRAINT [DF_Device_DeviceType]  DEFAULT ((3)) FOR [DeviceType]
--GO