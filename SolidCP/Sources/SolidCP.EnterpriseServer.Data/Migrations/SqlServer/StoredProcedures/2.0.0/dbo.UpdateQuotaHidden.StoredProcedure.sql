CREATE PROCEDURE [dbo].[UpdateQuotaHidden]
(
	@QuotaName nvarchar(50),
	@GroupID int,
	@HideQuota bit
)
AS
UPDATE Quotas
SET
	HideQuota = @HideQuota
WHERE QuotaName = @QuotaName AND GroupID = @GroupID
RETURN
GO