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
            vacancyStageInfoDomain.IsPassed = vacancyStageInfoSource.IsPassed;
            vacancyStageInfoDomain.StageId = vacancyStageInfoSource.StageId;

            var stage = destination.StageFlow.FirstOrDefault(x => x.Id == vacancyStageInfoSource.StageId);
            if (vacancyStageInfoSource.IsNew())
            {
                if (stage.IsCommentRequired)
                {
                    if (vacancyStageInfoSource.Comment == null)
                    {
                        if (vacancyStageInfoDomain.IsPassed)
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
            else if (stage.IsCommentRequired && vacancyStageInfoSource.Comment != null)
            {
                vacancyStageInfoDomain.Comment.Update(vacancyStageInfoSource.Comment);
            }

        }
    }
}
