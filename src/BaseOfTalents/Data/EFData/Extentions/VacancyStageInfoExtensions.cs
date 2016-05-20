using Domain.DTO.DTOModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Extentions
{
    public static class VacancyStageInfoExtensions
    {
        public static void Update(this VacancyStageInfo domain, VacancyStageInfoDTO dto)
        {
            if (dto.Comment == null)
            {
                if (dto.VacancyStage.IsCommentRequired)
                {
                    throw new ArgumentNullException("Vacancy stage info should have comment");
                }
                else
                {
                    domain.Comment = null;
                }
            }
            else
            {
                domain.Comment = new Comment()
                {
                    Message = dto.Comment.Message
                };
            }
            domain.VacancyStage = new VacancyStage()
            {
                Order = dto.VacancyStage.Order,
                IsCommentRequired = dto.VacancyStage.IsCommentRequired,
                StageId = dto.VacancyStage.StageId,
                State = dto.VacancyStage.State,
            };
            domain.State = dto.State;
            domain.CandidateId = dto.CandidateId;
            domain.VacancyId = dto.VacancyId;
        }
    }
}
