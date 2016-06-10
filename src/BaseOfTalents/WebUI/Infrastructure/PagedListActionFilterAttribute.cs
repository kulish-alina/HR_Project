using BaseOfTalents.DAL.Extensions;
using System.Globalization;
using System.Net.Http;
using System.Web.Http.Filters;

namespace BaseOfTalents.WebUI.Infrastructure
{
    public class PagedListActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(
            HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            var objectContent = actionExecutedContext.Response.Content
                as ObjectContent;
            if (objectContent == null)
            {
                return;
            }

            var pagedList = objectContent.Value as IPagedList;
            if (pagedList == null)
            {
                return;
            }

            actionExecutedContext.Response.Headers.Add(
                "X-Page-Index",
                pagedList.PageIndex.ToString(CultureInfo.InvariantCulture));
            actionExecutedContext.Response.Headers.Add(
                "X-Page-Size",
                pagedList.PageSize.ToString(CultureInfo.InvariantCulture));
            actionExecutedContext.Response.Headers.Add(
                "X-Total-Count",
                pagedList.TotalCount.ToString(CultureInfo.InvariantCulture));

            var listType = pagedList.List.GetType();
            actionExecutedContext.Response.Content = new ObjectContent(
                listType,
                pagedList.List,
                objectContent.Formatter);
        }
    }
}