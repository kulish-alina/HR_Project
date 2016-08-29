using DAL.DTO;
using Domain.Entities;
using System;
using System.Linq;

namespace DAL.Extensions
{
    public static class VacancyStageInfoExtension
    {
        public static void Update(this VacancyStageInfo vacancyStageInfoDomain, Vacancy destination, VacancyStageInfoDTO vacancyStageInfoSource)
        {
            vacancyStageInfoDomain.CandidateId = vacancyStageInfoSource.CandidateId;
            if (vacancyStageInfoSource.VacancyId.HasValue)
            {
                vacancyStageInfoDomain.VacancyId = vacancyStageInfoSource.VacancyId.Value;
            }
            else
            {
                vacancyStageInfoDomain.Vacancy = destination;
            }
            vacancyStageInfoDomain.DateOfPass = vacancyStageInfoSource.DateOfPass;
            vacancyStageInfoDomain.StageState = vacancyStageInfoSource.StageState;
            vacancyStageInfoDomain.StageId = vacancyStageInfoSource.StageId;

            var extendedStage = destination.StageFlow.FirstOrDefault(x => x.StageId == vacancyStageInfoSource.StageId);
            if (vacancyStageInfoSource.IsNew())
            {
                if (extendedStage.Stage.IsCommentRequired)
                {
                    if (vacancyStageInfoSource.Comment == null)
                    {
                        if (vacancyStageInfoDomain.StageState == Domain.Entities.Enum.StageState.Passed)
                        {
                            throw new Exception("Comment is needed");
                        }
                    }
                    else
                    {
                        vacancyStageInfoDomain.Comment = new Comment();
                        vacancyStageInfoDomain.Comment.Update(vacancyStageInfoSource.Comment);
                    }
                }
            }
            else if (extendedStage.Stage.IsCommentRequired && vacancyStageInfoSource.Comment != null)
            {
                vacancyStageInfoDomain.Comment = new Comment();
                vacancyStageInfoDomain.Comment.Update(vacancyStageInfoSource.Comment);
            }

        }
    }
}
