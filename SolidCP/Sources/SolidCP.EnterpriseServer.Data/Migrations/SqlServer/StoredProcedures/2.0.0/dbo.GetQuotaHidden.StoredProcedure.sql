CREATE PROCEDURE [dbo].[GetQuotaHidden]
(
	@QuotaName nvarchar(50),
	@GroupID int,
	@HideQuota bit OUTPUT
)
AS
SELECT
	@HideQuota = HideQuota
FROM Quotas AS Q
WHERE QuotaName = @QuotaName AND GroupID = @GroupID
-- print  @ideQuota
RETURN
GO
