﻿ALTER TABLE [dbo].[oxite_Conferences_ScheduleItemFileRelationship] ADD
CONSTRAINT [FK_oxite_Conferences_ScheduleItemFileRelationship_oxite_Conferences_ScheduleItem] FOREIGN KEY ([ScheduleItemID]) REFERENCES [dbo].[oxite_Conferences_ScheduleItem] ([ScheduleItemID])


