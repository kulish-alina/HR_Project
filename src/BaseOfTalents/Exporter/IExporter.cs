using System.IO;

namespace Exporter
{
    public interface IExporter<in TExport>
    {
        Stream Export(TExport dataSet);
    }
}
