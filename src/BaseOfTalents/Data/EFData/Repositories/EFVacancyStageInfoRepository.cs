using Data.Infrastructure;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Repositories
{
    public class EFVacancyStageInfoRepository : EFBaseEntityRepository<VacancyStageInfo>
    {
        public EFVacancyStageInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public override void Remove(VacancyStageInfo entity)
        {
            var attachedVacancyStageInfo = DbContext.Set<VacancyStageInfo>().Attach(entity);
            var attachedVacancyStage = DbContext.Set<VacancyStage>().Attach(attachedVacancyStageInfo.VacancyStage);
            var attachedComment = DbContext.Set<Comment>().Attach(attachedVacancyStageInfo.Comment);

            DbContext.Set<VacancyStage>().Remove(attachedVacancyStage);
            DbContext.Set<Comment>().Remove(attachedComment);
            DbContext.Set<VacancyStageInfo>().Remove(attachedVacancyStageInfo);
        }
    }
}
