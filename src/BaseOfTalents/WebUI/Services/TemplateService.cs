using Mailer;

namespace WebUI.Services
{
    public class TemplateService
    {
        private ITemplateLoader _loader;
        public TemplateService(ITemplateLoader loader)
        {
            _loader = loader;
        }

        public string GetTemplate()
        {
            string template = _loader.Load();
            return template;
        }
    }
}