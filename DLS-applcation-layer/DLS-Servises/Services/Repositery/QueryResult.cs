namespace Application_Layer.IRepo.Services.Repositery
{
    public enum QueryResult
    {
        Succeeded,
        Failed,
        AlreadyExist,
        DataChangedByAnotherUser,
        DataHasRelatedData,
        RelatedDataAlreadyExist
    }
}